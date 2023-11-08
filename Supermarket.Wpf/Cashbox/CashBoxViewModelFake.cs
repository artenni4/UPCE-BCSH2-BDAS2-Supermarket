﻿using System.Collections.ObjectModel;
using Supermarket.Core.CashBoxes;
using Supermarket.Wpf.Dialog;

namespace Supermarket.Wpf.Cashbox;

public class CashBoxViewModelFake : CashboxViewModel
{
    public CashBoxViewModelFake() : base(new CashBoxServiceFake(), new DialogServiceFake())
    {
        DisplayedProducts = new ObservableCollection<CashBoxProduct>(new CashBoxProduct[]
        {
            new() { ProductId = 1, Name = "AAA", IsByWeight = true },
            new() { ProductId = 2, Name = "BBB", IsByWeight = true },
            new() { ProductId = 3, Name = "CCCCCCCC CCCC", IsByWeight = true },
            new() { ProductId = 1, Name = "AAA", IsByWeight = true },
            new() { ProductId = 2, Name = "BBB", IsByWeight = true },
            new() { ProductId = 3, Name = "CCCCCCCC CCCC", IsByWeight = true },
            new() { ProductId = 1, Name = "AAA", IsByWeight = true },
            new() { ProductId = 2, Name = "BBB", IsByWeight = true },
            new() { ProductId = 3, Name = "CCCCCCCC CCCC", IsByWeight = true },
            new() { ProductId = 3, Name = "CCCCCCCC CCCC", IsByWeight = true },
        });

        Categories = new ObservableCollection<CashBoxProductCategory>(new CashBoxProductCategory[]
        {
            new() { CategoryId = 1, Name = "AAA" },
            new() { CategoryId = 2, Name = "BBB" },
            new() { CategoryId = 3, Name = "CCCCCCCC CCCC" },
            new() { CategoryId = 1, Name = "AAA" },
            new() { CategoryId = 2, Name = "BBB" },
            new() { CategoryId = 3, Name = "CCCCCCCC CCCC" },
            new() { CategoryId = 1, Name = "AAA" },
            new() { CategoryId = 2, Name = "BBB" },
            new() { CategoryId = 3, Name = "CCCCCCCC CCCC" },
            new() { CategoryId = 1, Name = "AAA" },
            new() { CategoryId = 2, Name = "BBB" },
            new() { CategoryId = 3, Name = "CCCCCCCC CCCC" },
        });
    }
}