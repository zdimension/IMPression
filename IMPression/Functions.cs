using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMPression
{
    public class Functions
    {
        #region Fonctions Math.*

        public static Complex Truncate(Complex c)
        {
            return new Complex(Math.Truncate(c.Real), Math.Truncate(c.Imaginary));
        }

        public static Complex Floor(Complex c)
        {
            return new Complex(Math.Floor(c.Real), Math.Floor(c.Imaginary));
        }

        public static Complex Ceil(Complex c)
        {
            return new Complex(Math.Ceiling(c.Real), Math.Ceiling(c.Imaginary));
        }

        public static Complex Exp(Complex value)
        {
            var exp = Math.Exp(value.Real);
            return new Complex(exp * Math.Cos(value.Imaginary), exp * Math.Sin(value.Imaginary));
        }

        public static Complex Log10(Complex c)
        {
            return Log(c, 10.0);
        }

        public static Complex Log(Complex c, Complex d)
        {
            //if (d == Constant.E) return Ln(c);
            return Ln(c) / Ln(d);
        }

        public static Complex Sqrt(Complex value)
        {
            if (value.IsRealNonNegative())
            {
                return new Complex((double)Math.Sqrt(value.Real), 0.0);
            }

            Complex result;

            var absReal = Abs(value.Real);
            var absImag = Abs(value.Imaginary);
            double w;
            if (absReal >= absImag)
            {
                var ratio = value.Imaginary / value.Real;
                w = Sqrt(absReal) * Sqrt(0.5 * (1.0 + Sqrt(1.0 + (ratio * ratio))));
            }
            else
            {
                var ratio = value.Real / value.Imaginary;
                w = Sqrt(absImag) * Sqrt(0.5 * (Abs(ratio) + Sqrt(1.0 + (ratio * ratio))));
            }

            if (value.Real >= 0.0)
            {
                result = new Complex(w, value.Imaginary / (2.0 * w));
            }
            else if (value.Imaginary >= 0.0)
            {
                result = new Complex((absImag / (2.0 * w)).Real, w);
            }
            else
            {
                result = new Complex((absImag / (2.0 * w)).Real, -w);
            }

            return result;
        }

        public static Complex ArcSin(Complex value)
        {
            if (value.IsReal()) return Math.Asin(value.Real);

            if (value.IsImaginary() || value.Imaginary == 0d && value.Real < 0)
            {
                return -ArcSin(-value);
            }

            return -Complex.ImaginaryOne * Sqrt(Ln((1 - value.Square())) + (Complex.ImaginaryOne * value));
        }

        public static Complex ArcCos(Complex value)
        {
            if (value.IsReal()) return Math.Acos(value.Real);

            /*if (value.Imaginary < 0 || value.Imaginary == 0d && value.Real > 0)
			{
				return Constant.Pi - Acos(-value);
			}
			*/
            return -Complex.ImaginaryOne * Ln(value + Complex.ImaginaryOne * Sqrt(1 - value.Square()));

            //return (Constant.Pi / 2.0) + Constant.I * Ln(Constant.I * value + Sqrt(1 - value.Square()));
        }

        public static Complex ArcTan(Complex value)
        {
            if (value.IsReal()) return Math.Atan(value.Real);

            var iz = new Complex(-value.Imaginary, value.Real);
            return new Complex(0, 0.5) * (Ln(1 - iz) - Ln(1 + iz));
        }

        public static Complex ArcTan(Complex x, Complex y)
        {
            return 2.0 * ArcTan(y / (Sqrt(x.Square() + y.Square()) + x));
        }

        #endregion

        #region Fonctions polylogarithmiques

        public static Complex PolyLog(Complex v, Complex z)
        {
            return z.Module >= 1.0 ? Complex.Indeterminate : SumInf(k => Pow(z, k) / Pow(k, v), 1);
        }

        public static Complex PolyLog(Complex v, Complex p, Complex z)
        {
            if (z.Module >= 1) return Complex.Indeterminate;
            if ((p.Real % 1) != 0) return Complex.Indeterminate;
            if (p <= 0) return Complex.Indeterminate;
            return p.IsReal() ? SumInf(k => (Pow(-1, k + p) * StirlingS1(k, p) * Pow(z, k)) / (Pow(k, v) * Fact(k)), 1) : Complex.Indeterminate;
        }

        public static Complex RiemannSiegelTheta(Complex z)
        {
            return (-(Ln(Constants.Pi) / 2.0)) * z -
                   (Constants.I / 2.0) *
                   (LogGamma(0.25 + (Constants.I * z) / 2.0) - LogGamma(0.25 - (Constants.I * z) / 2.0));
        }

        public static Complex RiemannSiegelZ(Complex z)
        {
            return Exp(Constants.I * RiemannSiegelTheta(z)) * Zeta(Constants.I * z + 0.5);
        }

        #endregion

        #region Fonctions de calcul d'aire

        public static Complex AreaTriangle(Complex a, Complex b, Complex c)
        {
            var p = (a + b + c) / 2.0;
            return Sqrt(p * (p - a) * (p - b) * (p - c));
        }

        public static Complex AreaTriangle(Complex _base, Complex height)
        {
            return (_base / 2.0) * height;
        }

        public static Complex AreaSquare(Complex cote)
        {
            return cote.Square();
        }

        public static Complex AreaRectangle(Complex cote1, Complex cote2)
        {
            return cote1 * cote2;
        }

        public static Complex AreaTrapzeoid(Complex a, Complex b, Complex height)
        {
            return (a + b) / 2.0 * height;
        }

        public static Complex AreaCircle(Complex radius)
        {
            return Constants.Pi * radius.Square();
        }

        public static Complex AreaEllipse(Complex r1, Complex r2)
        {
            return Constants.Pi * r1 * r2;
        }

        public static Complex AreaParallelogram(Complex _base, Complex height)
        {
            return _base * height;
        }

        public static Complex AreaSector(Complex radius, Complex angle)
        {
            return (radius.Square() / 2.0) * Angle(angle);
        }

        public static Complex AreaPolygon(Complex sides, Complex length)
        {
            return (sides * length.Square()) / (4 * Tan(Constants.Pi / sides));
        }

        public static Complex AreaSphere(Complex radius)
        {
            return 4 * Constants.Pi * radius.Square();
        }

        public static Complex AreaCube(Complex side)
        {
            return 6.0 * side.Square();
        }

        public static Complex AreaParallelepiped(Complex a, Complex b, Complex c)
        {
            return 2 * a * b + 2 * b * c + 2 * a * c;
        }

        public static Complex AreaCylinder(Complex radius, Complex height)
        {
            return 2 * Constants.Pi * radius.Square() + 2 * Constants.Pi * radius * height;
        }

        #endregion

        #region Fonctions de calcul de volume

        public static Complex VolumePyramid(Complex sides, Complex length, Complex height)
        {
            return 1.0 / 3.0 * AreaPolygon(sides, length) * height;
        }

        public static Complex VolumeCube(Complex side)
        {
            return Pow(side, 3);
        }

        public static Complex VolumeSphere(Complex radius)
        {
            return (4 / 3 * Constants.Pi) * Pow(radius, 3);
        }

        public static Complex VolumeParallelepiped(Complex a, Complex b, Complex c)
        {
            return a * b * c;
        }

        public static Complex VolumeCylinder(Complex radius, Complex height)
        {
            return AreaCircle(radius) * height;
        }

        public static Complex VolumeEllipse(Complex r1, Complex r2, Complex r3)
        {
            return (4 / 3 * Constants.Pi) * r1 * r2 * r3;
        }

        #endregion

        #region Fonctions trigonométriques

        public static Complex Exsec(Complex a)
        {
            return Sec(a) - 1;
        }

        public static Complex Excsc(Complex a)
        {
            return Csc(a) - 1;
        }

        public static Complex Chord(Complex a)
        {
            return 2 * Sin(a / 2);
        }

        public static Complex Sec(Complex a)
        {
            return 1 / Cos(Angle(a));
        }

        public static Complex Csc(Complex a)
        {
            return 1 / Math.Sin(Angle(a));
        }

        public static Complex Cot(Complex a)
        {
            return 1 / Math.Tan(Angle(a));
        }

        public static Complex Sech(Complex a)
        {
            return 1 / Math.Cosh(Angle(a));
        }

        public static Complex Csch(Complex a)
        {
            return 1 / Math.Sinh(Angle(a));
        }

        public static Complex Coth(Complex a)
        {
            return 1 / Math.Tanh(Angle(a));
        }

        public static Complex ArcSec(Complex a)
        {
            return (UseDegrees ? RadToDeg(ArcCos(1 / a)) : ArcCos((1 / a)));
        }

        public static Complex ArcCsc(Complex a)
        {
            return (UseDegrees ? RadToDeg(ArcSin(1 / a)) : ArcSin((1 / a)));
        }

        public static Complex ArcCot(Complex a)
        {
            return (UseDegrees ? RadToDeg(Constants.Pi / 2 - ArcTan(a)) : Constants.Pi / 2 - ArcTan(a));
        }

        public static Complex ArcSinh(Complex a)
        {
            return (UseDegrees ? RadToDeg(Ln(a + Sqrt(a * a + 1.0))) : Ln(a + Sqrt(a * a + 1.0)));
        }

        public static Complex ArcCosh(Complex a)
        {
            return (UseDegrees
                ? RadToDeg(Ln(a + Sqrt(a * Sqrt(a - 1))))
                : Ln(a + Sqrt(a * Sqrt(a - 1))));
        }

        public static Complex ArcTanh(Complex a)
        {
            return (UseDegrees
                ? RadToDeg(Ln(1 / a + Sqrt(1 / a + 1) * Math.Sqrt(1 / a - 1)))
                : Ln(1 / a + Math.Sqrt(1 / a + 1) * Math.Sqrt(1 / a - 1)));
        }

        public static Complex ArcCoth(Complex a)
        {
            return (UseDegrees ? RadToDeg(0.5 * Ln((a + 1) / (a - 1))) : 0.5 * Ln((a + 1) / (a - 1)));
        }

        public static Complex ArcCsch(Complex a)
        {
            return (UseDegrees
                ? RadToDeg(Ln(1 / a + Math.Sqrt(1 / (a * a + 1))))
                : Ln(1 / a + Math.Sqrt(1 / (a * a + 1))));
        }

        public static Complex ArcSech(Complex a)
        {
            return (UseDegrees
                ? RadToDeg(Ln(1 / a + Math.Sqrt(1 / a + 1) * Math.Sqrt(1 / a - 1)))
                : Ln(1 / a + Math.Sqrt(1 / a + 1) * Math.Sqrt(1 / a - 1)));
        }

        public static Complex Sinc(Complex a)
        {
            return Math.Sin(Angle(a)) / (Angle(a));
        }

        public static Complex Cos(Complex a)
        {
            return (Exp(Angle(a) * Constants.I) + Exp(Angle(-a) * Constants.I)) / 2.0;
        }

        public static Complex Sin(Complex a)
        {
            return (Exp(Angle(a) * Constants.I) - Exp(Angle(-a) * Constants.I)) / (2.0 * Constants.I);
        }

        public static Complex Tan(Complex a)
        {
            if (Angle(Abs(a)) == Constants.Pi / 2.0) return Complex.Indeterminate;
            return Sin(a) / Cos(a);
        }

        public static Complex Cosh(Complex a)
        {
            return (Exp(Angle(a)) + Exp(-a)) / 2.0;
        }

        public static Complex Sinh(Complex a)
        {
            return (Exp(Angle(a)) - Exp(-a)) / 2.0;
        }

        public static Complex Tanh(Complex a)
        {
            return (Exp(Angle(a)) - Exp(-a)) / (Exp(Angle(a)) + Exp(-a));
        }

        public static Complex Versin(Complex a)
        {
            return 1 - Cos(a);
        }

        public static Complex Vercos(Complex a)
        {
            return 1 + Cos(a);
        }

        public static Complex Coversin(Complex a)
        {
            return 1 - Sin(a);
        }

        public static Complex Covercos(Complex a)
        {
            return 1 + Sin(a);
        }

        public static Complex Haversin(Complex a)
        {
            return Versin(a) / 2;
        }

        public static Complex Havercos(Complex a)
        {
            return Vercos(a) / 2;
        }

        public static Complex Hacoversin(Complex a)
        {
            return Coversin(a) / 2;
        }

        public static Complex Hacovercos(Complex a)
        {
            return Covercos(a) / 2;
        }

        #endregion

        #region Fonctions complexes

        public static Complex Re(Complex a)
        {
            return a.Real;
        }

        public static Complex Im(Complex a)
        {
            return a.Imaginary;
        }

        public static Complex Arg(Complex a)
        {
            return a.Argument;
        }

        public static Complex Conjugate(Complex a)
        {
            return a.Conjugate;
        }

        #endregion

        #region Fonctions entières

        public static Complex Nd(Complex value)
        {
            return new Complex(value.Real, value.Imaginary, ResultView.Decimal);
        }

        public static Complex Nf(Complex value)
        {
            return new Complex(value.Real, value.Imaginary, ResultView.Fraction);
        }

        public static Complex Np(Complex value)
        {
            return new Complex(value.Real, value.Imaginary, ResultView.PrimeFactor);
        }

        public static Complex Ns(Complex value)
        {
            return new Complex(value.Real, value.Imaginary, ResultView.Scientific);
        }

        public static Complex Abs(Complex value)
        {
            return value.IsReal() ? Quad.Abs(value.Real) : value.Module;
        }

        public static Complex Ln(Complex c)
        {
            /*if (c.IsReal() && c < 0.0) return Ln(-c) + (Constants.I * Constants.Pi);
            if (c.IsReal()) return Quad.Log(c.Real);

            return Math.Log(c.Module, Constants.E) + (Constants.I * (Complex)c.Argument);*/

            if(c.IsRealNonNegative()) return new Complex(Math.Log(c.Real, Constants.E), 0.0);
            return new Complex(0.5 * Math.Log(c.MagnitudeSquared(), Constants.E), c.Argument);
        }

        public static Complex Rand()
        {
            return _rnd.NextDouble();
        }

        public static Complex RandN(Complex a, Complex b)
        {
            if (b <= a)
            {
                var c = a;
                a = b;
                b = c;
            }
            return _rnd.Next((int)a, (int)b);
        }

        public static Complex Rand(Complex a, Complex b)
        {
            if (b <= a)
            {
                var c = a;
                a = b;
                b = c;
            }
            return _rnd.NextDouble() * (b - a) + a;
        }

        public static Complex Pow(Complex value, Complex power)
        {
            if (value.IsZero())
            {
                if (power.IsZero())
                {
                    return Complex.One;
                }

                if (power.Real > 0.0)
                {
                    return Complex.Zero;
                }

                if (power.Real < 0)
                {
                    return power.Imaginary == 0.0 ? new Complex(double.PositiveInfinity, 0.0) : new Complex(double.PositiveInfinity, double.PositiveInfinity);
                }

                return double.NaN;
            }
            /*if(value.IsReal() && value < 0.0)
			{
				return Math.Pow(value, power);
			}*/
            return Exp(power * Ln(value));
        }

        public static Complex Eq(Complex a, Complex b)
        {
            return a == b ? 1 : 0;
        }

        public static Complex Fact(Complex a)
        {
            return Gamma(a + 1);
        }

        public static Complex GCD(Complex a, Complex b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        public static Complex GCD(params Complex[] a)
        {
            return a.Aggregate(GCD);
        }

        public static Complex LCM(Complex a, Complex b)
        {
            return (a * b) / GCD(a, b);
        }

        public static Complex LCM(params Complex[] a)
        {
            return a.Aggregate(LCM);
        }

        public static Complex Fibo(Complex n)
        {
            /*if (n == 0) return 0;
			if (n == 1) return 1;
			return Fibo(n - 1) + Fibo(n - 2);*/
            Complex a = 0;
            Complex b = 1;
            for (int i = (int)(double)n; i >= 0; i--)
            {
                Complex d = a * (b * 2 - a);
                Complex e = a * a + b * b;
                a = d;
                b = e;
                if ((((uint)(double)n >> i) & 1) != 0)
                {
                    Complex c = a + b;
                    a = b;
                    b = c;
                }
            }
            return a;
        }

        public static Complex Fact2(Complex a)
        {
            return Math.Pow(2 / Constants.Pi, (0.25 * (1 - Math.Cos(Constants.Pi * a)))) * Math.Pow(2, (a / 2)) *
                   Gamma(a / 2 + 1);
        }

        public static Complex Prime(Complex a)
        {
            var n = (int)Math.Floor(a);

            var arr = new List<int> { 2, 3 };

            var co = 4;

            while (arr.Count() < n)
            {
                if (co % 2 != 0 && co % 3 != 0)
                {
                    var temp = 4;
                    while (temp * temp <= co)
                    {
                        if (co % temp == 0)
                            break;
                        temp++;
                    }
                    if (temp * temp > co)
                        arr.Add(co);
                }
                co++;
            }
            return n == 0 ? 2 : arr[n - 1];
        }

        public static Complex DivisorSigma(Complex k, Complex a)
        {
            if (a < 0) a = -a;

            var n = (int)Math.Floor(a);

            var factors = new List<int>();
            var max = (int)Math.Sqrt(n);
            for (var factor = 1; factor <= max; ++factor)
            {
                if (n % factor == 0)
                {
                    factors.Add(factor);
                    if (factor != n / factor)
                    {
                        factors.Add(n / factor);
                    }
                }
            }
            var sum = 0;
            factors.All(x =>
            {
                if (x == 1) sum++;
                else sum += (int)Pow(x, k);
                return true;
            });
            return sum;
        }

        public static Complex EulerPhi(Complex n)
        {
            return Sum(k => KroneckerDelta(GCD(n, k)), 1, n);
        }


        public static Complex Round(Complex a, Complex b)
        {
            return new Complex(Math.Round(a.Real, (int)b), Math.Round(a.Imaginary, (int)b), a.ViewMode, a.IsIndeterminate());
        }

        public static Complex Round(Complex a)
        {
            if (!a.IsReal()) return new Complex(Round(a.Real), Round(a.Imaginary), a.ViewMode, a.IsIndeterminate());
            return Abs(Frac(a)) < 0.5 ? Floor(a) : Ceil(a);
        }

        public static Complex CubeRoot(Complex a)
        {
            return Pow(a, Complex.One / 3.0);
        }

        public static Complex NthRoot(Complex a, Complex n)
        {
            return Pow(a, Complex.One / n);
        }

        public static Complex Max(params Complex[] a)
        {
            return a.Max();
        }

        public static Complex Min(params Complex[] a)
        {
            return a.Min();
        }

        public static Complex Sign(Complex a)
        {
            if (a < 0) return -1;
            if (a == 0) return 0;
            return a > 0 ? 1 : 0;
        }

        public static Complex Frac(Complex a)
        {
            return a - Math.Truncate(a);
        }

        public static Complex Quotient(Complex a, Complex b)
        {
            return Math.Floor(a / b);
        }

        public static Complex KroneckerDelta(params Complex[] a)
        {
            return a.All(x => x == a[0]) ? 1 : 0;
        }

        public static Complex KroneckerDelta(Complex a)
        {
            return a == 0 ? 1 : 0;
        }

        public static Complex DiscreteDelta(Complex a)
        {
            return a == 0 ? 1 : 0;
        }

        public static Complex DiscreteDelta(params Complex[] a)
        {
            return a.All(x => x == 0) ? 1 : 0;
        }

        public static Complex EulerE(Complex n)
        {
            if (n % 2 != 0) return 0;
            if (n == 0) return 1;
            var first = new List<double>
            {
                -1.0,
                5.0,
                -61.0,
                1385.0,
                -50521.0,
                2702765.0,
                -199360981.0,
                19391512145.0,
                -2404879675441.0,
                370371188237525.0,
                -69348874393137901.0,
                15514534163557086905.0
            };
            return n * 2 <= first.Count ? first[(int)(n / 2) - 1] : double.NaN;
        }

        public static Complex BernoulliB(Complex n)
        {
            if (n == 1) return -1.0 / 2.0;
            if (n % 2 != 0) return 0;
            if (n == 0) return 1;
            var first = new List<double>
            {
                1.0 / 6.0,
                -1.0 / 30.0,
                1.0 / 42.0,
                -1.0 / 30.0,
                5.0 / 66.0,
                -691.0 / 2730.0,
                7.0 / 6.0,
                -3617.0 / 510.0,
                43867.0 / 798.0,
                -174611.0 / 330.0,
                854513.0 / 138.0,
                -236364091.0 / 2730.0,
                8553103.0 / 6.0,
                -23749461029.0 / 870.0,
                8615841276005.0 / 14322.0,
                -7709321041217.0 / 510.0,
                2577687858367.0 / 6.0,
                -26315271553053477373.0 / 1919190.0,
                2929993913841559.0 / 6.0,
                -261082718496449122051.0 / 13530.0
            };
            return n * 2 <= first.Count ? first[(int)(n / 2) - 1] : double.NaN;
        }

        public static Complex HeavisideTheta(Complex z)
        {
            if (z == 0.0) return Complex.Indeterminate;
            return z > 0.0 ? 1.0 : 0.0;
        }

        public static Complex HeavisideTheta(params Complex[] zz)
        {
            if (zz.Any(x => x == 0.0)) return Complex.Indeterminate;
            return zz.All(x => x > 0.0) ? 1.0 : 0.0;
        }

        public static Complex StirlingS1(Complex n, Complex k)
        {
            if (n == 0 && k == 0) return 1;
            if (n == 0 || k == 0) return 0;

            return (n - 1) * StirlingS1(n - 1, k) + StirlingS1(n - 1, k - 1);
        }

        public static Complex StirlingS2(Complex n, Complex k)
        {
            if (n == 0 && k == 0) return 1;
            if (n == 0 || k == 0) return 0;

            return k * StirlingS2(n - 1, k) + StirlingS2(n - 1, k - 1);
        }

        public static Complex BellB(Complex n)
        {
            return Sum(k => StirlingS2(n, k), 0, n);
        }

        public static Complex BellB(Complex n, Complex x)
        {
            return Sum(k => StirlingS2(n, k) * Pow(x, k), 0, n);
        }

        public static Complex PartitionsP(Complex n)
        {
            var first = new List<double>
            {
                1.0,
                1.0,
                2.0,
                3.0,
                5.0,
                7.0,
                11.0,
                15.0,
                22.0,
                30.0,
                42.0,
                56.0,
                77.0,
                101.0,
                135.0,
                176.0,
                231.0,
                297.0,
                385.0,
                490.0,
                627.0,
                792.0,
                1002.0,
                1255.0,
                1575.0,
                1958.0,
                2436.0,
                3010.0,
                3718.0,
                4565.0,
                5604.0,
                6842.0,
                8349.0,
                10143.0,
                12310.0,
                14883.0,
                17977.0,
                21637.0,
                26015.0,
                31185.0,
                37338.0,
                44583.0,
                53174.0,
                63261.0,
                75175.0,
                89134.0,
                105558.0,
                124754.0,
                147273.0,
                173525.0
            };
            //TODO: Implémenter la fonction
            return n <= first.Count ? first[(int)n - 1] : double.NaN;
        }

        public static Complex PartitionsQ(Complex n)
        {
            var first = new List<double>
            {
                1.0,
                1.0,
                1.0,
                2.0,
                2.0,
                3.0,
                4.0,
                5.0,
                6.0,
                8.0,
                10.0,
                12.0,
                15.0,
                18.0,
                22.0,
                27.0,
                32.0,
                38.0,
                46.0,
                54.0,
                64.0,
                76.0,
                89.0,
                104.0,
                122.0,
                142.0,
                165.0,
                192.0,
                222.0,
                256.0,
                296.0,
                340.0,
                390.0,
                448.0,
                512.0,
                585.0,
                668.0,
                760.0,
                864.0,
                982.0,
                1113.0,
                1260.0,
                1426.0,
                1610.0,
                1816.0,
                2048.0,
                2304.0,
                2590.0,
                2910.0,
                3264.0,
                3658.0,
                4097.0,
                4582.0,
                5120.0,
                5718.0,
                6378.0
            };
            //TODO: Implémenter la fonction
            return n <= first.Count ? first[(int)n - 1] : double.NaN;
        }

        public static Complex Average(params Complex[] a)
        {
            return a.Sum() / (double)a.Count();
        }

        public static Complex LucasL(Complex a)
        {
            return Pow(Constants.Phi, a) + Pow(Constants.Phi, -a) * Math.Cos(a * Constants.Pi);
        }

        public static Complex ProductLog(Complex z)
        {
            if (z > (1 / Constants.E - 0.1)) throw new MathException("", 0, "Le rayon de convergence est de 1 / e.");
            return SumInf(k => (Pow(-k, k - 1) / Fact(k)) * Pow(z, k), 1);
        }

        public static Complex ProductLog(Complex a, Complex z)
        {
            return SumInf(k => (Pow(-k, k - 1) / Fact(k)) * Pow(z, k), 1);
        }

        public static Complex Mediane(params Complex[] a)
        {
            switch (a.Length)
            {
                case 0:
                    return double.PositiveInfinity;
                case 1:
                    return a[0];
            }
            Array.Sort(a);
            return a.Length % 2 == 0 ? Average(a[a.Length / 2 - 1], a[a.Length / 2]) : a[(a.Length - 1) / 2];
        }

        public static Complex Q1(params Complex[] a)
        {
            if (a.Length == 0) return double.PositiveInfinity;
            if (a.Length == 1) return a[0];
            Array.Sort(a);
            return a[(int)Math.Ceiling((double)(a.Length - 1) / 4.0) - 1];
        }

        public static Complex Q3(params Complex[] a)
        {
            if (a.Length == 0) return double.PositiveInfinity;
            if (a.Length == 1) return a[0];
            Array.Sort(a);
            return a[(int)Math.Ceiling((double)(a.Length - 1) * (3.0 / 4.0)) - 1];
        }

        public static Complex Somme(params Complex[] a)
        {
            return a.Sum();
        }

        public static Complex DigitCount(Complex n, Complex b)
        {
            return n - SumInf(k => Floor(n / Pow(2.0, k)), 1);
        }

        public static Complex JacobiSymbol(Complex a, Complex n)
        {
            if (GCD(a, n) != 1.0) return 0;
            return Pow(a, (n - 1.0) / 2.0) * (Complex)n.Module;
        }

        public static Complex MobiusMu(Complex n)
        {
            if (n == 1) return 1;
            Complex sum = 0.0;
            for (double k = 1; k <= n; k++)
            {
                if (GCD(k, n) == 1.0)
                {
                    sum += Exp(2.0 * Constants.Pi * Constants.I * (k / n));
                }
            }
            return sum;
        }

        public static Complex PrimePi(Complex x)
        {
            if (!x.IsReal()) return Complex.Indeterminate;
            if (x.Real < 0) return 0.0;
            return Sum(k => UnitStep(x - Prime(k)), 1, Floor(x));
        }

        public static Complex UnitStep(Complex x)
        {
            if (!x.IsReal()) return Complex.Indeterminate;
            if (x >= 0.0) return 1.0;
            if (x < 0.0) return 0.0;
            return Complex.Indeterminate;
        }

        public static Complex UnitStep(params Complex[] z)
        {
            if (z.Any(x => x.IsImaginary())) return Complex.Indeterminate;
            if (z.Any(x => x < 0)) return 0;
            return z.All(x => x >= 0) ? 1 : 0;
        }

        public static Complex RamanujanTau(Complex n)
        {
            var gamma = 5;
            return 0.5 *
                   Integrate(z => Pow(DedekindEta(z), 24) / Exp(2 * Constants.Pi * Constants.I * n * z), Constants.I * gamma - 1,
                       Constants.I * gamma + 1);
        }

        public static Complex RamanujanTauTheta(Complex z)
        {
            return -z * Ln(2 * Constants.Pi) - 0.25 * (Zeta(0, 6 + Constants.I * z) - Zeta(0, 6 - Constants.I * z));
        }

        public static Complex DiracDelta(params Complex[] x)
        {
            return 0; // je ne comprends pas à quoi sert cette fonction, elle retourne toujours zéro, même sur WolframAlpha
                      //return (1 / Constant.Pi) * CalculateLimit(e => e / (e.Square() + x.Sum(y => y.Square())), 0);
        }

        #endregion

        #region Fonctions Beta

        public static Complex BetaLn(Complex z, Complex w)
        {
            if (z <= 0.0)
            {
                throw new MathException(z.ToString(), 0, "z doit être positif.");
            }
            if (w <= 0.0)
            {
                throw new MathException(w.ToString(), 0, "w doit être positif.");
            }
            return LogGamma(z) + LogGamma(w) - LogGamma(z + w);
        }

        public static Complex Beta(Complex z, Complex w)
        {
            return Math.Exp(BetaLn(z, w));
        }

        public static Complex Beta(Complex z1, Complex z2, Complex a, Complex b)
        {
            return Integrate(x => Pow(x, a - 1) * Pow(1 - x, b - 1), z1, z2);
        }

        public static Complex BetaIncomplete(Complex a, Complex b, Complex x)
        {
            return BetaRegularized(a, b, x) * Beta(a, b);
        }

        public static Complex BetaRegularized(Complex z1, Complex z2, Complex a, Complex b)
        {
            return Beta(z1, z2, a, b) / Beta(a, b);
        }

        public static Complex Beta(Complex z, Complex a, Complex b)
        {
            return Integrate(t => Pow(t, a - 1) * Pow(1 - t, b - 1), 0, z);
        }

        public static Complex BetaRegularized(Complex z, Complex a, Complex b)
        {
            return Beta(z, a, b) / Beta(a, b);
        }

        #endregion

        #region Fonctions Gamma

        private static double StirlingGamma(Complex x)
        {
            double[] STIR =
            {
                7.87311395793093628397E-4,
                -2.29549961613378126380E-4,
                -2.68132617805781232825E-3,
                3.47222221605458667310E-3,
                8.33333333333482257126E-2
            };
            var MAXSTIR = 143.01608;

            var w = 1.0 / x;
            var y = Math.Exp(x);

            w = 1.0 + w * PolynomialDegree(w, STIR, 4);

            if (x > MAXSTIR)
            {
                /* Avoid overflow in Math.Pow() */
                var v = Math.Pow(x, 0.5 * x - 0.25);
                y = v * (v / y);
            }
            else
            {
                y = Math.Pow(x, x - 0.5) / y;
            }
            y = 2.50662827463100050242E0 * y * w;
            return y;
        }


        public static Complex Gamma(Complex x)
        {
            double[] P =
            {
                1.60119522476751861407E-4,
                1.19135147006586384913E-3,
                1.04213797561761569935E-2,
                4.76367800457137231464E-2,
                2.07448227648435975150E-1,
                4.94214826801497100753E-1,
                9.99999999999999996796E-1
            };
            double[] Q =
            {
                -2.31581873324120129819E-5,
                5.39605580493303397842E-4,
                -4.45641913851797240494E-3,
                1.18139785222060435552E-2,
                3.58236398605498653373E-2,
                -2.34591795718243348568E-1,
                7.14304917030273074085E-2,
                1.00000000000000000320E0
            };

            double p, z;

            var q = Math.Abs(x);

            if (q > 33.0)
            {
                if (x < 0.0)
                {
                    p = Math.Floor(q);
                    if (p == q) throw new ArithmeticException("gamma: overflow");
                    //int i = (int)p;
                    z = q - p;
                    if (z > 0.5)
                    {
                        p += 1.0;
                        z = q - p;
                    }
                    z = q * Math.Sin(Math.PI * z);
                    if (z == 0.0) throw new ArithmeticException("gamma: overflow");
                    z = Math.Abs(z);
                    z = Math.PI / (z * StirlingGamma(q));

                    return -z;
                }
                else
                {
                    return StirlingGamma(x);
                }
            }

            z = 1.0;
            while (x >= 3.0)
            {
                x -= 1.0;
                z *= x;
            }

            while (x < 0.0)
            {
                if (x == 0.0)
                {
                    throw new MathException($"Gamma({x})", 0, "x doit être supérieur à 0.");
                }
                else if (x > -1.0E-9)
                {
                    return (z / ((1.0 + Constants.EulerGamma * x) * x));
                }
                z /= x;
                x += 1.0;
            }

            while (x < 2.0)
            {
                if (x == 0.0)
                {
                    throw new ArithmeticException("gamma: singular");
                }
                else if (x < 1.0E-9)
                {
                    return (z / ((1.0 + 0.5772156649015329 * x) * x));
                }
                z /= x;
                x += 1.0;
            }

            if ((x == 2.0) || (x == 3.0)) return z;

            x -= 2.0;
            p = PolynomialDegree(x, P, 6);
            q = PolynomialDegree(x, Q, 7);
            return z * p / q;
        }

        public static Complex PolyGamma(Complex z)
        {
            return SumInf(k => (1.0 / k) - (1.0 / (k + z - 1)), 1) - Constants.EulerGamma;
        }

        public static Complex LogGamma(Complex z) => Ln(Gamma(z));



        public static Complex DiGamma(Complex x)
        {
            const double c = 12.0;
            const double d1 = -0.57721566490153286;
            const double d2 = 1.6449340668482264365;
            const double s = 1e-6;
            const double s3 = 1.0 / 12.0;
            const double s4 = 1.0 / 120.0;
            const double s5 = 1.0 / 252.0;
            const double s6 = 1.0 / 240.0;
            const double s7 = 1.0 / 132.0;
            if (double.IsNegativeInfinity(x) || double.IsNaN(x))
            {
                return double.NaN;
            }
            if (x <= 0 && Math.Floor(x) == x)
            {
                return double.NegativeInfinity;
            }
            if (x < 0)
            {
                return DiGamma(1.0 - x) + (Math.PI / Math.Tan(-Math.PI * x));
            }
            if (x <= s)
            {
                return d1 - (1 / x) + (d2 * x);
            }
            double result = 0;
            while (x < c)
            {
                result -= 1 / x;
                x++;
            }
            if (x >= c)
            {
                var r = 1 / x;
                result += Math.Log(x) - (0.5 * r);
                r *= r;

                result -= r * (s3 - (r * (s4 - (r * (s5 - (r * (s6 - (r * s7))))))));
            }
            return result;
        }

        public static Complex DiGammaInv(Complex p)
        {
            if (double.IsNaN(p))
            {
                return double.NaN;
            }
            if (double.IsNegativeInfinity(p))
            {
                return 0.0;
            }
            if (double.IsPositiveInfinity(p))
            {
                return double.PositiveInfinity;
            }
            var x = Math.Exp(p);
            for (var d = 1.0; d > 1.0e-15; d /= 2.0)
            {
                x += d * Math.Sign(p - DiGamma(x));
            }
            return x;
        }

        public static Complex GammaRegularized(Complex a, Complex z)
        {
            return GammaIncomplete(a, z) / Gamma(a);
        }

        public static Complex GammaIncomplete(Complex a, Complex x)
        {
            double result = 0;
            double igammaepsilon = 0;
            double ans = 0;
            double ax = 0;
            double c = 0;
            double r = 0;
            double tmp = 0;

            igammaepsilon = 0.000000000000001;
            if ((double)(x) <= (double)(0) || (double)(a) <= (double)(0))
            {
                result = 0;
                return result;
            }
            if ((double)(x) > (double)(1) && (double)(x) > (double)(a))
            {
                result = 1 - incompletegammac(a, x);
                return result;
            }
            ax = a * Math.Log(x) - x - lngamma(a, ref tmp);
            if ((double)(ax) < (double)(-709.78271289338399))
            {
                result = 0;
                return result;
            }
            ax = Math.Exp(ax);
            r = a;
            c = 1;
            ans = 1;
            do
            {
                r = r + 1;
                c = c * x / r;
                ans = ans + c;
            } while ((double)(c / ans) > (double)(igammaepsilon));
            result = ans * ax / a;
            return result;
        }

        private static Complex lngamma(double x,
            ref double sgngam)
        {
            double result = 0;
            double p = 0;
            double q = 0;
            double z = 0;
            double logpi = 0;
            double ls2pi = 0;
            double tmp = 0;

            sgngam = 0;

            sgngam = 1;
            logpi = 1.14472988584940017414;
            ls2pi = 0.91893853320467274178;
            if ((double)(x) < (double)(-34.0))
            {
                q = -x;
                var w = lngamma(q, ref tmp);
                p = (int)Math.Floor(q);
                var i = (int)Math.Round(p);
                if (i % 2 == 0)
                {
                    sgngam = -1;
                }
                else
                {
                    sgngam = 1;
                }
                z = q - p;
                if ((double)(z) > (double)(0.5))
                {
                    p = p + 1;
                    z = p - q;
                }
                z = q * Math.Sin(Math.PI * z);
                result = logpi - Math.Log(z) - w;
                return result;
            }
            if ((double)(x) < (double)(13))
            {
                z = 1;
                p = 0;
                var u = x;
                while ((double)(u) >= (double)(3))
                {
                    p = p - 1;
                    u = x + p;
                    z = z * u;
                }
                while ((double)(u) < (double)(2))
                {
                    z = z / u;
                    p = p + 1;
                    u = x + p;
                }
                if ((double)(z) < (double)(0))
                {
                    sgngam = -1;
                    z = -z;
                }
                else
                {
                    sgngam = 1;
                }
                if ((double)(u) == (double)(2))
                {
                    result = Math.Log(z);
                    return result;
                }
                p = p - 2;
                x = x + p;
                var b = -1378.25152569120859100;
                b = -38801.6315134637840924 + x * b;
                b = -331612.992738871184744 + x * b;
                b = -1162370.97492762307383 + x * b;
                b = -1721737.00820839662146 + x * b;
                b = -853555.664245765465627 + x * b;
                double c = 1;
                c = -351.815701436523470549 + x * c;
                c = -17064.2106651881159223 + x * c;
                c = -220528.590553854454839 + x * c;
                c = -1139334.44367982507207 + x * c;
                c = -2532523.07177582951285 + x * c;
                c = -2018891.41433532773231 + x * c;
                p = x * b / c;
                result = Math.Log(z) + p;
                return result;
            }
            q = (x - 0.5) * Math.Log(x) - x + ls2pi;
            if ((double)(x) > (double)(100000000))
            {
                result = q;
                return result;
            }
            p = 1 / (x * x);
            if ((double)(x) >= (double)(1000.0))
            {
                q = q +
                    ((7.9365079365079365079365 * 0.0001 * p - 2.7777777777777777777778 * 0.001) * p +
                     0.0833333333333333333333) / x;
            }
            else
            {
                var a = 8.11614167470508450300 * 0.0001;
                a = -(5.95061904284301438324 * 0.0001) + p * a;
                a = 7.93650340457716943945 * 0.0001 + p * a;
                a = -(2.77777777730099687205 * 0.001) + p * a;
                a = 8.33333333333331927722 * 0.01 + p * a;
                q = q + a / x;
            }
            result = q;
            return result;
        }

        private static Complex incompletegammac(double a,
            double x)
        {
            double result = 0;
            double igammaepsilon = 0;
            double igammabignumber = 0;
            double igammabignumberinv = 0;
            double ans = 0;
            double ax = 0;
            double c = 0;
            double t = 0;
            double y = 0;
            double z = 0;
            double pkm1 = 0;
            double pkm2 = 0;
            double qkm1 = 0;
            double qkm2 = 0;
            double tmp = 0;

            igammaepsilon = 0.000000000000001;
            igammabignumber = 4503599627370496.0;
            igammabignumberinv = 2.22044604925031308085 * 0.0000000000000001;
            if ((double)(x) <= (double)(0) || (double)(a) <= (double)(0))
            {
                result = 1;
                return result;
            }
            if ((double)(x) < (double)(1) || (double)(x) < (double)(a))
            {
                result = 1 - GammaIncomplete(a, x);
                return result;
            }
            ax = a * Math.Log(x) - x - lngamma(a, ref tmp);
            if ((double)(ax) < (double)(-709.78271289338399))
            {
                result = 0;
                return result;
            }
            ax = Math.Exp(ax);
            y = 1 - a;
            z = x + y + 1;
            c = 0;
            pkm2 = 1;
            qkm2 = x;
            pkm1 = x + 1;
            qkm1 = z * x;
            ans = pkm1 / qkm1;
            do
            {
                c = c + 1;
                y = y + 1;
                z = z + 2;
                var yc = y * c;
                var pk = pkm1 * z - pkm2 * yc;
                var qk = qkm1 * z - qkm2 * yc;
                if ((double)(qk) != (double)(0))
                {
                    var r = pk / qk;
                    t = Math.Abs((ans - r) / r);
                    ans = r;
                }
                else
                {
                    t = 1;
                }
                pkm2 = pkm1;
                pkm1 = pk;
                qkm2 = qkm1;
                qkm1 = qk;
                if ((double)(Math.Abs(pk)) > (double)(igammabignumber))
                {
                    pkm2 = pkm2 * igammabignumberinv;
                    pkm1 = pkm1 * igammabignumberinv;
                    qkm2 = qkm2 * igammabignumberinv;
                    qkm1 = qkm1 * igammabignumberinv;
                }
            } while ((double)(t) > (double)(igammaepsilon));
            result = ans * ax;
            return result;
        }

        public static Complex Subfactorial(Complex z)
        {
            return GammaIncomplete(z + 1, -1) / Constants.E;
        }

        public static Complex CatalanNumber(Complex z)
        {
            /*if (z == 0.0) return 1.0;
			if (z == -1.0) return -1.0;
			if (z < 0.0) return 0.0;
			return ((2.0 * (2.0 * z - 1.0)) / (z + 1.0)) * CatalanNumber(z - 1.0);*/
            return Fact(2.0 * z) / (Fact(z + 1.0) * Fact(z));
        }


        public static Complex LogIntegral(Complex z)
        {
            if (z == 0.0) return 0.0;
            if (z == 1) return double.NegativeInfinity;
            return Integrate(t => 1.0 / Ln(t), 0, z);
        }

        public static Complex Gamma(Complex a, Complex z1, Complex z2)
        {
            return Integrate(t => Pow(t, a - 1) * Exp(-t), z1, z2);
        }

        public static Complex GammaRegularized(Complex a, Complex z1, Complex z2)
        {
            return Gamma(a, z1, z2) / Gamma(a);
        }

        #endregion

        #region Intégrales exponentielles

        public static Complex ExpIntegralEi(Complex x)
        {
            double result = 0;
            double eul = 0;
            double f = 0;
            double f1 = 0;
            double f2 = 0;
            double w = 0;

            eul = 0.5772156649015328606065;
            if ((double)(x) <= (double)(0))
            {
                result = 0;
                return result;
            }
            if ((double)(x) < (double)(2))
            {
                f1 = -5.350447357812542947283;
                f1 = f1 * x + 218.5049168816613393830;
                f1 = f1 * x - 4176.572384826693777058;
                f1 = f1 * x + 55411.76756393557601232;
                f1 = f1 * x - 331338.1331178144034309;
                f1 = f1 * x + 1592627.163384945414220;
                f2 = 1.000000000000000000000;
                f2 = f2 * x - 52.50547959112862969197;
                f2 = f2 * x + 1259.616186786790571525;
                f2 = f2 * x - 17565.49581973534652631;
                f2 = f2 * x + 149306.2117002725991967;
                f2 = f2 * x - 729494.9239640527645655;
                f2 = f2 * x + 1592627.163384945429726;
                f = f1 / f2;
                result = eul + Math.Log(x) + x * f;
                return result;
            }
            if ((double)(x) < (double)(4))
            {
                w = 1 / x;
                f1 = 1.981808503259689673238E-2;
                f1 = f1 * w - 1.271645625984917501326;
                f1 = f1 * w - 2.088160335681228318920;
                f1 = f1 * w + 2.755544509187936721172;
                f1 = f1 * w - 4.409507048701600257171E-1;
                f1 = f1 * w + 4.665623805935891391017E-2;
                f1 = f1 * w - 1.545042679673485262580E-3;
                f1 = f1 * w + 7.059980605299617478514E-5;
                f2 = 1.000000000000000000000;
                f2 = f2 * w + 1.476498670914921440652;
                f2 = f2 * w + 5.629177174822436244827E-1;
                f2 = f2 * w + 1.699017897879307263248E-1;
                f2 = f2 * w + 2.291647179034212017463E-2;
                f2 = f2 * w + 4.450150439728752875043E-3;
                f2 = f2 * w + 1.727439612206521482874E-4;
                f2 = f2 * w + 3.953167195549672482304E-5;
                f = f1 / f2;
                result = Math.Exp(x) * w * (1 + w * f);
                return result;
            }
            if ((double)(x) < (double)(8))
            {
                w = 1 / x;
                f1 = -1.373215375871208729803;
                f1 = f1 * w - 7.084559133740838761406E-1;
                f1 = f1 * w + 1.580806855547941010501;
                f1 = f1 * w - 2.601500427425622944234E-1;
                f1 = f1 * w + 2.994674694113713763365E-2;
                f1 = f1 * w - 1.038086040188744005513E-3;
                f1 = f1 * w + 4.371064420753005429514E-5;
                f1 = f1 * w + 2.141783679522602903795E-6;
                f2 = 1.000000000000000000000;
                f2 = f2 * w + 8.585231423622028380768E-1;
                f2 = f2 * w + 4.483285822873995129957E-1;
                f2 = f2 * w + 7.687932158124475434091E-2;
                f2 = f2 * w + 2.449868241021887685904E-2;
                f2 = f2 * w + 8.832165941927796567926E-4;
                f2 = f2 * w + 4.590952299511353531215E-4;
                f2 = f2 * w + -4.729848351866523044863E-6;
                f2 = f2 * w + 2.665195537390710170105E-6;
                f = f1 / f2;
                result = Math.Exp(x) * w * (1 + w * f);
                return result;
            }
            if ((double)(x) < (double)(16))
            {
                w = 1 / x;
                f1 = -2.106934601691916512584;
                f1 = f1 * w + 1.732733869664688041885;
                f1 = f1 * w - 2.423619178935841904839E-1;
                f1 = f1 * w + 2.322724180937565842585E-2;
                f1 = f1 * w + 2.372880440493179832059E-4;
                f1 = f1 * w - 8.343219561192552752335E-5;
                f1 = f1 * w + 1.363408795605250394881E-5;
                f1 = f1 * w - 3.655412321999253963714E-7;
                f1 = f1 * w + 1.464941733975961318456E-8;
                f1 = f1 * w + 6.176407863710360207074E-10;
                f2 = 1.000000000000000000000;
                f2 = f2 * w - 2.298062239901678075778E-1;
                f2 = f2 * w + 1.105077041474037862347E-1;
                f2 = f2 * w - 1.566542966630792353556E-2;
                f2 = f2 * w + 2.761106850817352773874E-3;
                f2 = f2 * w - 2.089148012284048449115E-4;
                f2 = f2 * w + 1.708528938807675304186E-5;
                f2 = f2 * w - 4.459311796356686423199E-7;
                f2 = f2 * w + 1.394634930353847498145E-8;
                f2 = f2 * w + 6.150865933977338354138E-10;
                f = f1 / f2;
                result = Math.Exp(x) * w * (1 + w * f);
                return result;
            }
            if ((double)(x) < (double)(32))
            {
                w = 1 / x;
                f1 = -2.458119367674020323359E-1;
                f1 = f1 * w - 1.483382253322077687183E-1;
                f1 = f1 * w + 7.248291795735551591813E-2;
                f1 = f1 * w - 1.348315687380940523823E-2;
                f1 = f1 * w + 1.342775069788636972294E-3;
                f1 = f1 * w - 7.942465637159712264564E-5;
                f1 = f1 * w + 2.644179518984235952241E-6;
                f1 = f1 * w - 4.239473659313765177195E-8;
                f2 = 1.000000000000000000000;
                f2 = f2 * w - 1.044225908443871106315E-1;
                f2 = f2 * w - 2.676453128101402655055E-1;
                f2 = f2 * w + 9.695000254621984627876E-2;
                f2 = f2 * w - 1.601745692712991078208E-2;
                f2 = f2 * w + 1.496414899205908021882E-3;
                f2 = f2 * w - 8.462452563778485013756E-5;
                f2 = f2 * w + 2.728938403476726394024E-6;
                f2 = f2 * w - 4.239462431819542051337E-8;
                f = f1 / f2;
                result = Math.Exp(x) * w * (1 + w * f);
                return result;
            }
            if ((double)(x) < (double)(64))
            {
                w = 1 / x;
                f1 = 1.212561118105456670844E-1;
                f1 = f1 * w - 5.823133179043894485122E-1;
                f1 = f1 * w + 2.348887314557016779211E-1;
                f1 = f1 * w - 3.040034318113248237280E-2;
                f1 = f1 * w + 1.510082146865190661777E-3;
                f1 = f1 * w - 2.523137095499571377122E-5;
                f2 = 1.000000000000000000000;
                f2 = f2 * w - 1.002252150365854016662;
                f2 = f2 * w + 2.928709694872224144953E-1;
                f2 = f2 * w - 3.337004338674007801307E-2;
                f2 = f2 * w + 1.560544881127388842819E-3;
                f2 = f2 * w - 2.523137093603234562648E-5;
                f = f1 / f2;
                result = Math.Exp(x) * w * (1 + w * f);
                return result;
            }
            w = 1 / x;
            f1 = -7.657847078286127362028E-1;
            f1 = f1 * w + 6.886192415566705051750E-1;
            f1 = f1 * w - 2.132598113545206124553E-1;
            f1 = f1 * w + 3.346107552384193813594E-2;
            f1 = f1 * w - 3.076541477344756050249E-3;
            f1 = f1 * w + 1.747119316454907477380E-4;
            f1 = f1 * w - 6.103711682274170530369E-6;
            f1 = f1 * w + 1.218032765428652199087E-7;
            f1 = f1 * w - 1.086076102793290233007E-9;
            f2 = 1.000000000000000000000;
            f2 = f2 * w - 1.888802868662308731041;
            f2 = f2 * w + 1.066691687211408896850;
            f2 = f2 * w - 2.751915982306380647738E-1;
            f2 = f2 * w + 3.930852688233823569726E-2;
            f2 = f2 * w - 3.414684558602365085394E-3;
            f2 = f2 * w + 1.866844370703555398195E-4;
            f2 = f2 * w - 6.345146083130515357861E-6;
            f2 = f2 * w + 1.239754287483206878024E-7;
            f2 = f2 * w - 1.086076102793126632978E-9;
            f = f1 / f2;
            result = Math.Exp(x) * w * (1 + w * f);
            return result;
        }

        public static Complex ExpIntegralE(Complex x, Complex n)
        {
            double result = 0;
            double t = 0;
            double yk = 0;
            double xk = 0;
            double pk = 0;
            double big = 0;
            double eul = 0;

            eul = 0.57721566490153286060;
            big = 1.44115188075855872 * Math.Pow(10, 17);
            if (((n < 0 || (double)(x) < (double)(0)) || (double)(x) > (double)(170)) ||
                ((double)(x) == (double)(0) && n < 2))
            {
                result = -1;
                return result;
            }
            if ((double)(x) == (double)(0))
            {
                result = (double)1 / (double)(n - 1);
                return result;
            }
            if (n == 0)
            {
                result = Math.Exp(-x) / x;
                return result;
            }
            if (n > 5000)
            {
                xk = x + n;
                yk = 1 / (xk * xk);
                t = n;
                result = yk * t * (6 * x * x - 8 * t * x + t * t);
                result = yk * (result + t * (t - 2.0 * x));
                result = yk * (result + t);
                result = (result + 1) * Math.Exp(-x) / xk;
                return result;
            }
            if ((double)(x) <= (double)(1))
            {
                var psi = -eul - Math.Log(x);
                var i = 0;
                for (i = 1; i <= n - 1; i++)
                {
                    psi = psi + (double)1 / (double)i;
                }
                var z = -x;
                xk = 0;
                yk = 1;
                pk = 1 - n;
                if (n == 1)
                {
                    result = 0.0;
                }
                else
                {
                    result = 1.0 / pk;
                }
                do
                {
                    xk = xk + 1;
                    yk = yk * z / xk;
                    pk = pk + 1;
                    if ((double)(pk) != (double)(0))
                    {
                        result = result + yk / pk;
                    }
                    t = (double)(result) != (double)(0) ? Math.Abs(yk / result) : 1;
                } while ((double)(t) >= double.Epsilon);
                t = 1;
                for (i = 1; i <= n - 1; i++)
                {
                    t = t * z / i;
                }
                result = psi * t - result;
                return result;
            }
            else
            {
                var k = 1;
                double pkm2 = 1;
                var qkm2 = x;
                var pkm1 = 1.0;
                var qkm1 = x + n;
                result = pkm1 / qkm1;
                do
                {
                    k = k + 1;
                    if (k % 2 == 1)
                    {
                        yk = 1;
                        xk = n + (double)(k - 1) / (double)2;
                    }
                    else
                    {
                        yk = x;
                        xk = (double)k / (double)2;
                    }
                    pk = pkm1 * yk + pkm2 * xk;
                    var qk = qkm1 * yk + qkm2 * xk;
                    if ((double)(qk) != (double)(0))
                    {
                        var r = pk / qk;
                        t = Math.Abs((result - r) / r);
                        result = r;
                    }
                    else
                    {
                        t = 1;
                    }
                    pkm2 = pkm1;
                    pkm1 = pk;
                    qkm2 = qkm1;
                    qkm1 = qk;
                    if ((double)(Math.Abs(pk)) > (double)(big))
                    {
                        pkm2 = pkm2 / big;
                        pkm1 = pkm1 / big;
                        qkm2 = qkm2 / big;
                        qkm1 = qkm1 / big;
                    }
                } while ((double)(t) >= double.Epsilon);
                result = result * Math.Exp(-x);
            }
            return result;
        }

        #endregion

        #region Fonctions Erf

        public static Complex Erf(Complex x)
        {
            if (x == 0)
            {
                return 0;
            }
            if (double.IsPositiveInfinity(x))
            {
                return 1;
            }
            if (double.IsNegativeInfinity(x))
            {
                return -1;
            }
            if (double.IsNaN(x))
            {
                return double.NaN;
            }

            return ErfImp(x, false);
        }

        public static Complex Erfc(Complex x)
        {
            if (x == 0)
            {
                return 1;
            }

            if (double.IsPositiveInfinity(x))
            {
                return 0;
            }

            if (double.IsNegativeInfinity(x))
            {
                return 2;
            }

            if (double.IsNaN(x))
            {
                return double.NaN;
            }

            return ErfImp(x, true);
        }

        public static Complex ErfInv(Complex z)
        {
            if (z == 0.0)
            {
                return 0.0;
            }

            if (z >= 1.0)
            {
                return double.PositiveInfinity;
            }

            if (z <= -1.0)
            {
                return double.NegativeInfinity;
            }

            double p, q, s;
            if (z < 0)
            {
                p = -z;
                q = 1 - p;
                s = -1;
            }
            else
            {
                p = z;
                q = 1 - z;
                s = 1;
            }

            return ErfInvImpl(p, q, s);
        }

        public static Complex ErfImp(Complex z, Complex invert)
        {
            if (invert != 1 && invert != 0) return 0;
            return ErfImp(z, invert == 1);
        }

        private static Complex ErfImp(double z, bool invert)
        {
            if (z < 0)
            {
                if (!invert)
                {
                    return -ErfImp(-z, false);
                }

                if (z < -0.5)
                {
                    return 2 - ErfImp(-z, true);
                }

                return 1 + ErfImp(-z, false);
            }

            double result;

            // Big bunch of selection statements now to pick which
            // implementation to use, try to put most likely options
            // first:
            if (z < 0.5)
            {
                // We're going to calculate erf:
                if (z < 1e-10)
                {
                    result = (z * 1.125) + (z * 0.003379167095512573896158903121545171688);
                }
                else
                {
                    // Worst case absolute error found: 6.688618532e-21
                    Complex[] nc =
                    {
                        0.00337916709551257388990745, -0.00073695653048167948530905,
                        -0.374732337392919607868241, 0.0817442448733587196071743, -0.0421089319936548595203468,
                        0.0070165709512095756344528, -0.00495091255982435110337458, 0.000871646599037922480317225
                    };
                    Complex[] dc =
                    {
                        1, -0.218088218087924645390535, 0.412542972725442099083918,
                        -0.0841891147873106755410271, 0.0655338856400241519690695, -0.0120019604454941768171266,
                        0.00408165558926174048329689, -0.000615900721557769691924509
                    };

                    result = (z * 1.125) + (z * Polynomial(z, nc) / Polynomial(z, dc));
                }
            }
            else if ((z < 110) || ((z < 110) && invert))
            {
                // We'll be calculating erfc:
                invert = !invert;
                double r, b;
                if (z < 0.75)
                {
                    // Worst case absolute error found: 5.582813374e-21
                    Complex[] nc =
                    {
                        -0.0361790390718262471360258, 0.292251883444882683221149, 0.281447041797604512774415,
                        0.125610208862766947294894, 0.0274135028268930549240776, 0.00250839672168065762786937
                    };
                    Complex[] dc =
                    {
                        1, 1.8545005897903486499845, 1.43575803037831418074962, 0.582827658753036572454135,
                        0.124810476932949746447682, 0.0113724176546353285778481
                    };
                    r = Polynomial(z - 0.5, nc) / Polynomial(z - 0.5, dc);
                    b = 0.3440242112F;
                }
                else if (z < 1.25)
                {
                    // Worst case absolute error found: 4.01854729e-21
                    Complex[] nc =
                    {
                        -0.0397876892611136856954425, 0.153165212467878293257683, 0.191260295600936245503129,
                        0.10276327061989304213645, 0.029637090615738836726027, 0.0046093486780275489468812,
                        0.000307607820348680180548455
                    };
                    Complex[] dc =
                    {
                        1, 1.95520072987627704987886, 1.64762317199384860109595, 0.768238607022126250082483,
                        0.209793185936509782784315, 0.0319569316899913392596356, 0.00213363160895785378615014
                    };
                    r = Polynomial(z - 0.75, nc) / Polynomial(z - 0.75, dc);
                    b = 0.419990927F;
                }
                else if (z < 2.25)
                {
                    // Worst case absolute error found: 2.866005373e-21
                    Complex[] nc =
                    {
                        -0.0300838560557949717328341, 0.0538578829844454508530552,
                        0.0726211541651914182692959, 0.0367628469888049348429018, 0.00964629015572527529605267,
                        0.00133453480075291076745275, 0.778087599782504251917881e-4
                    };
                    Complex[] dc =
                    {
                        1, 1.75967098147167528287343, 1.32883571437961120556307, 0.552528596508757581287907,
                        0.133793056941332861912279, 0.0179509645176280768640766, 0.00104712440019937356634038,
                        -0.106640381820357337177643e-7
                    };
                    r = Polynomial(z - 1.25, nc) / Polynomial(z - 1.25, dc);
                    b = 0.4898625016F;
                }
                else if (z < 3.5)
                {
                    // Worst case absolute error found: 1.045355789e-21
                    Complex[] nc =
                    {
                        -0.0117907570137227847827732, 0.014262132090538809896674, 0.0202234435902960820020765,
                        0.00930668299990432009042239, 0.00213357802422065994322516, 0.00025022987386460102395382,
                        0.120534912219588189822126e-4
                    };
                    Complex[] dc =
                    {
                        1, 1.50376225203620482047419, 0.965397786204462896346934, 0.339265230476796681555511,
                        0.0689740649541569716897427, 0.00771060262491768307365526, 0.000371421101531069302990367
                    };
                    r = Polynomial(z - 2.25, nc) / Polynomial(z - 2.25, dc);
                    b = 0.5317370892F;
                }
                else if (z < 5.25)
                {
                    // Worst case absolute error found: 8.300028706e-22
                    Complex[] nc =
                    {
                        -0.00546954795538729307482955, 0.00404190278731707110245394,
                        0.0054963369553161170521356, 0.00212616472603945399437862, 0.000394984014495083900689956,
                        0.365565477064442377259271e-4, 0.135485897109932323253786e-5
                    };
                    Complex[] dc =
                    {
                        1, 1.21019697773630784832251, 0.620914668221143886601045, 0.173038430661142762569515,
                        0.0276550813773432047594539, 0.00240625974424309709745382, 0.891811817251336577241006e-4,
                        -0.465528836283382684461025e-11
                    };
                    r = Polynomial(z - 3.5, nc) / Polynomial(z - 3.5, dc);
                    b = 0.5489973426F;
                }
                else if (z < 8)
                {
                    // Worst case absolute error found: 1.700157534e-21
                    Complex[] nc =
                    {
                        -0.00270722535905778347999196, 0.0013187563425029400461378,
                        0.00119925933261002333923989, 0.00027849619811344664248235, 0.267822988218331849989363e-4,
                        0.923043672315028197865066e-6
                    };
                    Complex[] dc =
                    {
                        1, 0.814632808543141591118279, 0.268901665856299542168425,
                        0.0449877216103041118694989, 0.00381759663320248459168994, 0.000131571897888596914350697,
                        0.404815359675764138445257e-11
                    };
                    r = Polynomial(z - 5.25, nc) / Polynomial(z - 5.25, dc);
                    b = 0.5571740866F;
                }
                else if (z < 11.5)
                {
                    // Worst case absolute error found: 3.002278011e-22
                    Complex[] nc =
                    {
                        -0.00109946720691742196814323, 0.000406425442750422675169153,
                        0.000274499489416900707787024, 0.465293770646659383436343e-4, 0.320955425395767463401993e-5,
                        0.778286018145020892261936e-7
                    };
                    Complex[] dc =
                    {
                        1, 0.588173710611846046373373, 0.139363331289409746077541,
                        0.0166329340417083678763028, 0.00100023921310234908642639, 0.24254837521587225125068e-4
                    };
                    r = Polynomial(z - 8, nc) / Polynomial(z - 8, dc);
                    b = 0.5609807968F;
                }
                else if (z < 17)
                {
                    // Worst case absolute error found: 6.741114695e-21
                    Complex[] nc =
                    {
                        -0.00056907993601094962855594, 0.000169498540373762264416984,
                        0.518472354581100890120501e-4, 0.382819312231928859704678e-5, 0.824989931281894431781794e-7
                    };
                    Complex[] dc =
                    {
                        1, 0.339637250051139347430323, 0.043472647870310663055044,
                        0.00248549335224637114641629, 0.535633305337152900549536e-4, -0.117490944405459578783846e-12
                    };
                    r = Polynomial(z - 11.5, nc) / Polynomial(z - 11.5, dc);
                    b = 0.5626493692F;
                }
                else if (z < 24)
                {
                    // Worst case absolute error found: 7.802346984e-22
                    Complex[] nc =
                    {
                        -0.000241313599483991337479091, 0.574224975202501512365975e-4,
                        0.115998962927383778460557e-4, 0.581762134402593739370875e-6, 0.853971555085673614607418e-8
                    };
                    Complex[] dc =
                    {
                        1, 0.233044138299687841018015, 0.0204186940546440312625597,
                        0.000797185647564398289151125, 0.117019281670172327758019e-4
                    };
                    r = Polynomial(z - 17, nc) / Polynomial(z - 17, dc);
                    b = 0.5634598136F;
                }
                else if (z < 38)
                {
                    // Worst case absolute error found: 2.414228989e-22
                    Complex[] nc =
                    {
                        -0.000146674699277760365803642, 0.162666552112280519955647e-4,
                        0.269116248509165239294897e-5, 0.979584479468091935086972e-7, 0.101994647625723465722285e-8
                    };
                    Complex[] dc =
                    {
                        1, 0.165907812944847226546036, 0.0103361716191505884359634,
                        0.000286593026373868366935721, 0.298401570840900340874568e-5
                    };
                    r = Polynomial(z - 24, nc) / Polynomial(z - 24, dc);
                    b = 0.5638477802F;
                }
                else if (z < 60)
                {
                    // Worst case absolute error found: 5.896543869e-24
                    Complex[] nc =
                    {
                        -0.583905797629771786720406e-4, 0.412510325105496173512992e-5,
                        0.431790922420250949096906e-6, 0.993365155590013193345569e-8, 0.653480510020104699270084e-10
                    };
                    Complex[] dc =
                    {
                        1, 0.105077086072039915406159, 0.00414278428675475620830226,
                        0.726338754644523769144108e-4, 0.477818471047398785369849e-6
                    };
                    r = Polynomial(z - 38, nc) / Polynomial(z - 38, dc);
                    b = 0.5640528202F;
                }
                else if (z < 85)
                {
                    // Worst case absolute error found: 3.080612264e-21
                    Complex[] nc =
                    {
                        -0.196457797609229579459841e-4, 0.157243887666800692441195e-5,
                        0.543902511192700878690335e-7, 0.317472492369117710852685e-9
                    };
                    Complex[] dc =
                    {
                        1, 0.052803989240957632204885, 0.000926876069151753290378112,
                        0.541011723226630257077328e-5, 0.535093845803642394908747e-15
                    };
                    r = Polynomial(z - 60, nc) / Polynomial(z - 60, dc);
                    b = 0.5641309023F;
                }
                else
                {
                    // Worst case absolute error found: 8.094633491e-22
                    Complex[] nc =
                    {
                        -0.789224703978722689089794e-5, 0.622088451660986955124162e-6,
                        0.145728445676882396797184e-7, 0.603715505542715364529243e-10
                    };
                    Complex[] dc =
                    {
                        1, 0.0375328846356293715248719, 0.000467919535974625308126054,
                        0.193847039275845656900547e-5
                    };
                    r = Polynomial(z - 85, nc) / Polynomial(z - 85, dc);
                    b = 0.5641584396F;
                }

                var g = Math.Exp(-z * z) / z;
                result = (g * b) + (g * r);
            }
            else
            {
                // Any value of z larger than 28 will underflow to zero:
                result = 0;
                invert = !invert;
            }

            if (invert)
            {
                result = 1 - result;
            }

            return result;
        }

        public static Complex ErfInvImpl(Complex p, Complex q, Complex s)
        {
            double result;

            if (p <= 0.5)
            {
                const double y = 0.0891314744949340820313d;
                Complex[] nc =
                {
                    -0.000508781949658280665617, -0.00836874819741736770379, 0.0334806625409744615033,
                    -0.0126926147662974029034, -0.0365637971411762664006, 0.0219878681111168899165,
                    0.00822687874676915743155, -0.00538772965071242932965
                };
                Complex[] dc =
                {
                    1, -0.970005043303290640362, -1.56574558234175846809, 1.56221558398423026363,
                    0.662328840472002992063, -0.71228902341542847553, -0.0527396382340099713954,
                    0.0795283687341571680018, -0.00233393759374190016776, 0.000886216390456424707504
                };
                var g = p * (p + 10);
                var r = Polynomial(p, nc) / Polynomial(p, dc);
                result = (g * y) + (g * r);
            }
            else if (q >= 0.25)
            {
                const double y = 2.249481201171875;
                Complex[] nc =
                {
                    -0.202433508355938759655, 0.105264680699391713268, 8.37050328343119927838,
                    17.6447298408374015486, -18.8510648058714251895, -44.6382324441786960818, 17.445385985570866523,
                    21.1294655448340526258, -3.67192254707729348546
                };
                Complex[] dc =
                {
                    1, 6.24264124854247537712, 3.9713437953343869095, -28.6608180499800029974,
                    -20.1432634680485188801, 48.5609213108739935468, 10.8268667355460159008, -22.6436933413139721736,
                    1.72114765761200282724
                };
                var g = Math.Sqrt(-2 * Math.Log(q));
                var xs = q - 0.25;
                var r = Polynomial(xs, nc) / Polynomial(xs, dc);
                result = g / (y + r);
            }
            else
            {
                var x = Math.Sqrt(-Math.Log(q));
                if (x < 3)
                {
                    const double y = 0.807220458984375;
                    Complex[] nc =
                    {
                        -0.131102781679951906451, -0.163794047193317060787, 0.117030156341995252019,
                        0.387079738972604337464, 0.337785538912035898924, 0.142869534408157156766,
                        0.0290157910005329060432, 0.00214558995388805277169, -0.679465575181126350155e-6,
                        0.285225331782217055858e-7, -0.681149956853776992068e-9
                    };
                    Complex[] dc =
                    {
                        1, 3.46625407242567245975, 5.38168345707006855425, 4.77846592945843778382,
                        2.59301921623620271374, 0.848854343457902036425, 0.152264338295331783612, 0.01105924229346489121
                    };
                    var xs = x - 1.125;
                    var r = Polynomial(xs, nc) / Polynomial(xs, dc);
                    result = (y * x) + (r * x);
                }
                else if (x < 6)
                {
                    const double y = 0.93995571136474609375;
                    Complex[] nc =
                    {
                        -0.0350353787183177984712, -0.00222426529213447927281, 0.0185573306514231072324,
                        0.00950804701325919603619, 0.00187123492819559223345, 0.000157544617424960554631,
                        0.460469890584317994083e-5, -0.230404776911882601748e-9, 0.266339227425782031962e-11
                    };
                    Complex[] dc =
                    {
                        1, 1.3653349817554063097, 0.762059164553623404043, 0.220091105764131249824,
                        0.0341589143670947727934, 0.00263861676657015992959, 0.764675292302794483503e-4
                    };
                    var xs = x - 3;
                    var r = Polynomial(xs, nc) / Polynomial(xs, dc);
                    result = (y * x) + (r * x);
                }
                else if (x < 18)
                {
                    const double y = 0.98362827301025390625;
                    Complex[] nc =
                    {
                        -0.0167431005076633737133, -0.00112951438745580278863, 0.00105628862152492910091,
                        0.000209386317487588078668, 0.149624783758342370182e-4, 0.449696789927706453732e-6,
                        0.462596163522878599135e-8, -0.281128735628831791805e-13, 0.99055709973310326855e-16
                    };
                    Complex[] dc =
                    {
                        1, 0.591429344886417493481, 0.138151865749083321638, 0.0160746087093676504695,
                        0.000964011807005165528527, 0.275335474764726041141e-4, 0.282243172016108031869e-6
                    };
                    var xs = x - 6;
                    var r = Polynomial(xs, nc) / Polynomial(xs, dc);
                    result = (y * x) + (r * x);
                }
                else if (x < 44)
                {
                    const double y = 0.99714565277099609375;
                    Complex[] nc =
                    {
                        -0.0024978212791898131227, -0.779190719229053954292e-5, 0.254723037413027451751e-4,
                        0.162397777342510920873e-5, 0.396341011304801168516e-7, 0.411632831190944208473e-9,
                        0.145596286718675035587e-11, -0.116765012397184275695e-17
                    };
                    Complex[] dc =
                    {
                        1, 0.207123112214422517181, 0.0169410838120975906478, 0.000690538265622684595676,
                        0.145007359818232637924e-4, 0.144437756628144157666e-6, 0.509761276599778486139e-9
                    };
                    var xs = x - 18;
                    var r = Polynomial(xs, nc) / Polynomial(xs, dc);
                    result = (y * x) + (r * x);
                }
                else
                {
                    const double y = 0.99941349029541015625;
                    Complex[] nc =
                    {
                        -0.000539042911019078575891, -0.28398759004727721098e-6, 0.899465114892291446442e-6,
                        0.229345859265920864296e-7, 0.225561444863500149219e-9, 0.947846627503022684216e-12,
                        0.135880130108924861008e-14, -0.348890393399948882918e-21
                    };
                    Complex[] dc =
                    {
                        1, 0.0845746234001899436914, 0.00282092984726264681981, 0.468292921940894236786e-4,
                        0.399968812193862100054e-6, 0.161809290887904476097e-8, 0.231558608310259605225e-11
                    };
                    var xs = x - 44;
                    var r = Polynomial(xs, nc) / Polynomial(xs, dc);
                    result = (y * x) + (r * x);
                }
            }

            return s * result;
        }

        public static Complex Erf(Complex z1, Complex z2)
        {
            return Erf(z2) - Erf(z1);
        }

        public static Complex Erfi(Complex z)
        {
            return (2.0 / Sqrt(Constants.Pi)) * SumInf(k => Pow(z, 2.0 * k + 1.0) / (Fact(k) * (2.0 * k + 1.0)), 0);
        }

        #endregion

        #region Fonctions polynomiales

        public static Complex Polynomial(params Complex[] args)
        {
            var z = args[0];
            var coefficients = args;
            var a = coefficients.ToList();
            a.RemoveAt(0);
            coefficients = a.ToArray();
            var sum = coefficients[coefficients.Length - 1];
            for (var i = coefficients.Length - 2; i >= 0; --i)
            {
                sum *= z;
                sum += coefficients[i];
            }

            return sum;
        }

        private static double PolynomialDegree(double x, double[] coef, int N)
        {
            var ans = coef[0];

            for (var i = 1; i <= N; i++)
            {
                ans = ans * x + coef[i];
            }

            return ans;
        }

        [MathFunc(true)]
        public static Complex Polynomial(Complex z, Complex[] coeffs)
        {
            var c = coeffs.ToList();
            c.Insert(0, z);
            return Polynomial(c.ToArray());
        }

        public static Complex HermiteH(Complex n, Complex x)
        {
            double result = 0;
            double a = 1;
            var b = 2 * x;
            if (n == 0)
                return a;
            if (n == 1)
                return b;
            for (var i = 2; i <= n; i++)
            {
                result = 2 * x * b - 2 * (i - 1) * a;
                a = b;
                b = result;
            }
            return result;
        }

        public static Complex LaguerreL(Complex n, Complex z)
        {
            if (n == 0) return (1.0);

            var L0 = 1.0;
            var L1 = 1.0 - z;
            for (var k = 1; k < n; k++)
            {
                var L2 = ((2 * k + 1 - z) * L1 - k * L0) / (k + 1);
                L0 = L1;
                L1 = L2;
            }
            return (L1);
        }

        public static Complex LaguerreL(Complex n, Complex a, Complex z)
        {
            double l1 = 0;
            double r = 1;

            for (var i = 1.0; i <= n; i++)
            {
                var y = l1;
                l1 = r;
                r = ((2.0 * i - 1.0 + a - z) * l1 - (i - 1.0 + a) * y) / i;
            }

            return r;
        }

        public static Complex LegendreP(Complex n, Complex z)
        {
            return (1.0 / Pow(2.0, n)) *
                   Sum(k => Pow(-1.0, k) * Binomial(n, k) * Binomial(2.0 * n - 2.0 * k, n) * Pow(z, n - 2.0 * k), 0.0,
                       Math.Floor(n / 2.0));
        }

        public static Complex ChebyshevT(Complex n, Complex z)
        {
            if (n == 0) return (1.0);

            var T0 = 1.0;
            var T1 = z;
            for (var k = 1; k < n; k++)
            {
                var T2 = 2 * z * T1 - T0;
                T0 = T1;
                T1 = T2;
            }
            return (T1);
        }

        public static Complex ChebyshevU(Complex n, Complex z)
        {
            if (n == 0)
            {
                return 1;
            }
            if (n == 1)
            {
                return z;
            }

            return
                Sum(
                    k =>
                        (Pow(-1, k) * Gamma(n - k + 1.0) * Pow(2.0 * z, n - 2.0 * k)) /
                        (Fact(k) * Gamma(n - 2.0 * k + 1.0)),
                    0, Math.Floor(n / 2.0));
        }

        public static Complex GegenbauerC(Complex n, Complex a, Complex z)
        {
            var C0 = 1.0;
            if (n == 0) return (C0);

            var C1 = -2.0 * a * z;

            for (var k = 1.0; k < n; k++)
            {
                var C2 = (2.0 * z * (k + a) * C1 - (k - 1 + a) * C0) / (k + 1);
                C0 = C1;
                C1 = C2;
            }
            return (C1);
        }

        public static Complex JacobiP(Complex n, Complex a, Complex b, Complex z)
        {
            double x = 0;

            for (var k = 0.0; k <= n; k++)
            {
                x += (Pochhammer(-n, k) * Pochhammer(a + b + n + 1.0, k) * Pochhammer(a + k + 1.0, n - k)) / Fact(k);
            }

            return (1.0 / Gamma(n + 1.0)) * x;
            //return 0;
        }

        public static Complex LegendreP(Complex n, Complex u, Complex cof, Complex z)
        {
            double x = 0;

            for (var k = 0; k <= n; k++)
            {
                x += ((Pochhammer(-n, k) * Pochhammer(n + 1.0, k)) / (Gamma(k - u + 1.0) * Fact(k))) *
                     Pow((1.0 - z) / 2.0, k);
            }

            return (Pow(1.0 + z, u / 2.0) / Pow(1.0 - z, u / 2.0)) * x;
        }

        public static Complex LegendreP(Complex n, Complex u, Complex z)
        {
            return LegendreP(n, u, 2, z);
        }

        public static Complex Fibonacci(Complex n, Complex z)
        {
            double x = 0;

            for (var k = 0.0; k <= Math.Floor((n - 1.0) / 2.0); k++)
            {
                x += Binomial(n - k - 1.0, k) * Pow(z, (n - 2.0) * (k - 1.0));
            }

            return x;
        }

        public static Complex BernoulliB(Complex n, Complex x)
        {
            return Sum(k => Binomial(n, k) * BernoulliB(n - k) * Math.Pow(x, k), 0, n);
            //return Sum(n => (1 / n + 1) * Sum(k => Pow(-1, k) * Binomial(n, k) * Pow(x + k, m), 0, n), 0, m);
        }

        public static Complex Cyclotomic(Complex n, Complex z)
        {
            if (n != Truncate(n)) throw new MathException(n.ToString(), 0, "n doit être entier");
            return
                Product(
                    k =>
                        Pow(z - Pow(Constants.E, (2.0 * Constants.Pi * Constants.I * k) / n),
                            KroneckerDelta(GCD(k, n), 1)), 1, n);
        }

        #endregion

        #region Fonctions harmoniques

        public static Complex Harmonic(Complex t)
        {
            if (Math.Truncate(t) != t) return 0;
            return Constants.EulerGamma + DiGamma(t + 1.0);
        }

        public static Complex GeneralHarmonic(Complex n, Complex m)
        {
            if (Math.Truncate(n) != n) return 0;
            double sum = 0;
            for (var i = 0; i < n; i++)
            {
                sum += Math.Pow(i + 1, -m);
            }

            return sum;
        }

        #endregion

        #region Autres fonctions utiles

        public static Complex Binomial(Complex n, Complex k)
        {
            return Gamma(n + 1.0) / (Gamma(k + 1.0) * Gamma(n - k + 1.0));
        }

        public static Complex Multinomial(params Complex[] a)
        {
            if (a.Any(x1 => Math.Truncate(x1) != x1))
                throw new MathException($"Multinomial({a.First(x1 => Math.Truncate(x1) != x1)}; ...)", 0,
                    "Tous les paramètres doivent être entiers");
            if (a.Any(x2 => x2 <= 0))
                throw new MathException($"Multinomial({a.First(x2 => x2 <= 0)}; ...)", 0,
                    "Tous les paramètres doivent être strictement positifs.");

            double x = 1;
            a.All(y =>
            {
                x = x * Gamma(y + 1.0);
                return true;
            });

            return Gamma(a.Sum() + 1.0) / x;
        }

        public static Complex Pochhammer(Complex a, Complex n)
        {
            //if(Math.Truncate(-a) != -a) throw new ParseException((-a).ToString(), 0, "a doit être entier.");
            //if (!(-a >= 0)) throw new ParseException(a.ToString(), 0, "a doit être inférieur ou égal à 0");
            //if(!(a <= 0)) throw new ParseException(a.ToString(), 0, "a doit être inférieur ou égal à 0");
            //if (Math.Truncate(n) != n) throw new ParseException(n.ToString(), 0, "n doit être entier.");
            //if (!(n <= -a)) throw new ParseException(n.ToString(), 0, "n doit être inférieur ou égal à -a");


            return Gamma(a + n) / Gamma(a);
            //return (Pow(-1, n) * Fact(-a)) / Fact(-a-n);
            //return (Pow(-1, n) * Gamma(-a + 1)) / Gamma(-a - n + 1);
            //return Gamma(a + 1) / Gamma(a - n + 1);
            //return Product(k => a + k, 0, n - 1.0);
        }

        public static Complex Determinant(params Complex[] val)
        {
            var size = (int)Math.Sqrt(val.Count());
            if (size == 1) return val[0];
            var mat = new Complex[size, size];
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    var id = row * size + col;
                    mat[col, row] = val[id];
                }
            }
            return int_Det(mat);
        }

        private static Complex int_Det(Complex[,] matrix)
        {
            var frow = new List<Complex>();
            var size = matrix.GetLength(0);
            if (size == 2) return matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1];
            var mat = new Complex[size - 1, size];
            for (int row = 0; row < size; row++)
            {
                frow.Add(matrix[0, row]);
                for (int col = 0; col < size - 1; col++)
                {
                    mat[col, row] = matrix[col + 1, row];
                }
            }
            Complex result = 0;
            for (int i = 0; i < size; i++)
            {
                var fac = frow[i];
                var mtt = new List<Complex>();
                for (int row = 0; row < size; row++)
                {
                    if (row != i)
                    {
                        for (int col = 0; col < size - 1; col++)
                        {
                            mtt.Add(mat[col, row]);
                        }
                    }
                }
                var det = fac * Determinant(mtt.ToArray());
                if (i % 2 != 0) result -= det;
                else result += det;
            }


            return result;
        }

        #endregion

        #region Fonctions hypergéométriques

        public static Complex Hypergeometric1F1(Complex a, Complex b, Complex z)
        {
            /*Complex x = 0.0;
			Complex k = 0.0;

			Complex l = 0.0;

			while(true)
			{
				Complex nw = (Pochhammer(a, k) * Pow(z, k)) / (Pochhammer(b, k) * Fact(k));

				if(x == 2.5)
				{

				}

				if (Math.Abs(nw - l) <= 0.00000000000001)
					break;

				l = nw;
				x += nw;
				k++;
			}

			return x;*/
            return SumInf(k => (Pochhammer(a, k) * Pow(z, k)) / (Pochhammer(b, k) * Fact(k)), 0);
        }

        public static Complex Hypergeometric0F1(Complex b, Complex z)
        {
            return SumInf(k => Pow(z, k) / (Pochhammer(b, k) * Fact(k)), 0);
        }

        public static Complex ParabolicCylinderD(Complex v, Complex z)
        {
            return Pow(2.0, v / 2.0) * Math.Sqrt(Constants.Pi) * Pow(Constants.E, -(Pow(z, 2.0) / 4.0)) *
                   ((1.0 / Gamma((1.0 - v) / 2.0)) * Hypergeometric1F1(-v / 2.0, 0.5, z * z / 2.0) -
                    (Math.Sqrt(2.0) * z / Gamma(-v / 2.0)) * Hypergeometric1F1((1.0 - v) / 2.0, 3.0 / 2.0, z * z / 2.0));
        }


        public static Complex AppellF1(Complex a, Complex b1, Complex b2, Complex c, Complex z1, Complex z2)
        {
            // TODO: Non fonctionnelle
            return
                SumInf(
                    k =>
                        SumInf(
                            i =>
                                (Pochhammer(a, k + i) * Pochhammer(b1, k) * Pochhammer(b2, 1) * Pow(z1, k) * Pow(z2, i)) /
                                (Pochhammer(c, k + i) * Fact(k) * Fact(i)),
                            0), 0);
        }

        public static Complex ChebyshevTFunc(Complex nu, Complex z)
        {
            return Cos(nu * ArcCos(z));
        }

        public static Complex ChebyshevUFunc(Complex nu, Complex z)
        {
            return Sin((nu + 1.0) * ArcCos(z)) / Sqrt(1.0 - z.Square());
        }

        public static Complex FibonacciFunc(Complex nu, Complex z)
        {
            return (Pow(2.0, -nu) * Pow(z + Sqrt(z.Square() + 4.0), nu) -
                    Cos(nu * Constants.Pi) * Pow(2.0, nu) * Pow(z + Sqrt(z.Square() + 4.0), -nu)) /
                   Sqrt(z.Square() + 4.0);
        }

        //buggée
        public static Complex GegenbauerCFunc(Complex nu, Complex l, Complex z)
        {
            return ((Pow(2, 1 - 2 * l) * Sqrt(Constants.Pi) * Gamma(nu + 2 * l)) / (Fact(nu) * Gamma(l))) *
                   Hypergeometric2F1Regularized(-nu, nu + 2 * l, l + 0.5, (1 - z) / 2);
        }

        public static Complex GegenbauerCFunc(Complex nu, Complex z)
        {
            return (2 * Cos(nu * ArcCos(z))) / nu;
        }

        public static Complex Hypergeometric2F1Regularized(Complex a, Complex b, Complex c, Complex z)
        {
            /*return (Constant.Pi / Sin(Constant.Pi * (b - a))) *
				   ((Pow(-z, -a) / (Gamma(b) * Gamma(c - a))) *
					SumInf(
						k =>
							(Pochhammer(a, k) * Pochhammer(a - c + 1.0, k) * Pow(z, -k)) /
							(Fact(k) * Gamma(a - b + k + 1.0)), 0) - (Pow(-z, -b) / (Gamma(a) * Gamma(c - b))) * SumInf(
						k =>
							(Pochhammer(b, k) * Pochhammer(b - c + 1.0, k) * Pow(z, -k)) /
							(Fact(k) * Gamma(-a + b + k + 1.0)), 0));*/
            //if (Abs(z) < 1.0 || (Abs(z) == 1.0 && Re(c - a - b) > 0.0))
            return SumInf(k => (Pochhammer(a, k) * Pochhammer(b, k) * Pow(z, k)) / (Gamma(c + k) * Fact(k)), 0);
            //return Complex.Indeterminate;
        }

        public static Complex HermiteHFunc(Complex nu, Complex z)
        {
            return Pow(2.0, nu) * Sqrt(Constants.Pi) *
                   (1.0 / Gamma((1.0 - nu) / 2) * Hypergeometric1F1(-nu / 2.0, 0.5, z.Square()) -
                    (2.0 * z) / Gamma(-nu / 2.0) * Hypergeometric1F1((1.0 - nu) / 2.0, 1.5, z.Square()));
        }

        public static Complex Hypergeometric0F0(Complex z)
        {
            return Exp(z);
        }

        public static Complex Hypergeometric0F1Regularized(Complex b, Complex z)
        {
            return SumInf(k => Pow(z, k) / (Gamma(b + k) * Fact(k)), 0);
        }

        public static Complex Hypergeometric1F0(Complex a, Complex z)
        {
            return Pow(1.0 - z, -a);
        }

        public static Complex Hypergeometric1F1Regularized(Complex a, Complex b, Complex z)
        {
            return SumInf(k => (Pochhammer(a, k) * Pow(z, k)) / (Gamma(b + k) * Fact(k)), 0);
        }

        public static Complex Hypergeometric1F2(Complex a, Complex b1, Complex b2, Complex z)
        {
            return SumInf(k => (Pochhammer(a, k) * Pow(z, k)) / (Pochhammer(b1, k) * Pochhammer(b2, k) * Fact(k)), 0);
        }

        public static Complex Hypergeometric2F1(Complex a, Complex b, Complex c, Complex z)
        {
            return SumInf(k => (Pochhammer(a, k) * Pochhammer(b, k) * Pow(z, k)) / (Pochhammer(c, k) * Fact(k)), 0);
        }

        public static Complex HypergeometricU(Complex a, Complex b, Complex z)
        {
            return (Gamma(b - 1.0) / Gamma(a)) * Pow(z, 1.0 - b) * Hypergeometric1F1(a - b + 1.0, 2.0 - b, z) +
                   (Gamma(1.0 - b) / Gamma(a - b + 1.0)) * Hypergeometric1F1(a, b, z);
        }

        public static Complex JacobiPFunc(Complex nu, Complex a, Complex b, Complex z)
        {
            return (Gamma(a + nu + 1.0) / Gamma(nu + 1.0)) *
                   Hypergeometric2F1Regularized(-nu, a + b + nu + 1.0, a + 1.0, (1.0 - z) / 2.0);
        }

        public static Complex LaguerreLFunc(Complex nu, Complex l, Complex z)
        {
            return (Gamma(nu + l + 1.0) / Gamma(nu + 1.0)) * Hypergeometric1F1Regularized(-nu, l + 1.0, z);
        }

        public static Complex LaguerreLFunc(Complex nu, Complex z)
        {
            return Hypergeometric1F1(-nu, 1.0, z);
        }

        public static Complex LegendrePFunc(Complex nu, Complex z)
        {
            return Hypergeometric2F1(-nu, nu + 1.0, 1.0, (1.0 - z) / 2.0);
        }

        public static Complex WhittakerM(Complex nu, Complex u, Complex z)
        {
            return Pow(z, u + 0.5) * Exp(-z / 2.0) * Hypergeometric1F1(u - nu + 0.5, 2.0 * u + 1.0, z);
        }

        public static Complex WhittakerW(Complex nu, Complex u, Complex z)
        {
            return Pow(z, u + 0.5) * Exp(-z / 2.0) * HypergeometricU(u - nu + 0.5, 2.0 * u + 1.0, z);
        }

        #endregion

        #region Fonctions Bessel

        public static Complex BesselJ0(Complex x)
        {
            double ax;

            if ((ax = Math.Abs(x)) < 8.0)
            {
                var y = x * x;
                var ans1 = 57568490574.0 + y * (-13362590354.0 + y * (651619640.7
                                                                      +
                                                                      y *
                                                                      (-11214424.18 +
                                                                       y * (77392.33017 + y * (-184.9052456)))));
                var ans2 = 57568490411.0 + y * (1029532985.0 + y * (9494680.718
                                                                    + y * (59272.64853 + y * (267.8532712 + y * 1.0))));

                return ans1 / ans2;
            }
            else
            {
                var z = 8.0 / ax;
                var y = z * z;
                var xx = ax - 0.785398164;
                var ans1 = 1.0 + y * (-0.1098628627e-2 + y * (0.2734510407e-4
                                                              + y * (-0.2073370639e-5 + y * 0.2093887211e-6)));
                var ans2 = -0.1562499995e-1 + y * (0.1430488765e-3
                                                   + y * (-0.6911147651e-5 + y * (0.7621095161e-6
                                                                                  - y * 0.934935152e-7)));

                return Sqrt(0.636619772 / ax) *
                       (Cos(xx) * ans1 - z * Sin(xx) * ans2);
            }
        }

        public static Complex BesselJ(Complex nu, Complex x)
        {
            return SumInf(k => (Pow(-1.0, k) / (Gamma(k + nu + 1) * Fact(k))) * Pow(x / 2.0, 2.0 * k + nu), 0,
                0.0000000000001);
        }

        public static Complex BesselJ1(Complex x)
        {
            double ax, y, ans1, ans2;

            if ((ax = Abs(x)) < 8.0)
            {
                y = x * x;
                ans1 = x *
                       (72362614232.0 +
                        y *
                        (-7895059235.0 + y * (242396853.1 + y * (-2972611.439 + y * (15704.48260 + y * (-30.16036606))))));
                ans2 = 144725228442.0 +
                       y * (2300535178.0 + y * (18583304.74 + y * (99447.43394 + y * (376.9991397 + y * 1.0))));
                return ans1 / ans2;
            }
            else
            {
                var z = 8.0 / ax;
                var xx = ax - 2.356194491;
                y = z * z;
                ans1 = 1.0 + y * (0.183105e-2 + y * (-0.3516396496e-4 + y * (0.2457520174e-5 + y * (-0.240337019e-6))));
                ans2 = 0.04687499995 +
                       y * (-0.2002690873e-3 + y * (0.8449199096e-5 + y * (-0.88228987e-6 + y * 0.105787412e-6)));
                var ans = Sqrt(0.636619772 / ax) * (Cos(xx) * ans1 - z * Sin(xx) * ans2);

                if (x < 0.0)
                    ans = -ans;

                return ans;
            }
        }

        public static Complex BesselK(Complex nn, Complex x)
        {
            double result = 0;
            double k = 0;
            double nk1f = 0;
            double nkf = 0;
            double t = 0;
            double s = 0;
            double z0 = 0;
            double z = 0;
            double fn = 0;
            double pn = 0;
            double pk = 0;
            double i = 0;
            double n = 0;
            double eul = 0;

            eul = Constants.EulerGamma;
            if (nn < 0)
            {
                n = -nn;
            }
            else
            {
                n = nn;
            }
            if ((double)(x) <= (double)(9.55))
            {
                var ans = 0.0;
                z0 = 0.25 * x * x;
                fn = 1.0;
                pn = 0.0;
                var zmn = 1.0;
                var tox = 2.0 / x;
                if (n > 0)
                {
                    pn = -eul;
                    k = 1.0;
                    for (i = 1; i <= n - 1; i++)
                    {
                        pn = pn + 1.0 / k;
                        k = k + 1.0;
                        fn = fn * k;
                    }
                    zmn = tox;
                    if (n == 1)
                    {
                        ans = 1.0 / x;
                    }
                    else
                    {
                        nk1f = fn / n;
                        var kf = 1.0;
                        s = nk1f;
                        z = -z0;
                        var zn = 1.0;
                        for (i = 1; i <= n - 1; i++)
                        {
                            nk1f = nk1f / (n - i);
                            kf = kf * i;
                            zn = zn * z;
                            t = nk1f * zn / kf;
                            s = s + t;
                            zmn = zmn * tox;
                        }
                        s = s * 0.5;
                        t = Math.Abs(s);
                        ans = s * zmn;
                    }
                }
                var tlg = 2.0 * Math.Log(0.5 * x);
                pk = -eul;
                if (n == 0)
                {
                    pn = pk;
                    t = 1.0;
                }
                else
                {
                    pn = pn + 1.0 / n;
                    t = 1.0 / fn;
                }
                s = (pk + pn - tlg) * t;
                k = 1.0;
                do
                {
                    t = t * (z0 / (k * (k + n)));
                    pk = pk + 1.0 / k;
                    pn = pn + 1.0 / (k + n);
                    s = s + (pk + pn - tlg) * t;
                    k = k + 1.0;
                } while ((double)(Math.Abs(t / s)) > double.Epsilon);
                s = 0.5 * s / zmn;
                if (n % 2 != 0)
                {
                    s = -s;
                }
                ans = ans + s;
                result = ans;
                return result;
            }
            if ((double)(x) > (double)(Math.Log(double.MaxValue)))
            {
                result = 0;
                return result;
            }
            k = n;
            pn = 4.0 * k * k;
            pk = 1.0;
            z0 = 8.0 * x;
            fn = 1.0;
            t = 1.0;
            s = t;
            nkf = double.MaxValue;
            i = 0;
            do
            {
                z = pn - pk * pk;
                t = t * z / (fn * z0);
                nk1f = Math.Abs(t);
                if (i >= n && (double)(nk1f) > (double)(nkf))
                {
                    break;
                }
                nkf = nk1f;
                s = s + t;
                fn = fn + 1.0;
                pk = pk + 2.0;
                i = i + 1;
            } while ((double)(Math.Abs(t / s)) > double.Epsilon);
            result = Math.Exp(-x) * Math.Sqrt(Math.PI / (2.0 * x)) * s;
            return result;
        }

        public static Complex BesselY0(Complex x)
        {
            if (x < 8.0)
            {
                var y = x * x;

                var ans1 = -2957821389.0 + y * (7062834065.0 + y * (-512359803.6
                                                                    +
                                                                    y *
                                                                    (10879881.29 +
                                                                     y * (-86327.92757 + y * 228.4622733))));
                var ans2 = 40076544269.0 + y * (745249964.8 + y * (7189466.438
                                                                   + y * (47447.26470 + y * (226.1030244 + y * 1.0))));

                return (ans1 / ans2) + 0.636619772 * BesselJ0(x) * Ln(x);
            }
            else
            {
                var z = 8.0 / x;
                var y = z * z;
                var xx = x - 0.785398164;

                var ans1 = 1.0 + y * (-0.1098628627e-2 + y * (0.2734510407e-4
                                                              + y * (-0.2073370639e-5 + y * 0.2093887211e-6)));
                var ans2 = -0.1562499995e-1 + y * (0.1430488765e-3
                                                   + y * (-0.6911147651e-5 + y * (0.7621095161e-6
                                                                                  + y * (-0.934945152e-7))));
                return Sqrt(0.636619772 / x) *
                       (Sin(xx) * ans1 + z * Cos(xx) * ans2);
            }
        }

        public static Complex BesselY1(Complex x)
        {
            if (x < 8.0)
            {
                var y = x * x;
                var ans1 = x * (-0.4900604943e13 + y * (0.1275274390e13
                                                        + y * (-0.5153438139e11 + y * (0.7349264551e9
                                                                                       +
                                                                                       y *
                                                                                       (-0.4237922726e7 +
                                                                                        y * 0.8511937935e4)))));
                var ans2 = 0.2499580570e14 + y * (0.4244419664e12
                                                  + y * (0.3733650367e10 + y * (0.2245904002e8
                                                                                +
                                                                                y *
                                                                                (0.1020426050e6 +
                                                                                 y * (0.3549632885e3 + y)))));
                return (ans1 / ans2) + 0.636619772 * (BesselJ1(x) * Ln(x) - 1.0 / x);
            }
            else
            {
                var z = 8.0 / x;
                var y = z * z;
                var xx = x - 2.356194491;
                var ans1 = 1.0 + y * (0.183105e-2 + y * (-0.3516396496e-4
                                                         + y * (0.2457520174e-5 + y * (-0.240337019e-6))));
                var ans2 = 0.04687499995 + y * (-0.2002690873e-3
                                                + y * (0.8449199096e-5 + y * (-0.88228987e-6
                                                                              + y * 0.105787412e-6)));
                return Math.Sqrt(0.636619772 / x) *
                       (Math.Sin(xx) * ans1 + z * Math.Cos(xx) * ans2);
            }
        }

        public static Complex BesselY(Complex nu, Complex x)
        {
            var n = nu;

            if (n == 0) return BesselY0(x);
            if (n == 1) return BesselY1(x);

            var tox = 2.0 / x;
            var @by = BesselY1(x);
            var bym = BesselY0(x);
            for (var j = 1; j < n; j++)
            {
                var byp = j * tox * @by - bym;
                bym = by;
                by = byp;
            }
            return by;
        }

        public static Complex BesselI0(Complex x)
        {
            double ans;
            var ax = Math.Abs(x);

            if (ax < 3.75)
            {
                var y = x / 3.75;
                y = y * y;
                ans = 1.0 + y * (3.5156229 + y * (3.0899424 + y * (1.2067492
                                                                   +
                                                                   y * (0.2659732 + y * (0.360768e-1 + y * 0.45813e-2)))));
            }
            else
            {
                var y = 3.75 / ax;
                ans = (Math.Exp(ax) / Math.Sqrt(ax)) * (0.39894228 + y * (0.1328592e-1
                                                                          +
                                                                          y *
                                                                          (0.225319e-2 +
                                                                           y * (-0.157565e-2 + y * (0.916281e-2
                                                                                                    +
                                                                                                    y *
                                                                                                    (-0.2057706e-1 +
                                                                                                     y *
                                                                                                     (0.2635537e-1 +
                                                                                                      y * (-0.1647633e-1
                                                                                                           +
                                                                                                           y *
                                                                                                           0.392377e-2))))))));
            }

            return ans;
        }

        public static Complex BesselI(Complex x)
        {
            double ans;

            var ax = Math.Abs(x);

            if (ax < 3.75)
            {
                var y = x / 3.75;
                y = y * y;
                ans = ax * (0.5 + y * (0.87890594 + y * (0.51498869 + y * (0.15084934
                                                                           +
                                                                           y *
                                                                           (0.2658733e-1 +
                                                                            y * (0.301532e-2 + y * 0.32411e-3))))));
            }
            else
            {
                var y = 3.75 / ax;
                ans = 0.2282967e-1 + y * (-0.2895312e-1 + y * (0.1787654e-1 - y * 0.420059e-2));
                ans = 0.39894228 +
                      y * (-0.3988024e-1 + y * (-0.362018e-2 + y * (0.163801e-2 + y * (-0.1031555e-1 + y * ans))));
                ans *= Math.Exp(ax) / Math.Sqrt(ax);
            }
            return x < 0.0 ? -ans : ans;
        }

        public static Complex BesselI(Complex nu, Complex z)
        {
            int n = (int)Math.Truncate(nu);
            
			if (n < 0)
				throw new MathException("", 0, "n doit être positif.");
			else if (n == 0)
				return BesselI0(z);
			else if (n == 1)
				return BesselI(z);

			if (z == 0.0)
				return 0.0;

			
            var ACC = 40.0;
            var BIGNO = 1.0e+10;
            var BIGNI = 1.0e-10;

            var tox = 2.0 / Math.Abs(z);
            double bip = 0, ans = 0.0;
            var bi = 1.0;

            for (var j = 2 * (n + (int)Math.Sqrt(ACC * n)); j > 0; j--)
            {
                var bim = bip + j * tox * bi;
                bip = bi;
                bi = bim;

                if (Math.Abs(bi) > BIGNO)
                {
                    ans *= BIGNI;
                    bi *= BIGNI;
                    bip *= BIGNI;
                }

                if (j == n)
                    ans = bip;
            }

            ans *= BesselI0(z) / bi;
            return z < 0.0 && n % 2 == 1 ? -ans : ans;
        }

        private static void airy(double x,
            ref double ai,
            ref double aip,
            ref double bi,
            ref double bip)
        {
            double z = 0;
            double t = 0;
            double f = 0;
            double g = 0;
            double uf = 0;
            double ug = 0;
            double k = 0;
            double zeta = 0;
            var domflg = 0;
            double c1 = 0;
            double c2 = 0;
            double sqrt3 = 0;
            double sqpii = 0;

            ai = 0;
            aip = 0;
            bi = 0;
            bip = 0;

            sqpii = 5.64189583547756286948E-1;
            c1 = 0.35502805388781723926;
            c2 = 0.258819403792806798405;
            sqrt3 = Math.Sqrt(3);
            domflg = 0;
            if (x > 25.77)
            {
                ai = 0;
                aip = 0;
                bi = double.MaxValue;
                bip = double.MaxValue;
                return;
            }
            if (x < -2.09)
            {
                domflg = 15;
                t = Math.Sqrt(-x);
                zeta = -(2.0 * x * t / 3.0);
                t = Math.Sqrt(t);
                k = sqpii / t;
                z = 1.0 / zeta;
                var zz = z * z;
                var afn = -1.31696323418331795333E-1;
                afn = afn * zz - 6.26456544431912369773E-1;
                afn = afn * zz - 6.93158036036933542233E-1;
                afn = afn * zz - 2.79779981545119124951E-1;
                afn = afn * zz - 4.91900132609500318020E-2;
                afn = afn * zz - 4.06265923594885404393E-3;
                afn = afn * zz - 1.59276496239262096340E-4;
                afn = afn * zz - 2.77649108155232920844E-6;
                afn = afn * zz - 1.67787698489114633780E-8;
                var afd = 1.00000000000000000000E0;
                afd = afd * zz + 1.33560420706553243746E1;
                afd = afd * zz + 3.26825032795224613948E1;
                afd = afd * zz + 2.67367040941499554804E1;
                afd = afd * zz + 9.18707402907259625840E0;
                afd = afd * zz + 1.47529146771666414581E0;
                afd = afd * zz + 1.15687173795188044134E-1;
                afd = afd * zz + 4.40291641615211203805E-3;
                afd = afd * zz + 7.54720348287414296618E-5;
                afd = afd * zz + 4.51850092970580378464E-7;
                uf = 1.0 + zz * afn / afd;
                var agn = 1.97339932091685679179E-2;
                agn = agn * zz + 3.91103029615688277255E-1;
                agn = agn * zz + 1.06579897599595591108E0;
                agn = agn * zz + 9.39169229816650230044E-1;
                agn = agn * zz + 3.51465656105547619242E-1;
                agn = agn * zz + 6.33888919628925490927E-2;
                agn = agn * zz + 5.85804113048388458567E-3;
                agn = agn * zz + 2.82851600836737019778E-4;
                agn = agn * zz + 6.98793669997260967291E-6;
                agn = agn * zz + 8.11789239554389293311E-8;
                agn = agn * zz + 3.41551784765923618484E-10;
                var agd = 1.00000000000000000000E0;
                agd = agd * zz + 9.30892908077441974853E0;
                agd = agd * zz + 1.98352928718312140417E1;
                agd = agd * zz + 1.55646628932864612953E1;
                agd = agd * zz + 5.47686069422975497931E0;
                agd = agd * zz + 9.54293611618961883998E-1;
                agd = agd * zz + 8.64580826352392193095E-2;
                agd = agd * zz + 4.12656523824222607191E-3;
                agd = agd * zz + 1.01259085116509135510E-4;
                agd = agd * zz + 1.17166733214413521882E-6;
                agd = agd * zz + 4.91834570062930015649E-9;
                ug = z * agn / agd;
                var theta = zeta + 0.25 * Math.PI;
                f = Math.Sin(theta);
                g = Math.Cos(theta);
                ai = k * (f * uf - g * ug);
                bi = k * (g * uf + f * ug);
                var apfn = 1.85365624022535566142E-1;
                apfn = apfn * zz + 8.86712188052584095637E-1;
                apfn = apfn * zz + 9.87391981747398547272E-1;
                apfn = apfn * zz + 4.01241082318003734092E-1;
                apfn = apfn * zz + 7.10304926289631174579E-2;
                apfn = apfn * zz + 5.90618657995661810071E-3;
                apfn = apfn * zz + 2.33051409401776799569E-4;
                apfn = apfn * zz + 4.08718778289035454598E-6;
                apfn = apfn * zz + 2.48379932900442457853E-8;
                var apfd = 1.00000000000000000000E0;
                apfd = apfd * zz + 1.47345854687502542552E1;
                apfd = apfd * zz + 3.75423933435489594466E1;
                apfd = apfd * zz + 3.14657751203046424330E1;
                apfd = apfd * zz + 1.09969125207298778536E1;
                apfd = apfd * zz + 1.78885054766999417817E0;
                apfd = apfd * zz + 1.41733275753662636873E-1;
                apfd = apfd * zz + 5.44066067017226003627E-3;
                apfd = apfd * zz + 9.39421290654511171663E-5;
                apfd = apfd * zz + 5.65978713036027009243E-7;
                uf = 1.0 + zz * apfn / apfd;
                var apgn = -3.55615429033082288335E-2;
                apgn = apgn * zz - 6.37311518129435504426E-1;
                apgn = apgn * zz - 1.70856738884312371053E0;
                apgn = apgn * zz - 1.50221872117316635393E0;
                apgn = apgn * zz - 5.63606665822102676611E-1;
                apgn = apgn * zz - 1.02101031120216891789E-1;
                apgn = apgn * zz - 9.48396695961445269093E-3;
                apgn = apgn * zz - 4.60325307486780994357E-4;
                apgn = apgn * zz - 1.14300836484517375919E-5;
                apgn = apgn * zz - 1.33415518685547420648E-7;
                apgn = apgn * zz - 5.63803833958893494476E-10;
                var apgd = 1.00000000000000000000E0;
                apgd = apgd * zz + 9.85865801696130355144E0;
                apgd = apgd * zz + 2.16401867356585941885E1;
                apgd = apgd * zz + 1.73130776389749389525E1;
                apgd = apgd * zz + 6.17872175280828766327E0;
                apgd = apgd * zz + 1.08848694396321495475E0;
                apgd = apgd * zz + 9.95005543440888479402E-2;
                apgd = apgd * zz + 4.78468199683886610842E-3;
                apgd = apgd * zz + 1.18159633322838625562E-4;
                apgd = apgd * zz + 1.37480673554219441465E-6;
                apgd = apgd * zz + 5.79912514929147598821E-9;
                ug = z * apgn / apgd;
                k = sqpii * t;
                aip = -(k * (g * uf + f * ug));
                bip = k * (f * uf - g * ug);
                return;
            }
            if (x > 2.09)
            {
                domflg = 5;
                t = Math.Sqrt(x);
                zeta = 2.0 * x * t / 3.0;
                g = Math.Exp(zeta);
                t = Math.Sqrt(t);
                k = 2.0 * t * g;
                z = 1.0 / zeta;
                var an = 3.46538101525629032477E-1;
                an = an * z + 1.20075952739645805542E1;
                an = an * z + 7.62796053615234516538E1;
                an = an * z + 1.68089224934630576269E2;
                an = an * z + 1.59756391350164413639E2;
                an = an * z + 7.05360906840444183113E1;
                an = an * z + 1.40264691163389668864E1;
                an = an * z + 9.99999999999999995305E-1;
                var ad = 5.67594532638770212846E-1;
                ad = ad * z + 1.47562562584847203173E1;
                ad = ad * z + 8.45138970141474626562E1;
                ad = ad * z + 1.77318088145400459522E2;
                ad = ad * z + 1.64234692871529701831E2;
                ad = ad * z + 7.14778400825575695274E1;
                ad = ad * z + 1.40959135607834029598E1;
                ad = ad * z + 1.00000000000000000470E0;
                f = an / ad;
                ai = sqpii * f / k;
                k = -(0.5 * sqpii * t / g);
                var apn = 6.13759184814035759225E-1;
                apn = apn * z + 1.47454670787755323881E1;
                apn = apn * z + 8.20584123476060982430E1;
                apn = apn * z + 1.71184781360976385540E2;
                apn = apn * z + 1.59317847137141783523E2;
                apn = apn * z + 6.99778599330103016170E1;
                apn = apn * z + 1.39470856980481566958E1;
                apn = apn * z + 1.00000000000000000550E0;
                var apd = 3.34203677749736953049E-1;
                apd = apd * z + 1.11810297306158156705E1;
                apd = apd * z + 7.11727352147859965283E1;
                apd = apd * z + 1.58778084372838313640E2;
                apd = apd * z + 1.53206427475809220834E2;
                apd = apd * z + 6.86752304592780337944E1;
                apd = apd * z + 1.38498634758259442477E1;
                apd = apd * z + 9.99999999999999994502E-1;
                f = apn / apd;
                aip = f * k;
                if ((double)(x) > (double)(8.3203353))
                {
                    var bn16 = -2.53240795869364152689E-1;
                    bn16 = bn16 * z + 5.75285167332467384228E-1;
                    bn16 = bn16 * z - 3.29907036873225371650E-1;
                    bn16 = bn16 * z + 6.44404068948199951727E-2;
                    bn16 = bn16 * z - 3.82519546641336734394E-3;
                    var bd16 = 1.00000000000000000000E0;
                    bd16 = bd16 * z - 7.15685095054035237902E0;
                    bd16 = bd16 * z + 1.06039580715664694291E1;
                    bd16 = bd16 * z - 5.23246636471251500874E0;
                    bd16 = bd16 * z + 9.57395864378383833152E-1;
                    bd16 = bd16 * z - 5.50828147163549611107E-2;
                    f = z * bn16 / bd16;
                    k = sqpii * g;
                    bi = k * (1.0 + f) / t;
                    var bppn = 4.65461162774651610328E-1;
                    bppn = bppn * z - 1.08992173800493920734E0;
                    bppn = bppn * z + 6.38800117371827987759E-1;
                    bppn = bppn * z - 1.26844349553102907034E-1;
                    bppn = bppn * z + 7.62487844342109852105E-3;
                    var bppd = 1.00000000000000000000E0;
                    bppd = bppd * z - 8.70622787633159124240E0;
                    bppd = bppd * z + 1.38993162704553213172E1;
                    bppd = bppd * z - 7.14116144616431159572E0;
                    bppd = bppd * z + 1.34008595960680518666E0;
                    bppd = bppd * z - 7.84273211323341930448E-2;
                    f = z * bppn / bppd;
                    bip = k * t * (1.0 + f);
                    return;
                }
            }
            f = 1.0;
            g = x;
            t = 1.0;
            uf = 1.0;
            ug = x;
            k = 1.0;
            z = x * x * x;
            while (t > double.Epsilon)
            {
                uf = uf * z;
                k = k + 1.0;
                uf = uf / k;
                ug = ug * z;
                k = k + 1.0;
                ug = ug / k;
                uf = uf / k;
                f = f + uf;
                k = k + 1.0;
                ug = ug / k;
                g = g + ug;
                t = Math.Abs(uf / f);
            }
            uf = c1 * f;
            ug = c2 * g;
            if (domflg % 2 == 0)
            {
                ai = uf - ug;
            }
            if (domflg / 2 % 2 == 0)
            {
                bi = sqrt3 * (uf + ug);
            }
            k = 4.0;
            uf = x * x / 2.0;
            ug = z / 3.0;
            f = uf;
            g = 1.0 + ug;
            uf = uf / 3.0;
            t = 1.0;
            while (t > double.Epsilon)
            {
                uf = uf * z;
                ug = ug / k;
                k = k + 1.0;
                ug = ug * z;
                uf = uf / k;
                f = f + uf;
                k = k + 1.0;
                ug = ug / k;
                uf = uf / k;
                g = g + ug;
                k = k + 1.0;
                t = Math.Abs(ug / g);
            }
            uf = c1 * f;
            ug = c2 * g;
            if (domflg / 4 % 2 == 0)
            {
                aip = uf - ug;
            }
            if (domflg / 8 % 2 == 0)
            {
                bip = sqrt3 * (uf + ug);
            }
        }

        public static Complex AiryAi(Complex x)
        {
            double ai = 0, aip = 0, bi = 0, bip = 0;
            airy(x, ref ai, ref aip, ref bi, ref bip);
            return ai;
        }

        public static Complex AiryAiPrime(Complex x)
        {
            double ai = 0, aip = 0, bi = 0, bip = 0;
            airy(x, ref ai, ref aip, ref bi, ref bip);
            return aip;
        }

        public static Complex AiryBi(Complex x)
        {
            double ai = 0, aip = 0, bi = 0, bip = 0;
            airy(x, ref ai, ref aip, ref bi, ref bip);
            return bi;
        }

        public static Complex AiryBiPrime(Complex x)
        {
            double ai = 0, aip = 0, bi = 0, bip = 0;
            airy(x, ref ai, ref aip, ref bi, ref bip);
            return bip;
        }

        public static Complex StruveH(Complex v, Complex z)
        {
            return Pow(z / 2.0, v + 1) *
                   SumInf(k => Pow(-1, k) / (Gamma(k + 1.5) * Gamma(k + v + 1.5)) * Pow(z / 2, 2 * k), 0);
        }

        public static Complex StruveL(Complex v, Complex z)
        {
            return Pow(z / 2.0, v + 1) * SumInf(k => 1.0 / (Gamma(k + 1.5) * Gamma(k + v + 1.5)) * Pow(z / 2, 2 * k), 0);
        }

        public static Complex SphericalBesselJ(Complex v, Complex z)
        {
            return Sqrt(Constants.Pi / 2.0) * (1 / Sqrt(z)) * BesselJ(v + 0.5, z);
        }

        public static Complex SphericalBesselY(Complex v, Complex z)
        {
            return Sqrt(Constants.Pi / 2.0) * (1 / Sqrt(z)) * BesselY(v + 0.5, z);
        }

        public static Complex HankelH1(Complex nu, Complex z)
        {
            return new Complex(BesselJ(nu, z), BesselY(nu, z));
        }

        public static Complex HankelH2(Complex nu, Complex z)
        {
            return new Complex(BesselJ(nu, z), -BesselY(nu, z));
        }

        public static Complex KelvinBei(Complex v, Complex z)
        {
            var fs1 = new Complex(Sqrt(2.0) / 2.0, Sqrt(2.0) / 2.0);
            return -0.5 * Constants.I * Exp(-0.75 * Constants.I * Constants.Pi * v) * Pow(z, v) * Pow(fs1 * z, -v) *
                   (Exp((3.0 * Constants.I * Constants.Pi * v) / 2.0) * BesselI(v, fs1 * z) - BesselJ(v, fs1 * z));
        }

        public static Complex SphericalHankelH1(Complex nu, Complex z)
        {
            return Sqrt(Constants.Pi / 2.0) * (1.0 / Sqrt(z)) * HankelH1(nu + 0.5, z);
        }

        public static Complex SphericalHankelH2(Complex nu, Complex z)
        {
            return Sqrt(Constants.Pi / 2.0) * (1.0 / Sqrt(z)) * HankelH2(nu + 0.5, z);
        }

        public static Complex SphericalHarmonicYFunc(Complex l, Complex u, Complex v, Complex phi)
        {
            return Sqrt((2.0 * l + 1.0) / (4.0 * Constants.Pi)) * (Sqrt(Gamma(l - u + 1.0)) / Sqrt(Gamma(l + u + 1.0))) *
                   Exp(Constants.I * phi * u) * LegendreP(l, u, 2, Cos(v));
        }

        public static Complex SphericalHarmonicY(Complex n, Complex m, Complex v, Complex phi)
        {
            return Sqrt(((2.0 * n + 1.0) * Fact(n - m)) / (4.0 * Constants.Pi * Fact(n + m))) * Exp(Constants.I * phi * m) *
                   LegendreP(n, m, 2, Cos(v));
        }

        #endregion

        #region Fonctions Zeta

        public static Complex Zeta(Complex s)
        {
            return s.Real <= 0 ? Complex.Indeterminate : SumInf(k => 1.0 / Pow(k, s), 1);
        }

        public static Complex Zeta(Complex s, Complex a)
        {
            return SumInf(k => 1 / Pow(Pow(a + k, 2), s / 2), 0);
        }

        public static Complex LerchPhi(Complex z, Complex s, Complex a)
        {
            return SumInf(k => Pow(z, k) / Pow(Pow(a + k, 2), s / 2), 0);
        }

        public static Complex StieltjesGamma(Complex n)
        {
            if (n < 0) return Complex.Indeterminate;
            if ((n % 1 != 0)) return Complex.Indeterminate;

            return ((Pow(-1, n) * Fact(n)) / (Constants.Pi * 2.0)) *
                   Integrate(x => Exp(-n * Constants.I * x) * Zeta(Exp(Constants.I * x) + 1.0), 0, 2 * Constants.Pi);
        }

        #endregion

        #region Intégrales elliptiques

        public static Complex EllipticE(Complex z)
        {
            double result = 0;
            double p = 0;
            double q = 0;
            z = 1 - z;
            if (z == 0.0)
            {
                result = 1;
                return result;
            }
            p = 1.53552577301013293365E-4;
            p = p * z + 2.50888492163602060990E-3;
            p = p * z + 8.68786816565889628429E-3;
            p = p * z + 1.07350949056076193403E-2;
            p = p * z + 7.77395492516787092951E-3;
            p = p * z + 7.58395289413514708519E-3;
            p = p * z + 1.15688436810574127319E-2;
            p = p * z + 2.18317996015557253103E-2;
            p = p * z + 5.68051945617860553470E-2;
            p = p * z + 4.43147180560990850618E-1;
            p = p * z + 1.00000000000000000299E0;
            q = 3.27954898576485872656E-5;
            q = q * z + 1.00962792679356715133E-3;
            q = q * z + 6.50609489976927491433E-3;
            q = q * z + 1.68862163993311317300E-2;
            q = q * z + 2.61769742454493659583E-2;
            q = q * z + 3.34833904888224918614E-2;
            q = q * z + 4.27180926518931511717E-2;
            q = q * z + 5.85936634471101055642E-2;
            q = q * z + 9.37499997197644278445E-2;
            q = q * z + 2.49999999999888314361E-1;
            result = p - q * z * Math.Log(z);
            return result;
        }

        public static Complex EllipticK(Complex z)
        {
            return EllipticF(Constants.Pi / 2, z);
        }

        public static Complex EllipticEIncomplete(Complex z, Complex m)
        {
            double result = 0;
            double pio2 = 0;
            double a = 0;
            double b = 0;
            double c = 0;
            double e = 0;
            double temp = 0;
            double lphi = 0;
            double t = 0;
            double ebig = 0;
            var d = 0;
            var md = 0;
            var npio2 = 0;
            var s = 0;

            pio2 = 1.57079632679489661923;
            if (m == 0.0)
            {
                result = z;
                return result;
            }
            lphi = z;
            npio2 = (int)Math.Floor(lphi / pio2);
            if (npio2 % 2 != 0)
            {
                npio2 = npio2 + 1;
            }
            lphi = lphi - npio2 * pio2;
            if (lphi < 0.0)
            {
                lphi = -lphi;
                s = -1;
            }
            else
            {
                s = 1;
            }
            a = 1.0 - m;
            ebig = EllipticE(m);
            if (a == 0.0)
            {
                temp = Math.Sin(lphi);
                if (s < 0)
                {
                    temp = -temp;
                }
                result = temp + npio2 * ebig;
                return result;
            }
            t = Math.Tan(lphi);
            b = Math.Sqrt(a);
            if (Math.Abs(t) > 10.0)
            {
                e = 1.0 / (b * t);
                if (Math.Abs(e) < 10.0)
                {
                    e = Math.Atan(e);
                    temp = ebig + m * Math.Sin(lphi) * Math.Sin(e) - EllipticEIncomplete(e, m);
                    if (s < 0)
                    {
                        temp = -temp;
                    }
                    result = temp + npio2 * ebig;
                    return result;
                }
            }
            c = Math.Sqrt(m);
            a = 1.0;
            d = 1;
            e = 0.0;
            md = 0;
            while (Math.Abs(c / a) > double.Epsilon)
            {
                temp = b / a;
                lphi = lphi + Math.Atan(t * temp) + md * Math.PI;
                md = (int)((lphi + pio2) / Math.PI);
                t = t * (1.0 + temp) / (1.0 - temp * t * t);
                c = 0.5 * (a - b);
                temp = Math.Sqrt(a * b);
                a = 0.5 * (a + b);
                b = temp;
                d = d + d;
                e = e + c * Math.Sin(lphi);
            }
            temp = ebig / EllipticK(m);
            temp = temp * ((Math.Atan(t) + md * Math.PI) / (d * a));
            temp = temp + e;
            if (s < 0)
            {
                temp = -temp;
            }
            result = temp + npio2 * ebig;
            return result;
        }

        public static Complex EllipticKIncomplete(Complex z, Complex m)
        {
            double result = 0;
            double a = 0;
            double b = 0;
            double c = 0;
            double temp = 0;
            double pio2 = 0;
            double t = 0;
            double k = 0;
            var d = 0;
            var md = 0;
            var s = 0;
            var npio2 = 0;

            pio2 = 1.57079632679489661923;
            if ((double)(m) == (double)(0))
            {
                result = z;
                return result;
            }
            a = 1 - m;
            if ((double)(a) == (double)(0))
            {
                result = Math.Log(Math.Tan(0.5 * (pio2 + z)));
                return result;
            }
            npio2 = (int)Math.Floor(z / pio2);
            if (npio2 % 2 != 0)
            {
                npio2 = npio2 + 1;
            }
            if (npio2 != 0)
            {
                k = EllipticK(1 - a);
                z = z - npio2 * pio2;
            }
            else
            {
                k = 0;
            }
            if ((double)(z) < (double)(0))
            {
                z = -z;
                s = -1;
            }
            else
            {
                s = 0;
            }
            b = Math.Sqrt(a);
            t = Math.Tan(z);
            if ((double)(Math.Abs(t)) > (double)(10))
            {
                var e = 1.0 / (b * t);
                if ((double)(Math.Abs(e)) < (double)(10))
                {
                    e = Math.Atan(e);
                    if (npio2 == 0)
                    {
                        k = EllipticK(1 - a);
                    }
                    temp = k - EllipticKIncomplete(e, m);
                    if (s < 0)
                    {
                        temp = -temp;
                    }
                    result = temp + npio2 * k;
                    return result;
                }
            }
            a = 1.0;
            c = Math.Sqrt(m);
            d = 1;
            md = 0;
            while ((double)(Math.Abs(c / a)) > double.Epsilon)
            {
                temp = b / a;
                z = z + Math.Atan(t * temp) + md * Math.PI;
                md = (int)((z + pio2) / Math.PI);
                t = t * (1.0 + temp) / (1.0 - temp * t * t);
                c = 0.5 * (a - b);
                temp = Math.Sqrt(a * b);
                a = 0.5 * (a + b);
                b = temp;
                d = d + d;
            }
            temp = (Math.Atan(t) + md * Math.PI) / (d * a);
            if (s < 0)
            {
                temp = -temp;
            }
            result = temp + npio2 * k;
            return result;
        }

        public static Complex ArithmeticGeometricMean(Complex a, Complex b)
        {
            return (Constants.Pi * (a + b)) / (4 * EllipticK(Pow((a - b) / (a + b), 2)));
        }

        public static Complex EllipticF(Complex z, Complex m)
        {
            return Integrate(t => 1 / Sqrt(1 - m * Sin(t).Square()), 0, z);
        }

        public static Complex EllipticPi(Complex n, Complex z, Complex m)
        {
            return Integrate(t => 1 / ((1 - n * Sin(t).Square()) * Sqrt(1 - m * Sin(t).Square())), 0, z);
        }

        public static Complex EllipticPi(Complex n, Complex m)
        {
            return EllipticPi(n, Constants.Pi / 2, m);
        }

        #endregion

        #region Intégrales de Fresnel

        public static Complex FresnelS(Complex x)
        {
            double s = 0;

            double xxa = 0;
            double f = 0;
            double g = 0;
            double cc = 0;
            double ss = 0;
            double t = 0;
            double u = 0;
            double x2 = 0;
            double fn = 0;
            double fd = 0;
            double gn = 0;
            double gd = 0;
            double mpi = 0;
            double mpio2 = 0;

            mpi = 3.14159265358979323846;
            mpio2 = 1.57079632679489661923;
            xxa = x;
            x = Math.Abs(xxa);
            x2 = x * x;
            if ((double)(x2) < (double)(2.5625))
            {
                t = x2 * x2;
                var sn = -2.99181919401019853726E3;
                sn = sn * t + 7.08840045257738576863E5;
                sn = sn * t - 6.29741486205862506537E7;
                sn = sn * t + 2.54890880573376359104E9;
                sn = sn * t - 4.42979518059697779103E10;
                sn = sn * t + 3.18016297876567817986E11;
                var sd = 1.00000000000000000000E0;
                sd = sd * t + 2.81376268889994315696E2;
                sd = sd * t + 4.55847810806532581675E4;
                sd = sd * t + 5.17343888770096400730E6;
                sd = sd * t + 4.19320245898111231129E8;
                sd = sd * t + 2.24411795645340920940E10;
                sd = sd * t + 6.07366389490084639049E11;
                var cn = -4.98843114573573548651E-8;
                cn = cn * t + 9.50428062829859605134E-6;
                cn = cn * t - 6.45191435683965050962E-4;
                cn = cn * t + 1.88843319396703850064E-2;
                cn = cn * t - 2.05525900955013891793E-1;
                cn = cn * t + 9.99999999999999998822E-1;
                var cd = 3.99982968972495980367E-12;
                cd = cd * t + 9.15439215774657478799E-10;
                cd = cd * t + 1.25001862479598821474E-7;
                cd = cd * t + 1.22262789024179030997E-5;
                cd = cd * t + 8.68029542941784300606E-4;
                cd = cd * t + 4.12142090722199792936E-2;
                cd = cd * t + 1.00000000000000000118E0;
                s = Math.Sign(xxa) * x * x2 * sn / sd;
                return s;
            }
            if ((double)(x) > (double)(36974.0))
            {
                s = Math.Sign(xxa) * 0.5;
                return s;
            }
            x2 = x * x;
            t = mpi * x2;
            u = 1 / (t * t);
            t = 1 / t;
            fn = 4.21543555043677546506E-1;
            fn = fn * u + 1.43407919780758885261E-1;
            fn = fn * u + 1.15220955073585758835E-2;
            fn = fn * u + 3.45017939782574027900E-4;
            fn = fn * u + 4.63613749287867322088E-6;
            fn = fn * u + 3.05568983790257605827E-8;
            fn = fn * u + 1.02304514164907233465E-10;
            fn = fn * u + 1.72010743268161828879E-13;
            fn = fn * u + 1.34283276233062758925E-16;
            fn = fn * u + 3.76329711269987889006E-20;
            fd = 1.00000000000000000000E0;
            fd = fd * u + 7.51586398353378947175E-1;
            fd = fd * u + 1.16888925859191382142E-1;
            fd = fd * u + 6.44051526508858611005E-3;
            fd = fd * u + 1.55934409164153020873E-4;
            fd = fd * u + 1.84627567348930545870E-6;
            fd = fd * u + 1.12699224763999035261E-8;
            fd = fd * u + 3.60140029589371370404E-11;
            fd = fd * u + 5.88754533621578410010E-14;
            fd = fd * u + 4.52001434074129701496E-17;
            fd = fd * u + 1.25443237090011264384E-20;
            gn = 5.04442073643383265887E-1;
            gn = gn * u + 1.97102833525523411709E-1;
            gn = gn * u + 1.87648584092575249293E-2;
            gn = gn * u + 6.84079380915393090172E-4;
            gn = gn * u + 1.15138826111884280931E-5;
            gn = gn * u + 9.82852443688422223854E-8;
            gn = gn * u + 4.45344415861750144738E-10;
            gn = gn * u + 1.08268041139020870318E-12;
            gn = gn * u + 1.37555460633261799868E-15;
            gn = gn * u + 8.36354435630677421531E-19;
            gn = gn * u + 1.86958710162783235106E-22;
            gd = 1.00000000000000000000E0;
            gd = gd * u + 1.47495759925128324529E0;
            gd = gd * u + 3.37748989120019970451E-1;
            gd = gd * u + 2.53603741420338795122E-2;
            gd = gd * u + 8.14679107184306179049E-4;
            gd = gd * u + 1.27545075667729118702E-5;
            gd = gd * u + 1.04314589657571990585E-7;
            gd = gd * u + 4.60680728146520428211E-10;
            gd = gd * u + 1.10273215066240270757E-12;
            gd = gd * u + 1.38796531259578871258E-15;
            gd = gd * u + 8.39158816283118707363E-19;
            gd = gd * u + 1.86958710162783236342E-22;
            f = 1 - u * fn / fd;
            g = t * gn / gd;
            t = mpio2 * x2;
            cc = Math.Cos(t);
            ss = Math.Sin(t);
            t = mpi * x;
            s = 0.5 - (f * cc + g * ss) / t;
            s = s * Math.Sign(xxa);

            return s;
        }

        public static Complex FresnelC(Complex x)
        {
            double c = 0;

            double xxa = 0;
            double f = 0;
            double g = 0;
            double cc = 0;
            double ss = 0;
            double t = 0;
            double u = 0;
            double x2 = 0;
            double fn = 0;
            double fd = 0;
            double gn = 0;
            double gd = 0;
            double mpi = 0;
            double mpio2 = 0;

            mpi = 3.14159265358979323846;
            mpio2 = 1.57079632679489661923;
            xxa = x;
            x = Math.Abs(xxa);
            x2 = x * x;
            if ((double)(x2) < (double)(2.5625))
            {
                t = x2 * x2;
                var sn = -2.99181919401019853726E3;
                sn = sn * t + 7.08840045257738576863E5;
                sn = sn * t - 6.29741486205862506537E7;
                sn = sn * t + 2.54890880573376359104E9;
                sn = sn * t - 4.42979518059697779103E10;
                sn = sn * t + 3.18016297876567817986E11;
                var sd = 1.00000000000000000000E0;
                sd = sd * t + 2.81376268889994315696E2;
                sd = sd * t + 4.55847810806532581675E4;
                sd = sd * t + 5.17343888770096400730E6;
                sd = sd * t + 4.19320245898111231129E8;
                sd = sd * t + 2.24411795645340920940E10;
                sd = sd * t + 6.07366389490084639049E11;
                var cn = -4.98843114573573548651E-8;
                cn = cn * t + 9.50428062829859605134E-6;
                cn = cn * t - 6.45191435683965050962E-4;
                cn = cn * t + 1.88843319396703850064E-2;
                cn = cn * t - 2.05525900955013891793E-1;
                cn = cn * t + 9.99999999999999998822E-1;
                var cd = 3.99982968972495980367E-12;
                cd = cd * t + 9.15439215774657478799E-10;
                cd = cd * t + 1.25001862479598821474E-7;
                cd = cd * t + 1.22262789024179030997E-5;
                cd = cd * t + 8.68029542941784300606E-4;
                cd = cd * t + 4.12142090722199792936E-2;
                cd = cd * t + 1.00000000000000000118E0;
                c = Math.Sign(xxa) * x * cn / cd;
                return c;
            }
            if ((double)(x) > (double)(36974.0))
            {
                c = Math.Sign(xxa) * 0.5;
                return c;
            }
            x2 = x * x;
            t = mpi * x2;
            u = 1 / (t * t);
            t = 1 / t;
            fn = 4.21543555043677546506E-1;
            fn = fn * u + 1.43407919780758885261E-1;
            fn = fn * u + 1.15220955073585758835E-2;
            fn = fn * u + 3.45017939782574027900E-4;
            fn = fn * u + 4.63613749287867322088E-6;
            fn = fn * u + 3.05568983790257605827E-8;
            fn = fn * u + 1.02304514164907233465E-10;
            fn = fn * u + 1.72010743268161828879E-13;
            fn = fn * u + 1.34283276233062758925E-16;
            fn = fn * u + 3.76329711269987889006E-20;
            fd = 1.00000000000000000000E0;
            fd = fd * u + 7.51586398353378947175E-1;
            fd = fd * u + 1.16888925859191382142E-1;
            fd = fd * u + 6.44051526508858611005E-3;
            fd = fd * u + 1.55934409164153020873E-4;
            fd = fd * u + 1.84627567348930545870E-6;
            fd = fd * u + 1.12699224763999035261E-8;
            fd = fd * u + 3.60140029589371370404E-11;
            fd = fd * u + 5.88754533621578410010E-14;
            fd = fd * u + 4.52001434074129701496E-17;
            fd = fd * u + 1.25443237090011264384E-20;
            gn = 5.04442073643383265887E-1;
            gn = gn * u + 1.97102833525523411709E-1;
            gn = gn * u + 1.87648584092575249293E-2;
            gn = gn * u + 6.84079380915393090172E-4;
            gn = gn * u + 1.15138826111884280931E-5;
            gn = gn * u + 9.82852443688422223854E-8;
            gn = gn * u + 4.45344415861750144738E-10;
            gn = gn * u + 1.08268041139020870318E-12;
            gn = gn * u + 1.37555460633261799868E-15;
            gn = gn * u + 8.36354435630677421531E-19;
            gn = gn * u + 1.86958710162783235106E-22;
            gd = 1.00000000000000000000E0;
            gd = gd * u + 1.47495759925128324529E0;
            gd = gd * u + 3.37748989120019970451E-1;
            gd = gd * u + 2.53603741420338795122E-2;
            gd = gd * u + 8.14679107184306179049E-4;
            gd = gd * u + 1.27545075667729118702E-5;
            gd = gd * u + 1.04314589657571990585E-7;
            gd = gd * u + 4.60680728146520428211E-10;
            gd = gd * u + 1.10273215066240270757E-12;
            gd = gd * u + 1.38796531259578871258E-15;
            gd = gd * u + 8.39158816283118707363E-19;
            gd = gd * u + 1.86958710162783236342E-22;
            f = 1 - u * fn / fd;
            g = t * gn / gd;
            t = mpio2 * x2;
            cc = Math.Cos(t);
            ss = Math.Sin(t);
            t = mpi * x;
            c = 0.5 + (f * ss - g * cc) / t;
            c = c * Math.Sign(xxa);

            return c;
        }

        #endregion

        #region Intégrales trigonométriques

        public static Complex SinIntegral(Complex x)
        {
            double si = 0;

            double z = 0;
            double c = 0;
            double s = 0;
            double f = 0;
            double g = 0;
            var sg = 0;
            double fn = 0;
            double fd = 0;
            double gn = 0;
            double gd = 0;

            si = 0;

            if ((double)(x) < (double)(0))
            {
                sg = -1;
                x = -x;
            }
            else
            {
                sg = 0;
            }
            if ((double)(x) == (double)(0))
            {
                return 0;
            }
            if ((double)(x) > (double)(1.0E9))
            {
                si = 1.570796326794896619 - Math.Cos(x) / x;
                return si;
            }
            if ((double)(x) <= (double)(4))
            {
                z = x * x;
                var sn = -8.39167827910303881427E-11;
                sn = sn * z + 4.62591714427012837309E-8;
                sn = sn * z - 9.75759303843632795789E-6;
                sn = sn * z + 9.76945438170435310816E-4;
                sn = sn * z - 4.13470316229406538752E-2;
                sn = sn * z + 1.00000000000000000302E0;
                var sd = 2.03269266195951942049E-12;
                sd = sd * z + 1.27997891179943299903E-9;
                sd = sd * z + 4.41827842801218905784E-7;
                sd = sd * z + 9.96412122043875552487E-5;
                sd = sd * z + 1.42085239326149893930E-2;
                sd = sd * z + 9.99999999999999996984E-1;
                s = x * sn / sd;
                var cn = 2.02524002389102268789E-11;
                cn = cn * z - 1.35249504915790756375E-8;
                cn = cn * z + 3.59325051419993077021E-6;
                cn = cn * z - 4.74007206873407909465E-4;
                cn = cn * z + 2.89159652607555242092E-2;
                cn = cn * z - 1.00000000000000000080E0;
                var cd = 4.07746040061880559506E-12;
                cd = cd * z + 3.06780997581887812692E-9;
                cd = cd * z + 1.23210355685883423679E-6;
                cd = cd * z + 3.17442024775032769882E-4;
                cd = cd * z + 5.10028056236446052392E-2;
                cd = cd * z + 4.00000000000000000080E0;
                c = z * cn / cd;
                if (sg != 0)
                {
                    s = -s;
                }
                si = s;
                return si;
            }
            s = Math.Sin(x);
            z = 1.0 / (x * x);
            if ((double)(x) < (double)(8))
            {
                fn = 4.23612862892216586994E0;
                fn = fn * z + 5.45937717161812843388E0;
                fn = fn * z + 1.62083287701538329132E0;
                fn = fn * z + 1.67006611831323023771E-1;
                fn = fn * z + 6.81020132472518137426E-3;
                fn = fn * z + 1.08936580650328664411E-4;
                fn = fn * z + 5.48900223421373614008E-7;
                fd = 1.00000000000000000000E0;
                fd = fd * z + 8.16496634205391016773E0;
                fd = fd * z + 7.30828822505564552187E0;
                fd = fd * z + 1.86792257950184183883E0;
                fd = fd * z + 1.78792052963149907262E-1;
                fd = fd * z + 7.01710668322789753610E-3;
                fd = fd * z + 1.10034357153915731354E-4;
                fd = fd * z + 5.48900252756255700982E-7;
                f = fn / (x * fd);
                gn = 8.71001698973114191777E-2;
                gn = gn * z + 6.11379109952219284151E-1;
                gn = gn * z + 3.97180296392337498885E-1;
                gn = gn * z + 7.48527737628469092119E-2;
                gn = gn * z + 5.38868681462177273157E-3;
                gn = gn * z + 1.61999794598934024525E-4;
                gn = gn * z + 1.97963874140963632189E-6;
                gn = gn * z + 7.82579040744090311069E-9;
                gd = 1.00000000000000000000E0;
                gd = gd * z + 1.64402202413355338886E0;
                gd = gd * z + 6.66296701268987968381E-1;
                gd = gd * z + 9.88771761277688796203E-2;
                gd = gd * z + 6.22396345441768420760E-3;
                gd = gd * z + 1.73221081474177119497E-4;
                gd = gd * z + 2.02659182086343991969E-6;
                gd = gd * z + 7.82579218933534490868E-9;
                g = z * gn / gd;
            }
            else
            {
                fn = 4.55880873470465315206E-1;
                fn = fn * z + 7.13715274100146711374E-1;
                fn = fn * z + 1.60300158222319456320E-1;
                fn = fn * z + 1.16064229408124407915E-2;
                fn = fn * z + 3.49556442447859055605E-4;
                fn = fn * z + 4.86215430826454749482E-6;
                fn = fn * z + 3.20092790091004902806E-8;
                fn = fn * z + 9.41779576128512936592E-11;
                fn = fn * z + 9.70507110881952024631E-14;
                fd = 1.00000000000000000000E0;
                fd = fd * z + 9.17463611873684053703E-1;
                fd = fd * z + 1.78685545332074536321E-1;
                fd = fd * z + 1.22253594771971293032E-2;
                fd = fd * z + 3.58696481881851580297E-4;
                fd = fd * z + 4.92435064317881464393E-6;
                fd = fd * z + 3.21956939101046018377E-8;
                fd = fd * z + 9.43720590350276732376E-11;
                fd = fd * z + 9.70507110881952025725E-14;
                f = fn / (x * fd);
                gn = 6.97359953443276214934E-1;
                gn = gn * z + 3.30410979305632063225E-1;
                gn = gn * z + 3.84878767649974295920E-2;
                gn = gn * z + 1.71718239052347903558E-3;
                gn = gn * z + 3.48941165502279436777E-5;
                gn = gn * z + 3.47131167084116673800E-7;
                gn = gn * z + 1.70404452782044526189E-9;
                gn = gn * z + 3.85945925430276600453E-12;
                gn = gn * z + 3.14040098946363334640E-15;
                gd = 1.00000000000000000000E0;
                gd = gd * z + 1.68548898811011640017E0;
                gd = gd * z + 4.87852258695304967486E-1;
                gd = gd * z + 4.67913194259625806320E-2;
                gd = gd * z + 1.90284426674399523638E-3;
                gd = gd * z + 3.68475504442561108162E-5;
                gd = gd * z + 3.57043223443740838771E-7;
                gd = gd * z + 1.72693748966316146736E-9;
                gd = gd * z + 3.87830166023954706752E-12;
                gd = gd * z + 3.14040098946363335242E-15;
                g = z * gn / gd;
            }
            si = 1.570796326794896619 - f * c - g * s;
            if (sg != 0)
            {
                si = -si;
            }
            return si;
        }

        public static Complex CosIntegral(Complex x)
        {
            double ci = 0;

            double z = 0;
            double c = 0;
            double s = 0;
            double f = 0;
            double g = 0;
            var sg = 0;
            double fn = 0;
            double fd = 0;
            double gn = 0;
            double gd = 0;
            ci = 0;

            if ((double)(x) < (double)(0))
            {
                sg = -1;
                x = -x;
            }
            else
            {
                sg = 0;
            }
            if ((double)(x) == (double)(0))
            {
                ci = -double.MaxValue;
                return ci;
            }
            if ((double)(x) > (double)(1.0E9))
            {
                ci = Math.Sin(x) / x;
                return ci;
            }
            if ((double)(x) <= (double)(4))
            {
                z = x * x;
                var sn = -8.39167827910303881427E-11;
                sn = sn * z + 4.62591714427012837309E-8;
                sn = sn * z - 9.75759303843632795789E-6;
                sn = sn * z + 9.76945438170435310816E-4;
                sn = sn * z - 4.13470316229406538752E-2;
                sn = sn * z + 1.00000000000000000302E0;
                var sd = 2.03269266195951942049E-12;
                sd = sd * z + 1.27997891179943299903E-9;
                sd = sd * z + 4.41827842801218905784E-7;
                sd = sd * z + 9.96412122043875552487E-5;
                sd = sd * z + 1.42085239326149893930E-2;
                sd = sd * z + 9.99999999999999996984E-1;
                s = x * sn / sd;
                var cn = 2.02524002389102268789E-11;
                cn = cn * z - 1.35249504915790756375E-8;
                cn = cn * z + 3.59325051419993077021E-6;
                cn = cn * z - 4.74007206873407909465E-4;
                cn = cn * z + 2.89159652607555242092E-2;
                cn = cn * z - 1.00000000000000000080E0;
                var cd = 4.07746040061880559506E-12;
                cd = cd * z + 3.06780997581887812692E-9;
                cd = cd * z + 1.23210355685883423679E-6;
                cd = cd * z + 3.17442024775032769882E-4;
                cd = cd * z + 5.10028056236446052392E-2;
                cd = cd * z + 4.00000000000000000080E0;
                c = z * cn / cd;
                if (sg != 0)
                {
                    s = -s;
                }
                ci = 0.57721566490153286061 + Math.Log(x) + c;
                return ci;
            }
            s = Math.Sin(x);
            c = Math.Cos(x);
            z = 1.0 / (x * x);
            if ((double)(x) < (double)(8))
            {
                fn = 4.23612862892216586994E0;
                fn = fn * z + 5.45937717161812843388E0;
                fn = fn * z + 1.62083287701538329132E0;
                fn = fn * z + 1.67006611831323023771E-1;
                fn = fn * z + 6.81020132472518137426E-3;
                fn = fn * z + 1.08936580650328664411E-4;
                fn = fn * z + 5.48900223421373614008E-7;
                fd = 1.00000000000000000000E0;
                fd = fd * z + 8.16496634205391016773E0;
                fd = fd * z + 7.30828822505564552187E0;
                fd = fd * z + 1.86792257950184183883E0;
                fd = fd * z + 1.78792052963149907262E-1;
                fd = fd * z + 7.01710668322789753610E-3;
                fd = fd * z + 1.10034357153915731354E-4;
                fd = fd * z + 5.48900252756255700982E-7;
                f = fn / (x * fd);
                gn = 8.71001698973114191777E-2;
                gn = gn * z + 6.11379109952219284151E-1;
                gn = gn * z + 3.97180296392337498885E-1;
                gn = gn * z + 7.48527737628469092119E-2;
                gn = gn * z + 5.38868681462177273157E-3;
                gn = gn * z + 1.61999794598934024525E-4;
                gn = gn * z + 1.97963874140963632189E-6;
                gn = gn * z + 7.82579040744090311069E-9;
                gd = 1.00000000000000000000E0;
                gd = gd * z + 1.64402202413355338886E0;
                gd = gd * z + 6.66296701268987968381E-1;
                gd = gd * z + 9.88771761277688796203E-2;
                gd = gd * z + 6.22396345441768420760E-3;
                gd = gd * z + 1.73221081474177119497E-4;
                gd = gd * z + 2.02659182086343991969E-6;
                gd = gd * z + 7.82579218933534490868E-9;
                g = z * gn / gd;
            }
            else
            {
                fn = 4.55880873470465315206E-1;
                fn = fn * z + 7.13715274100146711374E-1;
                fn = fn * z + 1.60300158222319456320E-1;
                fn = fn * z + 1.16064229408124407915E-2;
                fn = fn * z + 3.49556442447859055605E-4;
                fn = fn * z + 4.86215430826454749482E-6;
                fn = fn * z + 3.20092790091004902806E-8;
                fn = fn * z + 9.41779576128512936592E-11;
                fn = fn * z + 9.70507110881952024631E-14;
                fd = 1.00000000000000000000E0;
                fd = fd * z + 9.17463611873684053703E-1;
                fd = fd * z + 1.78685545332074536321E-1;
                fd = fd * z + 1.22253594771971293032E-2;
                fd = fd * z + 3.58696481881851580297E-4;
                fd = fd * z + 4.92435064317881464393E-6;
                fd = fd * z + 3.21956939101046018377E-8;
                fd = fd * z + 9.43720590350276732376E-11;
                fd = fd * z + 9.70507110881952025725E-14;
                f = fn / (x * fd);
                gn = 6.97359953443276214934E-1;
                gn = gn * z + 3.30410979305632063225E-1;
                gn = gn * z + 3.84878767649974295920E-2;
                gn = gn * z + 1.71718239052347903558E-3;
                gn = gn * z + 3.48941165502279436777E-5;
                gn = gn * z + 3.47131167084116673800E-7;
                gn = gn * z + 1.70404452782044526189E-9;
                gn = gn * z + 3.85945925430276600453E-12;
                gn = gn * z + 3.14040098946363334640E-15;
                gd = 1.00000000000000000000E0;
                gd = gd * z + 1.68548898811011640017E0;
                gd = gd * z + 4.87852258695304967486E-1;
                gd = gd * z + 4.67913194259625806320E-2;
                gd = gd * z + 1.90284426674399523638E-3;
                gd = gd * z + 3.68475504442561108162E-5;
                gd = gd * z + 3.57043223443740838771E-7;
                gd = gd * z + 1.72693748966316146736E-9;
                gd = gd * z + 3.87830166023954706752E-12;
                gd = gd * z + 3.14040098946363335242E-15;
                g = z * gn / gd;
            }
            ci = f * s - g * c;

            return ci;
        }

        public static Complex SinhIntegral(Complex x)
        {
            double shi = 0;

            double k = 0;
            double c = 0;
            double s = 0;
            double a = 0;
            var sg = 0;
            double b2 = 0;

            if ((double)(x) < (double)(0))
            {
                sg = -1;
                x = -x;
            }
            else
            {
                sg = 0;
            }
            if ((double)(x) == (double)(0))
            {
                shi = 0;
                return shi;
            }
            if ((double)(x) < (double)(8.0))
            {
                var z = x * x;
                a = 1.0;
                s = 1.0;
                c = 0.0;
                k = 2.0;
                do
                {
                    a = a * z / k;
                    c = c + a / k;
                    k = k + 1.0;
                    a = a / k;
                    s = s + a / k;
                    k = k + 1.0;
                } while ((double)(Math.Abs(a / s)) >= double.Epsilon);
                s = s * x;
            }
            else
            {
                double b0 = 0;
                double b1 = 0;
                if ((double)(x) < (double)(18.0))
                {
                    a = (576.0 / x - 52.0) / 10.0;
                    k = Math.Exp(x) / x;
                    b0 = 1.83889230173399459482E-17;
                    b1 = 0.0;
                    chebiterationshichi(a, -9.55485532279655569575E-17, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 2.04326105980879882648E-16, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 1.09896949074905343022E-15, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -1.31313534344092599234E-14, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 5.93976226264314278932E-14, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -3.47197010497749154755E-14, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -1.40059764613117131000E-12, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 9.49044626224223543299E-12, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -1.61596181145435454033E-11, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -1.77899784436430310321E-10, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 1.35455469767246947469E-9, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -1.03257121792819495123E-9, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -3.56699611114982536845E-8, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 1.44818877384267342057E-7, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 7.82018215184051295296E-7, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -5.39919118403805073710E-6, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -3.12458202168959833422E-5, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 8.90136741950727517826E-5, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 2.02558474743846862168E-3, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 2.96064440855633256972E-2, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 1.11847751047257036625E0, ref b0, ref b1, ref b2);
                    s = k * 0.5 * (b0 - b2);
                    b0 = -8.12435385225864036372E-18;
                    b1 = 0.0;
                    chebiterationshichi(a, 2.17586413290339214377E-17, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 5.22624394924072204667E-17, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -9.48812110591690559363E-16, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 5.35546311647465209166E-15, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -1.21009970113732918701E-14, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -6.00865178553447437951E-14, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 7.16339649156028587775E-13, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -2.93496072607599856104E-12, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -1.40359438136491256904E-12, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 8.76302288609054966081E-11, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -4.40092476213282340617E-10, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -1.87992075640569295479E-10, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 1.31458150989474594064E-8, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -4.75513930924765465590E-8, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -2.21775018801848880741E-7, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 1.94635531373272490962E-6, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 4.33505889257316408893E-6, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -6.13387001076494349496E-5, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -3.13085477492997465138E-4, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 4.97164789823116062801E-4, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 2.64347496031374526641E-2, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 1.11446150876699213025E0, ref b0, ref b1, ref b2);
                    c = k * 0.5 * (b0 - b2);
                }
                else
                {
                    if ((double)(x) <= (double)(88.0))
                    {
                        a = (6336.0 / x - 212.0) / 70.0;
                        k = Math.Exp(x) / x;
                        b0 = -1.05311574154850938805E-17;
                        b1 = 0.0;
                        chebiterationshichi(a, 2.62446095596355225821E-17, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 8.82090135625368160657E-17, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -3.38459811878103047136E-16, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -8.30608026366935789136E-16, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 3.93397875437050071776E-15, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.01765565969729044505E-14, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -4.21128170307640802703E-14, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -1.60818204519802480035E-13, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 3.34714954175994481761E-13, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 2.72600352129153073807E-12, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.66894954752839083608E-12, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -3.49278141024730899554E-11, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -1.58580661666482709598E-10, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -1.79289437183355633342E-10, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.76281629144264523277E-9, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.69050228879421288846E-8, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.25391771228487041649E-7, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.16229947068677338732E-6, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.61038260117376323993E-5, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 3.49810375601053973070E-4, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.28478065259647610779E-2, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.03665722588798326712E0, ref b0, ref b1, ref b2);
                        s = k * 0.5 * (b0 - b2);
                        b0 = 8.06913408255155572081E-18;
                        b1 = 0.0;
                        chebiterationshichi(a, -2.08074168180148170312E-17, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -5.98111329658272336816E-17, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 2.68533951085945765591E-16, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 4.52313941698904694774E-16, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -3.10734917335299464535E-15, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -4.42823207332531972288E-15, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 3.49639695410806959872E-14, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 6.63406731718911586609E-14, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -3.71902448093119218395E-13, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -1.27135418132338309016E-12, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 2.74851141935315395333E-12, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 2.33781843985453438400E-11, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 2.71436006377612442764E-11, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -2.56600180000355990529E-10, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -1.61021375163803438552E-9, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -4.72543064876271773512E-9, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -3.00095178028681682282E-9, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 7.79387474390914922337E-8, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.06942765566401507066E-6, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.59503164802313196374E-5, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 3.49592575153777996871E-4, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.28475387530065247392E-2, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.03665693917934275131E0, ref b0, ref b1, ref b2);
                        c = k * 0.5 * (b0 - b2);
                    }
                    else
                    {
                        shi = sg != 0 ? double.MinValue : double.MaxValue;
                        return shi;
                    }
                }
            }
            if (sg != 0)
            {
                s = -s;
            }
            shi = s;

            return shi;
        }

        public static Complex CoshIntegral(Complex x)
        {
            double chi = 0;

            double k = 0;
            double c = 0;
            double s = 0;
            double a = 0;
            var sg = 0;
            double b2 = 0;

            if ((double)(x) < (double)(0))
            {
                sg = -1;
                x = -x;
            }
            else
            {
                sg = 0;
            }
            if ((double)(x) == (double)(0))
            {
                chi = double.MinValue;
                return chi;
            }
            if ((double)(x) < (double)(8.0))
            {
                var z = x * x;
                a = 1.0;
                s = 1.0;
                c = 0.0;
                k = 2.0;
                do
                {
                    a = a * z / k;
                    c = c + a / k;
                    k = k + 1.0;
                    a = a / k;
                    s = s + a / k;
                    k = k + 1.0;
                } while ((double)(Math.Abs(a / s)) >= double.Epsilon);
                s = s * x;
            }
            else
            {
                double b0 = 0;
                double b1 = 0;
                if ((double)(x) < (double)(18.0))
                {
                    a = (576.0 / x - 52.0) / 10.0;
                    k = Math.Exp(x) / x;
                    b0 = 1.83889230173399459482E-17;
                    b1 = 0.0;
                    chebiterationshichi(a, -9.55485532279655569575E-17, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 2.04326105980879882648E-16, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 1.09896949074905343022E-15, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -1.31313534344092599234E-14, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 5.93976226264314278932E-14, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -3.47197010497749154755E-14, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -1.40059764613117131000E-12, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 9.49044626224223543299E-12, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -1.61596181145435454033E-11, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -1.77899784436430310321E-10, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 1.35455469767246947469E-9, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -1.03257121792819495123E-9, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -3.56699611114982536845E-8, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 1.44818877384267342057E-7, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 7.82018215184051295296E-7, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -5.39919118403805073710E-6, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -3.12458202168959833422E-5, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 8.90136741950727517826E-5, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 2.02558474743846862168E-3, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 2.96064440855633256972E-2, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 1.11847751047257036625E0, ref b0, ref b1, ref b2);
                    s = k * 0.5 * (b0 - b2);
                    b0 = -8.12435385225864036372E-18;
                    b1 = 0.0;
                    chebiterationshichi(a, 2.17586413290339214377E-17, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 5.22624394924072204667E-17, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -9.48812110591690559363E-16, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 5.35546311647465209166E-15, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -1.21009970113732918701E-14, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -6.00865178553447437951E-14, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 7.16339649156028587775E-13, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -2.93496072607599856104E-12, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -1.40359438136491256904E-12, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 8.76302288609054966081E-11, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -4.40092476213282340617E-10, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -1.87992075640569295479E-10, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 1.31458150989474594064E-8, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -4.75513930924765465590E-8, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -2.21775018801848880741E-7, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 1.94635531373272490962E-6, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 4.33505889257316408893E-6, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -6.13387001076494349496E-5, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, -3.13085477492997465138E-4, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 4.97164789823116062801E-4, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 2.64347496031374526641E-2, ref b0, ref b1, ref b2);
                    chebiterationshichi(a, 1.11446150876699213025E0, ref b0, ref b1, ref b2);
                    c = k * 0.5 * (b0 - b2);
                }
                else
                {
                    if ((double)(x) <= (double)(88.0))
                    {
                        a = (6336.0 / x - 212.0) / 70.0;
                        k = Math.Exp(x) / x;
                        b0 = -1.05311574154850938805E-17;
                        b1 = 0.0;
                        chebiterationshichi(a, 2.62446095596355225821E-17, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 8.82090135625368160657E-17, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -3.38459811878103047136E-16, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -8.30608026366935789136E-16, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 3.93397875437050071776E-15, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.01765565969729044505E-14, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -4.21128170307640802703E-14, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -1.60818204519802480035E-13, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 3.34714954175994481761E-13, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 2.72600352129153073807E-12, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.66894954752839083608E-12, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -3.49278141024730899554E-11, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -1.58580661666482709598E-10, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -1.79289437183355633342E-10, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.76281629144264523277E-9, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.69050228879421288846E-8, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.25391771228487041649E-7, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.16229947068677338732E-6, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.61038260117376323993E-5, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 3.49810375601053973070E-4, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.28478065259647610779E-2, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.03665722588798326712E0, ref b0, ref b1, ref b2);
                        s = k * 0.5 * (b0 - b2);
                        b0 = 8.06913408255155572081E-18;
                        b1 = 0.0;
                        chebiterationshichi(a, -2.08074168180148170312E-17, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -5.98111329658272336816E-17, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 2.68533951085945765591E-16, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 4.52313941698904694774E-16, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -3.10734917335299464535E-15, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -4.42823207332531972288E-15, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 3.49639695410806959872E-14, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 6.63406731718911586609E-14, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -3.71902448093119218395E-13, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -1.27135418132338309016E-12, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 2.74851141935315395333E-12, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 2.33781843985453438400E-11, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 2.71436006377612442764E-11, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -2.56600180000355990529E-10, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -1.61021375163803438552E-9, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -4.72543064876271773512E-9, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, -3.00095178028681682282E-9, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 7.79387474390914922337E-8, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.06942765566401507066E-6, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.59503164802313196374E-5, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 3.49592575153777996871E-4, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.28475387530065247392E-2, ref b0, ref b1, ref b2);
                        chebiterationshichi(a, 1.03665693917934275131E0, ref b0, ref b1, ref b2);
                        c = k * 0.5 * (b0 - b2);
                    }
                    else
                    {
                        chi = double.MaxValue;
                        return chi;
                    }
                }
            }
            if (sg != 0)
            {
                s = -s;
            }
            chi = 0.57721566490153286061 + Math.Log(x) + c;

            return chi;
        }

        private static void chebiterationshichi(double x,
            double c,
            ref double b0,
            ref double b1,
            ref double b2)
        {
            b2 = b1;
            b1 = b0;
            b0 = x * b1 - b2 + c;
        }

        #endregion

        #region Fonctions elliptiques

        public static Complex EllipticTheta1(Complex z, Complex q)
        {
            if (q.Module >= 1)
                throw new MathException($"EllepticTheta1(z={z}; q={q})", 0, "q doit être entre 0 et 1.");

            return 2 * Pow(q, 1 / 4) * SumInf(k => Pow(-1, k) * Pow(q, k * (k + 1)) * Sin((2 * k + 1) * z), 0);
        }

        public static Complex EllipticTheta2(Complex z, Complex q)
        {
            if (q.Module >= 1)
                throw new MathException($"EllepticTheta2(z={z}; q={q})", 0, "q doit être entre 0 et 1.");

            return 2 * Pow(q, 1 / 4) * SumInf(k => Pow(q, k * (k + 1)) * Cos((2 * k + 1) * z), 0);
        }

        public static Complex EllipticTheta3(Complex z, Complex q)
        {
            if (q.Module >= 1)
                throw new MathException($"EllepticTheta3(z={z}; q={q})", 0, "q doit être entre 0 et 1.");

            return 1 + 2 * SumInf(k => Pow(q, Pow(k, 2)) * Cos(2 * k * z), 1);
        }

        public static Complex EllipticTheta4(Complex z, Complex q)
        {
            if (q.Module >= 1)
                throw new MathException($"EllepticTheta4(z={z}; q={q})", 0, "q doit être entre 0 et 1.");

            return 1 + 2 * SumInf(k => Pow(-1, k) * Pow(q, Pow(k, 2)) * Cos(2 * k * z), 1);
        }

        public static Complex DedekindEta(Complex z)
        {
            if (z.Imaginary <= 0) return Complex.Indeterminate;
            return Pow(Constants.E, (Constants.Pi * Constants.I * z) / 12.0) *
                   ProductInf(k => 1.0 - Pow(Constants.E, 2.0 * Constants.Pi * Constants.I * k * z), 1);
        }

        public static Complex EllipticNomeQ(Complex m)
        {
            return Exp(-((Constants.Pi * EllipticK(1.0 - m)) / EllipticK(m)));
        }

        public static Complex EllipticThetaPrime1(Complex z, Complex q)
        {
            if (q.Module >= 1)
                throw new MathException($"EllepticThetaPrime1(z={z}; q={q})", 0,
                    "Le module de q doit être inférieur à 1.");

            return 2.0 * Pow(q, 0.25) *
                   SumInf(k => Pow(-1, k) * Pow(q, k * (k + 1.0)) * (2.0 * k + 1.0) * Cos(z * (2.0 * k + 1.0)), 0);
        }

        public static Complex EllipticThetaPrime2(Complex z, Complex q)
        {
            if (q.Module >= 1)
                throw new MathException($"EllepticThetaPrime2(z={z}; q={q})", 0,
                    "Le module de q doit être inférieur à 1.");

            return -2.0 * Pow(q, 0.25) *
                   SumInf(k => Pow(q, k * (k + 1.0)) * (2.0 * k + 1.0) * Sin(z * (2.0 * k + 1.0)), 0);
        }

        public static Complex EllipticThetaPrime3(Complex z, Complex q)
        {
            if (q.Module >= 1)
                throw new MathException($"EllepticThetaPrime3(z={z}; q={q})", 0,
                    "Le module de q doit être inférieur à 1.");

            return -4.0 * SumInf(k => Pow(q, Pow(k, 2.0)) * k * Sin(z * 2.0 * k), 1);
        }

        public static Complex EllipticThetaPrime4(Complex z, Complex q)
        {
            if (q.Module >= 1)
                throw new MathException($"EllepticThetaPrime4(z={z}; q={q})", 0,
                    "Le module de q doit être inférieur à 1.");

            return -4.0 * SumInf(k => Pow(-1, k) * k * Pow(q, Pow(k, 2.0)) * Sin(z * 2.0 * k), 1);
        }

        public static Complex InverseEllipticNomeQ(Complex z)
        {
            return 16.0 * z * ProductInf(k => Pow((Pow(z, 2.0 * k) + 1.0) / (Pow(z, 2.0 * k - 1.0) + 1.0), 8), 1);
        }

        public static Complex KleinInvariantJ(Complex z)
        {
            if (z.Imaginary <= 0) return Complex.Indeterminate;
            return
                Pow(
                    Pow(EllipticTheta2(0, Exp(Constants.Pi * Constants.I * z)), 8) +
                    Pow(EllipticTheta3(0, Exp(Constants.Pi * Constants.I * z)), 8) +
                    Pow(EllipticTheta4(0, Exp(Constants.Pi * Constants.I * z)), 8), 3) /
                (54.0 *
                 Pow(
                     EllipticTheta2(0, Exp(Constants.Pi * Constants.I * z)) *
                     EllipticTheta3(0, Exp(Constants.Pi * Constants.I * z)) *
                     EllipticTheta4(0, Exp(Constants.Pi * Constants.I * z)), 8));
        }

        public static Complex ModularLambda(Complex z)
        {
            if (z.Imaginary <= 0) return Complex.Indeterminate;
            return 16.0 * Exp(Constants.I * Constants.Pi * z) *
                   ProductInf(
                       k =>
                           Pow(
                               (1.0 + Exp(2.0 * k * Constants.Pi * Constants.I * z)) /
                               (1.0 + Exp((2.0 * k - 1.0) * Constants.Pi * Constants.I * z)), 8), 1);
        }

        /*public static Complex NevilleThetaC(Complex z, Complex m)
		{
			return ((Sqrt(2 * Constant.Pi) * NthRoot(EllipticNomeQ(m), 4)) / (NthRoot(m, 4) * Sqrt(EllipticK(m)))) *
				   SumInf(
					   k =>
						   Pow(EllipticNomeQ(m), k * (k + 1)) *
						   Cos(((2 * k + 1) * Constant.Pi * z) / (2 * EllipticK(m))), 0);
		}*/

        #endregion

        #region Fonctions de conversion

        [MathFunc("Convertit un nombre décimal en hexadécimal (base 16).")]
        public static Complex Hex(Complex dec)
        {
            return new Complex(dec.Real, dec.Imaginary, ResultView.Hexadecimal);
        }

        [MathFunc("Convertit un nombre décimal en binaire (base 2).")]
        public static Complex Bin(Complex dec)
        {
            return new Complex(dec.Real, dec.Imaginary, ResultView.Binary);
        }

        [MathFunc("Convertit un nombre décimal en octal (base 8).")]
        public static Complex Oct(Complex dec)
        {
            return new Complex(dec.Real, dec.Imaginary, ResultView.Octal);
        }

        [MathFunc("Convertit des secondes en minutes.")]
        public static Complex SecMin(Complex sec)
        {
            return sec / 60.0;
        }

        [MathFunc("Convertit des minutes en secondes.")]
        public static Complex MinSec(Complex min)
        {
            return min * 60.0;
        }

        [MathFunc("Convertit une température de Celsius à Fahrenheit.")]
        public static Complex CelsFahr(Complex cels)
        {
            return 32.0 + (cels * 9) / 5.0;
        }

        [MathFunc("Convertit une température de Fahrenheit à Celsius.")]
        public static Complex FahrCels(Complex fahr)
        {
            return (fahr - 32.0) / 1.8;
        }

        [MathFunc("Ajoute la TVA à un montant.")]
        public static Complex TTC(Complex montant)
        {
            return 1.2 * montant;
        }

        [MathFunc("Enlève la TVA à un montant.")]
        public static Complex HT(Complex montant)
        {
            return montant / 1.2;
        }

        [MathFunc("Convertit des mètres en pieds.")]
        public static Complex MFt(Complex m)
        {
            return m * 3.2808;
        }

        #endregion

        #region Intégration

        [MathFunc(true)]
        public static Complex IntegrateEx(Func<Complex, Complex> f, Complex start, Complex end, int method = 1,
            double times = 100000)
        {
            if (times <= 0) times = 100000;
            if (times % 2 != 0) times++;
            Complex ans = 0;
            if (method == 1)
            {
                // ?
                var sizeOfInterval = ((end - start) / times);
                Complex sum = f(start);
                for (Complex i = 1; i < times; i += 2)
                    sum += 4 * f(start + sizeOfInterval * i);
                for (Complex i = 2; i < times - 1; i += 2)
                    sum += 2 * f(start + sizeOfInterval * i);
                sum += f(end);
                ans = sum * sizeOfInterval / 3;
            }
            else if (method == 2)
            {
                // Méthode des trapèzes
                Complex h = (end - start) / times;
                Complex res = (f(start) + f(end)) / 2;
                for (int i = 1; i < times; i++)
                {
                    res += f(start + i * h);
                }
                ans = h * res;
            }
            else if (method == 3)
            {
                // Méthode de Simpson
                Complex step = (end - start) / times;
                Complex factor = step / 3;
                Complex offset = step;
                int m = 4;
                Complex sum = f(start) + f(end);
                for (int i = 0; i < times - 1; i++)
                {
                    sum += m * f(start + offset);
                    m = 6 - m;
                    offset += step;
                }
                ans = factor * sum;
            }
            else if (method == 4)
            {
                // Méthode 3 points
                Complex midpoint = (end + start) / 2;
                ans = (end - start) / 6 * (f(start) + f(end) + (4 * f(midpoint)));
            }
            else if (method == 5)
            {
                // Méthode des trapèzes - 2 points
                ans = (end - start) / 2 * (f(start) + f(end));
            }
            else if (method == 6)
            {
                // Méthode des trapèzes (MathNetNumerics)
                Complex step = (end - start) / times;
                Complex offset = step;
                Complex sum = 0.5 * (f(start) + f(end));
                for (int i = 0; i < times - 1; i++)
                {
                    sum += f(start + offset);
                    offset += step;
                }
                ans = step * sum;
            }
            /*else if(method == 7)
			{
				// YAMP
				var y = Values;

				if (x.Length != y.Length)
					throw new YAMPDifferentLengthsException(x.Length, y.Length);

				var sum = 0.0;

				for (var i = 1; i < N - 1; i += 2)
					sum += (x[i + 2].Re - x[i].Re) * (y[i].Re + 4.0 * y[i + 1].Re + y[i + 2].Re);

				ans = sum / 6.0;
			}*/
            return Round(ans, 15);
        }

        public static Complex TestIntg()
        {
            return CalculateLimit(x => (x.Square() - 1) / (x - 1), 1);
        }

        public static Complex Integrate(Func<Complex, Complex> function, Complex lowerLimit, Complex upperLimit,
            IntegrationAlgorithm integrationAlgorithm = IntegrationAlgorithm.SimpsonsRule,
            double numberOfIntervals = 100000) // 0.000001M
        {
            double sum = 0;

            switch (integrationAlgorithm)
            {
                case IntegrationAlgorithm.RectangleMethod:
                    {
                        var sizeOfInterval = ((upperLimit - lowerLimit) / numberOfIntervals);

                        for (var i = 0; i < numberOfIntervals; i++)
                            sum += function(lowerLimit + sizeOfInterval * i) * sizeOfInterval;

                        return sum;
                    }
                case IntegrationAlgorithm.TrapezoidalRule:
                    {
                        var sizeOfInterval = ((upperLimit - lowerLimit) / numberOfIntervals);

                        sum = function(lowerLimit) + function(upperLimit);

                        for (var i = 1; i < numberOfIntervals; i++)
                            sum += 2 * function(lowerLimit + i * sizeOfInterval);

                        return sum * sizeOfInterval / 2;
                    }
                case IntegrationAlgorithm.SimpsonsRule:
                    {
                        /*var sizeOfInterval = ((upperLimit - lowerLimit) / numberOfIntervals);

                        sum = function(lowerLimit);

                        for (var i = 1; i < numberOfIntervals; i += 2)
                            sum += 4 * function(lowerLimit + sizeOfInterval * i);

                        for (var i = 2; i < numberOfIntervals - 1; i += 2)
                            sum += 2 * function(lowerLimit + sizeOfInterval * i);

                        sum += function(upperLimit);

                        return sum * sizeOfInterval / 3;*/
                        /*var iterations = 10000;
                        return Abs(upperLimit - lowerLimit) *
                               Enumerable.Range(0, iterations)
                                   .Select(
                                       index =>
                                           new Tuple<Complex, Complex>(
                                               lowerLimit + index * ((lowerLimit - upperLimit) / iterations),
                                               ((lowerLimit - upperLimit) / iterations))).Sum(subdomain => (function(lowerLimit) + 4 * function((lowerLimit + upperLimit) / 2) + function(upperLimit) / 6) / iterations);*/
                        Complex h = (upperLimit - lowerLimit) / numberOfIntervals;
                        Complex sum1 = function(lowerLimit + h / 2);
                        Complex sum2 = 0;
                        for (Complex i = 1; i < numberOfIntervals; i++)
                        {
                            sum1 += function(lowerLimit + h * i + h / 2.0);
                            sum2 += function(lowerLimit + h * i);
                        }
                        return (h / 6.0) * (function(lowerLimit) + function(upperLimit) + 4 * sum1 + 2 * sum2);
                    }
                default:
                    return 0;
            }
        }

        public enum IntegrationAlgorithm
        {
            RectangleMethod,
            TrapezoidalRule,
            SimpsonsRule
        }

        #endregion

        #region Limites
        [MathFunc(true)]
        public static Complex CalculateLimit(Func<Complex, Complex> f, Complex approach)
        {
            Complex below = limitFromBelow(f, approach);
            Complex above = limitFromAbove(f, approach);
            return below == above ? below : Complex.NaN;
        }
        [MathFunc(true)]
        public static Complex limitFromBelow(Func<Complex, Complex> function, Complex approach)
        {
            for (Complex d = approach - 10; d <= approach; d = approach
                    - ((approach - d) / 10))
            {
                if (function(d) == Complex.PositiveInfinity)
                {
                    return Complex.PositiveInfinity;
                }
                else if (function(d) == Complex.NegativeInfinity)
                {
                    return Complex.NegativeInfinity;
                }
                else if (Complex.IsNaN(function(d)))
                {
                    return function(approach + ((approach - d) * 10));
                }
                else
                {
                    if (d == approach)
                    {
                        return function(d);
                    }
                    else if (approach - d < 0.00000000001)
                    {
                        d = approach;
                    }

                }
            }
            return Complex.NaN;
        }
        [MathFunc(true)]
        public static Complex limitFromAbove(Func<Complex, Complex> function, Complex approach)
        {
            for (Complex d = approach + 10; d <= approach; d = approach
                    - ((approach - d) / 10))
            {
                if (function(d) == Complex.PositiveInfinity)
                {
                    return Complex.PositiveInfinity;
                }
                else if (function(d) == Complex.NegativeInfinity)
                {
                    return Complex.NegativeInfinity;
                }
                else if (Complex.IsNaN(function(d)))
                {
                    return function(approach + ((approach - d) * 10));
                }
                else
                {
                    if (d == approach)
                    {
                        return function(d);
                    }
                    else if (d - approach < 0.00000000001)
                    {
                        d = approach;
                    }

                }
            }
            return Complex.NaN;
        }
        #endregion

        #region Dérivation
        [MathFunc(true)]
        public static Complex Derivate(Func<Complex, Complex> f, Complex x, double precision = double.Epsilon)
        {
            return (f(x - 2 * precision) - 8 * f(x - precision) - f(x + 2 * precision)) / ((2 * precision) * 6);
        }
        #endregion

        private static Random _rnd = new Random();

        public static bool UseDegrees;

        private static Complex DegToRad(Complex deg)
        {
            return (Constants.Pi / 180) * deg;
        }

        private static Complex RadToDeg(Complex rad)
        {
            return rad * (180.0 / Constants.Pi);
        }

        private static Complex Angle(Complex input)
        {
            return UseDegrees ? DegToRad(input) : input;
        }

        [MathFunc(true)]
        public static Complex Sum(Func<Complex, Complex> expr, Complex start, Complex end)
        {
            Complex x = 0;

            for (var k = start; k <= end; k++)
            {
                x += expr(k);
            }

            return x;
        }

        [MathFunc(true)]
        public static Complex SumInf(Func<Complex, Complex> expr, Complex start, double prec = 0.00000000000001)
        {
            Complex x = 0;

            var n = start;
            Parallel.Invoke(() =>
            {
                while (true)
                {
                    var nw = expr(n);
                    var mw = expr(n + 10.0);
                    if (Math.Abs(nw.Module - mw.Module) <= prec)
                    {
                        break;
                    }
                    x += nw;

                    n++;
                }
            });
            return x;
        }

        [MathFunc(true)]
        public static Complex Product(Func<Complex, Complex> expr, Complex start, Complex end)
        {
            Complex x = 1.0;

            for (var k = start; k <= end; k++)
            {
                x *= expr(k);
            }

            return x;
        }

        [MathFunc(true)]
        public static Complex ProductInf(Func<Complex, Complex> expr, Complex start, double prec = 0.00000000000001)
        {
            Complex x = 1.0;

            var n = start;
            Parallel.Invoke(() =>
            {
                while (true)
                {
                    var nw = expr(n);
                    var mw = expr(n + 10.0);
                    if (Math.Abs(nw.Module - mw.Module) <= prec)
                    {
                        break;
                    }
                    x *= nw;

                    n++;
                }
            });
            return x;
        }
    }


    [AttributeUsage(AttributeTargets.Method)]
    public sealed class MathFunc : Attribute
    {
        public string Description { get; set; }
        public string Name { get; set; }

        public bool Hide { get; set; }

        public MathFunc(string d)
        {
            Description = d;
            Name = "";
            Hide = false;
        }

        public MathFunc(bool h = false)
        {
            Description = "";
            Name = "";
            Hide = h;
        }
    }
}