using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;

namespace IMPression
{
    public class ExcelFunctions
    {
        /// <summary>
        /// Coupon payment frequency per year
        /// </summary>
        public enum PaymentFrequency
        {
            Annual = 1,
            Semiannual = 2,
            Quarterly = 4
        }

        /// <summary>
        /// Type of day count basis
        /// </summary>
        public enum DayCountBasis
        {
            /// <summary>
            /// US (NASD) 30/360
            /// </summary>
            US_NASD = 0,

            /// <summary>
            /// Actual/actual
            /// </summary>
            Actual = 1,

            /// <summary>
            /// Actual/360
            /// </summary>
            Actual_360 = 2,

            /// <summary>
            /// Actual/365
            /// </summary>
            Actual_365 = 3,

            /// <summary>
            /// European 30/360
            /// </summary>
            European_30_360 = 4
        }

        public enum AggregateFunction
        {
            AVERAGE = 1,
            COUNT = 2,
            COUNTA = 3,
            MAX = 4,
            MIN = 5,
            PRODUCT = 6,
            STDEV_S = 7,
            STDEV_P = 8,
            SUM = 9,
            VAR_S = 10,
            VAR_P = 11,
            MEDIAN = 12,
            MODE_SNGL = 13,
            LARGE = 14,
            SMALL = 15,
            PERCENTILE_INC = 16,
            QUARTILE_INC = 17,
            PERCENTILE_EXC = 18,
            QUARTILE_EXC = 19
        }

        public enum AggregateOptions
        {
            /// <summary>
            /// Ignore nested SUBTOTAL and AGGREGATE functions
            /// </summary>
            IgnoreNestedSUBTOTALandAGGREGATE = 0,

            /// <summary>
            /// Ignore hidden rows, nested SUBTOTAL and AGGREGATE functions
            /// </summary>
            IgnoreHiddenRowsAndNested = 1,

            /// <summary>
            /// Ignore error values, nested SUBTOTAL and AGGREGATE functions
            /// </summary>
            IgnoreErrorValuesAndNested = 2,

            /// <summary>
            /// Ignore hidden rows, error values, nested SUBTOTAL and AGGREGATE functions
            /// </summary>
            IgnoreHiddenRowsAndErrorValuesAndNested = 3,

            /// <summary>
            /// Ignore nothing
            /// </summary>
            IgnoreNothing = 4,

            /// <summary>
            /// Ignore hidden rows
            /// </summary>
            IgnoreHiddenRows = 5,

            /// <summary>
            /// Ignore error values
            /// </summary>
            IgnoreErrorValues = 6,

            /// <summary>
            /// Ignore hidden rows and error values
            /// </summary>
            IgnoreHiddenRowsAndErrorValues = 7
        }

        /// <summary>
        /// Returns the absolute value of a number
        /// </summary>
        /// <param name="number">The real number of which you want the absolute value.</param>
        /// <returns>The absolute value of a number. The absolute value of a number is the number without its sign.</returns>
        public static double ABS(double number)
        {
            return Math.Abs(number);
        }


        /// <summary>
        /// Returns the accrued interest for a security that pays periodic interest
        /// </summary>
        /// <param name="issue">The security's issue date.</param>
        /// <param name="first_interest">The security's first interest date.</param>
        /// <param name="settlement">The security's settlement date. The security settlement date is the date after the issue date when the security is traded to the buyer.</param>
        /// <param name="rate">The security's annual coupon rate.</param>
        /// <param name="par">The security's par value.</param>
        /// <param name="frequency">The number of coupon payments per year.</param>
        /// <param name="basis">The type of day count basis to use.</param>
        /// <param name="calc_method">A logical value that specifies the way to calculate the total accrued interest when the date of settlement is later than the date of first_interest. A value of TRUE (1) returns the total accrued interest from issue to settlement. A value of FALSE (0) returns the accrued interest from first_interest to settlement. If you do not enter the argument, it defaults to TRUE.</param>
        /// <returns>The accrued interest for a security that pays periodic interest.</returns>
        public static double ACCRINT(
            DateTime issue,
            DateTime first_interest,
            DateTime settlement,
            double rate,
            double par,
            PaymentFrequency frequency,
            DayCountBasis basis = DayCountBasis.US_NASD,
            bool calc_method = true)
        {
            if (rate <= 0) throw new ArgumentOutOfRangeException(nameof(rate), "'rate' must be greater than 0");
            if (par <= 0) throw new ArgumentOutOfRangeException(nameof(par), "'par' must be greater than 0");

            if (issue >= settlement)
                throw new ArgumentException("The issue date must be before the settlement date.", nameof(issue));


            throw new NotImplementedException();
            //return par * (rate / (double)frequency) * Functions.Sum(i => 0, 1, 2).Real;
        }

        /// <summary>
        /// Returns the accrued interest for a security that pays interest at maturity
        /// </summary>
        /// <param name="issue">The security's issue date.</param>
        /// <param name="settlement">The security's maturity date.</param>
        /// <param name="rate">The security's annual coupon rate.</param>
        /// <param name="par">The security's par value.</param>
        /// <param name="basis">The type of day count basis to use.</param>
        /// <returns>The accrued interest for a security that pays interest at maturity.</returns>
        public static double ACCRINTM(
            DateTime issue,
            DateTime settlement,
            double rate,
            double par,
            DayCountBasis basis = DayCountBasis.US_NASD)
        {
            if (rate <= 0) throw new ArgumentOutOfRangeException(nameof(rate), "'rate' must be greater than 0");
            if (par <= 0) throw new ArgumentOutOfRangeException(nameof(par), "'par' must be greater than 0");

            if (issue >= settlement)
                throw new ArgumentException("The issue date must be before the settlement date.", nameof(issue));


            throw new NotImplementedException();
            //return par * (rate / (double)frequency) * Functions.Sum(i => 0, 1, 2).Real;
        }

        /// <summary>
        /// Returns the arccosine of a number
        /// </summary>
        /// <param name="number">The cosine of the angle you want and must be from -1 to 1.</param>
        /// <returns>The arccosine, or inverse cosine, of a number.</returns>
        public static double ACOS(double number)
        {
            if (number < -1 || number > 1)
                throw new ArgumentOutOfRangeException(nameof(number), "The cosine of the angle must be from -1 to 1.");
            return Math.Acos(number);
        }

        /// <summary>
        /// Returns the inverse hyperbolic cosine of a number
        /// </summary>
        /// <param name="number">The hyperbolic cosine of the angle you want and must be equal to or greater than 1.</param>
        /// <returns>The inverse hyperbolic cosine of a number.</returns>
        public static double ACOSH(double number)
        {
            if (number < 1)
                throw new ArgumentOutOfRangeException(nameof(number),
                    "The hyperbolic cosine of the angle must be equal to or greater than 1.");
            return Functions.ArcCosh(number);
        }

        /// <summary>
        /// Returns the arccotangent of a number
        /// </summary>
        /// <param name="number">The cotangent of the angle you want.</param>
        /// <returns>The inverse cotangent of a number.</returns>
        public static double ACOT(double number)
        {
            return Functions.ArcCot(number);
        }

        /// <summary>
        /// Returns the hyperbolic arccotangent of a number
        /// </summary>
        /// <param name="number">The hyperbolic cotangent of the angle you want and absolute value must be greater than 1.</param>
        /// <returns>The inverse hyperbolic cotangent of a number.</returns>
        public static double ACOTH(double number)
        {
            if (Math.Abs(number) <= 1)
                throw new ArgumentOutOfRangeException(nameof(number),
                    "The absolute value of the hyperbolic cotangent of the angle must greater than 1.");
            return Functions.ArcCoth(number);
        }


        /// <summary>
        /// Returns an aggregate in a list or database
        /// </summary>
        /// <param name="function_num">Which function to use.</param>
        /// <param name="options">Which values to ignore in the evaluation range for the function.</param>
        /// <param name="refs">The numeric arguments for which you want the aggregate value.</param>
        /// <returns>An aggregate in a list or database. The AGGREGATE function can apply different aggregate functions to a list or database with the option to ignore hidden rows and error values.</returns>
        public static double AGGREGATE(AggregateFunction function_num, AggregateOptions options, params double[] refs)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns TRUE if all of its arguments are TRUE
        /// </summary>
        /// <param name="logical">Conditions you want to evaluate.</param>
        /// <returns>TRUE if all its arguments evaluate to TRUE; returns FALSE if one or more arguments evaluate to FALSE.</returns>
        public static bool AND(params bool[] logical)
        {
            return logical.All(x => x);
        }

        /// <summary>
        /// Converts a Roman number to Arabic, as a number
        /// </summary>
        /// <param name="text">The Roman numeral.</param>
        /// <returns>The resulting Arabic numeral.</returns>
        public static int ARABIC(string text)
        {
            text =
                text.Replace("CM", "DCCCC")
                    .Replace("CD", "CCCC")
                    .Replace("XC", "LXXXX")
                    .Replace("XL", "XXXX")
                    .Replace("IX", "VIIII")
                    .Replace("IV", "IIII").ToUpper().Trim();

            var result = 0;
            var negative = false;

            var numbers = new Dictionary<char, int>
            {
                {'I', 1},
                {'V', 5},
                {'X', 10},
                {'L', 50},
                {'C', 100},
                {'D', 500},
                {'M', 1000}
            };

            foreach (var current in text)
            {
                if (current == '-') negative = true;
                else if (numbers.ContainsKey(current)) result += numbers[current];
                else throw new ArgumentException("Invalid char: " + current, nameof(text));
            }

            return negative ? -result : result;
        }

        /// <summary>
        /// Changes full-width (double-byte) English letters or katakana within a character string to half-width (single-byte) characters
        /// </summary>
        /// <param name="text">The text or a reference to a cell that contains the text you want to change. If text does not contain any full-width letters, text is not changed.</param>
        /// <returns>The text with full-width (double-byte) characters replaced by half-width (single-byte) characters.</returns>
        public static string ASC(string text)
        {
            return text.Normalize(NormalizationForm.FormKC);
        }

        /// <summary>
        /// Returns the arcsine of a number
        /// </summary>
        /// <param name="number">The sine of the angle you want and must be from -1 to 1.</param>
        /// <returns>The arcsine, or inverse sine, of a number.</returns>
        public static double ASIN(double number)
        {
            if (number < -1 || number > 1)
                throw new ArgumentOutOfRangeException(nameof(number), "The sine of the angle must be from -1 to 1.");
            return Math.Acos(number);
        }

        /// <summary>
        /// Returns the inverse hyperbolic sine of a number
        /// </summary>
        /// <param name="number">The hyperbolic sine of the angle you want.</param>
        /// <returns>The inverse hyperbolic sine of a number.</returns>
        public static double ASINH(double number)
        {
            return Functions.ArcSinh(number);
        }

        /// <summary>
        /// Returns the arctangent of a number
        /// </summary>
        /// <param name="number">The tangent of the angle you want.</param>
        /// <returns>The arctangent, or inverse tangent, of a number.</returns>
        public static double ATAN(double number)
        {
            return Math.Atan(number);
        }

        /// <summary>
        /// Returns the arctangent from x- and y-coordinates
        /// </summary>
        /// <param name="x_num">The x-coordinate of the point.</param>
        /// <param name="y_num">The y-coordinate of the point.</param>
        /// <returns>The arctangent, or inverse tangent, of the specified x- and y-coordinates.</returns>
        public static double ATAN2(double x_num, double y_num)
        {
            if (x_num == 0 && y_num == 0) throw new DivideByZeroException();
            return Math.Atan2(x_num, y_num);
        }

        /// <summary>
        /// Returns the inverse hyperbolic tangent of a number
        /// </summary>
        /// <param name="number">The hyperbolic tangent of the angle you want and must be from -1 to 1.</param>
        /// <returns>The inverse hyperbolic tangent of a number.</returns>
        public static double ATANH(double number)
        {
            if (number < -1 || number > 1)
                throw new ArgumentOutOfRangeException(nameof(number),
                    "The hyperbolic tangent of the angle must be from -1 to 1.");
            return Functions.ArcTanh(number);
        }

        /// <summary>
        /// Returns the average of the absolute deviations of data points from their mean
        /// </summary>
        /// <param name="numbers">Set of numbers.</param>
        /// <returns>The average of the absolute deviations of data points from their mean. AVEDEV is a measure of the variability in a data set.</returns>
        public static double AVEDEV(params double[] numbers)
        {
            return numbers.Sum(x => Math.Abs(x - numbers.Average())) / numbers.Length;
        }

        /// <summary>
        /// Returns the average of its arguments
        /// </summary>
        /// <param name="numbers">Set of numbers.</param>
        /// <returns>The average (arithmetic mean) of the arguments.</returns>
        public static double AVERAGE(params double[] numbers)
        {
            return numbers.Average();
        }

        /// <summary>
        /// Returns the average of its arguments, including numbers, text, and logical values
        /// </summary>
        /// <param name="numbers">Set of numbers.</param>
        /// <returns>The average (arithmetic mean) of the arguments.</returns>
        public static double AVERAGEA(params double[] numbers)
        {
            return
                numbers.Where(x => x != double.NaN && x != double.NegativeInfinity && x != double.PositiveInfinity)
                    .Average();
        }

        /// <summary>
        /// Returns the average (arithmetic mean) of all the cells in a range that meet a given criteria
        /// </summary>
        /// <param name="numbers">Set of numbers.</param>
        /// <param name="criteria">The criteria that defines which cells are averaged.</param>
        /// <returns>The average (arithmetic mean) of all cells in a range that meet a given criteria.</returns>
        public static double AVERAGEIF(Func<double, bool> criteria, params double[] numbers)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the average (arithmetic mean) of all cells that meet multiple criteria
        /// </summary>
        /// <param name="numbers">Set of numbers and criterias.</param>
        /// <returns>The average (arithmetic mean) of all cells that meet multiple criteria.</returns>
        public static double AVERAGEIFS(params Tuple<double[], Func<double, bool>>[] numbers)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a number to text, using the ß (baht) currency format
        /// </summary>
        /// <param name="number">A number you want to convert to text.</param>
        /// <returns>The number converted to Thai text with the suffix "Baht."</returns>
        public static string BAHTTEXT(double number)
        {
            if (number == 0) return "";
            var result = "";
            var s1 = "";
            var s2 = "";
            var s3 = "";

            var s = Convert.ToString(number);
            var position = s.IndexOf(".");
            if (position >= 0)
            {
                s1 = s.Substring(0, position);
                s3 = s.Substring(position + 1);
                if (s3 == "00")
                {
                    s3 = "";
                }
            }
            else
            {
                s1 = s;
                s3 = "";
            }
            var L = s1.Length;
            if (L > 6)
            {
                s2 = s1.Substring(L - 6);
                s1 = s1.Substring(0, L - 6);
            }
            else
            {
                s2 = s1;
                s1 = "";
            }

            if (s1 != "" && Convert.ToInt32(s1) == 0) s1 = "";
            if (s2 != "" && Convert.ToInt32(s2) == 0) s2 = "";


            var speak =  new Func<string, bool, string>(delegate (string s4, bool b)
            {
                if (s4 == "") return ("");
                var L2 = s4.Length;
                if (b)
                {
                    if (L2 == 1)
                    {
                        s4 += "0";
                        L2++;
                    }
                    if (L2 > 2)
                    {
                        s4 = s4.Substring(0, 2);
                        L2 = 2;
                    }
                }
                var result2 = "";
                for (var i = 0; i < (b ? 2 : L2); i++)
                {
                    if (!b && s4.Substring(i, 1) == "-") result2 += "ติดลบ";
                    else
                    {
                        var c = Convert.ToInt32(s4.Substring(i, 1));
                        if (i == L2 - 1 && c == 1)
                        {
                            if (L2 == 1 && !b) return ("หนึ่ง");
                            if ((b || L2 > 1) && s4.Substring(b ? 0 : L2 - 1, 1) == "0") result2 += "หนึ่ง";
                            else result2 += "เอ็ด";
                        }
                        else if (i == L2 - 2 && c == 2) result2 += "ยี่สิบ";
                        else if (i == L2 - 2 && c == 1) result2 += "สิบ";
                        else if (c != 0)
                            result2 +=
                                new[] {"", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า"}[c] +
                                new[] {"", "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน"}[L - i];
                    }
                }
                return result2;
            });
            result = "";
            if (s1.Length > 0)
            {
                result += speak(s1, false) + "ล้าน";
            }
            if (s2.Length > 0)
            {
                result += speak(s2, false) + "บาท";
            }
            if (s3.Length > 0)
            {
                result += speak(s3, true) + "สตางค์";
            }
            else
            {
                result = result + "ถ้วน";
            }
            return result;
        }

        /// <summary>
        /// Converts a number into a text representation with the given radix (base)
        /// </summary>
        /// <param name="number">The number that you want to convert. Must be an integer greater than or equal to 0 and less than 2^53.</param>
        /// <param name="radix">The base radix that you want to convert the number into. Must be an integer greater than or equal to 2 and less than or equal to 36.</param>
        /// <param name="min_length">The minimum length of the returned string. Must be an integer greater than or equal to 0.</param>
        /// <returns>The number converted to the specified base.</returns>
        public static string BASE(int number, int radix, int min_length = 0)
        {
            if(radix < 2 || radix > 36) throw new ArgumentOutOfRangeException(nameof(radix), "The radix must be greater than or equal to 2 and less than or equal to 36.");
            if(number < 0 || number >= Math.Pow(2, 53)) throw new ArgumentOutOfRangeException(nameof(number), "The number must be greater than or equal to 0 and less than 2^53.");
            if(min_length < 0) throw new ArgumentOutOfRangeException(nameof(min_length), "The minimum length must be greater than or equal to 0");

            var result = Convert.ToString(number, radix);
            if (result.Length < min_length) result = result.PadLeft(min_length, '0');
            return result;
        }

        /// <summary>
        /// Returns the modified Bessel function In(x)
        /// </summary>
        /// <param name="x">The value at which to evaluate the function.</param>
        /// <param name="n">The order of the Bessel function.</param>
        /// <returns>The modified Bessel function In(x)</returns>
        public static double BESSELI(double x, int n)
        {
            return Functions.BesselI(n, x);
        }

        /// <summary>
        /// Returns the Bessel function Jn(x)
        /// </summary>
        /// <param name="x">The value at which to evaluate the function.</param>
        /// <param name="n">The order of the Bessel function.</param>
        /// <returns>The Bessel function Jn(x)</returns>
        public static double BESSELJ(double x, int n)
        {
            return Functions.BesselJ(n, x);
        }

        /// <summary>
        /// Returns the modified Bessel function Kn(x)
        /// </summary>
        /// <param name="x">The value at which to evaluate the function.</param>
        /// <param name="n">The order of the Bessel function.</param>
        /// <returns>The modified Bessel function Kn(x)</returns>
        public static double BESSELK(double x, int n)
        {
            return Functions.BesselK(n, x);
        }

        /// <summary>
        /// Returns the Bessel function Yn(x)
        /// </summary>
        /// <param name="x">The value at which to evaluate the function.</param>
        /// <param name="n">The order of the Bessel function.</param>
        /// <returns>The Bessel function Yn(x)</returns>
        public static double BESSELY(double x, int n)
        {
            return Functions.BesselY(n, x);
        }

        /// <summary>
        /// Converts a binary number to decimal.
        /// </summary>
        /// <param name="number">The binary number you want to convert.</param>
        /// <returns>The binary number converted to decimal.</returns>
        public static int BIN2DEC(string number)
        {
            if (number == "") return 0;
            return Convert.ToInt32(number, 2);
        }

        /// <summary>
        /// Converts a binary number to hexadecimal.
        /// </summary>
        /// <param name="number">The binary number you want to convert.</param>
        /// <param name="places">The number of characters to use. If places is omitted, BIN2HEX uses the minimum number of characters necessary.</param>
        /// <returns>The binary number converted to hexadecimal.</returns>
        public static string BIN2HEX(string number, int places = 0)
        {
            return BIN2DEC(number).ToString("X").PadLeft(places, '0');
        }

        /// <summary>
        /// Converts a binary number to octal.
        /// </summary>
        /// <param name="number">The binary number you want to convert.</param>
        /// <param name="places">The number of characters to use. If places is omitted, BIN2HEX uses the minimum number of characters necessary.</param>
        /// <returns>The binary number converted to octal.</returns>
        public static string BIN2OCT(string number, int places = 0)
        {
            return Convert.ToString(BIN2DEC(number), 8).PadLeft(places, '0');
        }

        /// <summary>
        /// Returns a bitwise 'AND' of two numbers.
        /// </summary>
        /// <param name="number1">First number. Must greater than or equal to 0.</param>
        /// <param name="number2">Second number. Must greater than or equal to 0.</param>
        /// <returns>Bitwise 'AND' of the specified numbers</returns>
        public static int BITAND(int number1, int number2)
        {
            if (number1 < 0)
                throw new ArgumentOutOfRangeException(nameof(number1), "number1 must be greater than or equal to 0");
            if (number2 < 0)
                throw new ArgumentOutOfRangeException(nameof(number2), "number2 must be greater than or equal to 0");

            return number1 & number2;
        }

        /// <summary>
        /// Returns a number shifted left by the specified number of bits.
        /// </summary>
        /// <param name="number1">First number. Must greater than or equal to 0.</param>
        /// <param name="shift_amount">Shift amount</param>
        /// <returns>The number shifted left by the specified number of bits</returns>
        public static int BITLSHIFT(int number1, int shift_amount)
        {
            if (number1 < 0)
                throw new ArgumentOutOfRangeException(nameof(number1), "number1 must be greater than or equal to 0");

            return number1 << shift_amount;
        }

        /// <summary>
        /// Returns a bitwise 'OR' of two numbers.
        /// </summary>
        /// <param name="number1">First number. Must greater than or equal to 0.</param>
        /// <param name="number2">Second number. Must greater than or equal to 0.</param>
        /// <returns>Bitwise 'OR' of the specified numbers</returns>
        public static int BITOR(int number1, int number2)
        {
            if (number1 < 0)
                throw new ArgumentOutOfRangeException(nameof(number1), "number1 must be greater than or equal to 0");
            if (number2 < 0)
                throw new ArgumentOutOfRangeException(nameof(number2), "number2 must be greater than or equal to 0");

            return number1 | number2;
        }

        /// <summary>
        /// Returns a <paramref name="number"/> shifted right by the specified <paramref name="shift_amount"/> of bits.
        /// </summary>
        /// <param name="number">First number. Must greater than or equal to 0.</param>
        /// <param name="shift_amount">Shift amount</param>
        /// <returns><paramref name="number">The number</paramref> shifted right by the specified number of bits</returns>
        public static int BITRSHIFT(int number, int shift_amount)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(nameof(number), "number1 must be greater than or equal to 0");
            return number >> shift_amount;
        }

        /// <summary>
        /// Returns a bitwise 'XOR' of two numbers.
        /// </summary>
        /// <param name="number1">First number. Must greater than or equal to 0.</param>
        /// <param name="number2">Second number. Must greater than or equal to 0.</param>
        /// <returns>Bitwise 'XOR' of the specified numbers</returns>
        public static int BITXOR(int number1, int number2)
        {
            if (number1 < 0)
                throw new ArgumentOutOfRangeException(nameof(number1), "number1 must be greater than or equal to 0");
            if (number2 < 0)
                throw new ArgumentOutOfRangeException(nameof(number2), "number2 must be greater than or equal to 0");

            return number1 ^ number2;
        }

        /// <summary>
        /// Returns <paramref name="number"/> rounded up, away from zero, to the nearest multiple of <paramref name="significance"/>.
        /// </summary>
        /// <param name="number">The value you want to round.</param>
        /// <param name="significance">The multiple to which you want to round.</param>
        /// <returns><paramref name="number"/> rounded up</returns>
        public static double CEILING(double number, double significance)
        {
            return significance * Math.Ceiling(number / significance);
        }

        /// <summary>
        /// Returns <paramref name="number"/> rounded up, away from zero, to the nearest multiple of <paramref name="significance"/>.
        /// </summary>
        /// <param name="number">The value you want to round.</param>
        /// <param name="significance">The multiple to which you want to round.</param>
        /// <param name="mode">For negative numbers, controls whether <paramref name="number"/> is rounded toward or away from zero. Does not affect positive numbers.</param>
        /// <returns><paramref name="number"/> rounded up</returns>
        public static double CEILING_MATH(double number, double significance = 1, int mode = 1)
        {
            if (number < 0 && mode == -1) return significance * Math.Floor(number / significance);
            return significance * Math.Ceiling(number / significance);
        }

        /// <summary>
        /// Returns a <paramref name="number"/> that is rounded up to the nearest integer or to the nearest multiple of <paramref name="significance"/>. Regardless of the sign of the number, the number is rounded up. However, if the number or the significance is zero, zero is returned.
        /// </summary>
        /// <param name="number">The value you want to round.</param>
        /// <param name="significance">The multiple to which you want to round.</param>
        /// <returns><paramref name="number"/> rounded up</returns>
        public static double CEILING_PRECISE(double number, double significance = 1)
        {
            if (significance == 0) return 0;
            if(significance < 0) return significance * Math.Floor(number / significance);
            return significance * Math.Ceiling(number / significance);
        }

        /// <summary>
        /// Returns the character specified by a <paramref name="number"/>.
        /// </summary>
        /// <param name="number">A number between 1 and 255 specifying which character you want. The character is from the character set used by your computer.</param>
        /// <returns>The character specified by the <paramref name="number"/>.</returns>
        public static char CHAR(int number)
        {
            return (char) number;
        }

        /// <summary>
        /// Chooses a value from a list of values.
        /// </summary>
        /// <param name="index_num">Specifies which value argument is selected.</param>
        /// <param name="values">Value arguments from which CHOOSE selects a value or an action to perform based on index_num.</param>
        /// <returns>The value at the specified index.</returns>
        public static object CHOOSE(int index_num, params object[] values)
        {
            return values[index_num];
        }

        /// <summary>
        /// Removes all nonprintable characters from text.
        /// </summary>
        /// <param name="text">Any information from which you want to remove nonprintable characters.</param>
        /// <returns>The specified <paramref name="text"/> with all nonprintable characters removed.</returns>
        public static string CLEAN(string text)
        {
            //TODO: Implement this better
            return new string(text.Where(x => (int)x >= 32).ToArray());
        }

        /// <summary>
        /// Returns a numeric code for the specified <paramref name="character"/>.
        /// </summary>
        /// <param name="character">The character</param>
        /// <returns>The numeric code for the <paramref name="character"/></returns>
        public static int CODE(char character)
        {
            return (int) character;
        }

        /// <summary>
        /// Returns the number of combinations for a given number of items.
        /// </summary>
        /// <param name="number">The number of items.</param>
        /// <param name="number_chosen">The number of items in each combination.</param>
        /// <returns>The number of combinations for a given number of items.</returns>
        public static int COMBIN(int number, int number_chosen)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(nameof(number),
                    "The number of items must be greater than or equal to 0");
            if (number_chosen < 0)
                throw new ArgumentOutOfRangeException(nameof(number_chosen),
                    "The number of items in each combination must be greater than or equal to 0");
            if (number < number_chosen)
                throw new ArgumentOutOfRangeException(nameof(number),
                    "The number of items must greater than or equal to the number of items in each combination");
            return (int)(Functions.Fact(number) / (Functions.Fact(number_chosen) * Functions.Fact(number - number_chosen)));
        }

        /// <summary>
        /// Returns the number of combinations (with repetitions) for a given number of items.
        /// </summary>
        /// <param name="number">The number of items.</param>
        /// <param name="number_chosen">The number of items in each combination.</param>
        /// <returns>The number of combinations (with repetitions) for a given number of items.</returns>
        public static int COMBINA(int number, int number_chosen)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(nameof(number),
                    "The number of items must be greater than or equal to 0");
            if (number_chosen < 0)
                throw new ArgumentOutOfRangeException(nameof(number_chosen),
                    "The number of items in each combination must be greater than or equal to 0");
            if (number < number_chosen)
                throw new ArgumentOutOfRangeException(nameof(number),
                    "The number of items must greater than or equal to the number of items in each combination");
            return COMBIN(number + number_chosen - 1, number - 1);
        }

        /// <summary>
        /// Converts real and imaginary coefficients into a complex number of the form x + yi or x + yj.
        /// </summary>
        /// <param name="real_num">The real coefficient of the complex number.</param>
        /// <param name="i_num">The imaginary coefficient of the complex number.</param>
        /// <param name="suffix">The suffix for the imaginary component of the complex number. If omitted, suffix is assumed to be "i".</param>
        /// <returns>The resulting complex number.</returns>
        public static Complex COMPLEX(double real_num, double i_num, char suffix = 'i')
        {
            if(!"ij".Contains(suffix)) throw new ArgumentOutOfRangeException(nameof(suffix), "The suffix must be i or j.");
            return new Complex(real_num, i_num, usejinsteadofi: suffix == 'j');
        }

        /// <summary>
        /// Concatenates a set of values.
        /// </summary>
        /// <param name="values">The set of values to concatenate</param>
        /// <returns>The resulting string</returns>
        public static string CONCATENATE(params object[] values)
        {
            return string.Concat(values);
        }

        public static double CONFIDENCE(double alpha, double standard_dev, double size)
        {
            if(alpha <= 0 || alpha >= 1) throw new ArgumentOutOfRangeException(nameof(alpha), "The significance level (alpha) must be greater than 0 and less than 1.");
            if(standard_dev <= 0) throw new ArgumentOutOfRangeException(nameof(standard_dev), "The standard deviation must be greater than 0.");
            if(size < 1) throw new ArgumentOutOfRangeException(nameof(size), "The sample size must be greater than 1.");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the correlation coefficient of the <paramref name="array1"/> and <paramref name="array2"/> ranges.
        /// </summary>
        /// <param name="array1">The first range</param>
        /// <param name="array2">The second range</param>
        /// <returns>The correlation coefficient of the <paramref name="array1"/> and <paramref name="array2"/> ranges.</returns>
        public static double CORREL(double[] array1, double[] array2)
        {
            if(array1.Length != array2.Length) throw new ArgumentException("The two data sets must have equal length");

            return array1.Zip(array2, (x, y) => (x - array1.Average()) * (y - array2.Average())).Sum()
                / Math.Sqrt(array1.Sum(x => Math.Pow(x - array1.Average(), 2)) * array2.Sum(y => Math.Pow(y - array2.Average(), 2)));
        }

        /// <summary>
        /// Returns the cosine of the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle">The angle in radians for which you want the cosine.</param>
        /// <returns>The cosine of the given <paramref name="angle"/>.</returns>
        public static double COS(double angle)
        {
            return Math.Cos(angle);
        }

        /// <summary>
        /// Returns the hyperbolic cosine of the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle">The angle in radians for which you want the hyperbolic cosine.</param>
        /// <returns>The hyperbolic cosine of the given <paramref name="angle"/>.</returns>
        public static double COSH(double angle)
        {
            return Functions.Cosh(angle);
        }

        /// <summary>
        /// Returns the cotangent of the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle">The angle in radians for which you want the cotangent.</param>
        /// <returns>The cotangent of the given <paramref name="angle"/>.</returns>
        public static double COT(double angle)
        {
            return Functions.Cot(angle);
        }

        /// <summary>
        /// Returns the hyperbolic cotangent of the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle">The angle in radians for which you want the hyperbolic cotangent.</param>
        /// <returns>The hyperbolic cotangent of the given <paramref name="angle"/>.</returns>
        public static double COTH(double angle)
        {
            return Functions.Coth(angle);
        }

        /// <summary>
        /// Counts how many numbers are in the list of arguments
        /// </summary>
        /// <param name="values">List of arguments</param>
        /// <returns>How many numbers are in the list of arguments</returns>
        public static int COUNT(params object[] values)
        {
            return values.Count(x => x is int || x is double || x is Complex);
        }

        /// <summary>
        /// Counts how many values are in the list of arguments
        /// </summary>
        /// <param name="values">List of arguments</param>
        /// <returns>How many values are in the list of arguments</returns>
        public static int COUNTA(params object[] values)
        {
            return values.Count(x => x != null);
        }

        /// <summary>
        /// Counts the number of blank cells within a range
        /// </summary>
        /// <param name="values">List of arguments</param>
        /// <returns>The number of blank cells within a range</returns>
        public static int COUNTBLANK(params object[] values)
        {
            return values.Count(x => x == null || x.ToString() == "");
        }

        /// <summary>
        /// Counts the number of cells within a range that meet the given criteria
        /// </summary>
        /// <param name="criteria">The criteria</param>
        /// <param name="values">List of arguments</param>
        /// <returns>The number of cells within a range that meet the given criteria</returns>
        public static int COUNTIF(Func<object, bool> criteria, params object[] values)
        {
            return values.Count(criteria);
        }
        
        /// <summary>
        /// Counts the number of cells within a range that meet the given criteria
        /// </summary>
        /// <param name="values">List of arguments and criterias</param>
        /// <returns>The number of cells within a range that meet the given criteria</returns>
        public static int COUNTIFS(params Tuple<object[], Func<object, bool>>[] values)
        {
            throw new NotImplementedException();
            //return values.Sum(x => COUNTIF(x.Item2, x.Item1));
        }

        /// <summary>
        /// Returns covariance, the average of the products of paired deviations
        /// </summary>
        /// <param name="array1">The first cell range of integers.</param>
        /// <param name="array2">The second cell range of integers.</param>
        /// <returns>The average of the products of paired deviations</returns>
        public static double COVAR(double[] array1, double[] array2)
        {
            return array1.Zip(array2, (x, y) => (x - array1.Average()) * (y - array2.Average())).Sum() / array1.Length;
        }

        /// <summary>
        /// Returns covariance, the average of the products of paired deviations
        /// </summary>
        /// <param name="array1">The first cell range of integers.</param>
        /// <param name="array2">The second cell range of integers.</param>
        /// <returns>The average of the products of paired deviations</returns>
        public static double COVARIANCE_P(double[] array1, double[] array2)
        {
            return COVAR(array1, array2);
        }

        /// <summary>
        /// Returns the cosecant of the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle">The angle in radians for which you want the cosecant.</param>
        /// <returns>The cosecant of the given <paramref name="angle"/>.</returns>
        public static double CSC(double angle)
        {
            return Functions.Csc(angle);
        }

        /// <summary>
        /// Returns the hyperbolic cosecant of the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle">The angle in radians for which you want the hyperbolic cosecant.</param>
        /// <returns>The hyperbolic cosecant of the given <paramref name="angle"/>.</returns>
        public static double CSCH(double angle)
        {
            return Functions.Csch(angle);
        }

        /// <summary>
        /// Converts the specified <paramref name="number"/> to binary
        /// </summary>
        /// <param name="number">The number to convert</param>
        /// <returns>The number converted to binary</returns>
        public static string DEC2BIN(int number)
        {
            return Convert.ToString(number, 2);
        }

        /// <summary>
        /// Converts the specified <paramref name="number"/> to hexadecimal
        /// </summary>
        /// <param name="number">The number to convert</param>
        /// <returns>The number converted to hexadecimal</returns>
        public static string DEC2HEX(int number)
        {
            return Convert.ToString(number, 16);
        }

        /// <summary>
        /// Converts the specified <paramref name="number"/> to octal
        /// </summary>
        /// <param name="number">The number to convert</param>
        /// <returns>The number converted to octal</returns>
        public static string DEC2OCT(int number)
        {
            return Convert.ToString(number, 8);
        }

        /// <summary>
        /// Converts a text representation of a number in a given base into a decimal number
        /// </summary>
        /// <param name="text">The text representation</param>
        /// <param name="radix">The radix</param>
        /// <returns>The resulting decimal number</returns>
        public static int DECIMAL(string text, int radix)
        {
            return Convert.ToInt32(text, radix);
        }

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        /// <param name="radians">Angle in radians</param>
        /// <returns>Angle in degrees</returns>
        public static double DEGREES(double radians)
        {
            return radians * (180.0 / Constants.Pi);
        }

        /// <summary>
        /// Tests whether values are equal
        /// </summary>
        /// <param name="numbers">Values</param>
        /// <returns>1 if all the values are equal, 0 otherwise</returns>
        public static int DELTA(params double[] numbers)
        {
            return numbers.All(x => x == numbers[0]) ? 1 : 0;
        }

        /// <summary>
        /// Returns the sum of squares of deviations
        /// </summary>
        /// <param name="numbers">Values</param>
        /// <returns>The sum of squares of deviations</returns>
        public static double DEVSQ(params double[] numbers)
        {
            return numbers.Sum(x => Math.Pow(x - numbers.Average(), 2));
        }

        internal static int NumberOfDays(DayCountBasis basis)
        {
            if (basis == DayCountBasis.Actual_360 || basis == DayCountBasis.European_30_360 ||
                basis == DayCountBasis.US_NASD) return 360;
            else return 365;
        }

        /// <summary>
        /// Returns the discount rate for a security
        /// </summary>
        /// <param name="settlement">The security's settlement date.</param>
        /// <param name="maturity">The security's maturity date.</param>
        /// <param name="par">The security's price per $100 face value.</param>
        /// <param name="redemption">The security's redemption value per $100 face value.</param>
        /// <param name="basis">The type of day count basis to use.</param>
        /// <returns>The discount rate for a security</returns>
        public static double DISC(DateTime settlement, DateTime maturity, double par, double redemption, DayCountBasis basis = DayCountBasis.US_NASD)
        {
            return (redemption - par) / redemption * (NumberOfDays(basis) / (maturity - settlement).Days);
        }

        /// <summary>
        /// Converts a <paramref name="number"/> to text, using the $ (dollar) currency format
        /// </summary>
        /// <param name="number">A number.</param>
        /// <param name="decimals">The number of digits to the right of the decimal point. If decimals is negative, number is rounded to the left of the decimal point. If you omit decimals, it is assumed to be 2.</param>
        /// <returns>The specified <paramref name="number"/> converted to text, using the $ (dollar) currency format</returns>
        public static string DOLLAR(decimal number, int decimals = 2)
        {
            number = Math.Abs(number);
            if (decimals < 0)
            {
                var digits = (decimal) Math.Pow(10, Math.Abs(decimals));
                number = Math.Round(number / digits, 0) * digits;
            }
            else number = Math.Round(number, decimals);
            return number.ToString("C" + (decimals < 0 ? 0 : decimals), new CultureInfo("en-US"));
        }

        /// <summary>
        /// Returns the serial number of the date that is the indicated number of months before or after the start date
        /// </summary>
        /// <param name="start_date">A date that represents the start date</param>
        /// <param name="months">The number of months before or after start_date. A positive value for months yields a future date; a negative value yields a past date.</param>
        /// <returns>The serial number of the date that is the indicated number of months before or after the start date</returns>
        public static DateTime EDATE(DateTime start_date, int months)
        {
            return start_date.AddMonths(months);
        }

        /// <summary>
        /// Returns the effective annual interest rate
        /// </summary>
        /// <param name="nominal_rate">The nominal interest rate.</param>
        /// <param name="npery">The number of compounding periods per year.</param>
        /// <returns>The effective annual interest rate</returns>
        public static double EFFECT(double nominal_rate, int npery)
        {
            if(nominal_rate <= 0) throw new ArgumentOutOfRangeException(nameof(nominal_rate), "The nominal interest rate must be greater than 0.");
            if(npery < 1) throw new ArgumentOutOfRangeException(nameof(npery), "The number of compounding periods per year must be greater than or equal to 1.");
            return Math.Pow(1 + nominal_rate / npery, npery) - 1;
        }

        /// <summary>
        /// Returns a URL-encoded string
        /// </summary>
        /// <param name="text">A string to be URL encoded.</param>
        /// <returns>A URL-encoded string</returns>
        public static string ENCODEURL(string text)
        {
            return WebUtility.UrlEncode(text);
        }

        /// <summary>
        /// Returns the serial number of the last day of the month before or after a specified number of months
        /// </summary>
        /// <param name="start_date">A date that represents the starting date.</param>
        /// <param name="months">The number of months before or after start_date. A positive value for months yields a future date; a negative value yields a past date.</param>
        /// <returns>The serial number of the last day of the month before or after a specified number of months</returns>
        public static DateTime EOMONTH(DateTime start_date, int months)
        {
            var final = EDATE(start_date, months);
            return new DateTime(final.Year, final.Month, DateTime.DaysInMonth(final.Year, final.Month));
        }

        /// <summary>
        /// Returns the error function
        /// </summary>
        /// <param name="lower_limit">The lower bound for integrating ERF.</param>
        /// <returns>The error function integrated between 0 and <paramref name="lower_limit"/></returns>
        public static double ERF(double lower_limit)
        {
            return Functions.Erf(lower_limit);
        }

        /// <summary>
        /// Returns the error function
        /// </summary>
        /// <param name="lower_limit">The lower bound for integrating ERF.</param>
        /// <param name="upper_limit">The upper bound for integrating ERF.</param>
        /// <returns>The error function integrated between <paramref name="lower_limit"/> and <paramref name="upper_limit"/></returns>
        public static double ERF(double lower_limit, double upper_limit)
        {
            return Functions.Erf(lower_limit, upper_limit);
        }

        /// <summary>
        /// Returns the error function
        /// </summary>
        /// <param name="lower_limit">The lower bound for integrating ERF.</param>
        /// <returns>The error function integrated between 0 and <paramref name="lower_limit"/></returns>
        public static double ERF_PRECISE(double lower_limit)
        {
            return Functions.Erf(lower_limit);
        }

        /// <summary>
        /// Returns the complementary ERF function integrated between <paramref name="x"/> and infinity
        /// </summary>
        /// <param name="x">The lower bound for integrating ERF.</param>
        /// <returns>The complementary ERF function integrated between <paramref name="x"/> and infinity</returns>
        public static double ERFC(double x)
        {
            return Functions.Erfc(x);
        }

        /// <summary>
        /// Returns the complementary ERF function integrated between <paramref name="x"/> and infinity
        /// </summary>
        /// <param name="x">The lower bound for integrating ERF.</param>
        /// <returns>The complementary ERF function integrated between <paramref name="x"/> and infinity</returns>
        public static double ERFC_PRECISE(double x)
        {
            return Functions.Erfc(x);
        }

        /// <summary>
        /// Checks to see if two values are identical
        /// </summary>
        /// <param name="a">The first value</param>
        /// <param name="b">The second value</param>
        /// <returns>TRUE if the two values are identical, FALSE otherwise</returns>
        public static bool EXACT(object a, object b)
        {
            if (a == null || b == null) return false;
            return a == b || a.Equals(b);
        }

        /// <summary>
        /// Returns e raised to the power of a given number
        /// </summary>
        /// <param name="number">The exponent applied to the base e.</param>
        /// <returns>e raised to the power of a given number</returns>
        public static double EXP(double number)
        {
            return Functions.Exp(number);
        }
    }
}
