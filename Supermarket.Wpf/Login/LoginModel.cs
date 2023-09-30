using System.ComponentModel;

namespace Supermarket.Wpf.Login
{
    public class LoginModel : INotifyPropertyChanged
    {
        private string login;
        private string password;

        public string Login
        {
            get { return login; } set { login = value; OnPropertyChanged("Login"); } 
        }

        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged("Password"); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
