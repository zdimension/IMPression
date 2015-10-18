using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMPression.Geometry.Shapes
{
    public class Rectangle : IShape
    {
        public Complex Length { get; set; }
        public Complex Width { get; set; }

        public Rectangle(Complex l, Complex w)
        {
            Length = l;
            Width = w;
        }

        public Complex Surface()
        {
            return Length * Width;
        }

        public Complex Perimeter()
        {
            return 2 * Length + 2 * Width;
        }

        public int Sides => 4;
    }
}
