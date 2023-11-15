namespace Supermarket.Core.Domain.Common;

public class RepositoryInconsistencyException : RepositoryException
{
    public RepositoryInconsistencyException()
    {
    }

    public RepositoryInconsistencyException(string? message) : base(message)
    {
    }

    public RepositoryInconsistencyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}