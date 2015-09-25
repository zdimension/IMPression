#region Libs

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Funcs = IMPression.MathFunctions;

#endregion

namespace IMPression
{
    public class EquationParser
    {
        public static bool UseDegrees = false;
        private Term m_term = new Term();
        public static List<string> FunctionsList => Function.FunctionsList;
        public string Equation { get; private set; }
        public Complex Value => m_term.Value;
        public string ValueAsString => Value.ToString();

        public string UsedVarsAsString
        {
            get
            {
                var sb = new StringBuilder();
                var vars = m_term.GetUsedVars();
                foreach (var var in vars)
                    sb.AppendFormat("{0}={1}, ", var, m_term.GetVar(var));
                return sb.ToString();
            }
        }

        public void SetLastAnswer(double value)
        {
            m_term.SetVar("ans", value);
        }

        public Complex Calculate(string equation)
        {
            return Calculate(DeCleanUp(equation), "");
        }

        public Complex Calculate(string equation, string variablestring)
        {
            equation = DeCleanUp(equation);

            if (variablestring.Length > 0)
            {
                foreach (
                    var split in
                        variablestring.Split(';')
                            .Select(varstring => varstring.Split('='))
                            .Where(split => split.Length != 1 || split[0].Trim().Length != 0))
                {
                    if (split.Length != 2)
                        throw new ParseException(variablestring, 0,
                            "La liste doit être séparée par des points-virgules, ex. 'x=10; y=12'");
                    var varname = split[0].Trim();
                    var vareq = split[1].Trim();
                    var v = new Term();
                    v.Parse(vareq);
                    m_term.SetVar(varname, v.Value);
                }
            }
            m_term.Parse(equation);
            return m_term.Value;
        }

        public Complex Calculate(string equation, params Var[] vars)
        {
            equation = DeCleanUp(equation);

            if (vars.Any())
            {
                vars.All(x =>
                {
                    m_term.SetVar(x.Name, x.Value);
                    return true;
                });
            }
            m_term.Parse(equation);
            return Funcs.Round(m_term.Value, 15);
        }

        public Complex Calculate(string eq, List<Var> vars)
        {
            return Calculate(eq, vars.ToArray());
        }

        public Complex CalcForVar(string varname, double varvalue)
        {
            m_term.SetVar(varname, varvalue);
            return new Complex(Funcs.Abs(m_term.Value.Real) < Funcs.Pow(10, -15) ? 0 : m_term.Value.Real,
                Funcs.Abs(m_term.Value.Imaginary) < Funcs.Pow(10, -15) ? 0 : m_term.Value.Imaginary);
        }

        public string CleanUp(string expr, bool vis = false)
        {
            var res = expr;


            res = Regex.Replace(res, "Sqrt", "√", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "Curt", "∛", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "Degree", "°", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "EllipticTheta1", "ϑ₁", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "EllipticTheta2", "ϑ₂", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "EllipticTheta3", "ϑ₃", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "EllipticTheta4", "ϑ₄", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "Sum", "Σnx", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "DivisorSigma", "σ", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "EulerGamma", "γ", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "DiGamma", "Ψ", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "Inv(?![a-zA-Z])", "⁻¹", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "Gamma", "Γ", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "ArcSinh", "Sinh⁻¹", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "ArcCosh", "Cosh⁻¹", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "ArcTanh", "Tanh⁻¹", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "ArcCoth", "Coth⁻¹", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "ArcCsch", "Csch⁻¹", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "ArcSech", "Sech⁻¹", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "ArcSin", "Sin⁻¹", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "ArcCos", "Cos⁻¹", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "ArcTan", "Tan⁻¹", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "ArcCot", "Cot⁻¹", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "ArcCsc", "Csc⁻¹", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "ArcSec", "Sec⁻¹", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "Log10", "Log₁₀", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, @"fact\((.*)\)", vis ? @"$1!" : @"($1)!", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, @"fact2\((.*)\)", vis ? @"$1!!" : @"($1)‼", RegexOptions.IgnoreCase);
            if (vis)
            {
                res = Regex.Replace(res, @"abs\((.*)\)", @"|$1|", RegexOptions.IgnoreCase);

                res = Regex.Replace(res, @"floor\((.*)\)", @"⌊$1⌋", RegexOptions.IgnoreCase);
                res = Regex.Replace(res, @"ceil\((.*)\)", @"⌈$1⌉", RegexOptions.IgnoreCase);
                res = Regex.Replace(res, @"rnd\((.*)\)", @"⌊$1⌉", RegexOptions.IgnoreCase);
            }

            res = Regex.Replace(res, "(?<![a-zA-Z ])pi(?![a-zA-Z])", "π", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "(?<![a-zA-Z ])theta(?![a-zA-Z])", "θ", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "(?<![a-zA-Z ])e(?![a-zA-Z-+0-9])", "ℯ", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "(?<![a-zA-Z ])tau(?![a-zA-Z])", "τ", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, @"EulerPhi\(", "φ(", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "Phi", "φ", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "/", "÷", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "[*]", "×", RegexOptions.IgnoreCase);

            res = res.Replace("!!", "‼");

            res = Regex.Replace(res, "avr", "x\u0305");

            //res = Regex.Replace(res, @"\(\((.*)\)\)", "($1)", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, @"(?<![\w\u2070\u00B9\u00B2\u00B3\u2074-\u2079∛√₁₂₃₄φ₀])\((\d)\)(?<![!‼])", "$1",
                RegexOptions.IgnoreCase);

            res = Regex.Replace(res, @"E([+-][0-9]+)", delegate(Match match)
            {
                var resu = "×10";
                resu += Superscript(match.Groups[1].ToString(), true);
                return resu;
            });

            res = Regex.Replace(res, @"([\d]+)\^([\d]+)", delegate(Match match)
            {
                var resu = match.Groups[1].ToString();
                resu += Superscript(match.Groups[2].ToString());
                return resu;
            });

            return res;
        }

        public static string Superscript(string n, bool rp = false)
        {
            string ret = "";
            foreach (char c1 in n)
            {
                switch (c1)
                {
                    case '+':
                        if (!rp) ret += '\x207A';
                        continue;
                    case '-':
                        ret += '\x207B';
                        continue;
                }

                int c = int.Parse("" + c1);

                switch (c)
                {
                    case 0:
                        ret += '\x2070';
                        break;
                    case 1:
                        ret += '\x00B9';
                        break;
                    case 2:
                        ret += '\x00B2';
                        break;
                    case 3:
                        ret += '\x00B3';
                        break;
                    default:
                        ret += (char) ((c - 4) + '\x2074');
                        break;
                }
            }
            return ret;
        }

        public static string Subscript(string expr)
        {
            return expr.Where(char.IsDigit).Aggregate("", (current, c) => current + (char) ('₀' + int.Parse(c.ToString())));
        }

        public string DeCleanUp(string expr)
        {
            var res = expr;

            res = Regex.Replace(res, "x\u0305", "avr");

            res = Regex.Replace(res, "∛", "Curt", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "√", "Sqrt", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "°", "Degree", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "ϑ₁", "EllipticTheta1", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "ϑ₂", "EllipticTheta2", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "ϑ₃", "EllipticTheta3", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "ϑ₄", "EllipticTheta4", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "σ", "DivisorSigma", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "Σnx", "Sum", RegexOptions.IgnoreCase);


            res = Regex.Replace(res, "Ψ", "DiGamma", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "Γ", "Gamma", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "γ", "EulerGamma", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "sin⁻¹", "ArcSin", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "cos⁻¹", "ArcCos", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "tan⁻¹", "ArcTan", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "cot⁻¹", "ArcCot", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "csc⁻¹", "ArcCsc", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "sec⁻¹", "ArcSec", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "sinh⁻¹", "ArcSinh", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "cosh⁻¹", "ArcCosh", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "tanh⁻¹", "ArcTanh", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "coth⁻¹", "ArcCoth", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "csch⁻¹", "ArcCsch", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "sech⁻¹", "ArcSech", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "⁻¹", "Inv", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "Log₁₀", "Log10", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, @"×10([\u2070\u00B9\u00B2\u00B3\u2074-\u2079]+)", delegate(Match match)
            {
                var resu = "E+";
                match.ToString().All(x =>
                {
                    resu += suptonb(x);
                    return true;
                });
                return resu;
            });

            res = Regex.Replace(res, @"([\xB2\xB3\xB9\u2070\u2074-\u207B]+)", delegate(Match match)
            {
                var resu = "^(";
                match.ToString().All(x =>
                {
                    resu += suptonb(x);
                    return true;
                });
                resu += ")";
                return resu;
            });

            res = Regex.Replace(res, @"(.*)!", @"fact($1)", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, @"(.*)‼", @"fact2($1)", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, @"\|(.*)\|", "abs($1)", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, @"⌊(.*)⌋", @"floor($1)", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, @"⌈(.*)⌉", @"ceil($1)", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, @"⌊(.*)⌉", @"rnd($1)", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "π", "Pi", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "θ", "Theta", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "τ", "Tau", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "ℯ", "E", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, @"φ\(", "EulerPhi(", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "φ", "Phi", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, "÷", "/", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, "×", "*", RegexOptions.IgnoreCase);

            res = Regex.Replace(res, @"\(\((.*)\)\)", "($1)", RegexOptions.IgnoreCase);
            res = Regex.Replace(res, @"([\D]+)\((\d)\)", "$1($2)", RegexOptions.IgnoreCase);

            return res;
        }

        private static char suptonb(char sup)
        {
            switch (sup)
            {
                case '\xB2':
                    return '2';
                case '\xB3':
                    return '3';
                case '\xB9':
                    return '1';
                case '\u2070':
                    return '0';
                case '\u2074':
                    return '4';
                case '\u2075':
                    return '5';
                case '\u2076':
                    return '6';
                case '\u2077':
                    return '7';
                case '\u2078':
                    return '8';
                case '\u2079':
                    return '9';
                case '\u207A':
                    return '+';
                case '\u207B':
                    return '-';
                default:
                    return ' ';
            }
        }

        public void Write(XmlTextWriter wr)
        {
            wr.WriteStartElement("eqp");
            wr.WriteElementString("eq", Equation);
            var vars = m_term.GetUsedVars();
            if (vars.Length > 0)
            {
                foreach (var var in vars)
                {
                    wr.WriteStartElement("var");
                    wr.WriteAttributeString("name", var);
                    wr.WriteAttributeString("value", m_term.GetVar(var).ToString());
                    wr.WriteEndElement();
                }
            }
            wr.WriteEndElement();
        }

        public void Read(XmlElement node)
        {
            if (node == null)
                return;
            Debug.Assert(node.Name == "eqp");
            foreach (var element in node.ChildNodes.OfType<XmlElement>())
            {
                switch (element.Name)
                {
                    case "eq":
                        Equation = element.InnerText;
                        continue;
                    case "var":
                        var name = element.GetAttribute("name");
                        var value = XmlConvert.ToDouble(element.GetAttribute("value"));
                        m_term.SetVar(name, value);
                        break;
                }
            }
            Calculate(Equation);
        }
    }

    public class Var
    {
        public Var(string n, Complex val)
        {
            Name = n;
            Value = val;
        }

        public Var(string n, string val)
        {
            Name = n;
            Value = new EquationParser().Calculate(val);
        }

        public string Name { get; set; }
        public Complex Value { get; set; }
    }
}