﻿using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.Navigation
{
    public class NavigationEventArgs
    {
        public required IViewModel NewViewModel { get; init; }
    }
}
