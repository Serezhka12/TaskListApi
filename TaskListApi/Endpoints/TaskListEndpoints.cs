using TaskListApi.DataContracts;
using TaskListApi.DataContracts.Dtos;
using TaskListApi.Services.Interfaces;

namespace TaskListApi.Endpoints;

public static class TaskListEndpoints
{
    public static void MapTaskListEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/users/{userId}/tasklists", async (string userId, CreateTaskListDto dto, ITaskListService service, CancellationToken cancellationToken) =>
        {
            var result = await service.CreateTaskListAsync(userId, dto.Name, cancellationToken);
            var response = new ApiResponse<TaskListResponseDto>
            {
                StatusCode = 201,
                Data = result,
                Error = null
            };
            return Results.Created($"/api/users/{userId}/tasklists/{result.Id}", response);
        });

        routes.MapGet("/api/users/{userId}/tasklists", async (string userId, int page, int pageSize, ITaskListService service, CancellationToken cancellationToken) =>
        {
            var (items, totalCount) = await service.GetTaskListsAsync(userId, page, pageSize, cancellationToken);
            var responseData = new
            {
                TotalCount = totalCount,
                Items = items.Select(t => new { t.Id, t.Name })
            };

            var response = new ApiResponse<object>
            {
                StatusCode = 200,
                Data = responseData,
                Error = null
            };

            return Results.Ok(response);
        });

        routes.MapGet("/api/users/{userId}/tasklists/{id}", async (string userId, string id, ITaskListService service, CancellationToken cancellationToken) =>
        {
            var result = await service.GetTaskListByIdAsync(userId, id, cancellationToken);
            var response = new ApiResponse<TaskListResponseDto>
            {
                StatusCode = 200,
                Data = result,
                Error = null
            };
            return Results.Ok(response);
        });

        routes.MapPut("/api/users/{userId}/tasklists/{id}", async (string userId, string id, UpdateTaskListDto dto, ITaskListService service, CancellationToken cancellationToken) =>
        {
            await service.UpdateTaskListAsync(userId, id, dto.Name, cancellationToken);
            var response = new ApiResponse<object>
            {
                StatusCode = 204,
                Data = null,
                Error = null
            };
            return Results.StatusCode(204);
        });

        routes.MapDelete("/api/users/{userId}/tasklists/{id}", async (string userId, string id, ITaskListService service, CancellationToken cancellationToken) =>
        {
            await service.DeleteTaskListAsync(userId, id, cancellationToken);
            var response = new ApiResponse<object>
            {
                StatusCode = 204,
                Data = null,
                Error = null
            };
            return Results.StatusCode(204);
        });

        routes.MapPost("/api/users/{userId}/tasklists/{id}/share", async (string userId, string id, AddSharedUserDto dto, ITaskListService service, CancellationToken cancellationToken) =>
        {
            await service.AddSharedUserAsync(userId, id, dto.SharedUserId, cancellationToken);
            var response = new ApiResponse<object>
            {
                StatusCode = 204,
                Data = null,
                Error = null
            };
            return Results.StatusCode(204);
        });

        routes.MapGet("/api/users/{userId}/tasklists/{id}/share", async (string userId, string id, ITaskListService service, CancellationToken cancellationToken) =>
        {
            var sharedUsers = await service.GetSharedUsersAsync(userId, id, cancellationToken);
            var response = new ApiResponse<IEnumerable<string>>
            {
                StatusCode = 200,
                Data = sharedUsers,
                Error = null
            };
            return Results.Ok(response);
        });

        routes.MapDelete("/api/users/{userId}/tasklists/{id}/share/{sharedUserId}", async (string userId, string id,
            string sharedUserId, ITaskListService service, CancellationToken cancellationToken) =>
        {
            await service.RemoveSharedUserAsync(userId, id, sharedUserId, cancellationToken);
            var response = new ApiResponse<object>
            {
                StatusCode = 204,
                Data = null,
                Error = null
            };
            return Results.StatusCode(204);
        });
    }
}