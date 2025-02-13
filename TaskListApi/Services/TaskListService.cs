using Mapster;
using TaskListApi.DataContracts.Dtos;
using TaskListApi.Exceptions;
using TaskListApi.Models;
using TaskListApi.Repositories.Interfaces;
using TaskListApi.Services.Interfaces;

namespace TaskListApi.Services.Implementation;

public class TaskListService : ITaskListService
{
    private readonly ITaskListRepository _repository;

    public TaskListService(ITaskListRepository repository)
    {
        _repository = repository;
    }

    public async Task<TaskListResponseDto> CreateTaskListAsync(string userId, string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 1 || name.Length > 255)
        {
            throw new ArgumentException("Task list name must be between 1 and 255 characters");
        }

        var taskList = new TaskList
        {
            Name = name,
            OwnerId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.CreateAsync(taskList, cancellationToken);
        return taskList.Adapt<TaskListResponseDto>();
    }

    public async Task<TaskListResponseDto> GetTaskListByIdAsync(string userId, string id, CancellationToken cancellationToken)
    {
        var taskList = await _repository.GetByIdAsync(id, cancellationToken);
        if (taskList == null)
        {
            throw new TaskListNotFoundException("Task list not found");
        }
        if (taskList.OwnerId != userId && !taskList.SharedUserIds.Contains(userId))
        {
            throw new TaskListAccessDeniedException("Access denied");
        }
        return taskList.Adapt<TaskListResponseDto>();
    }

    public async Task UpdateTaskListAsync(string userId, string id, string name, CancellationToken cancellationToken)
    {
        var taskList = await _repository.GetByIdAsync(id, cancellationToken);
        if (taskList == null)
        {
            throw new TaskListNotFoundException("Task list not found");
        }
        if (taskList.OwnerId != userId && !taskList.SharedUserIds.Contains(userId))
        {
            throw new TaskListAccessDeniedException("Access denied");
        }
        if (string.IsNullOrWhiteSpace(name) || name.Length < 1 || name.Length > 255)
        {
            throw new ArgumentException("Task list name must be between 1 and 255 characters");
        }
        taskList.Name = name;
        await _repository.UpdateAsync(taskList, cancellationToken);
    }

    public async Task DeleteTaskListAsync(string userId, string id, CancellationToken cancellationToken)
    {
        var taskList = await _repository.GetByIdAsync(id, cancellationToken);
        if (taskList == null)
        {
            throw new TaskListNotFoundException("Task list not found");
        }
        if (taskList.OwnerId != userId)
        {
            throw new TaskListAccessDeniedException("Only the owner can delete the task list");
        }
        await _repository.DeleteAsync(id, cancellationToken);
    }

    public async Task<(IEnumerable<TaskListResponseDto> Items, long TotalCount)> GetTaskListsAsync(string userId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _repository.GetTaskListsForUserAsync(userId, page, pageSize, cancellationToken);
        var dtos = items.Select(x => x.Adapt<TaskListResponseDto>());
        return (dtos, totalCount);
    }

    public async Task AddSharedUserAsync(string userId, string taskListId, string sharedUserId, CancellationToken cancellationToken)
    {
        var taskList = await _repository.GetByIdAsync(taskListId, cancellationToken);
        if (taskList == null)
        {
            throw new TaskListNotFoundException("Task list not found");
        }
        if (taskList.OwnerId != userId && !taskList.SharedUserIds.Contains(userId))
        {
            throw new TaskListAccessDeniedException("Access denied");
        }
        if (taskList.SharedUserIds.Contains(sharedUserId))
        {
            throw new DuplicateSharedUserException("The shared user relationship already exists");
        }
        taskList.SharedUserIds.Add(sharedUserId);
        await _repository.UpdateAsync(taskList, cancellationToken);
    }

    public async Task<IEnumerable<string>> GetSharedUsersAsync(string userId, string taskListId, CancellationToken cancellationToken)
    {
        var taskList = await _repository.GetByIdAsync(taskListId, cancellationToken);
        if (taskList == null)
        {
            throw new TaskListNotFoundException("Task list not found");
        }
        if (taskList.OwnerId != userId && !taskList.SharedUserIds.Contains(userId))
        {
            throw new TaskListAccessDeniedException("Access denied");
        }
        return taskList.SharedUserIds;
    }

    public async Task RemoveSharedUserAsync(string userId, string taskListId, string sharedUserId, CancellationToken cancellationToken)
    {
        var taskList = await _repository.GetByIdAsync(taskListId, cancellationToken);
        if (taskList == null)
        {
            throw new TaskListNotFoundException("Task list not found");
        }
        if (taskList.OwnerId != userId && !taskList.SharedUserIds.Contains(userId))
        {
            throw new TaskListAccessDeniedException("Access denied");
        }
        if (!taskList.SharedUserIds.Contains(sharedUserId))
        {
            throw new TaskListNotFoundException("Shared user relationship does not exist");
        }
        taskList.SharedUserIds.Remove(sharedUserId);
        await _repository.UpdateAsync(taskList, cancellationToken);
    }
}

