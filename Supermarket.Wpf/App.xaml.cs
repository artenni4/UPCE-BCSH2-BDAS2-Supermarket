using Microsoft.Extensions.DependencyInjection;
using Supermarket.Wpf.Main;
using System.Diagnostics;
using System.Windows;
using Supermarket.Core;
using Supermarket.Infrastructure;
using Supermarket.Wpf.ViewModelResolvers;

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
            Trace.Listeners.Add(new ConsoleTraceListener());
            
            var serviceCollection = new ServiceCollection()
                .AddCore()
                .AddInfrastructure()
                .AddWpf();
            
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var resolver = _serviceProvider.GetRequiredService<IViewModelResolver>();
            var mainWindow = new MainWindow()
            {
                DataContext = resolver.Resolve<MainViewModel>().Result,
            };

            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
