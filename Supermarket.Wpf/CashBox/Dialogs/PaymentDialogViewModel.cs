using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using Supermarket.Core.UseCases.CashBox;
using Supermarket.Wpf.Common.Dialogs.Confirmation;
using Supermarket.Wpf.Dialog;

namespace Supermarket.Wpf.CashBox.Dialogs;

public class PaymentDialogViewModel : NotifyPropertyChangedBase, IDialogViewModel<PaymentDialogResult, IReadOnlyList<CashBoxSoldProduct>>
{
    private readonly IDialogService _dialogService;
    private readonly ICashBoxService _cashBoxService;

    private IReadOnlyList<CashBoxSoldProduct>? _soldProducts;
    
    public ObservableCollection<Coupon> Coupons { get; } = new();
    
    public ICommand PayByCardCommand { get; }
    public ICommand PayWithCashCommand { get; }
    public ICommand PayWithCouponCommand { get; }

    public PaymentDialogViewModel(IDialogService dialogService, ICashBoxService cashBoxService)
    {
        _dialogService = dialogService;
        _cashBoxService = cashBoxService;
        PayByCardCommand = new RelayCommand(PayByCard);
        PayWithCashCommand = new RelayCommand(PayWithCash);
        PayWithCouponCommand = new RelayCommand(PayWithCoupon, _ => _soldProducts is not null);

        Coupons.CollectionChanged += OnCouponsOnCollectionChanged;
    }

    private void OnCouponsOnCollectionChanged(object? o, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
    {
        if (Coupons.Count == 0)
        {
            Total = null;
        }
        else
        {
            Total = Price - Coupons.Sum(c => c.Discount);
        }
    }

    private async void PayWithCoupon(object? obj)
    {
        if (_soldProducts is null)
        {
            return;
        }
        
        var dialogResult = await _dialogService.ShowInputDialogAsync("Zadejte kupon", inputLabel: null);
        if (!dialogResult.IsOk(out var coupon))
        {
            return;
        }

        try
        {
            var validCoupon = await _cashBoxService.CheckCouponAsync(coupon, _soldProducts, Coupons);
            Coupons.Add(validCoupon);
        }
        catch (CouponExceedsCostException)
        {
            MessageBox.Show("Kupon převyšuje cenu produktů", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (InvalidCouponException)
        {
            MessageBox.Show("Kupon je neplatný", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (CouponAlreadyUsedException)
        {
            MessageBox.Show("Kupon již byl použit", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void PayWithCash(object? obj)
    {
        if (Price.HasValue == false)
        {
            return;
        }
        
        var dialogResult = await _dialogService.ShowConfirmationDialogAsync("POKLADNÁ VYŽADUJE VLOŽENI HOTOVOSTI", ConfirmationButtons.Ok);
        if (!dialogResult.IsOk())
        {
            return;
        }

        var total = Total ?? Price.Value;
        var paymentResult = new PaymentDialogResult(CashBoxPaymentType.Cash, total, Coupons.ToArray());
        ResultReceived?.Invoke(this, DialogResult<PaymentDialogResult>.Ok(paymentResult));
    }

    private async void PayByCard(object? obj)
    {
        if (Price.HasValue == false)
        {
            return;
        }
        
        var dialogResult = await _dialogService.ShowConfirmationDialogAsync("POKLADNÁ VYŽADUJE ZAPLACENÍ KARTOU", ConfirmationButtons.Ok);
        if (!dialogResult.IsOk())
        {
            return;
        }
        
        var total = Total ?? Price.Value;
        var paymentResult = new PaymentDialogResult(CashBoxPaymentType.Card, total, Coupons.ToArray());
        ResultReceived?.Invoke(this, DialogResult<PaymentDialogResult>.Ok(paymentResult));
    }

    public void SetParameters(IReadOnlyList<CashBoxSoldProduct> products)
    {
        Price = products.Sum(p => p.OverallPrice);
        _soldProducts = products;
    }

    private decimal? _price;
    public decimal? Price
    {
        get => _price;
        private set => SetProperty(ref _price, value);
    }
    
    private decimal? _total;

    public decimal? Total
    {
        get => _total;
        private set => SetProperty(ref _total, value);
    }
    
    public event EventHandler<DialogResult<PaymentDialogResult>>? ResultReceived;
}