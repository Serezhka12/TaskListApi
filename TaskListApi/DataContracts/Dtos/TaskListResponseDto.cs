namespace TaskListApi.DataContracts.Dtos;

public class TaskListResponseDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> SharedUserIds { get; set; }
}