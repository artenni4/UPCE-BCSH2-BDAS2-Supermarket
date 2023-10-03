using Supermarket.Wpf.Common;

namespace Supermarket.Wpf.Login
{
    public class LoginModel : NotifyPropertyChangedBase
    {
        private string? login;
        public string? Login
        {
            get => login;
            set => SetProperty(ref login, value);
        }

        private string? password;
        public string? Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
    }
}
