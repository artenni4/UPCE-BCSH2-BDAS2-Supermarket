using System.Collections.ObjectModel;
using System.Linq;
using Supermarket.Core.CashBoxes;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;

namespace Supermarket.Wpf.Cashbox;

public class CashBoxViewModelFake : CashboxViewModel
{
    public CashBoxViewModelFake() : base(new CashBoxServiceFake(), new DialogServiceFake(), new LoggedUserServiceFake())
    {
        var cashBoxProducts = new CashBoxProduct[]
        {
            new() { ProductId = 1, Name = "AAA", IsByWeight = true, MeasureUnit = "kg", Price = 100 },
            new() { ProductId = 2, Name = "BBB", IsByWeight = true, MeasureUnit = "kg", Price = 100  },
            new() { ProductId = 3, Name = "CCCCCCCC CCCC", IsByWeight = true, MeasureUnit = "kg", Price = 100  },
            new() { ProductId = 1, Name = "AAA", IsByWeight = true, MeasureUnit = "kg", Price = 100  },
            new() { ProductId = 2, Name = "BBB", IsByWeight = true, MeasureUnit = "kg", Price = 100  },
            new() { ProductId = 3, Name = "CCCCCCCC CCCC", IsByWeight = true, MeasureUnit = "kg", Price = 100  },
            new() { ProductId = 1, Name = "AAA", IsByWeight = true, MeasureUnit = "kg", Price = 100  },
            new() { ProductId = 2, Name = "BBB", IsByWeight = true, MeasureUnit = "kg", Price = 100  },
            new() { ProductId = 3, Name = "CCCCCCCC CCCC", IsByWeight = true, MeasureUnit = "kg", Price = 100  },
            new() { ProductId = 3, Name = "CCCCCCCC CCCC", IsByWeight = true, MeasureUnit = "kg", Price = 100  },
        };
        
        DisplayedProducts = new ObservableCollection<CashBoxProduct>(cashBoxProducts);

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

        SelectedProducts = new ObservableCollection<SelectedProductModel>(cashBoxProducts.Select(cashBoxProduct => new SelectedProductModel
        {
            CashBoxProduct = cashBoxProduct,
            Count = 10,
        }).ToArray());
    }
}