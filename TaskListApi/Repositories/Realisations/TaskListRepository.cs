using MongoDB.Driver;
using TaskListApi.Models;
using TaskListApi.Repositories.Interfaces;

namespace TaskListApi.Repositories.Realisations;

public class TaskListRepository : ITaskListRepository
{
    private readonly IMongoCollection<TaskList> _collection;

    public TaskListRepository(IMongoClient mongoClient, IConfiguration configuration)
    {
        var databaseName = configuration["MongoDbSettings:DatabaseName"]
                           ?? throw new ArgumentException("DatabaseName is not configured");
        var database = mongoClient.GetDatabase(databaseName);
        _collection = database.GetCollection<TaskList>("tasklists");
    }

    public async Task CreateAsync(TaskList taskList)
    {
        await _collection.InsertOneAsync(taskList);
    }

    public async Task<TaskList> GetByIdAsync(string id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(TaskList taskList)
    {
        await _collection.ReplaceOneAsync(x => x.Id == taskList.Id, taskList);
    }

    public async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(x => x.Id == id);
    }

    public async Task<(IEnumerable<TaskList> Items, long TotalCount)> GetTaskListsForUserAsync(string userId, int page, int pageSize)
    {
        var filter = Builders<TaskList>.Filter.Or(
            Builders<TaskList>.Filter.Eq(x => x.OwnerId, userId),
            Builders<TaskList>.Filter.AnyEq(x => x.SharedUserIds, userId)
        );

        var totalCount = await _collection.CountDocumentsAsync(filter);
        var items = await _collection.Find(filter)
            .SortByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}