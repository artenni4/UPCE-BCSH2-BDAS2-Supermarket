using Microsoft.Extensions.DependencyInjection;
using Supermarket.Core.Common;
using Supermarket.Core.Employees;
using Supermarket.Core.Products;
using Supermarket.Core.Products.Categories;
using Supermarket.Infrastructure.Common;
using Supermarket.Infrastructure.Employees;
using Supermarket.Infrastructure.Products;
using Supermarket.Infrastructure.Products.Categories;
using Supermarket.Wpf.Cashbox;
using Supermarket.Wpf.Login;
using Supermarket.Wpf.Main;
using Supermarket.Wpf.Navigation;
using System;
using System.Windows;

namespace Supermarket.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            AddCore(serviceCollection);
            AddInfrastructure(serviceCollection);
            AddWpf(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = new MainWindow()
            {
                DataContext = _serviceProvider.GetRequiredService<MainViewModel>()
            };

            var navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            navigationService.NavigateTo(NavigateWindow.Login);

            mainWindow.Show();
            base.OnStartup(e);
        }

        private static void AddCore(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IEmployeeService, EmployeeService>();
            serviceCollection.AddScoped<IProductService, ProductService>();
            serviceCollection.AddScoped<IProductCategoryService, ProductCategoryService>();
        }

        private static void AddInfrastructure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<IEmployeeRepository, EmployeeRepository>();
            serviceCollection.AddScoped<IProductRepository, ProductRepository>();
            serviceCollection.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        }

        private static void AddWpf(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<LoginViewModel>();
            serviceCollection.AddSingleton<CashboxViewModel>();
            serviceCollection.AddSingleton<MainViewModel>();
            serviceCollection.AddSingleton<INavigationService, NavigationService>();
        }
    }
}
