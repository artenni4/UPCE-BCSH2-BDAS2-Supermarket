using Supermarket.Wpf.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Supermarket.Wpf.Login
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : UserControl
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (popupMenu.Visibility == Visibility.Collapsed)
                    popupMenu.Visibility = Visibility.Visible;
                else if (popupMenu.Visibility == Visibility.Visible)
                    popupMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            PopupMenu newUserControl = new PopupMenu();

            Window newWindow = new Window
            {
                Content = newUserControl,
                WindowStyle = WindowStyle.None,
                WindowState = WindowState.Maximized,
                AllowsTransparency = true,
                Background = Brushes.Transparent
            };

            newWindow.Show();
        }

    }
}
