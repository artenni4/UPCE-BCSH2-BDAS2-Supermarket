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
}

public class DialogResult
{
    protected readonly ResultType Type;
    private DialogResult(ResultType type)
    {
        Type = type;
    }

    public static DialogResult Ok() => new(ResultType.Ok);
    public static DialogResult Cancel() => new(ResultType.Cancelled);

    public bool IsOk()
    {
        return Type == ResultType.Ok;
    }
    
    public bool IsCancelled()
    {
        return Type == ResultType.Cancelled;
    }
}