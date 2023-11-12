using System.Diagnostics.CodeAnalysis;

namespace Supermarket.Wpf.Dialog;

public class DialogResult<TResult>
{
    private readonly ResultType _type;
    private readonly TResult? _result;

    private DialogResult(ResultType type, TResult? result)
    {
        _type = type;
        _result = result;
    }

    public static DialogResult<TResult> Ok(TResult result) => new(ResultType.Ok, result);
    public static DialogResult<TResult> Cancel() => new(ResultType.Cancelled, result: default);
    
    public bool IsOk([NotNullWhen(true)] out TResult? result)
    {
        if (_type == ResultType.Ok)
        {
            result = _result!;
            return true;
        }
        result = default;
        return false;
    }
    
    public bool IsCancelled()
    {
        return _type == ResultType.Cancelled;
    }
}

public class DialogResult
{
    private readonly ResultType _type;
    private DialogResult(ResultType type)
    {
        _type = type;
    }

    public static DialogResult Ok() => new(ResultType.Ok);
    public static DialogResult Cancel() => new(ResultType.Cancelled);

    public bool IsOk()
    {
        return _type == ResultType.Ok;
    }
    
    public bool IsCancelled()
    {
        return _type == ResultType.Cancelled;
    }
}