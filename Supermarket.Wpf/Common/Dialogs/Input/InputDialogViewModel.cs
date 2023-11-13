using System;
using System.Windows.Input;
using Supermarket.Wpf.Common.Dialogs.Confirmation;
using Supermarket.Wpf.Dialog;

namespace Supermarket.Wpf.Common.Dialogs.Input;

public class InputDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<string, InputDialogParameters>
{
    public ICommand ConfirmCommand { get; }
    public ICommand CancelCommand { get; }

    private Func<string?, bool>? _validator;
    
    private string? _inputValue;
    public string? InputValue
    {
        get => _inputValue;
        set => SetProperty(ref _inputValue, value);
    }

    public InputDialogViewModel()
    {
        ConfirmCommand = new RelayCommand(Confirm, CanConfirm);
        CancelCommand = new RelayCommand(Cancel);
    }

    private bool CanConfirm(object? arg)
    {
        return _validator?.Invoke(InputValue) ?? true;
    }

    private void Cancel(object? obj)
    {
        ResultReceived?.Invoke(this, DialogResult<string>.Cancel());
    }

    private void Confirm(object? obj)
    {
        ResultReceived?.Invoke(this, DialogResult<string>.Ok(InputValue ?? string.Empty));
    }

    private string? _title;
    public string? Title
    {
        get => _title;
        private set => SetProperty(ref _title, value);
    }
    
    private string? _inputLabel;
    public string? InputLabel
    {
        get => _inputLabel;
        private set => SetProperty(ref _inputLabel, value);
    }

    public void SetParameters(InputDialogParameters parameters)
    {
        Title = parameters.Title;
        InputLabel = parameters.InputLabel;
        _validator = parameters.Validator;
    }

    public event EventHandler<DialogResult<string>>? ResultReceived;
}