using TaskListApi.Models;

namespace TaskListApi.Services.Interfaces;

public interface ITaskListService
{
    Task<TaskList> CreateTaskListAsync(string userId, string name);
    Task<TaskList> GetTaskListByIdAsync(string userId, string id);
    Task UpdateTaskListAsync(string userId, string id, string name);
    Task DeleteTaskListAsync(string userId, string id);
    Task<(IEnumerable<TaskList> Items, long TotalCount)> GetTaskListsAsync(string userId, int page, int pageSize);
    Task AddSharedUserAsync(string userId, string taskListId, string sharedUserId);
    Task<IEnumerable<string>> GetSharedUsersAsync(string userId, string taskListId);
    Task RemoveSharedUserAsync(string userId, string taskListId, string sharedUserId);
}