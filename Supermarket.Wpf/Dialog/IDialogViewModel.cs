using System;
using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.Dialog;

public interface IDialogViewModel<TResult, TParameters> : IViewModel
{
    /// <summary>
    /// Initializes dialog with parameters and checks whether they are valid
    /// </summary>
    void SetParameters(TParameters parameters);
    
    /// <summary>
    /// Raised when result was received in dialog
    /// </summary>
    event EventHandler<DialogResult<TResult>> ResultReceived;
}

public interface IDialogViewModel<TResult> : IViewModel
{
    /// <summary>
    /// Raised when result was received in dialog
    /// </summary>
    event EventHandler<DialogResult<TResult>> ResultReceived;
}

public interface IDialogViewModel : IViewModel
{
    /// <summary>
    /// Raised when result was received in dialog
    /// </summary>
    event EventHandler<DialogResult> ResultReceived;
}