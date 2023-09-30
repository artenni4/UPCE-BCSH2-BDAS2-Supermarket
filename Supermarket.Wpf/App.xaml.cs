using Microsoft.Extensions.DependencyInjection;
using Supermarket.Core.Employees;
using Supermarket.Infrastructure.Employees;
using Supermarket.Wpf.Login;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
            var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }

        private void AddCore(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IEmployeeService, EmployeeService>();
        }

        private void AddInfrastructure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IEmployeeRepository, EmployeeRepository>();
        }

        private void AddWpf(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<LoginWindow>();
            serviceCollection.AddSingleton<LoginViewModel>();
        }
    }
}
