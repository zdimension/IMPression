using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMPression.Geometry.Shapes
{
    public class Triangle : IShape
    {
        public Complex SideA { get; private set; }

        public Complex SideB { get; private set; }

        public Complex SideC { get; private set; }

        public Complex AngleAB { get; private set; }

        public Complex AngleAC { get; private set; }

        public Complex AngleBC { get; private set; }

        public bool IsRight => 
            AngleAB == Constants.Pi / 2 || 
            AngleAC == Constants.Pi / 2 || 
            AngleBC == Constants.Pi / 2;

        public bool IsEquilateral => SideA == SideB && SideB == SideC;

        public bool IsIsoscele => !IsEquilateral && ((SideA == SideB) || (SideA == SideC) || (SideB == SideC));

        public bool IsAcute => 
            AngleAB < Constants.Pi / 2 && 
            AngleAC < Constants.Pi / 2 && 
            AngleBC < Constants.Pi / 2;

        /// <summary>
        /// Creates a new triangle
        /// </summary>
        /// <param name="sideA">First side</param>
        /// <param name="sideB">Second side</param>
        /// <param name="sideC">Third side</param>
        public Triangle (Complex sideA, Complex sideB, Complex sideC)
        {
            SideA = sideA;
            SideB = sideB;
            SideC = sideC;
            AngleAB = Functions.ArcCos((sideA.Square() + sideB.Square() - sideC.Square()) / (2 * sideA * sideB));
            AngleBC = Functions.ArcCos((sideC.Square() + sideB.Square() - sideA.Square()) / (2 * sideC * sideB));
            AngleAC = Functions.ArcCos((sideA.Square() + sideC.Square() - sideB.Square()) / (2 * sideA * sideC));
        }

        public Complex Surface()
        {
            var s = Perimeter() / 2;
            return Functions.Sqrt(s * (s - SideA) * (s - SideB) * (s - SideC));
        }

        public Complex Perimeter()
        {
            return SideA + SideB + SideC;
        }

        public int Sides => 3;
    }
}
