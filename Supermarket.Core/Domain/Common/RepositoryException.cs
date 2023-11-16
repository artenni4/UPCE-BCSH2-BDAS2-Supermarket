namespace Supermarket.Core.Domain.Common;

/// <summary>
/// Generic exception for errors in data access layer
/// </summary>
public abstract class RepositoryException : CoreException
{
    public RepositoryException()
    {
    }

    public RepositoryException(string? message) : base(message)
    {
    }

    public RepositoryException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}