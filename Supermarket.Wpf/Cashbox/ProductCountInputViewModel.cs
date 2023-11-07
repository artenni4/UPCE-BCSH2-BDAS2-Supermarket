using System;
using System.Windows.Input;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.Dialog;

namespace Supermarket.Wpf.Cashbox;

public class ProductCountInputViewModel : NotifyPropertyChangedBase, IDialogViewModel<DialogResult<decimal>>
{
    public ICommand Confirm { get; }
    public ICommand Cancel { get; }

    public ProductCountInputViewModel()
    {
        Confirm = new RelayCommand(ConfirmCount, CanConfirmCount);
        Cancel = new RelayCommand(CancelDialog);
    }

    private void CancelDialog(object? obj)
    {
        ResultReceived?.Invoke(this, DialogResult<decimal>.Cancel());
    }

    public void SetParameters(EmptyParameters parameters) { }

    private void ConfirmCount(object? obj)
    {
        if (ProductCount is null)
        {
            return;
        }
        
        var result = decimal.Parse(ProductCount);
        ResultReceived?.Invoke(this, DialogResult<decimal>.Ok(result));
    }

    private bool CanConfirmCount(object? arg)
    {
        return decimal.TryParse(ProductCount, out _);
    }

    public event EventHandler<DialogResult<decimal>>? ResultReceived;

    private string? _productCount;
    public string? ProductCount
    {
        get => _productCount;
        set => SetProperty(ref _productCount, value);
    }
}