using TaskListApi.Models;
using TaskListApi.Repositories.Interfaces;
using TaskListApi.Services.Interfaces;

namespace TaskListApi.Services.Realizations
{
    public class TaskListService(ITaskListRepository repository) : ITaskListService
    {
        /// <summary>
        /// Creates a new task list.
        /// </summary>
        public async Task<TaskList> CreateTaskListAsync(string userId, string name)
        {
            // Validate task list name length (must be between 1 and 255 characters)
            if (string.IsNullOrWhiteSpace(name) || name.Length < 1 || name.Length > 255)
            {
                throw new ArgumentException("Task list name must be between 1 and 255 characters.");
            }

            var taskList = new TaskList
            {
                Name = name,
                OwnerId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await repository.CreateAsync(taskList);
            return taskList;
        }

        /// <summary>
        /// Retrieves a task list by its ID.
        /// </summary>
        public async Task<TaskList> GetTaskListByIdAsync(string userId, string id)
        {
            var taskList = await repository.GetByIdAsync(id);
            if (taskList == null)
            {
                throw new Exception("Task list not found.");
            }
            if (taskList.OwnerId != userId && !taskList.SharedUserIds.Contains(userId))
            {
                throw new UnauthorizedAccessException("Access denied.");
            }
            return taskList;
        }

        /// <summary>
        /// Updates an existing task list.
        /// </summary>
        public async Task UpdateTaskListAsync(string userId, string id, string name)
        {
            var taskList = await repository.GetByIdAsync(id);
            if (taskList == null)
            {
                throw new Exception("Task list not found.");
            }
            if (taskList.OwnerId != userId && !taskList.SharedUserIds.Contains(userId))
            {
                throw new UnauthorizedAccessException("Access denied.");
            }
            if (string.IsNullOrWhiteSpace(name) || name.Length < 1 || name.Length > 255)
            {
                throw new ArgumentException("Task list name must be between 1 and 255 characters.");
            }
            taskList.Name = name;
            await repository.UpdateAsync(taskList);
        }

        /// <summary>
        /// Deletes a task list. Only the owner can delete the task list.
        /// </summary>
        public async Task DeleteTaskListAsync(string userId, string id)
        {
            var taskList = await repository.GetByIdAsync(id);
            if (taskList == null)
            {
                throw new Exception("Task list not found.");
            }
            if (taskList.OwnerId != userId)
            {
                throw new UnauthorizedAccessException("Only the owner can delete the task list.");
            }
            await repository.DeleteAsync(id);
        }

        /// <summary>
        /// Retrieves task lists for the specified user with pagination.
        /// </summary>
        public async Task<(IEnumerable<TaskList> Items, long TotalCount)> GetTaskListsAsync(string userId, int page, int pageSize)
        {
            return await repository.GetTaskListsForUserAsync(userId, page, pageSize);
        }

        /// <summary>
        /// Adds a share relationship for the task list with a specified user.
        /// </summary>
        public async Task AddSharedUserAsync(string userId, string taskListId, string sharedUserId)
        {
            var taskList = await repository.GetByIdAsync(taskListId);
            if (taskList == null)
            {
                throw new Exception("Task list not found.");
            }
            if (taskList.OwnerId != userId && !taskList.SharedUserIds.Contains(userId))
            {
                throw new UnauthorizedAccessException("Access denied.");
            }
            if (taskList.SharedUserIds.Contains(sharedUserId))
            {
                throw new Exception("Share relationship already exists.");
            }
            taskList.SharedUserIds.Add(sharedUserId);
            await repository.UpdateAsync(taskList);
        }

        /// <summary>
        /// Retrieves the list of users with whom the task list is shared.
        /// </summary>
        public async Task<IEnumerable<string>> GetSharedUsersAsync(string userId, string taskListId)
        {
            var taskList = await repository.GetByIdAsync(taskListId);
            if (taskList == null)
            {
                throw new Exception("Task list not found.");
            }
            if (taskList.OwnerId != userId && !taskList.SharedUserIds.Contains(userId))
            {
                throw new UnauthorizedAccessException("Access denied.");
            }
            return taskList.SharedUserIds;
        }

        /// <summary>
        /// Removes the share relationship for a specified user.
        /// </summary>
        public async Task RemoveSharedUserAsync(string userId, string taskListId, string sharedUserId)
        {
            var taskList = await repository.GetByIdAsync(taskListId);
            if (taskList == null)
            {
                throw new Exception("Task list not found.");
            }
            if (taskList.OwnerId != userId && !taskList.SharedUserIds.Contains(userId))
            {
                throw new UnauthorizedAccessException("Access denied.");
            }
            if (!taskList.SharedUserIds.Contains(sharedUserId))
            {
                throw new Exception("Share relationship does not exist.");
            }
            taskList.SharedUserIds.Remove(sharedUserId);
            await repository.UpdateAsync(taskList);
        }
    }
}
