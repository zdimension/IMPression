using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IMPression
{
    public class Constants
    {
        [MathConst("e")]
        public static Complex E
            => 2.718281828459045235360287471352662497757247093699959574966967627724076630353547594571382d;

        [MathConst("pi")]
        public static Complex Pi => 4.0 * Functions.ArcTan(1);

        [MathConst("tau")]
        public static Complex Tau => 2.0 * Pi;

        [MathConst("phi")]
        public static Complex Phi => 0.5 * (1.0 + Functions.Sqrt(5));

        [MathConst("eulergamma")]
        public static Complex EulerGamma
            => 0.577215664901532860606512090082402431042159335939923598805767234884867726777664670936947d;

        [MathConst("catalan")]
        public static Complex Catalan
            => 0.9159655941772190150546035149323841107741493742816721342664981196217630197762547694794d;

        [MathConst("c")]
        public static Complex SpeedOfLight => 2.99792458e8;

        [MathConst("°")]
        public static Complex Degree => Pi / 180;

        [MathConst("glaisher")]
        public static Complex Glaisher
            => 1.2824271291006226368753425688697917277676889273250011920637400217404063088588264611297d;

        [MathConst("khinchin")]
        public static Complex Khinchin
            => 2.6854520010653064453097148354817956938203822939944629530511523455572188595371520028011d;

        [MathConst("i")]
        public static Complex I => new Complex(0.0, 1.0);

        [MathConst("maxvalue")]
        public static Complex MaxValue => new Complex(Quad.MaxValue, Quad.MaxValue);

        [MathConst("minvalue")]
        public static Complex MinValue => new Complex(Quad.MinValue, Quad.MinValue);

        [MathConst("epsilon")]
        public static Complex Epsilon => new Complex(Quad.Epsilon, Quad.Epsilon);


        public static string GetConstantName(double value, string format = "", IFormatProvider formatProvider = null,
            int decimals = -1)
        {
            double norm = Functions.Round(value, 14);

            if (norm == Functions.Round(Pi, 14)) return "pi";
            if (norm == Functions.Round(Phi, 14)) return "phi";
            if (norm == Functions.Round(EulerGamma, 14)) return "eulergamma";
            if (norm == Functions.Round(Catalan, 14)) return "catalan";
            if (norm == Functions.Round(E, 14)) return "e";
            if (norm == Functions.Round(SpeedOfLight, 14)) return "c";
            if (norm == Functions.Round(Degree, 14)) return "degree";
            if (norm == Functions.Round(Glaisher, 14)) return "glaisher";
            if (norm == Functions.Round(Khinchin, 14)) return "khinchin";
            if (norm == Functions.Round(Tau, 14)) return "tau";

            return decimals == -1 ? value.ToString() : Functions.Round(value, decimals).ToString(format, formatProvider);
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MathConst : Attribute
    {
        public string Name { get; set; }

        public bool Hide { get; set; }

        public MathConst(string n)
        {
            Name = n;
            Hide = false;
        }

        public MathConst(bool h = false)
        {
            Name = "";
            Hide = h;
        }
    }
}