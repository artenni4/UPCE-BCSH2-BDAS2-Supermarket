namespace Supermarket.Domain.Common
{
    /// <summary>
    /// Thrown when data in database is in inconsistent or bad state and cannot be normally processed
    /// </summary>
    internal class InconsistencyException : CoreException
    {
        public InconsistencyException()
        {
        }

        public InconsistencyException(string? message) : base(message)
        {
        }

        public InconsistencyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
