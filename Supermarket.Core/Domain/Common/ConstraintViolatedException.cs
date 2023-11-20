namespace Supermarket.Core.Domain.Common;

public class ConstraintViolatedException : CoreException
{
    public ConstraintViolatedException(string message) : base(message)
    {
    }
    
    public ConstraintViolatedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ConstraintViolatedException(RepositoryOperationFailedException cause) 
        : base("Operation cannot be executed due to repository operation failure", cause)
    {
    }
}