using System;
using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.Dialog;

public interface IDialogViewModel<TResult> : IViewModel
{
    event EventHandler<TResult> ResultReceived;
}

public interface IDialogViewModel : IViewModel
{
    event EventHandler ResultReceived;
}