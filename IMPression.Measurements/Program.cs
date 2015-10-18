using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMPression.Measurements
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Functions.BesselY0(5));
            //Console.WriteLine(MathFunctions.y0(5));
            Console.WriteLine(-0.30851762524903378007364898421204661138634706162734);
            Console.ReadLine();
        }
    }
}
