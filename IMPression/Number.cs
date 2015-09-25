using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace IMPression
{
    internal class Number : EquationValue
    {
        public Number(string src, int start)
        {
            var number = GetNumber(src, start);
            if (number.Length > 0)
                m_sb.Append(number);
            double result;
            if (number.StartsWith("0x"))
            {
                try
                {
                    result = Convert.ToInt32(number, 16);
                }
                catch
                {
                    throw new ParseException(src, start, $"Impossible de convertir '{number}' en nombre hexadécimal");
                }
                Value = result;
                return;
            }
            if (number.StartsWith("0o"))
            {
                try
                {
                    result = Convert.ToInt32(number.Substring(2), 8);
                }
                catch
                {
                    throw new ParseException(src, start, $"Impossible de convertir '{number}' en nombre octal");
                }
                Value = result;
                return;
            }
            if (number.StartsWith("0b"))
            {
                try
                {
                    result = Convert.ToInt32(number.Substring(2), 2);
                }
                catch
                {
                    throw new ParseException(src, start, $"Impossible de convertir '{number}' en nombre binaire");
                }
                Value = result;
                return;
            }
            var val = m_sb.ToString();
            var invarSeperator = CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator;
            val = val.Replace(",", invarSeperator);
            if (double.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out result) == false)
                throw new ParseException(src, start, $"Impossible de convertir '{val}' en nombre à virgule");
            Value = result;
        }

        public static string GetNumber(string equation, int index)
        {
            var sb = new StringBuilder();
            var chars = new[]
            {',', '.', 'e', 'E', 'x', 'o', 'b', '-', '+', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            var hex = new[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
            var octal = new[] {'0', '1', '2', '3', '4', '5', '6', '7'};
            var bin = new[] {'0', '1'};

            var isScientific = false;
            var isHex = false;
            var isOctal = false;
            var isBinary = false;
            while (index < equation.Length)
            {
                var ch = equation[index++];
                if (isHex
                    ? hex.Contains(ch)
                    : (isOctal ? octal.Contains(ch) : (isBinary ? bin.Contains(ch) : (ch >= '0' && ch <= '9'))))
                {
                    sb.Append(ch);
                    continue;
                }
                if (chars.Contains(ch))
                {
                    if ((ch == 'x' || ch == 'b' || ch == 'o') && equation[index - 2] != '0') break;
                    // support pour l'écriture scientifique
                    if (ch == 'e' || ch == 'E')
                    {
                        if (isScientific || isHex)
                            break;
                        isScientific = true;
                        if (index >= equation.Length)
                            break;
                        var nextchar = equation[index];
                        if (chars.Contains(nextchar) == false)
                            break;
                    }
                    if (ch == 'x')
                    {
                        if (isHex || isScientific)
                            break;
                        isHex = true;
                        if (index >= equation.Length)
                            break;
                        var nextchar = equation[index];
                        if (hex.Contains(nextchar) == false)
                            break;
                    }
                    if (ch == 'o')
                    {
                        if (isHex || isOctal || isScientific)
                            break;
                        isOctal = true;
                        if (index >= equation.Length)
                            break;
                        var nextchar = equation[index];
                        if (octal.Contains(nextchar) == false)
                            break;
                    }
                    if (ch == 'b')
                    {
                        if (isHex || isOctal || isBinary || isScientific)
                            break;
                        isBinary = true;
                        if (index >= equation.Length)
                            break;
                        var nextchar = equation[index];
                        if (bin.Contains(nextchar) == false)
                            break;
                    }
                    // +- seulement autorisé si précédé par 'e'
                    var lastchar = char.ToLower(sb[sb.Length - 1]);
                    if (ch == '-' && lastchar != 'e')
                        break;
                    if (ch == '+' && lastchar != 'e')
                        break;
                    sb.Append(ch);
                    continue;
                }
                break;
            }
            return sb.ToString();
        }

        public static bool IsValidDigit(string equation, int index)
        {
            var ch = equation[index];
            if (ch < '0' || ch > '9')
                return false;
            return GetNumber(equation, index).Length > 0;
        }

        public override bool IsValid(string equation, int index)
        {
            return IsValidDigit(equation, index);
        }
    }
}