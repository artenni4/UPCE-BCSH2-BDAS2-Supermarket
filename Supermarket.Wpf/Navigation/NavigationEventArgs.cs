﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Wpf.Navigation
{
    internal class NavigationEventArgs
    {
        public required object NewViewModel { get; init; }
    }
}