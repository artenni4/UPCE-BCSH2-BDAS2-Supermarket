using System.Collections.ObjectModel;
using System.Linq;
using Supermarket.Core.UseCases.CashBox;
using Supermarket.Wpf.Dialog;
using Supermarket.Wpf.LoggedUser;
using Supermarket.Wpf.Navigation;

namespace Supermarket.Wpf.CashBox;

public class CashBoxViewModelFake : CashBoxViewModel
{
    public CashBoxViewModelFake() 
        : base(new CashBoxServiceFake(), new DialogServiceFake(), new LoggedUserServiceFake(), new NavigationServiceFake())
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
        
        DisplayedProducts.Update(cashBoxProducts);

        Categories.Update(new CashBoxProductCategory[]
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

        SelectedProducts.Update(cashBoxProducts.Select(cashBoxProduct => new SelectedProductModel
        {
            CashBoxProduct = cashBoxProduct,
            Count = 10,
        }).ToArray());
    }
}