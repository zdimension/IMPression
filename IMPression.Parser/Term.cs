using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMPression.Parser
{
    internal class Term : EquationValue
    {
        public Term()
        {
            Stack = new List<EquationElement>();
        }

        public override Complex Value
        {
            get
            {
                Complex value = 0;
                if (Stack.Count == 0)
                    return value;

                var left = Stack[0] as EquationValue;
                if (left != null) value = left.Value;

                for (var index = 1; index < Stack.Count; index++)
                {
                    Operator op = null;
                    EquationValue right = null;
                    if (index < Stack.Count)
                        op = Stack[index] as Operator;
                    if (index + 1 < Stack.Count)
                        right = Stack[index + 1] as EquationValue;
                    if (op != null && right != null)
                    {
                        if (op.Value == "-" && Stack.Count > 2 && Stack[Stack.Count - 3] is Operator &&
                            (Stack[Stack.Count - 3] as Operator).Value == "~" && !(right.Signed))
                        {
                            value = op.Perform(value, -right.Value);
                        }
                        else
                        {
                            value = op.Perform(value, right.Value);
                        }
                    }
                    index += 1;
                }
                if (Signed)
                    return -value;
                return value;
            }
        }

        public List<EquationElement> Stack { get; set; }

        private EquationElement LastElement => Stack.Count == 0 ? null : Stack[Stack.Count - 1];

        private int OperatorCount
        {
            get
            {
                var cnt = 0;
                for (var index = Stack.Count - 1; index >= 0; index--)
                {
                    if (Stack[index] is Operator)
                    {
                        cnt++;
                        continue;
                    }
                    break;
                }
                return cnt;
            }
        }

        public static bool IsValidDigit(char ch) => ch == '(' || ch == '[';

        public override bool IsValid(string equation, int index)
        {
            return IsValidDigit(equation[index]);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("(");
            foreach (var item in Stack)
                sb.Append(item);
            sb.Append(")");
            return sb.ToString();
        }

        public void Parse(string equation)
        {
            Parse(equation, this);
        }

        public void Parse(string equation, EquationElement root)
        {
            Stack.Clear();
            Parse(equation, 0, root);
            CombineTerms(new[] {"^"});
            CombineTopDown();
            CombineTerms(new[] {"*", "/", "%"});
        }

        private EquationElement PopSignOperator()
        {
            if (Stack.Count > 1)
            {
                var index = Stack.Count - 1;
                var op1 = Stack[index] as Operator;
                var op2 = Stack[index - 1] as Operator;
                if (op2 != null && op1 != null && (op1.IsSign))
                {
                    if(op2.IsSign) Stack.RemoveAt(index);
                    return op1;
                }
            }
            if (Stack.Count == 1)
            {
                var index = Stack.Count - 1;
                var op1 = Stack[index] as Operator;
                if (op1 != null && (op1.IsSign || op1.CanStartTerm))
                {
                    Stack.RemoveAt(index);
                    return op1;
                }
            }
            return null;
        }

        private EquationElement Add(EquationElement item)
        {
            item.Parent = this;
            if (item is EquationValue)
            {
                // insérer l'opérateur '*' entre les valeurs
                if (LastElement is EquationValue)
                    Stack.Add(new Operator("*"));
                var c = PopSignOperator();
                if (c != null && (c as Operator).Value == "-")
                {
                    ((EquationValue) item).Signed = true;
                }
                if ((c != null && (c as Operator).Value == "~") || (Stack.Count > 1 && Stack[Stack.Count - 2] is Operator && (Stack[Stack.Count - 2] as Operator).Value == "~"))
                {
                    ((EquationValue) item).Value = ~(int) (((EquationValue) item).Value);
                    
                }

                Stack.Add(item);
                return item;
            }
            if (item is Operator)
            {
                // seul - peut démarrer un terme
                if (((Operator) item).CanStartTerm == false && Stack.Count == 0)
                    throw new ParseException(item.ToString(), 0,
                        "Le terme ne peut pas commencer par un opérateur autre que + ou -");
                // si il y a un second opérateur s'assurer que c'est un signe
                var op = LastElement as Operator;
                if (op != null && ((Operator) item).IsSign == false)
                    throw new ParseException(ToString().Replace("(", "").Replace(")", ""), 0,
                        "Plusieurs opérateurs à la suite");
                if (OperatorCount >= 2)
                    throw new ParseException(ToString().Replace("(", "").Replace(")", ""), 0,
                        "Plusieurs opérateurs à la suite");
                Stack.Add(item);
                return item;
            }
            return null;
        }

        private int Parse(string equation, int index, EquationElement root)
        {
            while (index < equation.Length)
            {
                var ch = equation[index];
                if (char.IsWhiteSpace(ch))
                {
                    index++;
                    continue;
                }
                if (Operator.IsValidOperator(equation, index))
                {
                    var n = Add(new Operator(equation, index));
                    index += n.Length;
                    continue;
                }
                if (Number.IsValidDigit(equation, index))
                {
                    var n = Add(new Number(equation, index));
                    index += n.Length;
                    continue;
                }
                if (Function.IsValidDigit(equation, index))
                {
                    var n = Add(new Function(equation, index, root));
                    index += n.Length;
                    continue;
                }
                if (Constant.IsValidDigit(equation, index))
                {
                    var n = Add(new Constant(equation, index));
                    index += n.Length;
                    continue;
                }
                if (Variable.IsValidDigit(equation, index, root))
                {
                    var n = Add(new Variable(equation, index, root));
                    index += n.Length;
                    continue;
                }
                index++;

                if (IsValidDigit(ch))
                {
                    var endindex = FindMatchingEnd(ch, equation, index - 1);
                    if (endindex > index)
                    {
                        var len = endindex - index;
                        var s = equation.Substring(index, len);
                        var g = Add(new Term()) as Term;
                        len = g.Parse(s, 0, root) + 1;
                        index += len;
                        continue;
                    }
                    throw new ParseException(equation, index - 1,
                        "Pas de parenthèses fermantes correspondantes trouvées");
                }
            }
            return index;
        }

        private void CombineTopDown()
        {
            // traitement spécial de l'opérateur de puissance. Si un opérateur est élevé à la puissance d'une puissance, la puissance doit être calculée en premier.
            // ex. 2^2^3 = 2^(2^3) = 256. Ceci est réalisé en combinant de droite à gauche (ou de haut en bas)
            var operatorCh = "^";
            foreach (var element in Stack)
            {
                (element as Term)?.CombineTopDown();
            }
            var index = Stack.Count - 1;
            while (index > 0)
            {
                var op = Stack[index] as Operator;
                index--;
                if (op == null || op.Value != operatorCh)
                    continue;

                var left = Stack[index];
                var right = Stack[index + 2];

                var newterm = new Term {Parent = this};
                newterm.Add(left);
                newterm.Add(op);
                newterm.Add(right);

                if (((EquationValue) left).Signed)
                {
                    ((EquationValue) left).Signed = false;
                    newterm.Signed = true;
                }

                Stack.RemoveRange(index, newterm.Stack.Count);
                Stack.Insert(index, newterm);
            }
        }

        private void CombineTerms(string[] operators)
        {
            foreach (var element in Stack)
            {
                (element as Term)?.CombineTerms(operators);
            }
            if (NeedSubTerms(operators) == false)
                return;

            var startIndex = 0;
            while (startIndex < Stack.Count)
            {
                startIndex = FindOperator(startIndex, operators);
                if (startIndex < 0)
                    return;

                var newterm = new Term {Parent = this};
                startIndex--;
                var startpoint = startIndex;

                while (startIndex < Stack.Count)
                {
                    var item = Stack[startIndex];
                    if (item is EquationValue)
                    {
                        newterm.Add(item);
                        startIndex++;
                    }
                    if (item is Operator)
                    {
                        var op = item as Operator;
                        if (op == null || operators.Contains(op.Value) == false)
                        {
                            Stack.RemoveRange(startpoint, newterm.Stack.Count);
                            Stack.Insert(startpoint, newterm);
                            break;
                        }
                        newterm.Add(item);
                        startIndex++;
                    }

                    if (startIndex >= Stack.Count)
                    {
                        Stack.RemoveRange(startpoint, newterm.Stack.Count);
                        Stack.Insert(startpoint, newterm);
                        return;
                    }
                }
            }
        }

        private bool NeedSubTerms(IEnumerable<string> operators)
        {
            // si tous les opérateurs d'un terme sont du type correct, alors pas besoin de sous-termes.
            return Stack.OfType<Operator>().Any(op => operators.Contains(op.Value) == false);
        }

        private int FindOperator(int startIndex, string[] operators)
        {
            for (var index = startIndex; index < Stack.Count; index++)
            {
                var op = Stack[index] as Operator;
                if (op != null && operators.Contains(op.Value))
                    return index;
            }
            return -1;
        }

        public static int FindMatchingEnd(char beginChar, string equation, int beginCharindex)
        {
            var index = beginCharindex;
            var matchCount = 0;
            var endChar = ')';
            if (beginChar == '[')
                endChar = ']';
            while (index < equation.Length - 1)
            {
                index++;
                var ch = equation[index];
                if (ch == beginChar)
                {
                    matchCount++;
                    continue;
                }
                if (ch == endChar)
                {
                    if (matchCount == 0)
                        return index;
                    matchCount--;
                }
            }
            return -1;
        }

        public static string ExtractName(string equation, int index)
        {
            return ExtractName(equation, index, true);
        }

        public static string ExtractName(string equation, int index, bool allowDigits)
        {
            var sb = new StringBuilder();
            while (index < equation.Length)
            {
                var ch = equation[index];
                if (char.IsLetter(ch))
                    sb.Append(ch);
                else if (allowDigits && (char.IsDigit(ch)) && (sb.Length > 0))
                    // chiffre autorisé si il n'est pas en première position
                    sb.Append(ch);
                else
                    break;
                index++;
            }
            return sb.ToString();
        }
    }
}