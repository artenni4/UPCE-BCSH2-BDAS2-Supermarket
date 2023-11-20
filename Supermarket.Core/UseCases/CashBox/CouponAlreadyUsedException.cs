using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.UseCases.CashBox;

public class CouponAlreadyUsedException : CoreException
{
    public CouponAlreadyUsedException(Coupon coupon) : base($"Coupon {coupon.Code} already used")
    {
    }
}