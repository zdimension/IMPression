using Funcs = IMPression.MathFunctions;

namespace IMPression
{
    public class Fraction
    {
        public static string DoubleToFraction(double d)
        {
            double inValue = Funcs.Abs(d);
            int sign = (int)Funcs.Sign(d);
            long fractionNumerator = (long) inValue;
            double fractionDenominator = 1;
            double previousDenominator = 0;
            double remainingDigits = inValue;
            int maxIterations = 594;
            while (remainingDigits != Funcs.Floor(remainingDigits)
                   && Funcs.Abs(inValue - (fractionNumerator / fractionDenominator)) > double.Epsilon)
            {
                remainingDigits = 1.0 / (remainingDigits - Funcs.Floor(remainingDigits));
                double scratch = fractionDenominator;
                fractionDenominator = (Funcs.Floor(remainingDigits) * fractionDenominator) + previousDenominator;
                fractionNumerator = (long) (inValue * fractionDenominator + 0.5);
                previousDenominator = scratch;
                if (maxIterations-- < 0)
                    break;
            }
            return (fractionNumerator * sign) + "/" + fractionDenominator;
        }
    }
}