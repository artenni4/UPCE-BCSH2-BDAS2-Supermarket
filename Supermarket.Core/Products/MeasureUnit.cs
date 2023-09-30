using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Products
{
    public class MeasureUnit
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
    }
}
