using System;
using System.Linq;
using System.Text;

namespace IMPression
{
    internal class Operator : EquationElement
    {
        public Operator(string src, int start)
        {
            if (IsValid(src, start))
            {
                Value = GetOp(src, start);
                Add(Value);
            }
        }

        public Operator(string ch)
        {
            if (IsValidOperator(ch, 0))
            {
                Value = ch;
                Add(Value);
            }
        }

        public string Value { get; }
        public bool CanStartTerm => Value == "-" || Value == "+" || Value == "~";
        public bool IsSign => Value == "-" || Value == "+";

        public static string GetOp(string equation, int index)
        {
            var sb = new StringBuilder();
            while (index < equation.Length)
            {
                var ch = equation[index++];
                if (operators.Any(x => x.ToCharArray().Contains(ch)))
                    sb.Append(ch);
                else break;
                if (operators.Contains(sb.ToString()))
                {
                    if (index < equation.Length && !operators.Contains(sb.ToString() + equation[index]))
                        break;
                }
            }
            return sb.ToString();
        }

        private static string[] operators =
        {
            "+", "-", "/", "*", "^", "%", "\\", "~", "&", "|", "xor", "<<", ">>", ">>>"
        };

        public static bool IsValidOperator(string equation, int index)
        {
            return operators.Contains(GetOp(equation, index));
        }

        public override bool IsValid(string equation, int index)
        {
            return IsValidOperator(equation, index);
        }

        public Complex Perform(Complex left, Complex right)
        {
            switch (Value)
            {
                case "-":
                    return left - right;
                case "+":
                    return left + right;
                case "*":
                    return left * right;
                case "/":
                    if (right == 0)
                    {
                        throw new ParseException($"{left} / {right}", 0, "Division par zéro");
                    }
                    return left / right;
                case "\\":
                    return MathFunctions.Floor(left / right);
                case "^":
                    return MathFunctions.Pow(left, right);
                case "%":
                    return left - right * MathFunctions.Floor(left / right);
                case "&":
                    return (int) left & (int) right;
                case "|":
                    return (int) left | (int) right;
                case "xor":
                    return (int) left ^ (int) right;
                case "<<":
                    return (int) left << (int) right;
                case ">>":
                    return (int) left >> (int) right;
                case ">>>":
                    return
                        Convert.ToInt32(
                            Convert.ToString((int) left, 2).Insert(0, new string('0', (int) right)).Substring(0, 32), 2);
            }

            throw new ParseException($"{left} {Value} {right}", 0, $"Opérateur non supporté : '{Value}'");
        }
    }
}