using System;
using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.ViewModelResolvers;

public class ResolvedViewModelEventArgs : EventArgs
{
    public required IViewModel ViewModel { get; init; }
}