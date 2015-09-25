using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMPression;

namespace IMPressive.Prompt
{
    internal class Program
    {
        public static List<string> Funcs
        {
            get
            {
                var st = new List<string>();
                Function.FunctionsList.All(x =>
                {
                    st.Add(x.Contains('(') ? x.Substring(0, x.IndexOf('(')) : x);
                    return true;
                });
                if (Vars?.Count > 0) st.AddRange(Vars.Keys);
                st = st.OrderBy(x => x).Distinct().ToList();
                return st;
            }
        }

        private static Dictionary<string, Var> Vars;
        private static EquationParser parser;

        private static void Main(string[] args)
        {
            parser = new EquationParser();
            Vars = new Dictionary<string, Var> {{"ans", new Var("ans", 0)}};
            Console.WriteLine("Console IMP build 131 (c) 2015");
            Console.WriteLine("Tapez 'help' pour une liste des commandes disponibles.");
            while (true)
            {
                Console.Write("> ");
                var flc = Console.ReadLine().Trim().ToLower().Split(' ');
                var cmd = flc[0];
                switch (cmd)
                {
                    case "exit":
                        return;
                    case "help":
                        if (flc.Count() > 1)
                        {
                            ShowHelp(flc[1]);
                        }
                        else
                        {
                            Console.WriteLine("Liste des commandes");
                            Console.WriteLine("fl               Afficher la liste des fonctions disponibles");
                            Console.WriteLine("cl               Afficher la liste des constantes disponibles");
                            Console.WriteLine("exit             Quitter");
                            Console.WriteLine("help             Afficher l'aide");
                            Console.WriteLine("help <fonction>  Afficher l'aide d'une fonction");
                        }
                        break;
                    case "fl":
                        break;
                    case "cl":
                        break;
                    default:
                        Calculate(cmd);
                        break;
                }
            }
        }

        private static string oldop = "";

        static string WrapText(string text)
        {
            string[] originalLines = text.Split(new string[] { " " },
                StringSplitOptions.None);

            List<string> wrappedLines = new List<string>();

            StringBuilder actualLine = new StringBuilder();
            double actualWidth = 0;

            foreach (var item in originalLines)
            {
                actualLine.Append(item + " ");
                actualWidth += item.Length;

                if (actualWidth > 80)
                {
                    wrappedLines.Add(actualLine.ToString());
                    actualLine.Clear();
                    actualWidth = 0;
                }
            }

            if (actualLine.Length > 0)
                wrappedLines.Add(actualLine.ToString());

            return string.Join("\n", wrappedLines.ToArray());
        }

        public static void Calculate(string cmd)
        {
            try
            {
                Complex res = 0.0;
                string op = parser.DeCleanUp(cmd).Trim();
                if (op.Contains("="))
                {
                    if (!op.StartsWith("="))
                    {
                        var splash = op.Split('=');
                        if (splash.Count() == 2)
                        {
                            string varname = splash[0];
                            if (Constant.cs.Any(x => x.Equals(varname, StringComparison.OrdinalIgnoreCase)))
                                throw new ParseException(op, 0,
                                    $"Une constante nommée '{varname}' existe déjà.");
                            if (Funcs.Any(x => x.Equals(varname, StringComparison.OrdinalIgnoreCase)))
                                throw new ParseException(op, 0,
                                    $"Une fonction nommée '{varname}' existe déjà.");
                            var fullop = splash[1];
                            Parallel.Invoke(() => res = parser.Calculate(fullop, Vars.Values.ToArray()));
                            if (Vars.ContainsKey(varname)) Vars[varname] = new Var(varname, res);
                            else Vars.Add(varname, new Var(varname, res));
                            Console.WriteLine("  " + res);
                        }
                    }
                }
                else
                {
                    Parallel.Invoke(() => res = parser.Calculate(op, Vars.Values.ToArray()));
                    Console.WriteLine("  " + parser.CleanUp(res.ToString()).Replace("÷", "/"));
                    oldop = parser.CleanUp(cmd);
                    if (!Complex.IsNaN(res) && !Complex.IsInfinity(res) && !Complex.IsIndeterminate(res))
                        Vars["ans"] = new Var("ans", res);
                    else Vars["ans"] = new Var("ans", 0);
                }
            }
            catch (Exception e)
            {
                Throw(e);
            }
        }

        public static void Throw(Exception e)
        {
            if (e is ParseException || e.InnerException is ParseException)
            {
                Console.WriteLine((e.InnerException ?? e as ParseException).Message);
            }
            else
            {
                if (e.InnerException != null)
                {
                    Console.WriteLine("Erreur : " + e.InnerException.Message);
                }
                else
                {
                    Console.WriteLine("Erreur : " + e.Message);
                }
            }
        }

        public static void ShowHelp(string fn)
        {
            string i = fn.Trim().ToLower();
            string n = "";
            string h = "";
            string n2 = "";
            string sp = "";
            string plain = i.Contains('(') ? i.Substring(0, i.IndexOf('(')) : i;

            switch (plain.ToLower())
            {
                case "pi":
                    h =
                        "π (pi) est une constante mathématique qui représente le [b]rapport[/b] entre la circonférence d'un cercle et son diamètre.\nπ vaut environ [i]3,14[/i].\n\n[u]Exemple :[/u]\nPour un cercle de rayon 5,\nA (aire) = π × 5²\nP (périmètre) = 2 × π × 5";
                    break;
                case "e":
                    h =
                        "e est une constante mathématique valant environ 2,71828 et parfois appelée « nombre d'Euler ».\nCe nombre est défini comme étant la base du logarithme naturel (voir [i]ln[/i]). Autrement dit, ln(e) = 1.";
                    break;
                case "phi":
                    h =
                        "Le [b]nombre d'or[/b], aussi représenté par la lettre φ (phi), est une proportion, définie comme l'unique rapport a/b tel que (a + b) ÷ a = a ÷ b. φ vaut exactement (1 + √5) ÷ 2.";
                    break;
                case "eulergamma":
                    h =
                        "La [b]constante d'Euler-Mascheroni[/b], aussi appelé constante Gamma et souvent réprésentée par le signe γ, est une constante utilisée principalement en théorie des nombres, définie comme la limite de la différence entre la série harmonique et le logarithme naturel.";
                    break;
                case "catalan":
                    h =
                        "La [b]constante de Catalan[/b], symbolisée par la lettre [i]K[/i] majuscule, est une constante mathématique valant environ 0,915965594.";
                    break;
                case "c":
                    h =
                        "La [b]vitesse de la lumière[/b] dans le vide est une constante physique, notée [i]c[/i] pour célérité, et qui vaut environ 299 792 458 mètres par secondes, ce qui équivaut à 1 079 252 848,8 kilomètres par heure.";
                    break;

                case "abs":
                    n = "|[x]a[/x]|";
                    n2 = "abs([x]a[/x])";
                    h =
                        "Retourne la [b]valeur absolue[/b] de [x]a[/x].\n\n[u]Exemple :[/u]\nabs([x]5[/x]) = 5\nabs([x]-8[/x]) = 8";
                    break;
                case "fact":
                    n = "[x]a[/x]!";
                    n2 = "fact([x]a[/x])";
                    sp = "[x]a[/x] ∈ ℕ";
                    h =
                        "Calcule la [b]factorielle[/b] de [x]a[/x].\n\n[u]Exemple :[/u]\nfact([x]5[/x]) = 1 × 2 × 3 × 4 × 5 = 120\nfact([x]0[/x]) = 1";
                    break;
                case "fact2":
                    n = "[x]a[/x]!!";
                    n2 = "fact2([x]a[/x])";
                    sp = "[x]a[/x] ∈ ℕ";
                    h =
                        "Calcule la [b]double factorielle[/b] de [x]a[/x], soit le produit des nombres pairs ou impairs successifs compris entre 0 et [x]a[/x] inclus.\n\n[u]Exemple :[/u]\nfact2([x]5[/x]) = 1 × 3 × 5 = 15\nfact2([x]6[/x]) = 2 × 4 × 6 = 48\nfact([x]0[/x]) = 1";
                    break;

                // Trigonométriques
                case "sin":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule le [b]sinus[/b] de l'angle [x]a[/x] spécifié.\n\n[u]Exemples :[/u]\nsin([x]0[/x]) = 0 \nsin([x]π[/x]) = 0\nsin([x]π ÷ 2[/x]) = 1";
                    break;
                case "cos":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule le [b]cosinus[/b] de l'angle [x]a[/x] spécifié.\n\n[u]Exemples :[/u]\ncos([x]0[/x]) = 1\ncos([x]π[/x]) = -1\ncos([x]π ÷ 2[/x]) = 0";
                    break;
                case "tan":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule la [b]tangente[/b] de l'angle [x]a[/x] spécifié.\n\n[u]Exemples :[/u]\ntan([x]0[/x]) = 0\ntan([x]π[/x]) = 0";
                    break;
                case "csc":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule la [b]cosécante[/b] de l'angle [x]a[/x] spécifié.\n\n[u]Exemple :[/u]\ncsc([x]π ÷ 2[/x]) = 1";
                    break;
                case "sec":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule la [b]sécante[/b] de l'angle [x]a[/x] spécifié.\n\n[u]Exemples :[/u]\nsec([x]0[/x]) = 1\nsec([x]π[/x]) = -1";
                    break;
                case "cot":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule la [b]cotangente[/b] de l'angle [x]a[/x] spécifié.\n\n[u]Exemples :[/u]\ncot([x]π ÷ 4[/x]) = 1";
                    break;

                // Hyperboliques
                case "sinh":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule le [b]sinus hyperbolique[/b] de l'angle [x]a[/x] spécifié.";
                    break;
                case "cosh":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule le [b]cosinus hyperbolique[/b] de l'angle [x]a[/x] spécifié.";
                    break;
                case "tanh":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule la [b]tangente hyperbolique[/b] de l'angle [x]a[/x] spécifié.";
                    break;
                case "csch":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule la [b]cosécante hyperbolique[/b] de l'angle [x]a[/x] spécifié.";
                    break;
                case "sech":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule la [b]sécante hyperbolique[/b] de l'angle [x]a[/x] spécifié.";
                    break;
                case "coth":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule la [b]cotangente hyperbolique[/b] de l'angle [x]a[/x] spécifié.";
                    break;


                // Inverses
                case "arcsin":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]\noù -1 ≤ a ≤ 1";
                    h =
                        "Retourne l'angle dont le [b]sinus[/b] est [x]a[/x].\n\n[u]Exemples :[/u]\nsin([x]0[/x]) = 0\nsin([x]π[/x]) = 0\nsin([x]π ÷ 2[/x]) = 1";
                    break;
                case "arccos":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]\noù -1 ≤ a ≤ 1";
                    h =
                        "Retourne l'angle dont le [b]cosinus[/b] est [x]a[/x].\n\n[u]Exemples :[/u]\ncos([x]0[/x]) = 1\ncos([x]π[/x]) = -1\ncos([x]π ÷ 2[/x]) = 0";
                    break;
                case "arctan":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    if (i == "arctan(a)")
                    {
                        h =
                            "Retourne l'angle dont la [b]tangente[/b] est [x]a[/x].\n\n[u]Exemples :[/u]\ntan([x]0[/x]) = 0\ntan([x]π[/x]) = 0";
                    }
                    else
                    {
                        h =
                            "Retourne l'angle [b]en radians[/b] entre la partie positive de l'axe des [x]x[/x] d'un plan, et le point de ce plan au coordonnées ([x]a[/x],[y]b[/y]).";
                    }
                    break;
                case "arccsc":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Retourne l'angle dont la [b]cosécante[/b] est [x]a[/x].\n\n[u]Exemple :[/u]\ncsc([x]π ÷ 2[/x]) = 1";
                    break;
                case "arcsec":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Retourne l'angle dont la [b]sécante[/b] est [x]a[/x].\n\n[u]Exemples :[/u]\nsec([x]0[/x]) = 1\nsec([x]π[/x]) = -1";
                    break;
                case "arccot":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Retourne l'angle dont la [b]cotangente[/b] est [x]a[/x].\n\n[u]Exemples :[/u]\ncot([x]π ÷ 4[/x]) = 1";
                    break;
                case "arcsinh":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Retourne l'angle dont le [b]sinus hyperbolique[/b] [x]a[/x].";
                    break;
                case "arccosh":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Retourne l'angle dont le [b]cosinus hyperbolique[/b] est [x]a[/x].";
                    break;
                case "arctanh":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Retourne l'angle dont la [b]tangente hyperbolique[/b] est [x]a[/x].";
                    break;
                case "arccsch":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Retourne l'angle dont la [b]cosécante hyperbolique[/b] est [x]a[/x].";
                    break;
                case "arcsech":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Retourne l'angle dont la [b]sécante hyperbolique[/b] est [x]a[/x].";
                    break;
                case "arccoth":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Retourne l'angle dont la [b]cotangente hyperbolique[/b] est [x]a[/x].";
                    break;
                case "sinc":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule le [b]sinus cardinal[/b] de l'angle [x]a[/x] spécifié.";
                    break;

                case "sqrt":
                    sp = "x ≥ 0";
                    h =
                        "Calcule la [b]racine carrée[/b] de [x]a[/x].\nLa racine carrée de [x]x[/x] est le nombre qui multiplié par lui-même donne [x]x[/x].\n\n[b]Attention :[/b] La racine carrée de -1 n'est pas définie. On la note i. Les racines carrées de nombres négatifs sont dont calculées à partir de i (√(2) ≈ 1,414; √(-2) ≈ 1,414 × i).";
                    break;
                case "curt":
                    h =
                        "Calcule la [b]racine cubique[/b] de [x]a[/x].\nLa racine cubique de [x]x[/x] est le nombre qui multiplié par lui-même deux fois ([x]x[/x] × [x]x[/x] × [x]x[/x]) donne [x]x[/x].";
                    break;
                case "root":
                    h =
                        "Calcule la [b]racine [y]b[/y]-ième[/b] de [x]a[/x].";
                    break;

                case "rand":
                    if (n == "rand()")
                        h = "Retourne un [b]nombre aléatoire[/b] compris entre 0,0 et 1,0.";
                    else
                        h = "Retourne un [b]nombre aléatoire[/b] compris entre [x]a[/x] et [y]b[/y].";
                    break;
                case "randn":
                    h = "Retourne un [b]nombre aléatoire[/b] compris entre [x]a[/x] et [y]b[/y].";
                    break;
                case "round":
                    h = "[b]Arrondit[/b] [x]a[/x] à [y]b[/y] décimales.";
                    break;
                case "eq":
                    h = "Retourne 1 si [x]a[/x] = [y]b[/y], sinon 0.";
                    break;

                case "log":
                    h = "Calcule le [b]logarithme[/b] de base [y]b[/y] de [x]a[/x].";
                    break;
                case "log10":
                    h = "Calcule le [b]logarithme de base 10[/b] de [x]a[/x].";
                    break;
                case "ln":
                    h = "Calcule le [b]logarithme naturel[/b] (de base e) de [x]a[/x].";
                    break;
                case "exp":
                    h = "Calcule l'[b]exponentielle[/b] de [x]a[/x] (e^[x]a[/x]).";
                    break;

                case "ceil":
                    h = "Arrondit par [b]excès[/b] [x]a[/x].\n\n[u]Exemples :[/u]\nceil([x]5.5[/x]) = 6";
                    break;
                case "floor":
                    h = "Arrondit par [b]défaut[/b] [x]a[/x].\n\n[u]Exemples :[/u]\nfloor([x]5.5[/x]) = 5";
                    break;

                case "fibo":
                    h = "Retourne le [x]a[/x]-ième nombre de la [b]suite de Fibonacci[/b].";
                    break;
                case "int":
                    h = "Retourne la [b]partie entière[/b] de [x]a[/x].";
                    break;

                case "max":
                    h = "Retourne la [b]plus grande valeur[/b] spécifiée.";
                    break;
                case "min":
                    h = "Retourne la [b]plus petite valeur[/b] spécifiéee.";
                    break;
                case "gcd":
                    h = "Calcule le [b]plus grand diviseur commun[/b] (PGCD) des nombres spécifiés.";
                    break;
                case "lcm":
                    h = "Calcule le [b]plus petit multiple commun[/b] (PPCM) des nombres spécifiés.";
                    break;

                case "gamma":
                    sp = "x > 0";
                    h = "Calcule la valeur de la [b]fonction gamma[/b] pour [x]a[/x].";
                    break;
                case "gammaln":
                    h = "Calcule le logarithme de la [b]fonction gamma[/b] pour [x]a[/x].";
                    break;
                case "gammaupreg":
                    h = "";
                    break;
                case "gammaupinc":
                    h = "";
                    break;
                case "gammalowinc":
                    h = "";
                    break;
                case "gammalowreg":
                    h = "";
                    break;
                case "gammalowreginv":
                    h = "";
                    break;
                case "digamma":
                    h = "Calcule la valeur de la [b]fonction digamma[/b] pour [x]a[/x].";
                    break;
                case "loggamma":
                    h = "Calcule la valeur de la [b]fonction loggamma[/b] pour [x]a[/x].";
                    break;

                case "avr":
                    h = "Calcule la [b]moyenne[/b] de la série statistique spécifiée.";
                    break;
                case "med":
                    h = "Détermine la [b]médiane[/b] de la série statistique spécifiée.";
                    break;
                case "q1":
                    h = "Détermine le [b]premier quartile[/b] de la série statistique spécifiée.";
                    break;
                case "q3":
                    h = "Détermine le [b]troisième quartile[/b] de la série statistique spécifée.";
                    break;

                case "binomial":
                    h = "Retourne le [b]coefficient binomial[/b].";
                    break;

                case "degree":
                    h =
                        "Retourne le [b]nombre de radians[/b] dans 1 degré.\n\n[u]Exemples :[/u]\n[i]Pour calculer le cosinus de 45 degrés, saisir [/i][b]cos[/b](45°)";
                    break;

                case "frac":
                    h =
                        "Retourne la [b]partie fractionnaire[/b] de [x]a[/x].\nCette fonction peut aussi être définie comme [x]a[/x]-int([x]a[/x]).\n\n[u]Exemples :[/u]\nfrac([x]1.5[/x]) = 0.5\nfrac([x]-0.96[/x]) = -0.96";
                    break;

                case "khinchin":
                    h =
                        "La [b]constante de Khinchin[/b] est la valeur de la moyenne géométrique que prennent l'infinité des dénominateurs de la fraction continue d'un nombre réel.\nCette constante vaut environ 2,6854520010.";
                    break;

                case "multinomial":
                    h = "Retourne le [b]coefficient multinomial[/b].";
                    break;

                case "polylog":
                    h = "Calcule la [b]fonction polylogarithme[/b] pour [x]a[/x] et [y]b[/y].";
                    break;

                case "productlog":
                    if (i == "productlog(a)")
                    {
                        h = "Calcule la [b]fonction W de Lambert[/b] (aussi appelée ProductLog) pour [x]a[/x].";
                    }
                    else
                    {
                        h =
                            "Calcule la [b]fonction W de Lambert[/b] (aussi appelée ProductLog) pour [x]a[/x] et [y]b[/y].";
                    }
                    break;

                case "quotient":
                    h = "Calcule le quotient de [x]a[/x] et [y]b[/y].";
                    break;

                case "sgn":
                    h =
                        "Retourne le signe de [x]a[/x].\n\n[u]Exemples :[/u]\nsgn([x]5[/x]) = 1\nsgn([x]0[/x]) = 0\nsgn([x]-5[/x]) = -1";
                    break;

                case "sum":
                    h = "Calcule la somme de tous les nombres spécifiés.";
                    break;
            }

            if (n == "") n = new EquationParser().DeCleanUp(i);
            if (n2 == "" && n != "") n2 = i;

            if (h == "")
            {
                var fc = typeof(MathFunctions).GetMethods().FirstOrDefault(x => n.StartsWith(x.Name.ToLower()));
                if (Attribute.IsDefined(fc, typeof(MathFunc)))
                {
                    h = (Attribute.GetCustomAttribute(fc, typeof(MathFunc)) as MathFunc).Description;
                }
                if (h == "")
                {
                    h = "[i]Pas d'aide disponible[/i]";
                }
            }

            if(!n.Contains("("))
            {
                if(typeof(MathFunctions).GetMethods().Select(x => x.Name.ToLower()).Any(x => x.Contains(n.ToLower())))
                {
                    n = Function.FunctionsList.First(x => (x.Contains('(') ? x.ToLower().Substring(0, x.IndexOf('(')) : x.ToLower()) == n.ToLower());
                }
            }

            if (n.Contains("(a)"))
            {
                n = n.Replace("(a)", "([x]a[/x])");
            }
            if (n.Contains("(a; b)"))
            {
                n = n.Replace("(a; b)", "([x]a[/x]; [y]b[/y])");
            }
            if (n.Contains("(a; b; c)"))
            {
                n = n.Replace("(a; b; c)", "([x]a[/x]; [y]b[/y]; [z]c[/z])");
            }
            if (n.Contains("(a; b; c; d)"))
            {
                n = n.Replace("(a; b; c)", "([x]a[/x]; [y]b[/y]; [z]c[/z]; d)");
            }
            if (n.Contains("(a; b; c; ...)"))
            {
                n = n.Replace("(a; b; c; ...)", "([x]a[/x]; [y]b[/y]; [z]c[/z]; ...)");
            }
            if (n.Contains("(z; a; b; c; ...)"))
            {
                n = n.Replace("(z; a; b; c; ...)", "(z; [x]a[/x]; [y]b[/y]; [z]c[/z]; ...)");
            }
            //n2 = new EquationParser().DeCleanUp(n);

            WriteWColor($"Fonction : {n}", true);
            //if(n2 != "" && n2 != n) WriteWColor($"Alias : {n2}", true);
            if(sp != "")
            {
                if(sp.StartsWith("$"))
                {
                    WriteWColor(parser.DeCleanUp(sp).Substring(1), true);
                }
                else
                {
                    WriteWColor($"où {parser.DeCleanUp(sp)}", true);
                }
            }
            WriteWColor($"\n{parser.DeCleanUp(h)}", true);
        }

        public static void WriteWColor(string t, bool nl = false)
        {
            t = WrapText(t);
            var bclr = Console.ForegroundColor;
            int index = 0;
            while(index < t.Length)
            {
                var ch = t[index];
                if(ch == '[' && t[index + 1] == '/')
                {
                    Console.ForegroundColor = bclr;
                    index += 4;
                }
                else if (ch == '[' && t[index + 1] != '/')
                {
                    var nc = t[index + 1];
                    if (nc == 'x')
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    if (nc == 'y')
                        Console.ForegroundColor = ConsoleColor.Red;
                    if(nc == 'z')
                        Console.ForegroundColor = ConsoleColor.Green;
                    index += 3;
                }
                else
                {
                    if(ch == '\n') Console.WriteLine();
                    else Console.Write(ch);
                    index++;
                }
            }
            if(nl) Console.WriteLine();
        }
    }
}