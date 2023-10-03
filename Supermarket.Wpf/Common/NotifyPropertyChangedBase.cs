using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Supermarket.Wpf.Common
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        protected void SetProperty<TProperty>(ref TProperty prop, TProperty value, [CallerMemberName] string? propertyName = default)
        {
            prop = value;
            OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
