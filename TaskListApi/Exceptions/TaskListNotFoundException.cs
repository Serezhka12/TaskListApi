namespace TaskListApi.Exceptions;

public class TaskListNotFoundException(string message) : Exception(message);