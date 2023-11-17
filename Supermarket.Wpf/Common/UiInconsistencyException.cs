namespace Supermarket.Wpf.Common;

public class UiInconsistencyException : Exception
{
    public UiInconsistencyException()
    {
    }

    public UiInconsistencyException(string? message) : base(message)
    {
    }

    public UiInconsistencyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}