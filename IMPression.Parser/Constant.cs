using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IMPression.Parser
{
    public class Constant : EquationValue
    {
        public static List<string> cs
        {
            get
            {
                return typeof (Constants).GetProperties(BindingFlags.Public |
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
                Value = v;
                m_sb.Clear();
                m_sb.Append(n);
            }
        }
        private static bool IsConstant(string equation, int index, out string n, out Complex val)
        {
            var spi = Term.ExtractName(equation, index, false).ToLower();
            if (cs.Contains(spi, StringComparer.OrdinalIgnoreCase))
            {
                n = spi;
                var consts =
                    typeof (Constants).GetProperties(BindingFlags.Public |
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
}