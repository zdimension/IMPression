using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMPression.Geometry.Shapes
{
    public class Square : IShape
    {
        public Complex Side { get; set; }

        public Square(Complex side)
        {
            Side = side;
        }

        public Complex Surface()
        {
            return Side.Square();
        }

        public Complex Perimeter()
        {
            return 4 * Side;
        }

        public int Sides => 4;
    }
}
