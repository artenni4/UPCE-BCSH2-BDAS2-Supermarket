namespace Supermarket.Domain.Common
{
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
