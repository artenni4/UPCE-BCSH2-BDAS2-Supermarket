namespace Supermarket.Core.Domain.Common
{
    /// <summary>
    /// Base exception for all custom exceptions
    /// </summary>
    public abstract class CoreException : Exception
    {
        public CoreException()
        {
        }

        public CoreException(string? message) : base(message)
        {
        }

        public CoreException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
