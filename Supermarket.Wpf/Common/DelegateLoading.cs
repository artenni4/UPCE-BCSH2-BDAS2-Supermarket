using System;
using System.Reflection;

namespace Supermarket.Wpf.Common;

public sealed class DelegateLoading : IDisposable
{
    private readonly IAsyncViewModel _asyncViewModel;

    public DelegateLoading(IAsyncViewModel asyncViewModel)
    {
        _asyncViewModel = asyncViewModel;
        InvokeEvent(nameof(IAsyncViewModel.LoadingStarted), EventArgs.Empty);
    }

    public void Dispose()
    {
        InvokeEvent(nameof(IAsyncViewModel.LoadingFinished), EventArgs.Empty);
    }

    private void InvokeEvent<TEventArgs>(string eventName, TEventArgs args) where TEventArgs : EventArgs
    {
        // Get the type of the object containing the event.
        var type = _asyncViewModel.GetType();

        // Get the event with the specified name.
        var eventInfo = type.GetEvent(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (eventInfo == null)
        {
            throw new ArgumentException($"Event {eventName} not found in type {type.FullName}");
        }

        // Retrieve the field that stores the event's delegate, which is usually in the format of "eventName + Event".
        var field = type.GetField(eventName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);

        if (field == null)
        {
            throw new InvalidOperationException("Event backing field not found.");
        }

        // Get the value of the field on the object, which is the delegate to invoke.
        var eventDelegate = (MulticastDelegate?)field.GetValue(_asyncViewModel);

        // If there's no delegate attached to the event, there's nothing to invoke.
        if (eventDelegate == null)
        {
            return;
        }

        // Invoke the delegate with the appropriate arguments.
        foreach (var handler in eventDelegate.GetInvocationList())
        {
            handler.Method.Invoke(handler.Target, new object[] { _asyncViewModel, args });
        }
    }
}