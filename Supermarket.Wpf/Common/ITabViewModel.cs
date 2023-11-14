namespace Supermarket.Wpf.Common;

/// <summary>
/// View model that is incorporated into a tab
/// </summary>
public interface ITabViewModel : IViewModel
{
    string TabHeader { get; }
}