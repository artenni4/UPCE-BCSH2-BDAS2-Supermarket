using System;
using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.Dialog;

public interface IDialogViewModel<TResult, TParameters> : IViewModel
{
    /// <summary>
    /// Initializes dialog with parameters and checks whether they are valid
    /// </summary>
    void SetParameters(TParameters parameters);
    event EventHandler<TResult> ResultReceived;
}

public interface IDialogViewModel<TResult> : IDialogViewModel<TResult, EmptyParameters>
{
}

public interface IDialogViewModel : IDialogViewModel<EmptyResult, EmptyParameters>
{
}