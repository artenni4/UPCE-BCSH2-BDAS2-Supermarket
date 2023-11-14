namespace Supermarket.Wpf.Common.Dialogs.Input;

public record InputDialogParameters(string Title, string InputLabel, Func<string?, bool>? Validator);