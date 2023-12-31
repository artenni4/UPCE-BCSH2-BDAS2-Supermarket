﻿using System.Windows.Input;
using Supermarket.Wpf.Dialog;

namespace Supermarket.Wpf.Common.Dialogs.Confirmation;

public class ConfirmationDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<EmptyResult, ConfirmationDialogParameters>
{
    public ICommand ConfirmCommand { get; }
    public ICommand CancelCommand { get; }

    public ConfirmationDialogViewModel()
    {
        ConfirmCommand = new RelayCommand(Confirm);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void Cancel(object? obj)
    {
        ResultReceived?.Invoke(this, DialogResult<EmptyResult>.Cancel());
    }

    private void Confirm(object? obj)
    {
        ResultReceived?.Invoke(this, DialogResult<EmptyResult>.Ok(EmptyResult.Value));
    }

    private string? _title;
    public string? Title
    {
        get => _title;
        private set => SetProperty(ref _title, value);
    }

    private bool _isCancelVisible;
    public bool IsCancelVisible
    {
        get => _isCancelVisible;
        private set => SetProperty(ref _isCancelVisible, value);
    }

    public void SetParameters(ConfirmationDialogParameters parameters)
    {
        Title = parameters.Title;
        IsCancelVisible = parameters.Buttons switch
        {
            ConfirmationButtons.OkCancel => true,
            ConfirmationButtons.Ok or _ => false
        };
    }

    public event EventHandler<DialogResult<EmptyResult>>? ResultReceived;
}