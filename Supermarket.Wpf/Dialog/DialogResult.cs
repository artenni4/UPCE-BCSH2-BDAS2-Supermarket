namespace Supermarket.Wpf.Dialog;

public class DialogResult<TResult>
{
    private DialogResult(ResultType type, TResult? result)
    {
        Type = type;
        Result = result;
    }

    public ResultType Type { get; }
    public TResult? Result { get; }

    public static DialogResult<TResult> Ok(TResult result) => new(ResultType.Ok, result);
    public static DialogResult<TResult> Cancel() => new(ResultType.Cancelled, result: default);
}