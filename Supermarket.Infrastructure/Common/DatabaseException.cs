namespace Supermarket.Core.Domain.Common;

/// <summary>
/// Generic exception for errors in data access layer
/// </summary>
public class DatabaseException : CoreException
{
    public DatabaseException()
    {
    }

    public DatabaseException(string? message) : base(message)
    {
    }

    public DatabaseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}