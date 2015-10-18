using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMPression.Geometry.Shapes
{
    public class Circle : IShape
    {
        public Complex Radius { get; set; }

        public Circle(Complex r)
        {
            Radius = r;
        }

        public Complex Surface()
        {
            return Constants.Pi * Radius.Square();
        }

        public Complex Perimeter()
        {
            return 2 * Constants.Pi * Radius;
        }

        public int Sides => 1;
    }
}
