using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMPression.Geometry.Shapes
{
    public class Parallelogram : IShape
    {
        public Complex Base { get; set; }
        public Complex Height { get; set; }

        public Parallelogram(Complex l, Complex w)
        {
            Base = l;
            Height = w;
        }

        public Complex Surface()
        {
            return Base * Height;
        }

        public Complex Perimeter()
        {
            return 2 * Base + 2 * Height;
        }

        public int Sides => 4;
    }
}
