using Microsoft.Extensions.DependencyInjection;
using Supermarket.Wpf.Main;
using Supermarket.Wpf.Navigation;
using System;
using System.Windows;
using Supermarket.Core;
using Supermarket.Domain;
using Supermarket.Infrastructure;

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
            var serviceCollection = new ServiceCollection()
                .AddDomain()
                .AddApplication()
                .AddInfrastructure()
                .AddWpf();
            
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
    }
}
