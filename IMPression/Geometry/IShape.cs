using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMPression.Geometry
{
    interface IShape
    {
        Complex Surface();

        Complex Perimeter();

        int Sides { get; }
    }
}
