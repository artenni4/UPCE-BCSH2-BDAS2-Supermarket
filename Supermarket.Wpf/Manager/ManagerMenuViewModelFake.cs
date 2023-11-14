using Supermarket.Wpf.ViewModelResolvers;

namespace Supermarket.Wpf.Manager;

public class ManagerMenuViewModelFake : ManagerMenuViewModel
{
    public ManagerMenuViewModelFake() : base(new ViewModelResolverFake())
    {
    }
}