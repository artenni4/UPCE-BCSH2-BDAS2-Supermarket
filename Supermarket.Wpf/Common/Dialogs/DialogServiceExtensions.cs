using Supermarket.Wpf.Common.Dialogs.Confirmation;
using Supermarket.Wpf.Common.Dialogs.DropDown;
using Supermarket.Wpf.Common.Dialogs.Input;
using Supermarket.Wpf.Dialog;

namespace Supermarket.Wpf.Common.Dialogs;

public static class DialogServiceExtensions
{
    public static async Task<DialogResult> ShowConfirmationDialogAsync(
        this IDialogService dialogService,
        string title, 
        ConfirmationButtons buttons = ConfirmationButtons.OkCancel)
    {
        var parameters = new ConfirmationDialogParameters(title, buttons);
        return await dialogService.ShowAsync<ConfirmationDialogViewModel, ConfirmationDialogParameters>(parameters);
    }

    public static async Task<DialogResult<string>> ShowInputDialogAsync(
        this IDialogService dialogService,
        string title,
        string? inputLabel,
        Func<string?, bool>? validator = default)
    {
        var parameters = new InputDialogParameters(title, inputLabel, validator);
        return await dialogService.ShowAsync<InputDialogViewModel, string, InputDialogParameters>(parameters);
    }
    
    public static async Task<DialogResult<TParsable>> ShowInputDialogAsync<TParsable>(
        this IDialogService dialogService,
        string title,
        string? inputLabel,
        IFormatProvider? formatProvider = default)
        where TParsable : IParsable<TParsable>
    {
        var result = await dialogService.ShowInputDialogAsync(title, inputLabel, Validator);
        
        if (result.IsOk(out var parsableStr))
        {
            return DialogResult<TParsable>.Ok(TParsable.Parse(parsableStr, formatProvider));
        }

        if (result.IsCancelled())
        {
            return DialogResult<TParsable>.Cancel();
        }

        throw new NotSupportedException($"{result} dialog result is not supported");
        
        bool Validator(string? arg) => TParsable.TryParse(arg, formatProvider, out _);
    }
    
    public static async Task<DialogResult<TValue>> ShowDropDownDialogAsync<TValue>(
        this IDialogService dialogService,
        string title,
        string displayProperty,
        IReadOnlyList<TValue> values) where TValue : class
    {
        var parameters = new DropDownDialogParameters(title, displayProperty, values);
        var result = await dialogService.ShowAsync<DropDownDialogViewModel, object, DropDownDialogParameters>(parameters);
        
        if (result.IsOk(out var value))
        {
            return DialogResult<TValue>.Ok((TValue)value);
        }

        if (result.IsCancelled())
        {
            return DialogResult<TValue>.Cancel();
        }

        throw new NotSupportedException($"{result} dialog result is not supported");
    }
}