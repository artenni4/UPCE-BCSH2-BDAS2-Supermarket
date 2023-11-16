namespace Supermarket.Core.Domain.Products
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

        public static readonly MeasureUnit Kilogram = new("kilogram", "kg");
        public static readonly MeasureUnit Gram = new("gram", "g");
        public static readonly MeasureUnit Litre = new("litr", "l");
        public static readonly MeasureUnit Millilitre = new("mililitr", "ml");
        public static readonly MeasureUnit Piece = new("kus", "ks");
        public static readonly MeasureUnit Meter = new("metr", "m");

        public static IReadOnlyList<MeasureUnit> Values { get; } = new List<MeasureUnit>()
        {
            Kilogram,
            Gram,
            Litre,
            Millilitre,
            Meter
        };

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MeasureUnit)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Abbreviation);
        }

        public bool Equals(MeasureUnit? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Abbreviation == other.Abbreviation;
        }

        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(MeasureUnit first, MeasureUnit second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(MeasureUnit? first, MeasureUnit? second)
        {
            if (ReferenceEquals(first, second)) return false;
            if (first is null || second is null) return true;
            return !first.Equals(second);
        }

    }
}
