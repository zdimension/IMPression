using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IMPression
{
    public class Function : EquationValue
    {
        private static Dictionary<string, FunctionCallback> Functions;
        private static Dictionary<string, FunctionCallback2> Functions2;
        private static Dictionary<string, FunctionCallback3> Functions3;
        private static Dictionary<string, FunctionCallback4> Functions4;
        private static Dictionary<string, FunctionCallback6> Functions6;
        private static Dictionary<string, FunctionCallbackInf> FunctionsInf;
        private static Dictionary<string, FunctionCallbackNone> FunctionsNone;

        public static Random Rnd { get; } = new Random();
        private int m_endindex;
        private string m_func;
        private int m_startIndex;
        private List<Term> m_terms;

        public Function(string src, int start, EquationElement root)
        {
            m_startIndex = start;
            m_func = Term.ExtractName(src, start).ToLower();
            start += m_func.Length;
            // sauter tous les espaces, mais le prochain caractère doit être une parenthèse ouvrante
            if (start == src.Length)
                throw new ParseException(src, m_startIndex, "La fonction doit commencer par '('");
            while (src[start] == ' ')
                start++;
            if (src[start] != '(')
                throw new ParseException(src, m_startIndex, "La fonction doit commencer par '('");
            var termstart = start;
            var end = Term.FindMatchingEnd('(', src, termstart);
            while (end < termstart)
            {
                src += ')';

                end = Term.FindMatchingEnd('(', src, termstart);
            }
            /*if (end < termstart)
            {
                throw new ParseException(src, m_startIndex, "Pas de parenthèse fermante correspondante trouvée");
            }*/

            m_endindex = end;
            var allterms = src.Substring(termstart + 1, end - termstart - 1);
            //string[] terms = allterms.Split(',');
            var terms = GetTerms(allterms);
            m_terms = new List<Term>();
            foreach (var term in terms)
            {
                var newterm = new Term();
                newterm.Parse(term, root);
                newterm.Parent = this;
                m_terms.Add(newterm);
            }
        }

        static Function()
        {
            FunctionsInf = new Dictionary<string, FunctionCallbackInf>(StringComparer.OrdinalIgnoreCase);
            Functions6 = new Dictionary<string, FunctionCallback6>(StringComparer.OrdinalIgnoreCase);
            Functions4 = new Dictionary<string, FunctionCallback4>(StringComparer.OrdinalIgnoreCase);
            Functions3 = new Dictionary<string, FunctionCallback3>(StringComparer.OrdinalIgnoreCase);
            Functions2 = new Dictionary<string, FunctionCallback2>(StringComparer.OrdinalIgnoreCase);
            Functions = new Dictionary<string, FunctionCallback>(StringComparer.OrdinalIgnoreCase);
            FunctionsNone = new Dictionary<string, FunctionCallbackNone>(StringComparer.OrdinalIgnoreCase);

            var funcs =
                typeof (MathFunctions).GetMethods(BindingFlags.Public |
                                                  BindingFlags.Static);
            foreach (var x in funcs)
            {
                if (Attribute.IsDefined(x, typeof (MathFunc)))
                    if ((Attribute.GetCustomAttribute(x, typeof (MathFunc)) as MathFunc).Hide) continue;
                var n = x.Name;
                if (Attribute.IsDefined(x, typeof (MathFunc)))
                    if ((Attribute.GetCustomAttribute(x, typeof (MathFunc)) as MathFunc).Name != "")
                        n = (Attribute.GetCustomAttribute(x, typeof (MathFunc)) as MathFunc).Name;
                switch (x.GetParameters().Count())
                {
                    case 0:
                        FunctionsNone.Add(n,
                            (FunctionCallbackNone) Delegate.CreateDelegate(typeof (FunctionCallbackNone), x));
                        break;
                    case 1:
                        if (Attribute.IsDefined(x.GetParameters()[0], typeof (ParamArrayAttribute)))
                            FunctionsInf.Add(n,
                                (FunctionCallbackInf) Delegate.CreateDelegate(typeof (FunctionCallbackInf), x));
                        else Functions.Add(n, (FunctionCallback) Delegate.CreateDelegate(typeof (FunctionCallback), x));
                        break;
                    case 2:
                        Functions2.Add(n, (FunctionCallback2) Delegate.CreateDelegate(typeof (FunctionCallback2), x));
                        break;
                    case 3:
                        Functions3.Add(n, (FunctionCallback3) Delegate.CreateDelegate(typeof (FunctionCallback3), x));
                        break;
                    case 4:
                        Functions4.Add(n, (FunctionCallback4) Delegate.CreateDelegate(typeof (FunctionCallback4), x));
                        break;
                    /*case 5:
                        del = typeof(FunctionCallback5);     FUTURE
                        break;*/
                    case 6:
                        Functions6.Add(n, (FunctionCallback6) Delegate.CreateDelegate(typeof (FunctionCallback6), x));
                        break;
                }
            }


            FunctionsList = new List<string>();
            Constant.cs.All(x =>
            {
                FunctionsList.Add(x);
                return true;
            });
            FunctionsNone.All(x =>
            {
                FunctionsList.Add(x.Key + "()");
                return true;
            });
            Functions.All(x =>
            {
                FunctionsList.Add(x.Key + "(a)");
                return true;
            });
            Functions2.All(x =>
            {
                FunctionsList.Add(x.Key + "(a; b)");
                return true;
            });
            Functions3.All(x =>
            {
                FunctionsList.Add(x.Key + "(a; b; c)");
                return true;
            });
            Functions4.All(x =>
            {
                FunctionsList.Add(x.Key + "(a; b; c; d)");
                return true;
            });
            Functions6.All(x =>
            {
                FunctionsList.Add(x.Key + "(a; b; c; d; e; f)");
                return true;
            });
            FunctionsInf.All(x =>
            {
                FunctionsList.Add(x.Key + (x.Key == "polynomial" ? "(z; a; b; c; ...)" : "(a; b; c; ...)"));
                return true;
            });
        }

        public static List<string> FunctionsList { get; }


        public override bool Signed
        {
            get { return m_signed; }
            set { m_signed = value; }
        }

        public override Complex Value
        {
            get
            {
                if (FunctionsInf.ContainsKey(m_func) && m_terms.Count > 1)
                {
                    var value = FunctionsInf[m_func](m_terms.Select(x => x.Value).ToArray());
                    if (Signed) return -value;
                    return value;
                }
                else
                {
                    if (m_terms[0].Stack.Count == 0 && FunctionsNone.ContainsKey(m_func))
                    {
                        var value = FunctionsNone[m_func]();
                        if (Signed) return -value;
                        return value;
                    }
                    else if (m_terms.Count == 1 && Functions.ContainsKey(m_func))
                    {
                        var value = Functions[m_func](m_terms[0].Value);
                        if (Signed) return -value;
                        return value;
                    }
                    else if (m_terms.Count == 2 && Functions2.ContainsKey(m_func))
                    {
                        var value = Functions2[m_func](m_terms[0].Value, m_terms[1].Value);
                        if (Signed) return -value;
                        return value;
                    }
                    else if (m_terms.Count == 3 && Functions3.ContainsKey(m_func))
                    {
                        var value = Functions3[m_func](m_terms[0].Value, m_terms[1].Value, m_terms[2].Value);
                        if (Signed) return -value;
                        return value;
                    }
                    else if (m_terms.Count == 4 && Functions4.ContainsKey(m_func))
                    {
                        var value = Functions4[m_func](m_terms[0].Value, m_terms[1].Value, m_terms[2].Value,
                            m_terms[3].Value);
                        if (Signed) return -value;
                        return value;
                    }
                    else if (m_terms.Count == 6 && Functions6.ContainsKey(m_func))
                    {
                        var value = Functions6[m_func](m_terms[0].Value, m_terms[1].Value, m_terms[2].Value,
                            m_terms[3].Value, m_terms[4].Value, m_terms[5].Value);
                        if (Signed) return -value;
                        return value;
                    }
                }


                throw new ParseException(m_func, 0, $"{m_func} n'accepte pas {m_terms.Count} arguments");
            }
        }

        public override int Length => m_endindex - m_startIndex + 1;


        public static bool IsValidDigit(string equation, int index)
        {
            var spi = Term.ExtractName(equation, index).ToLower();
            return Functions.ContainsKey(spi) ||
                   (Functions2.ContainsKey(spi) || (Functions3.ContainsKey(spi) || (Functions4.ContainsKey(spi) ||
                                                                                    (Functions6.ContainsKey(spi) ||
                                                                                     (FunctionsInf.ContainsKey(spi) ||
                                                                                      FunctionsNone.ContainsKey(spi))))));
        }

        public override bool IsValid(string equation, int index)
        {
            return IsValidDigit(equation, index);
        }

        private static IEnumerable<string> GetTerms(string allterms)
        {
            var splitIndex = allterms.IndexOf(';');
            if (splitIndex < 0)
                return new[] {allterms};

            var startChars = new[] {'(', '[', ';'};
            var start = 0;
            var result = new List<string>();
            while (start < allterms.Length)
            {
                var ss = allterms.IndexOfAny(startChars, start);
                if (ss < 0)
                {
                    var s = allterms.Substring(start).Trim();
                    if (s.Length > 0)
                        result.Add(s);
                    break;
                }
                int len;
                if (allterms[ss] == ';')
                {
                    // copier depuis le début
                    len = ss - start;
                    var s = allterms.Substring(start, len).Trim();
                    if (s.Length > 0)
                        result.Add(s);
                    start = ss + 1;
                    continue;
                }
                var termend = Term.FindMatchingEnd(allterms[ss], allterms, ss);
                len = termend - start + 1;
                var su = allterms.Substring(start, len).Trim();
                if (su.Length > 0)
                    result.Add(su);
                start = termend + 1;
                ss = allterms.IndexOfAny(startChars, start);
                if (ss > 0)
                    start = ss + 1;
            }
            return result.ToArray();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(m_func);
            sb.Append('(');
            for (var index = 0; index < m_terms.Count; index++)
            {
                sb.Append(m_terms[index]);
                if (index < m_terms.Count - 1)
                    sb.Append(';');
            }
            sb.Append(')');
            return sb.ToString();
        }


        private delegate Complex FunctionCallbackNone();

        private delegate Complex FunctionCallback(Complex value);

        private delegate Complex FunctionCallback2(Complex value, Complex value1);

        private delegate Complex FunctionCallback3(Complex value, Complex value1, Complex value2);

        private delegate Complex FunctionCallback4(Complex value, Complex value1, Complex value2, Complex value3);

        private delegate Complex FunctionCallback6(
            Complex value, Complex value1, Complex value2, Complex value3, Complex value4, Complex value5);

        private delegate Complex FunctionCallbackInf(params Complex[] values);
    }
}