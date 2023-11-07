using Supermarket.Wpf.Dialog;

namespace Supermarket.Wpf.Cashbox;

public class CashBoxViewModelFake : CashboxViewModel
{
    public CashBoxViewModelFake() : base(new CashBoxServiceFake(), new DialogServiceFake())
    {
    }
}