namespace IMPression
{
    public class Fraction
    {
        public static string DoubleToFraction(double d)
        {
            double inValue = Functions.Abs(d);
            int sign = (int)Functions.Sign(d);
            long fractionNumerator = (long) inValue;
            double fractionDenominator = 1;
            double previousDenominator = 0;
            double remainingDigits = inValue;
            int maxIterations = 594;
            while (remainingDigits != Functions.Floor(remainingDigits)
                   && Functions.Abs(inValue - (fractionNumerator / fractionDenominator)) > double.Epsilon)
            {
                remainingDigits = 1.0 / (remainingDigits - Functions.Floor(remainingDigits));
                double scratch = fractionDenominator;
                fractionDenominator = (Functions.Floor(remainingDigits) * fractionDenominator) + previousDenominator;
                fractionNumerator = (long) (inValue * fractionDenominator + 0.5);
                previousDenominator = scratch;
                if (maxIterations-- < 0)
                    break;
            }
            return (fractionNumerator * sign) + "/" + fractionDenominator;
        }
    }
}