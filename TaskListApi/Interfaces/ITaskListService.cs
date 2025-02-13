using TaskListApi.DataContracts.Dtos;

namespace TaskListApi.Services.Interfaces;

public interface ITaskListService
{
    Task<TaskListResponseDto> CreateTaskListAsync(string userId, string name, CancellationToken cancellationToken);
    Task<TaskListResponseDto> GetTaskListByIdAsync(string userId, string id, CancellationToken cancellationToken);
    Task UpdateTaskListAsync(string userId, string id, string name, CancellationToken cancellationToken);
    Task DeleteTaskListAsync(string userId, string id, CancellationToken cancellationToken);
    Task<(IEnumerable<TaskListResponseDto> Items, long TotalCount)> GetTaskListsAsync(string userId, int page, int pageSize, CancellationToken cancellationToken);
    Task AddSharedUserAsync(string userId, string taskListId, string sharedUserId, CancellationToken cancellationToken);
    Task<IEnumerable<string>> GetSharedUsersAsync(string userId, string taskListId, CancellationToken cancellationToken);
    Task RemoveSharedUserAsync(string userId, string taskListId, string sharedUserId, CancellationToken cancellationToken);
}