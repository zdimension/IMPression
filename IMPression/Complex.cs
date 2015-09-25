using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Funcs = IMPression.MathFunctions;

namespace IMPression
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Complex : IFormattable, IEquatable<Complex>
    {
        private bool _ind;

        public Complex(Complex b, ResultView view = ResultView.Auto) : this(b.Real, b.Imaginary, view, b._ind)
        {
        }

        public Complex(double real, double imaginary, ResultView view = ResultView.Auto, bool ind = false, string unit = "")
        {
            Real = real;
            Imaginary = imaginary;
            ViewMode = view;
            _ind = ind;
            Unit = unit;
        }

        public static Complex FromPolarCoordinates(Complex magnitude, Complex phase)
        {
            return new Complex(magnitude * Funcs.Cos(phase), magnitude * Funcs.Sin(phase));
        }

        public static readonly Complex Zero = new Complex(0.0, 0.0);
        public static readonly Complex One = new Complex(1.0, 0.0);
        public static readonly Complex ImaginaryOne = new Complex(0.0, 1.0);
        public static readonly Complex PositiveInfinity = new Complex(double.PositiveInfinity, double.PositiveInfinity);
        public static readonly Complex NegativeInfinity = new Complex(double.NegativeInfinity, double.NegativeInfinity);
        public static readonly Complex NaN = new Complex(double.NaN, double.NaN);
        public static readonly Complex Indeterminate = new Complex(0, 0, 0, true);
        public string Unit { get; }

        public double Real { get; }

        public double Imaginary { get; }

        public ResultView ViewMode { get; set; }

        public double Argument
            => Imaginary == 0.0 && Real < 0.0 ? (double) Constant.Pi : (double) Funcs.ArcTan(Imaginary, Real);

        public double Module => Funcs.Sqrt((Real * Real) + (Imaginary * Imaginary));

        public Complex Conjugate => new Complex(Real, -Imaginary);

        public static bool operator ==(Complex complex1, Complex complex2)
        {
            return complex1.Equals(complex2);
        }

        public static bool operator !=(Complex complex1, Complex complex2)
        {
            return !complex1.Equals(complex2);
        }

        public static Complex operator +(Complex sum)
        {
            return sum;
        }

        public static Complex operator -(Complex sub)
        {
            return new Complex(-sub.Real, -sub.Imaginary);
        }

        public static Complex operator +(Complex z1, Complex z2)
        {
            return new Complex(z1.Real + z2.Real, z1.Imaginary + z2.Imaginary);
        }

        public static Complex operator -(Complex z1, Complex z2)
        {
            return new Complex(z1.Real - z2.Real, z1.Imaginary - z2.Imaginary);
        }

        public static Complex operator +(double z1, Complex z2)
        {
            return new Complex(z1 + z2.Real, z2.Imaginary);
        }

        public static Complex operator -(double z1, Complex z2)
        {
            return new Complex(z1 - z2.Real, z2.Imaginary);
        }

        public static Complex operator +(Complex z1, double z2)
        {
            return new Complex(z1.Real + z2, z1.Imaginary);
        }

        public static Complex operator -(Complex z1, double z2)
        {
            return new Complex(z1.Real - z2, z1.Imaginary);
        }


        public static Complex operator +(int z1, Complex z2)
        {
            return new Complex(z1 + z2.Real, z2.Imaginary);
        }

        public static Complex operator -(int z1, Complex z2)
        {
            return new Complex(z1 - z2.Real, z2.Imaginary);
        }

        public static Complex operator +(Complex z1, int z2)
        {
            return new Complex(z1.Real + z2, z1.Imaginary);
        }

        public static Complex operator -(Complex z1, int z2)
        {
            return new Complex(z1.Real - z2, z1.Imaginary);
        }


        public static Complex operator *(Complex z1, Complex z2)
        {
            return new Complex(
                (z1.Real * z2.Real) - (z1.Imaginary * z2.Imaginary),
                (z1.Real * z2.Imaginary) + (z1.Imaginary * z2.Real));
        }

        public static Complex operator *(double z1, Complex z2)
        {
            return new Complex(z2.Real * z1, z2.Imaginary * z1);
        }

        public static Complex operator *(Complex z2, double z1)
        {
            return new Complex(z2.Real * z1, z2.Imaginary * z1);
        }

        public static Complex operator *(int z1, Complex z2)
        {
            return new Complex(z2.Real * (double) z1, z2.Imaginary * (double) z1);
        }

        public static Complex operator *(Complex z2, int z1)
        {
            return new Complex(z2.Real * (double) z1, z2.Imaginary * (double) z1);
        }

        public static Complex operator /(Complex dividend, Complex divisor)
        {
            if (dividend.IsZero() && divisor.IsZero())
            {
                return NaN;
            }

            if (divisor.IsZero())
            {
                return Indeterminate;
            }

            var modSquared = divisor.MagnitudeSquared();
            return new Complex(
                ((dividend.Real * divisor.Real) + (dividend.Imaginary * divisor.Imaginary)) / modSquared,
                ((dividend.Imaginary * divisor.Real) - (dividend.Real * divisor.Imaginary)) / modSquared);
        }

        public static Complex operator /(double dividend, Complex divisor)
        {
            if (dividend == 0.0d && divisor.IsZero())
            {
                return NaN;
            }

            if (divisor.IsZero())
            {
                return Indeterminate;
            }

            var zmod = divisor.MagnitudeSquared();
            return new Complex(dividend * divisor.Real / zmod, -dividend * divisor.Imaginary / zmod);
        }

        public static Complex operator ^(Complex a, Complex b) => Funcs.Pow(a, b);
        public static Complex operator ^(Complex a, double b) => Funcs.Pow(a, b);
        public static Complex operator ^(double a, Complex b) => Funcs.Pow(a, b);

        public static Complex operator /(Complex dividend, double divisor)
        {
            return divisor == 0.0d
                ? (dividend.IsZero() ? NaN : Indeterminate)
                : new Complex(dividend.Real / divisor, dividend.Imaginary / divisor);
        }

        public Complex Square()
        {
            return IsReal()
                ? new Complex(Real * Real, 0.0)
                : new Complex((Real * Real) - (Imaginary * Imaginary), 2.0 * Real * Imaginary);
        }

        public static Complex operator /(int dividend, Complex divisor)
        {
            if (dividend == 0 && divisor.IsZero())
            {
                return NaN;
            }

            if (divisor.IsZero())
            {
                return PositiveInfinity;
            }

            var zmod = divisor.MagnitudeSquared();
            return new Complex((double) dividend * divisor.Real / zmod, (double) -dividend * divisor.Imaginary / zmod);
        }

        public static Complex operator /(Complex dividend, int divisor)
        {
            return divisor == 0.0d
                ? (dividend.IsZero() ? NaN : Indeterminate)
                : new Complex(dividend.Real / (double) divisor, dividend.Imaginary / (double) divisor);
        }

        public double MagnitudeSquared() => (Real * Real) + (Imaginary * Imaginary);

        public bool IsZero()
        {
            return Real == 0.0 && Imaginary == 0.0;
        }

        public bool IsOne()
        {
            return Real == 1.0 && Imaginary == 0.0;
        }

        public bool IsImaginaryOne()
        {
            return Real == 0.0 && Imaginary == 1.0;
        }

        public bool IsNaN()
        {
            return double.IsNaN(Real) || double.IsNaN(Imaginary);
        }

        public bool IsInfinity()
        {
            return double.IsInfinity(Real) || double.IsInfinity(Imaginary);
        }

        public bool IsReal()
        {
            return Math.Abs(Imaginary) <= 0.0000000000001;
        }

        public bool IsImaginary()
        {
            return Math.Abs(Imaginary) > 0.0000000000001;
        }

        public bool IsRealNonNegative()
        {
            return Imaginary == 0.0 && Real >= 0;
        }

        public bool IsIndeterminate() => _ind;
        public static bool IsIndeterminate(Complex n) => n.IsIndeterminate();


        public static bool IsZero(Complex c)
        {
            return c.Real == 0.0 && c.Imaginary == 0.0;
        }

        public static bool IsOne(Complex c)
        {
            return c.Real == 1.0 && c.Imaginary == 0.0;
        }

        public static bool IsImaginaryOne(Complex c)
        {
            return c.Real == 0.0 && c.Imaginary == 1.0;
        }

        public static bool IsNaN(Complex c)
        {
            return double.IsNaN(c.Real) || double.IsNaN(c.Imaginary);
        }

        public static bool IsInfinity(Complex c)
        {
            return double.IsInfinity(c.Real) || double.IsInfinity(c.Imaginary);
        }

        public static bool IsReal(Complex c)
        {
            return c.IsReal();
        }

        public static bool IsImaginary(Complex c)
        {
            return c.IsImaginary();
        }

        public static bool IsRealNonNegative(Complex c)
        {
            return c.Imaginary == 0.0 && c.Real >= 0;
        }

        #region IFormattable

        internal static string int_Format(double d, string format, IFormatProvider prov, ResultView mode)
        {
            if (mode == ResultView.Fraction)
            {
                return Fraction.DoubleToFraction(d);
            }
            else if (mode == ResultView.PrimeFactor)
            {
                var fac = new List<long>();
                long num = (int) Funcs.Truncate(d);
                double surp = Funcs.Round(d - num, 15);
                if (num > 0)
                {
                    while (num % 2 == 0)
                    {
                        fac.Add(2);
                        num /= 2;
                    }
                    long factor = 3;
                    while (factor * factor <= num)
                    {
                        if (num % factor == 0)
                        {
                            fac.Add(factor);
                            num /= factor;
                        }
                        else
                        {
                            factor += 2;
                        }
                    }
                }
                if (num > 1) fac.Add(num);

                var pwl = fac.Distinct();
                var pwlt = new List<string>();
                pwl.All(x =>
                {
                    var ct = fac.LongCount(y => y == x);
                    pwlt.Add(x + (ct == 1 ? "" : EquationParser.Superscript(ct.ToString())));
                    return true;
                });

                return string.Join("*", pwlt) + (surp != 0 ? (num > 0 ? "+" : "") + surp : "");
            }
            else if (mode == ResultView.Scientific)
            {
                string res = d.ToString("0.0##E+0");
                res = res.Replace("E", "*10");
                string res1 = res.Substring(0, res.IndexOf("*10") + 3) +
                              EquationParser.Superscript((d < 1 ? "-" : "") + res.Substring(res.IndexOf("*10") + 4));
                return res1;
            }
            else if (mode == ResultView.Hexadecimal)
            {
                return "0x" + Convert.ToString((int) Funcs.Floor(d), 16) +
                       (Funcs.Frac(d) != 0.0 ? " + " + Funcs.Frac(d) : "");
            }
            else if (mode == ResultView.Binary)
            {
                return "0b" + Convert.ToString((int) Funcs.Floor(d), 2) +
                       (Funcs.Frac(d) != 0.0 ? " + " + Funcs.Frac(d) : "");
            }
            else if (mode == ResultView.Octal)
            {
                return "0o" + Convert.ToString((int) Funcs.Floor(d), 8) +
                       (Funcs.Frac(d) != 0.0 ? " + " + Funcs.Frac(d) : "");
            }
            else
            {
                return d.ToString(format, prov);
            }
        }

        public override string ToString()
        {
            return ToString(null, null);
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(IFormatProvider format)
        {
            return ToString(null, format);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (_ind) return "Indéterminé";

            if (IsZero()) return "0";
            if (IsImaginaryOne()) return "i";

            if (IsReal())
                return (ViewMode == ResultView.Auto)
                    ? Constant.GetConstantName(Real, format, formatProvider)
                    : int_Format(Real, format, formatProvider, ViewMode);


            string realr = "";
            string imagr = "";
            if (Funcs.Round(Real, 13) != 0)
                realr += (ViewMode == ResultView.Auto)
                    ? Constant.GetConstantName(Real, format, formatProvider, Imaginary != 0 ? 5 : -1)
                    : int_Format((Imaginary != 0 ? (double) Funcs.Round(Real, 5) : Real), format, formatProvider,
                        ViewMode);
            if (IsImaginary())
            {
                if (Funcs.Abs(Imaginary) == 1.0)
                {
                    imagr += "i";
                }
                else
                {
                    imagr += (ViewMode == ResultView.Auto)
                        ? Constant.GetConstantName(Imaginary > 0 ? Imaginary : -Imaginary, format, formatProvider,
                            Real != 0 ? 5 : -1) +
                          (Regex.IsMatch(
                              Constant.GetConstantName(Imaginary > 0 ? Imaginary : -Imaginary, format, formatProvider,
                                  Real != 0 ? 5 : -1), @"^[a-zA-Z]+$")
                              ? " * "
                              : "") + "i"
                        : int_Format(
                            (Real != 0
                                ? (double) Funcs.Round(Imaginary > 0 ? Imaginary : -Imaginary, 5)
                                : (Imaginary > 0 ? Imaginary : -Imaginary)), format, formatProvider, ViewMode);
                }

                if (Imaginary > 0) realr += " + ";
                else realr += " - ";
            }
            if (realr == " + ") realr = "";
            if (realr == " - ") realr = "-";
            return realr + imagr + (Unit != "" ? $" {Unit}" : "");
        }

        #endregion

        #region IEquatable<Complex>

        public bool Equals(Complex other)
        {
            if (IsNaN() || other.IsNaN())
            {
                return false;
            }

            if (IsInfinity() && other.IsInfinity())
            {
                return true;
            }

            return Funcs.Abs(Real - other.Real) < (10 * Funcs.Pow(2, -52)) &&
                   Funcs.Abs(Imaginary - other.Imaginary) < (10 * Funcs.Pow(2, -52));
        }

        public override int GetHashCode()
        {
            int hash = 27;
            hash = (13 * hash) + Real.GetHashCode();
            hash = (13 * hash) + Imaginary.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            return (obj is Complex) && Equals((Complex) obj);
        }

        #endregion

        #region Conversion

        public static explicit operator Complex(decimal value)
        {
            return new Complex((double) value, 0.0);
        }

        public static implicit operator Complex(byte value)
        {
            return new Complex(value, 0.0);
        }

        public static implicit operator Complex(short value)
        {
            return new Complex(value, 0.0);
        }

        public static implicit operator Complex(int value)
        {
            return new Complex(value, 0.0);
        }

        public static implicit operator Complex(double value)
        {
            return new Complex(value, 0.0);
        }

        public static implicit operator double(Complex value)
        {
            return value.Real;
        }

        /*public static implicit operator int(Complex value)
        {
            return (int) (value.Real);
        }*/

        #endregion

        public static Complex Reciprocal(Complex value)
        {
            if (value.IsZero())
            {
                return Zero;
            }

            return 1.0 / value;
        }
    }

    public static class ComplexExtensions
    {
        /// <summary>
        /// Calculer la somme de tous les nombres spécifiés
        /// </summary>
        /// <param name="ar">Liste de nombres</param>
        /// <returns>Somme des nombres</returns>
        public static Complex Sum(this IEnumerable<Complex> ar)
        {
            return new Complex(ar.Sum(x => x.Real), ar.Sum(y => y.Imaginary));
        }
    }

    public enum ResultView
    {
        Auto = 0,
        Fraction = 1, // 186438/386
        Decimal = 2, // 483
        PrimeFactor = 3, // 3*7*23
        Scientific = 4, // 4.83*10²
        Hexadecimal = 5, // 0x1E3
        Binary = 6, // 0b111100011
        Octal = 7, // 0o743
        ConstUnit = 8, // 25 cm
    }
}