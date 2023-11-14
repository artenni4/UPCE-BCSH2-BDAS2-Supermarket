using System.Collections.ObjectModel;

namespace Supermarket.Wpf.Common;

public static class ObservableCollectionExtensions
{
    /// <summary>
    /// Clears old values and adds new ones
    /// </summary>
    public static void Update<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> newValues)
    {
        observableCollection.Clear();
        observableCollection.AddRange(newValues);
    }
    
    /// <summary>
    /// Adds range of values to the collection
    /// </summary>
    public static void AddRange<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> values)
    {
        foreach (var value in values)
        {
            observableCollection.Add(value);
        }
    }
}