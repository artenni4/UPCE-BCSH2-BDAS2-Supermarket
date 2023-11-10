namespace Supermarket.Core.Domain.PaymentTypes
{
    public class PaymentType : IEquatable<PaymentType>
    {
        public string Name { get; }
    
        private PaymentType(string name) 
        {
            Name = name; 
        }

        public static readonly PaymentType Card = new("Karta");
        public static readonly PaymentType Cash = new("Hotovost");
        public static readonly PaymentType Coupon = new("Kupon");

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PaymentType)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public bool Equals(PaymentType? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(PaymentType first, PaymentType second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(PaymentType first, PaymentType second)
        {
            return !first.Equals(second);
        }

    }
}
