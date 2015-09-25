using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Funcs = IMPression.MathFunctions;

namespace IMPression
{
    public class Constant : EquationValue
    {
        public static List<string> cs
        {
            get
            {
                return typeof (Constant).GetProperties(BindingFlags.Public |
                                                       BindingFlags.Static)
                    .Where(x => Attribute.IsDefined(x, typeof (MathConst)))
                    .Select(x => (Attribute.GetCustomAttribute(x, typeof (MathConst)) as MathConst).Name)
                    .ToList();
            }
        }

        public Constant(string src, int start)
        {
            var n = "";
            Complex v = 0;
            if (IsConstant(src, start, out n, out v))
            {
                m_sb.Append(n);
                Value = v;
            }
        }

        [MathConst("e")]
        public static Complex E
            => 2.718281828459045235360287471352662497757247093699959574966967627724076630353547594571382d;

        [MathConst("pi")]
        public static Complex Pi => 4.0 * Funcs.ArcTan(1);

        [MathConst("tau")]
        public static Complex Tau => 2.0 * Pi;

        [MathConst("phi")]
        public static Complex Phi => 0.5 * (1.0 + Funcs.Sqrt(5));

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


        public static string GetConstantName(double value, string format = "", IFormatProvider formatProvider = null,
            int decimals = -1)
        {
            double norm = Funcs.Round(value, 14);

            if (norm == Funcs.Round(Pi, 14)) return "pi";
            if (norm == Funcs.Round(Phi, 14)) return "phi";
            if (norm == Funcs.Round(EulerGamma, 14)) return "eulergamma";
            if (norm == Funcs.Round(Catalan, 14)) return "catalan";
            if (norm == Funcs.Round(E, 14)) return "e";
            if (norm == Funcs.Round(SpeedOfLight, 14)) return "c";
            if (norm == Funcs.Round(Degree, 14)) return "degree";
            if (norm == Funcs.Round(Glaisher, 14)) return "glaisher";
            if (norm == Funcs.Round(Khinchin, 14)) return "khinchin";
            if (norm == Funcs.Round(Tau, 14)) return "tau";

            return decimals == -1 ? value.ToString() : Funcs.Round(value, decimals).ToString(format, formatProvider);
        }


        private static bool IsConstant(string equation, int index, out string n, out Complex val)
        {
            var spi = Term.ExtractName(equation, index, false).ToLower();
            if (cs.Contains(spi, StringComparer.OrdinalIgnoreCase))
            {
                n = spi;
                var consts =
                    typeof (Constant).GetProperties(BindingFlags.Public |
                                                    BindingFlags.Static);
                if (
                    consts.Any(
                        x =>
                            Attribute.IsDefined(x, typeof (MathConst)) &&
                            (Attribute.GetCustomAttribute(x, typeof (MathConst)) as MathConst).Name == spi.ToLower()))
                {
                    var prop =
                        consts.First(
                            x =>
                                Attribute.IsDefined(x, typeof (MathConst)) &&
                                (Attribute.GetCustomAttribute(x, typeof (MathConst)) as MathConst).Name == spi.ToLower());
                    val = (Complex) (prop.GetValue(null, null));
                    return true;
                }
                else
                {
                    val = double.NaN;
                    return false;
                }
            }
            n = null;
            val = double.NaN;
            return false;
        }

        public static bool IsValidDigit(string equation, int index)
        {
            var n = "";
            Complex v = 0;
            return IsConstant(equation, index, out n, out v);
        }

        public override bool IsValid(string equation, int index)
        {
            return IsValidDigit(equation, index);
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