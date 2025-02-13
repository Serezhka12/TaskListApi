namespace TaskListApi.Exceptions;

public class TaskListAccessDeniedException(string message) : Exception(message);