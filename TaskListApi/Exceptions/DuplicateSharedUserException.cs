namespace TaskListApi.Exceptions;

public class DuplicateSharedUserException(string message) : Exception(message);