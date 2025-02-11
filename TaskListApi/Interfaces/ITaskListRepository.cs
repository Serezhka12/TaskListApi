using TaskListApi.Models;

namespace TaskListApi.Repositories.Interfaces;

public interface ITaskListRepository
{
    Task CreateAsync(TaskList taskList, CancellationToken cancellationToken);
    Task<TaskList> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task UpdateAsync(TaskList taskList, CancellationToken cancellationToken);
    Task DeleteAsync(string id, CancellationToken cancellationToken);
    Task<(IEnumerable<TaskList> Items, long TotalCount)> GetTaskListsForUserAsync(string userId, int page, int pageSize, CancellationToken cancellationToken);
}