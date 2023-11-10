namespace Supermarket.Core.Domain.Common
{
    /// <summary>
    /// Thrown when data of application is in inconsistent
    /// or invalid state and cannot be normally processed
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
