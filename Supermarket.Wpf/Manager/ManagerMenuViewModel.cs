﻿using System.Collections.ObjectModel;
using System.Windows;
using Supermarket.Wpf.Manager.AddProducts;
using Supermarket.Wpf.Manager.SupermarketCashboxes;
using Supermarket.Wpf.Manager.SupermarketEmployees;
using Supermarket.Wpf.Manager.SupermarketLogs;
using Supermarket.Wpf.Manager.SupermarketProducts;
using Supermarket.Wpf.Manager.SupermarketSales;
using Supermarket.Wpf.Manager.SupermarketStorages;
using Supermarket.Wpf.ViewModelResolvers;
using System.Windows.Controls;
using System.Windows.Input;

namespace Supermarket.Wpf.Manager
{
    public class ManagerMenuViewModel : NotifyPropertyChangedBase, IViewModel, IAsyncInitialized
    {
        private readonly IViewModelResolver _viewModelResolver;
        
        private ITabViewModel? _selectedTabViewModel;
        public ITabViewModel? SelectedTabViewModel
        {
            get => _selectedTabViewModel;
            set
            {
                if (_selectedTabViewModel != value)
                {
                    SetProperty(ref _selectedTabViewModel, value);
                    _selectedTabViewModel?.ActivateIfNeeded();
                }
            }
        }

        private SupermarketProductsViewModel? _supermarketProductsViewModel;
        public SupermarketProductsViewModel? SupermarketProductsViewModel
        {
            get => _supermarketProductsViewModel;
            set => SetProperty(ref _supermarketProductsViewModel, value);
        }

        private AddProductsViewModel? _addProductsViewModel;
        public AddProductsViewModel? AddProductsViewModel
        {
            get => _addProductsViewModel;
            set => SetProperty(ref _addProductsViewModel, value);
        }

        private SupermarketEmployeesViewModel? _supermarketEmployeesViewModel;
        public SupermarketEmployeesViewModel? SupermarketEmployeesViewModel
        {
            get => _supermarketEmployeesViewModel;
            set => SetProperty(ref _supermarketEmployeesViewModel, value);
        }

        private SupermarketStoragesViewModel? _supermarketStoragesViewModel;
        public SupermarketStoragesViewModel? SupermarketStoragesViewModel
        {
            get => _supermarketStoragesViewModel;
            set => SetProperty(ref _supermarketStoragesViewModel, value);
        }

        private SupermarketLogsViewModel? _supermarketLogsViewModel;
        public SupermarketLogsViewModel? SupermarketLogsViewModel
        {
            get => _supermarketLogsViewModel;
            set => SetProperty(ref _supermarketLogsViewModel, value);
        }

        private SupermarketSalesViewModel? _supermarketSalesViewModel;
        public SupermarketSalesViewModel? SupermarketSalesViewModel
        {
            get => _supermarketSalesViewModel;
            set => SetProperty(ref _supermarketSalesViewModel, value);
        }

        private SupermarketCashboxesViewModel? _supermarketCashboxesViewModel;
        public SupermarketCashboxesViewModel? SupermarketCashboxesViewModel
        {
            get => _supermarketCashboxesViewModel;
            set => SetProperty(ref _supermarketCashboxesViewModel, value);
        }

        public ObservableCollection<ITabViewModel> TabViewModels { get; } = new();
        
        public ManagerMenuViewModel(IViewModelResolver viewModelResolver)
        {
            _viewModelResolver = viewModelResolver;
        }

        public async Task InitializeAsync()
        {
            SupermarketProductsViewModel = await _viewModelResolver.Resolve<SupermarketProductsViewModel>();
            AddProductsViewModel = await _viewModelResolver.Resolve<AddProductsViewModel>();
            SupermarketEmployeesViewModel = await _viewModelResolver.Resolve<SupermarketEmployeesViewModel>();
            SupermarketStoragesViewModel = await _viewModelResolver.Resolve<SupermarketStoragesViewModel>();
            SupermarketLogsViewModel = await _viewModelResolver.Resolve<SupermarketLogsViewModel>();
            SupermarketSalesViewModel = await _viewModelResolver.Resolve<SupermarketSalesViewModel>();
            SupermarketCashboxesViewModel = await _viewModelResolver.Resolve<SupermarketCashboxesViewModel>();
            
            TabViewModels.Add(SupermarketProductsViewModel);
            TabViewModels.Add(AddProductsViewModel);
            TabViewModels.Add(SupermarketEmployeesViewModel);
            TabViewModels.Add(SupermarketLogsViewModel);
            TabViewModels.Add(SupermarketSalesViewModel);
            TabViewModels.Add(SupermarketCashboxesViewModel);
            
            SelectedTabViewModel = SupermarketProductsViewModel;
        }
    }
}
