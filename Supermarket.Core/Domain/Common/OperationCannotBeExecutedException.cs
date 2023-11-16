namespace Supermarket.Core.Domain.Common;

public class OperationCannotBeExecutedException : CoreException
{
    public OperationCannotBeExecutedException(string message) : base(message)
    {
    }
    
    public OperationCannotBeExecutedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public OperationCannotBeExecutedException(RepositoryOperationFailedException cause) 
        : base("Operation cannot be executed due to repository operation failure", cause)
    {
    }
}