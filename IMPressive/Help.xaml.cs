using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IMPression;

namespace IMPressive
{
    /// <summary>
    /// Logique d'interaction pour Help.xaml
    /// </summary>
    public partial class Help : Window
    {
        public Help()
        {
            InitializeComponent();

            cbxFunc.ItemsSource = Funcs;
        }

        public List<string> Funcs
        {
            get
            {
                var st = new List<string>();
                Function.FunctionsList.All(x =>
                {
                    //st.Add(x.Contains('(') ? x.Substring(0, x.IndexOf('(')) : x);
                    st.Add(new EquationParser().CleanUp(x, true));
                    return true;
                });
                st = st.OrderBy(x => x).Distinct().ToList();
                return st;
            }
        }

        private string dc(string t)
        {
            string r = new EquationParser().DeCleanUp(t).ToLower();
            return r;
        }

        private void cbxFunc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string i = dc(cbxFunc.SelectedItem.ToString());
            string n = "";
            string h = "";
            string n2 = "";
            string sp = "";
            string plain = i.Contains('(') ? i.Substring(0, i.IndexOf('(')) : i;

            switch (plain.ToLower())
            {
                case "pi":
                    h =
                        "π (pi) est une constante mathématique qui représente le [b]rapport[/b] entre la circonférence d'un cercle et son diamètre.\nπ vaut environ [i]3,14[/i].\n\n[u]Exemple :[/u]\nPour un cercle de rayon 5,\nA (aire) = [m]π × 5²[/m]\nP (périmètre) = [m]2 × π × 5[/m]";
                    break;
                case "e":
                    h =
                        "[m]e[/m] est une constante mathématique valant environ 2,71828 et parfois appelée « nombre d'Euler ».\nCe nombre est défini comme étant la base du logarithme naturel (voir [i]ln[/i]). Autrement dit, [m]ln(e) = 1[/m].";
                    break;
                case "phi":
                    h =
                        "Le [b]nombre d'or[/b], aussi représenté par la lettre φ (phi), est une proportion, définie comme l'unique rapport [m]a/b[/m] tel que [m](a + b) ÷ a = a ÷ b[/m]. φ vaut exactement [m](1 + √5) ÷ 2[/m].";
                    break;
                case "eulergamma":
                    h =
                        "La [b]constante d'Euler-Mascheroni[/b], aussi appelé constante Gamma et souvent réprésentée par le signe [m]γ[/m], est une constante utilisée principalement en théorie des nombres, définie comme la limite de la différence entre la série harmonique et le logarithme naturel.";
                    break;
                case "catalan":
                    h =
                        "La [b]constante de Catalan[/b], symbolisée par la lettre [m][i]K[/i][/m] majuscule, est une constante mathématique valant environ 0,915965594.";
                    break;
                case "c":
                    h =
                        "La [b]vitesse de la lumière[/b] dans le vide est une constante physique, notée [i][m]c[/m][/i] pour célérité, et qui vaut environ 299 792 458 mètres par secondes, ce qui équivaut à 1 079 252 848,8 kilomètres par heure.";
                    break;

                case "abs":
                    n = "|[x]a[/x]|";
                    h =
                        "Retourne la [b]valeur absolue[/b] de [m][x]a[/x][/m].\n\n[u]Exemple :[/u]\n[m]abs([x]5[/x]) = 5[/m]\n[m]abs([x]-8[/x]) = 8[/m]";
                    break;
                case "fact":
                    n = "[x]a[/x]!";
                    n2 = "fact([x]a[/x])";
                    sp = "[x]a[/x] ∈ ℕ";
                    h =
                        "Calcule la [b]factorielle[/b] de [x]a[/x].\n\n[u]Exemple :[/u]\n[m]fact([x]5[/x]) = 1 × 2 × 3 × 4 × 5 = 120[/m]\n[m]fact([x]0[/x]) = 1";
                    break;
                case "fact2":
                    n = "[x]a[/x]!!";
                    n2 = "fact2([x]a[/x])";
                    sp = "[x]a[/x] ∈ ℕ";
                    h =
                        "Calcule la [b]double factorielle[/b] de [x]a[/x], soit le produit des nombres pairs ou impairs successifs compris entre 0 et [x]a[/x] inclus.\n\n[u]Exemple :[/u]\n[m]fact2([x]5[/x]) = 1 × 3 × 5 = 15[/m]\n[m]fact2([x]6[/x]) = 2 × 4 × 6 = 48[/m]\n[m]fact([x]0[/x]) = 1";
                    break;

                // Trigonométriques
                case "sin":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule le [b]sinus[/b] de l'angle [x]a[/x] spécifié.\n\n[u]Exemples :[/u]\n[m]sin([x]0[/x]) = 0[/m]\n[m]sin([x]π[/x]) = 0[/m]\n[m]sin([x]π ÷ 2[/x]) = 1[/m]";
                    break;
                case "cos":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule le [b]cosinus[/b] de l'angle [x]a[/x] spécifié.\n\n[u]Exemples :[/u]\n[m]cos([x]0[/x]) = 1[/m]\n[m]cos([x]π[/x]) = -1[/m]\n[m]cos([x]π ÷ 2[/x]) = 0[/m]";
                    break;
                case "tan":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule la [b]tangente[/b] de l'angle [x]a[/x] spécifié.\n\n[u]Exemples :[/u]\n[m]tan([x]0[/x]) = 0[/m]\n[m]tan([x]π[/x]) = 0[/m]";
                    break;
                case "csc":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule la [b]cosécante[/b] de l'angle [x]a[/x] spécifié.\n\n[u]Exemple :[/u]\n[m]csc([x]π ÷ 2[/x]) = 1[/m]";
                    break;
                case "sec":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule la [b]sécante[/b] de l'angle [x]a[/x] spécifié.\n\n[u]Exemples :[/u]\n[m]sec([x]0[/x]) = 1[/m]\n[m]sec([x]π[/x]) = -1[/m]";
                    break;
                case "cot":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Calcule la [b]cotangente[/b] de l'angle [x]a[/x] spécifié.\n\n[u]Exemples :[/u]\n[m]cot([x]π ÷ 4[/x]) = 1[/m]";
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
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]\noù [m]-1 ≤ a ≤ 1[/m]";
                    h =
                        "Retourne l'angle dont le [b]sinus[/b] est [x]a[/x].\n\n[u]Exemples :[/u]\n[m]sin([x]0[/x]) = 0[/m]\n[m]sin([x]π[/x]) = 0[/m]\n[m]sin([x]π ÷ 2[/x]) = 1[/m]";
                    break;
                case "arccos":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]\noù [m]-1 ≤ a ≤ 1[/m]";
                    h =
                        "Retourne l'angle dont le [b]cosinus[/b] est [x]a[/x].\n\n[u]Exemples :[/u]\n[m]cos([x]0[/x]) = 1[/m]\n[m]cos([x]π[/x]) = -1[/m]\n[m]cos([x]π ÷ 2[/x]) = 0[/m]";
                    break;
                case "arctan":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    if (i == "arctan(a)")
                    {
                        h =
                            "Retourne l'angle dont la [b]tangente[/b] est [x]a[/x].\n\n[u]Exemples :[/u]\n[m]tan([x]0[/x]) = 0[/m]\n[m]tan([x]π[/x]) = 0[/m]";
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
                        "Retourne l'angle dont la [b]cosécante[/b] est [x]a[/x].\n\n[u]Exemple :[/u]\n[m]csc([x]π ÷ 2[/x]) = 1[/m]";
                    break;
                case "arcsec":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Retourne l'angle dont la [b]sécante[/b] est [x]a[/x].\n\n[u]Exemples :[/u]\n[m]sec([x]0[/x]) = 1[/m]\n[m]sec([x]π[/x]) = -1[/m]";
                    break;
                case "arccot":
                    sp = "$[i]Les fonctions trigonométriques sont exprimées en radians.[/i]";
                    h =
                        "Retourne l'angle dont la [b]cotangente[/b] est [x]a[/x].\n\n[u]Exemples :[/u]\n[m]cot([x]π ÷ 4[/x]) = 1[/m]";
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
                        "Calcule la [b]racine carrée[/b] de [x]a[/x].\nLa racine carrée de [x]x[/x] est le nombre qui multiplié par lui-même donne [x]x[/x].\n\n[b]Attention :[/b] La racine carrée de -1 n'est pas définie. On la note [m]i[/m]. Les racines carrées de nombres négatifs sont dont calculées à partir de [m]i[/m] ([m]√(2) ≈ 1,414; √(-2) ≈ 1,414 × i[/m]).";
                    break;
                case "curt":
                    h =
                        "Calcule la [b]racine cubique[/b] de [x]a[/x].\nLa racine cubique de [x]x[/x] est le nombre qui multiplié par lui-même deux fois ([m][x]x[/x] × [x]x[/x] × [x]x[/x][/m]) donne [x]x[/x].";
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
                case "rnd":
                    h = "[b]Arrondit[/b] [x]a[/x] à [y]b[/y] décimales.";
                    break;
                case "eq":
                    h = "Retourne 1 si [m][x]a[/x] = [y]b[/y][/m], sinon 0.";
                    break;

                case "log":
                    h = "Calcule le [b]logarithme[/b] de base [y]b[/y] de [x]a[/x].";
                    break;
                case "log10":
                    h = "Calcule le [b]logarithme de base 10[/b] de [x]a[/x].";
                    break;
                case "ln":
                    h = "Calcule le [b]logarithme naturel[/b] (de base [m]e[/m]) de [x]a[/x].";
                    break;
                case "exp":
                    h = "Calcule l'[b]exponentielle[/b] de [x]a[/x] ([m]e[sup][x]a[/x][/sup][/m]).";
                    break;

                case "ceil":
                    h = "Arrondit par [b]excès[/b] [x]a[/x].\n\n[u]Exemples :[/u]\n[m]ceil([x]5.5[/x]) = 6[/m]";
                    break;
                case "floor":
                    h = "Arrondit par [b]défaut[/b] [x]a[/x].\n\n[u]Exemples :[/u]\n[m]floor([x]5.5[/x]) = 5[/m]";
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
                        "Retourne le [b]nombre de radians[/b] dans 1 degré.\n\n[u]Exemples :[/u]\n[i]Pour calculer le cosinus de 45 degrés, saisir [/i][m][b]cos[/b](45°)[/m]";
                    break;

                case "frac":
                    h =
                        "Retourne la [b]partie fractionnaire[/b] de [x]a[/x].\nCette fonction peut aussi être définie comme [m][x]a[/x]-int([x]a[/x])[/m].\n\n[u]Exemples :[/u]\n[m]frac([x]1.5[/x]) = 0.5\nfrac([x]-0.96[/x]) = -0.96";
                    break;

                case "khinchin":
                    h =
                        "La [b]constante de Khinchin[/b] est la valeur de la moyenne géométrique que prennent l'infinité des dénominateurs de la fraction continue d'un nombre réel.\nCette constante vaut environ [m]2,6854520010[/m].";
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
                        "Retourne le signe de [x]a[/x].\n\n[u]Exemples :[/u]\n[m]sgn([x]5[/x]) = 1\nsgn([x]0[/x]) = 0\nsgn([x]-5[/x]) = -1[/m]";
                    break;

                case "sum":
                    h = "Calcule la somme de tous les nombres spécifiés.";
                    break;
            }

            if (n == "") n = new EquationParser().CleanUp(i, true);
            if (n2 == "" && n != "") n2 = i;

            //if (h == "") h = "[i]Pas d'aide disponible[/i]";
            if (h == "")
            {
                var fc = typeof (MathFunctions).GetMethods().FirstOrDefault(x => n.StartsWith(x.Name.ToLower()));
                if (Attribute.IsDefined(fc, typeof (MathFunc)))
                {
                    h = (Attribute.GetCustomAttribute(fc, typeof (MathFunc)) as MathFunc).Description;
                }
                if (h == "")
                {
                    h = "[i]Pas d'aide disponible[/i]";
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
            n2 = new EquationParser().DeCleanUp(n);

            string html = "";
            html += "[font size=48][font face=Cambria]" + n + "[/font][/font]" +
                    (n2 != "" ? ("\n[font size=16][i]" + n2 + "[/i][/font]") : "\n") +
                    (sp != ""
                        ? (sp.StartsWith("$")
                            ? ("\n[font size=16]" + sp.Substring(1) + "[/font]")
                            : ("\n[font size=16]où [m]" + sp + "[/m][/font]"))
                        : "\n");
            html += "\n\n[font size=18]" + h + "[/font]";

            lblHelp.Html = html;
        }
    }
}