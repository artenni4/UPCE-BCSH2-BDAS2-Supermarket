namespace Supermarket.Domain.Products
{
    public class MeasureUnit : IEquatable<MeasureUnit>
    {
        public string Name { get; }
        public string Abbreviation { get; }

        private MeasureUnit(string name, string abbreviation)
        {
            Name = name;
            Abbreviation = abbreviation;
        }

        public static MeasureUnit Kilogram { get; } = new MeasureUnit("kilogram", "kg");
        public static MeasureUnit Gram { get; } = new MeasureUnit("gram", "g");
        public static MeasureUnit Litre { get; } = new MeasureUnit("litre", "l");

        public override bool Equals(object? obj)
        {
            if (obj == this) return true;
            if (!(obj is MeasureUnit other)) return false;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Abbreviation);
        }

        public bool Equals(MeasureUnit? other)
        {
            if (other == null) return false;
            return other.Name == Name && other.Abbreviation == Abbreviation;
        }
    }
}
