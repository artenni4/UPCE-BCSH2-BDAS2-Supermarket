using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.UseCases.CashBox;

public class CouponExceedsCostException : CoreException
{
    public CouponExceedsCostException(Coupon coupon) : base($"Coupon {coupon.Code} exceeds cost of products")
    {
        
    }
}