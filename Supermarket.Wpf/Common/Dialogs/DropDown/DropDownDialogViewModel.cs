using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Supermarket.Wpf.Dialog;

namespace Supermarket.Wpf.Common.Dialogs.DropDown;

public class DropDownDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<object, DropDownDialogParameters>
{
    public ICommand ConfirmCommand { get; }
    public ICommand CancelCommand { get; }

    public ObservableCollection<object> Values { get; set; } = new ();

    public DropDownDialogViewModel()
    {
        ConfirmCommand = new RelayCommand(Confirm, _ => InputValue is not null);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void Cancel(object? obj)
    {
        ResultReceived?.Invoke(this, DialogResult<object>.Cancel());
    }

    private void Confirm(object? obj)
    {
        Debug.Assert(InputValue != null, $"{nameof(InputValue)} must be selected from dropdown");
        
        ResultReceived?.Invoke(this, DialogResult<object>.Ok(InputValue));
    }
    
    private string? _displayProperty;
    public string? DisplayProperty
    {
        get => _displayProperty;
        private set => SetProperty(ref _displayProperty, value);
    }

    private string? _title;
    public string? Title
    {
        get => _title;
        private set => SetProperty(ref _title, value);
    }
    
    private object? _inputValue;
    public object? InputValue
    {
        get => _inputValue;
        set => SetProperty(ref _inputValue, value);
    }
    
    public void SetParameters(DropDownDialogParameters parameters)
    {
        Title = parameters.Title;
        DisplayProperty = parameters.DisplayProperty;
        foreach (var value in parameters.Values)
        {
            Values.Add(value);
        }
    }

    public event EventHandler<DialogResult<object>>? ResultReceived;
}