using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Auth;
using Supermarket.Core.CashBoxes;
using Supermarket.Core.Common;
using Supermarket.Core.Employees;
using Supermarket.Core.Login;
using Supermarket.Core.Products;
using Supermarket.Core.Products.Categories;
using Supermarket.Infrastructure.Common;
using Supermarket.Infrastructure.Database;
using Supermarket.Infrastructure.Employees;
using Supermarket.Infrastructure.Products;
using Supermarket.Infrastructure.Products.Categories;
using Supermarket.Wpf.Cashbox;
using Supermarket.Wpf.Common;
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
            // add configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            serviceCollection.AddSingleton<IConfiguration>(configuration);

            serviceCollection.AddScoped<IAuthService, AuthService>();
            
            serviceCollection.AddScopedWithProxy<ILoginService, LoginService>();
            serviceCollection.AddScopedWithProxy<ICashBoxService, CashBoxService>();
        }

        private static void AddInfrastructure(IServiceCollection serviceCollection)
        {
            // add database connection driver
            serviceCollection.AddConfigurationSection<DatabaseOptions>();
            serviceCollection.AddScoped(sp =>
            {
                var options = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
                return ActivatorUtilities.CreateInstance<OracleConnection>(sp, options.ConnectionString);
            });

            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<IEmployeeRepository, EmployeeRepository>();
            serviceCollection.AddScoped<IProductRepository, ProductRepository>();
            serviceCollection.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        }

        private static void AddWpf(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<INavigationService, NavigationService>();
            serviceCollection.AddSingleton<MainViewModel>();

            serviceCollection.AddTransient<LoginViewModel>();
            serviceCollection.AddTransient<CashboxViewModel>();
        }
    }
}
