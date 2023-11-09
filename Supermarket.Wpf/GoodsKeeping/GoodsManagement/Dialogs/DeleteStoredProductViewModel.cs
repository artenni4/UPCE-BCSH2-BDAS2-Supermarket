using Supermarket.Wpf.Common;
using Supermarket.Wpf.Dialog;
using System;
using System.Windows.Input;

namespace Supermarket.Wpf.GoodsKeeping.GoodsManagement.Dialogs
{
    public class DeleteStoredProductViewModel : NotifyPropertyChangedBase, IDialogViewModel<DialogResult<decimal>>
    {
        public ICommand Confirm { get; }
        public ICommand Cancel { get; }

        public event EventHandler<DialogResult<decimal>>? ResultReceived;

        private string? _deleteCount;
        public string? DeleteCount
        {
            get => _deleteCount;
            set => SetProperty(ref _deleteCount, value);
        }

        public DeleteStoredProductViewModel()
        {
            Confirm = new RelayCommand(ConfirmDelete, CanConfirmDelete);
            Cancel = new RelayCommand(CancelDelete);
        }

        private void CancelDelete(object? obj)
        {
            ResultReceived?.Invoke(this, DialogResult<decimal>.Cancel());
        }

        private void ConfirmDelete(object? obj)
        {
            if (DeleteCount is null)
            {
                return;
            }

            var result = decimal.Parse(DeleteCount);
            ResultReceived?.Invoke(this, DialogResult<decimal>.Ok(result));
        }

        private bool CanConfirmDelete(object? arg)
        {
            return decimal.TryParse(DeleteCount, out _);
        }

        public void SetParameters(EmptyParameters parameters) { }
    }
}
