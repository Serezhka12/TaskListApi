using Microsoft.AspNetCore.Mvc;
using TaskListApi.DataContracts.Dtos;
using TaskListApi.Models;
using TaskListApi.Services.Interfaces;
using TaskListApi.Utils;

namespace TaskListApi.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/tasklists")]
    public class TaskListsController(ITaskListService service) : ControllerBase
    {
        /// <summary>
        /// Creates a new task list.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateTaskList(
            string userId,
            [FromBody] CreateTaskListDto dto)
        {
            try
            {
                var taskList = await service.CreateTaskListAsync(userId, dto.Name);
                var response = new ApiResponse<TaskList>
                {
                    StatusCode = 201,
                    Data = taskList,
                    Error = null
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<object>
                {
                    StatusCode = 500,
                    Data = null,
                    Error = ex.Message
                };
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Retrieves task lists with pagination (only ID and name).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetTaskLists(
            string userId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var (items, totalCount) = await service.GetTaskListsAsync(userId, page, pageSize);
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

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<object>
                {
                    StatusCode = 500,
                    Data = null,
                    Error = ex.Message
                };

                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Retrieves detailed information about a specific task list.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskList(string userId, string id)
        {
            try
            {
                var taskList = await service.GetTaskListByIdAsync(userId, id);
                var response = new ApiResponse<TaskList>
                {
                    StatusCode = 200,
                    Data = taskList,
                    Error = null
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<object>
                {
                    StatusCode = 500,
                    Data = null,
                    Error = ex.Message
                };

                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Updates an existing task list.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaskList(
            string userId,
            string id,
            [FromBody] UpdateTaskListDto dto)
        {
            try
            {
                await service.UpdateTaskListAsync(userId, id, dto.Name);
                var response = new ApiResponse<object>
                {
                    StatusCode = 204,
                    Data = null,
                    Error = null
                };

                return StatusCode(204, response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<object>
                {
                    StatusCode = 500,
                    Data = null,
                    Error = ex.Message
                };

                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Deletes an existing task list (only the owner can delete).
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskList(string userId, string id)
        {
            try
            {
                await service.DeleteTaskListAsync(userId, id);
                var response = new ApiResponse<object>
                {
                    StatusCode = 204,
                    Data = null,
                    Error = null
                };

                return StatusCode(204, response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<object>
                {
                    StatusCode = 500,
                    Data = null,
                    Error = ex.Message
                };

                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Adds a shared user relationship to a task list.
        /// </summary>
        [HttpPost("{id}/share")]
        public async Task<IActionResult> AddSharedUser(string userId, string id, [FromBody] AddSharedUserDto dto)
        {
            try
            {
                await service.AddSharedUserAsync(userId, id, dto.SharedUserId);
                var response = new ApiResponse<object>
                {
                    StatusCode = 204,
                    Data = null,
                    Error = null
                };

                return StatusCode(204, response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<object>
                {
                    StatusCode = 500,
                    Data = null,
                    Error = ex.Message
                };

                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Retrieves the list of shared users for a task list.
        /// </summary>
        [HttpGet("{id}/share")]
        public async Task<IActionResult> GetSharedUsers(string userId, string id)
        {
            try
            {
                var sharedUsers = await service.GetSharedUsersAsync(userId, id);
                var response = new ApiResponse<IEnumerable<string>>
                {
                    StatusCode = 200,
                    Data = sharedUsers,
                    Error = null
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<object>
                {
                    StatusCode = 500,
                    Data = null,
                    Error = ex.Message
                };

                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Removes a shared user relationship from a task list.
        /// </summary>
        [HttpDelete("{id}/share/{sharedUserId}")]
        public async Task<IActionResult> RemoveSharedUser(string userId, string id, string sharedUserId)
        {
            try
            {
                await service.RemoveSharedUserAsync(userId, id, sharedUserId);
                var response = new ApiResponse<object>
                {
                    StatusCode = 204,
                    Data = null,
                    Error = null
                };

                return StatusCode(204, response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<object>
                {
                    StatusCode = 500,
                    Data = null,
                    Error = ex.Message
                };

                return StatusCode(500, response);
            }
        }
    }
}
