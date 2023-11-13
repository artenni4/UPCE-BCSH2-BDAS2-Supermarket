using System.Collections.Generic;

namespace Supermarket.Wpf.Common.Dialogs.DropDown;

public record DropDownDialogParameters(string Title, string DisplayProperty, IReadOnlyList<object> Values);
