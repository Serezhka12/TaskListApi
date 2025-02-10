using TaskListApi.Models;

namespace TaskListApi.Repositories.Interfaces;

public interface ITaskListRepository
{
    Task CreateAsync(TaskList taskList);
    Task<TaskList> GetByIdAsync(string id);
    Task UpdateAsync(TaskList taskList);
    Task DeleteAsync(string id);
    Task<(IEnumerable<TaskList> Items, long TotalCount)> GetTaskListsForUserAsync(string userId, int page, int pageSize);
}