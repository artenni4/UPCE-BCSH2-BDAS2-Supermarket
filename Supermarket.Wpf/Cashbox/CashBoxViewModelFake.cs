﻿using Supermarket.Core.CashBoxes;

namespace Supermarket.Wpf.Cashbox;

public class CashBoxViewModelFake : CashboxViewModel
{
    public CashBoxViewModelFake() : base(new CashBoxServiceFake())
    {
    }
}