﻿using System;
using System.Windows;
using System.Windows.Input;
using Supermarket.Core.Domain.Auth;
using Supermarket.Core.Domain.Auth.LoggedEmployees;
using Supermarket.Core.UseCases.CashBoxes;
using Supermarket.Wpf.Common;
using Supermarket.Wpf.Dialog;

namespace Supermarket.Wpf.Cashbox;

public class LoginAssistantViewModel : NotifyPropertyChangedBase, IDialogViewModel<DialogResult<ILoggedEmployee>>, IAsyncViewModel
{
    private readonly ICashBoxService _cashBoxService;

    public ICommand AssistantLoginCommand { get; }
    public ICommand CancelCommand { get; }
    
    public LoginAssistantViewModel(ICashBoxService cashBoxService)
    {
        _cashBoxService = cashBoxService;

        AssistantLoginCommand = new RelayCommand(AssistantLogin);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void Cancel(object? obj) => ResultReceived?.Invoke(this, DialogResult<ILoggedEmployee>.Cancel());

    private async void AssistantLogin(object? obj)
    {
        if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
        {
            return;
        }

        try
        {
            using var _ = new DelegateLoading(this);
            var loggedEmployee = await _cashBoxService.AssistantLoginAsync(new LoginData { Login = Login, Password = Password }, 1);
            ResultReceived?.Invoke(this, DialogResult<ILoggedEmployee>.Ok(loggedEmployee.Employee));
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
    public void SetParameters(EmptyParameters parameters) { }

    public event EventHandler<DialogResult<ILoggedEmployee>>? ResultReceived;
}