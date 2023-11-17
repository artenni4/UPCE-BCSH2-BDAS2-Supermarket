using System.Windows;
using System.Windows.Input;
using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.UseCases.CashBox;
using Supermarket.Wpf.Dialog;

namespace Supermarket.Wpf.CashBox.Dialogs;

public class LoginAssistantDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<LoggedSupermarketEmployee, LoginAssistantParameters>, IAsyncViewModel
{
    private readonly ICashBoxService _cashBoxService;

    private int? _cashBoxId;
    public ICommand AssistantLoginCommand { get; }
    public ICommand CancelCommand { get; }
    
    public LoginAssistantDialogViewModel(ICashBoxService cashBoxService)
    {
        _cashBoxService = cashBoxService;

        AssistantLoginCommand = new RelayCommand(AssistantLogin, CanLogin);
        CancelCommand = new RelayCommand(Cancel);
    }

    private bool CanLogin(object? arg)
    {
        return !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);
    }

    private void Cancel(object? obj) => ResultReceived?.Invoke(this, DialogResult<LoggedSupermarketEmployee>.Cancel());

    private async void AssistantLogin(object? obj)
    {
        if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password) || !_cashBoxId.HasValue)
        {
            return;
        }

        try
        {
            using var _ = new DelegateLoading(this);
            var loggedEmployee = await _cashBoxService.AssistantLoginAsync(new LoginData { Login = Login, Password = Password }, _cashBoxId.Value);
            ResultReceived?.Invoke(this, DialogResult<LoggedSupermarketEmployee>.Ok(loggedEmployee));
        }
        catch (InvalidCredentialsException)
        {
            MessageBox.Show("Špatné příhlašovací údaje", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (PermissionDeniedException)
        {
            MessageBox.Show("Přístup opdepřen", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private string? _login;
    public string? Login
    {
        get => _login;
        set => SetProperty(ref _login, value);
    }

    private string? _password;
    public string? Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public event EventHandler? LoadingStarted;
    public event EventHandler? LoadingFinished;

    public void SetParameters(LoginAssistantParameters parameters)
    {
        _cashBoxId = parameters.CashBoxId;
    }

    public event EventHandler<DialogResult<LoggedSupermarketEmployee>>? ResultReceived;
}