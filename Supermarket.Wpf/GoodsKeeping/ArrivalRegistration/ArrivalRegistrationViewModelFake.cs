using System.Collections.ObjectModel;
using Supermarket.Core.CashBoxes;
using Supermarket.Core.GoodsKeeping;
using Supermarket.Wpf.Dialog;

namespace Supermarket.Wpf.GoodsKeeping.ArrivalRegistration;

public class ArrivalRegistrationViewModelFake : ArrivalRegistrationViewModel
{
    public ArrivalRegistrationViewModelFake() : base(new GoodsKeepingServiceFake(), new DialogServiceFake())
    {
        DisplayedProducts = new ObservableCollection<GoodsKeepingProduct>(new GoodsKeepingProduct[]
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

        Categories = new ObservableCollection<GoodsKeepingProductCategory>(new GoodsKeepingProductCategory[]
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