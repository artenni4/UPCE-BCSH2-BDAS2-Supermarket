﻿using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.ViewModelResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Wpf.Manager.SupermarketSales
{
    public class SupermarketSalesViewModel : NotifyPropertyChangedBase, IAsyncViewModel/*, IAsyncInitialized*/
    {
        private readonly IManagerMenuService _managerMenuService;

        public event EventHandler? LoadingStarted;
        public event EventHandler? LoadingFinished;

        public SupermarketSalesViewModel(IManagerMenuService managerMenuService)
        {
            _managerMenuService = managerMenuService;
        }

        //public Task InitializeAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
