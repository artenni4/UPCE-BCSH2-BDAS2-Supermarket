namespace Supermarket.Core.Domain.Common
{
    /// <summary>
    /// Thrown when data of application is in inconsistent
    /// or invalid state and cannot be normally processed
    /// </summary>
    internal class ApplicationInconsistencyException : CoreException
    {
        public ApplicationInconsistencyException()
        {
        }

        public ApplicationInconsistencyException(string? message) : base(message)
        {
        }

        public ApplicationInconsistencyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
