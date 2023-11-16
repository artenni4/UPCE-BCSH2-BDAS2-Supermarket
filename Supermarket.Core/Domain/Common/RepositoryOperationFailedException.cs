namespace Supermarket.Core.Domain.Common;

public class RepositoryOperationFailedException : RepositoryException
{
    public RepositoryOperationFailedException(string operation, string details) 
        : base(FormatMessage(operation, details))
    {
    }

    public RepositoryOperationFailedException(string operation, string details, Exception innerException)
        : base(FormatMessage(operation, details), innerException)
    {
    }

    private static string FormatMessage(string operation, string details) => $"Failed operation: {operation}\nDetails: {details}";
}