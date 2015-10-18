using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using IMPression;
using IMPression.Parser;
using IMPressive.Outils;
using IMPressive.Properties;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace IMPressive
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool Stopwatch = false;

        public EquationParser parser = new EquationParser();

        public MainWindow()
        {
            InitializeComponent();
            txtOperation.ItemsSource = Funcs;
            lblHelp.Text = "";
            EquationParser.UseDegrees = Settings.Default.UseDegrees;
            Vars = new Dictionary<string, Var> {{"ans", new Var("ans", 0)}};
        }

        private void txtOperation_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Calculate();
            }
            else if (e.Key == Key.Up)
            {
                txtOperation.Text = lblOldOp.Text;
            }
        }

        private void Calculate()
        {
            try
            {
                Complex res = 0.0;
                string op = parser.DeCleanUp(txtOperation.Text).Trim();
                if (op.Contains("="))
                {
                    if (!op.StartsWith("="))
                    {
                        var splash = op.Split('=');
                        if (splash.Count() == 2)
                        {
                            string varname = splash[0];
                            if(Constant.cs.Any(x => x.Equals(varname, StringComparison.OrdinalIgnoreCase)))
                                throw new ParseException(op, 0, $"Une constante nommée '{varname}' existe déjà.");
                            if (Funcs.Any(x => x.Equals(varname, StringComparison.OrdinalIgnoreCase)))
                                throw new ParseException(op, 0, $"Une fonction nommée '{varname}' existe déjà.");
                            var fullop = splash[1];
                            Stopwatch sw = null;
                            if (Stopwatch)
                            {
                                sw = new Stopwatch();
                                sw.Start();
                            }
                            Parallel.Invoke(() => res = parser.Calculate(fullop, Vars.Values.ToArray()));
                            if (Stopwatch)
                            {
                                sw.Stop();
                                MessageBox.Show(sw.Elapsed.ToString());
                            }
                            if (Vars.ContainsKey(varname)) Vars[varname] = new Var(varname, res);
                            else Vars.Add(varname, new Var(varname, res));
                            txtOperation.ItemsSource = Funcs;
                            txtResult.Text = res.ToString();
                            lblOldOp.Text = parser.CleanUp(txtOperation.Text);
                            txtOperation.Text = "";
                        }
                    }
                }
                else
                {
                    Stopwatch sw = null;
                    if (Stopwatch)
                    {
                        sw = new Stopwatch();
                        sw.Start();
                    }
                    Parallel.Invoke(() => res = parser.Calculate(op, Vars.Values.ToArray()));
                    if (Stopwatch)
                    {
                        sw.Stop();
                        MessageBox.Show(sw.Elapsed.ToString());
                    }
                    txtResult.Text = parser.CleanUp(res.ToString()).Replace("÷", "/");
                    lblOldOp.Text = parser.CleanUp(txtOperation.Text);
                    txtOperation.Text = "";
                    if (!Complex.IsNaN(res) && !Complex.IsInfinity(res) && !Complex.IsIndeterminate(res))
                        Vars["ans"] = new Var("ans", res);
                    else Vars["ans"] = new Var("ans", 0);
                    txtOperation.ItemsSource = Funcs;
                }
                FormattedText formattedText = new FormattedText(txtResult.Text, CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(txtResult.FontFamily, txtResult.FontStyle, txtResult.FontWeight, txtResult.FontStretch),
                    txtResult.FontSize, Brushes.Black, null, TextOptions.GetTextFormattingMode(txtResult));
                double wd = txtResult.Width - formattedText.Width - 30;
                if (wd <= 0) wd = 78;
                lblOldOp.Width = wd;
            }
            catch (Exception e)
            {
                e.Throw();
            }
        }

        public List<string> Funcs
        {
            get
            {
                var st = new List<string>();
                Function.FunctionsList.All(x =>
                {
                    st.Add(x.Contains('(') ? x.Substring(0, x.IndexOf('(')) : x);
                    return true;
                });
                if(Vars?.Count > 0) st.AddRange(Vars.Keys);
                st = st.OrderBy(x => x).Distinct().ToList();
                return st;
            }
        }


        private void btnEquals_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
        }

        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "0";
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "1";
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "2";
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "3";
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "4";
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "5";
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "6";
        }

        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "7";
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "8";
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "9";
        }

        private void btnComma_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += ",";
        }

        private void btnDiv_Click(object sender, RoutedEventArgs e)
        {
            if (txtOperation.Text == "") txtOperation.Text += "ans";
            txtOperation.Text += "÷";
        }

        private void btnTimes_Click(object sender, RoutedEventArgs e)
        {
            if (txtOperation.Text == "") txtOperation.Text += "ans";
            txtOperation.Text += "×";
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "-";
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "+";
        }

        private void btnInverse_Click(object sender, RoutedEventArgs e)
        {
            if (txtOperation.Text == "") txtOperation.Text += "1/ans";
            else txtOperation.Text += "1/";
        }

        private void btnSqrt_Click(object sender, RoutedEventArgs e)
        {
            if (txtOperation.Text == "") txtOperation.Text += "√(ans)";
            else txtOperation.Text += "√(";
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text = "";
            txtResult.Text = "0";
            lblOldOp.Text = "";
        }

        private void btnLP_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "(";
        }

        private void btnRP_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += ")";
        }

        private void btnGraph_Click(object sender, RoutedEventArgs e)
        {
            var rsf = new GraphWindow();
            rsf.ShowDialog();
        }

        private void btnTrigFunc_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += (sender as Button).Content + "(";
        }

        private void btnSqr_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "²";
        }

        private void btnCube_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "³";
        }

        private void btnPow_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "^";
        }

        private void btnFact_Click(object sender, RoutedEventArgs e)
        {
            if (txtOperation.Text == "") txtOperation.Text += "ans!";
            else txtOperation.Text += "fact(";
        }


        private double Memory;
        private Dictionary<string, Var> Vars;

        private void btnMC_Click(object sender, RoutedEventArgs e)
        {
            Memory = 0;
        }

        private void btnMR_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += Memory.ToString();
        }

        private void btnMS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Memory = parser.Calculate(parser.DeCleanUp(txtOperation.Text), Vars.Values.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnMP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Memory += parser.Calculate(parser.DeCleanUp(txtOperation.Text), Vars.Values.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnMM_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Memory -= parser.Calculate(parser.DeCleanUp(txtOperation.Text), Vars.Values.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void chkInverted_Checked(object sender, RoutedEventArgs e)
        {
            btnCos.Focus();
            btnCos.Content = "cos⁻¹";
            btnSin.Content = "sin⁻¹";
            btnTan.Content = "tan⁻¹";
            btnCot.Content = "cot⁻¹";
            btnSec.Content = "sec⁻¹";
            btnCsc.Content = "csc⁻¹";
            btnCosh.Content = "cosh⁻¹";
            btnSinh.Content = "sinh⁻¹";
            btnTanh.Content = "tanh⁻¹";
            btnCoth.Content = "coth⁻¹";
            btnSech.Content = "sech⁻¹";
            btnCsch.Content = "csch⁻¹";
            /*btnCos.FontSize = 12;
            btnSin.FontSize = 12;
            btnTan.FontSize = 12;
            btnCot.FontSize = 12;
            btnSec.FontSize = 12;
            btnCsc.FontSize = 12;
            btnCosh.FontSize = 12;
            btnSinh.FontSize = 12;
            btnTanh.FontSize = 12;
            btnCoth.FontSize = 12;
            btnSech.FontSize = 12;
            btnCsch.FontSize = 12;*/
        }

        private void chkInverted_Unchecked(object sender, RoutedEventArgs e)
        {
            btnCos.Focus();
            btnCos.Content = "cos";
            btnSin.Content = "sin";
            btnTan.Content = "tan";
            btnCot.Content = "cot";
            btnSec.Content = "sec";
            btnCsc.Content = "csc";
            btnCosh.Content = "cosh";
            btnSinh.Content = "sinh";
            btnTanh.Content = "tanh";
            btnCoth.Content = "coth";
            btnSech.Content = "sech";
            btnCsch.Content = "csch";
        }

        private void txtOperation_TextChanged(object sender, RoutedEventArgs e)
        {
            //txtOperation.Text = parser.CleanUp(txtOperation.Text);
            //txtOperation.se
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }

        private void btnCubeRt_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "∛(";
        }

        private void btnNthRoot_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "root(";
        }

        private void btnLog10_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "log10(";
        }

        private void btnExp_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "exp(";
        }

        private void btnRand_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "rand()";
        }

        private void Tooltip(object sender, MouseEventArgs e)
        {
            string n = (sender as Button)?.Content.ToString() ??
                       ((sender as SplitButton)?.Content.ToString() ?? (sender as DropDownButton).Content.ToString());
            bool p = true;
            string h = "";
            string a1 = "";
            string a2 = "";

            switch (n)
            {
                case "+":
                    n = "";
                    a1 = "x";
                    a2 = "y";
                    h = "Ajoute x à y.";
                    break;
                case "-":
                    n = "";
                    a1 = "x";
                    a2 = "y";
                    h = "Soustrait y à x.";
                    break;
                case "×":
                    n = "";
                    a1 = "x";
                    a2 = "y";
                    h = "Multiplie x par y.";
                    break;
                case "÷":
                    n = "";
                    a1 = "x";
                    a2 = "y";
                    h = "Divise x par y.";
                    break;

                case "C":
                    n = "";
                    h = "Efface l'opération en cours.";
                    break;

                case "Graphique":
                    n = "";
                    h = "Ouvre une fenêtre de graphique.";
                    break;
                case "Graphique 3D":
                    n = "";
                    h = "Ouvre une fenêtre de graphique 3D.";
                    break;
                case "Graphique 3D complexe":
                    n = "";
                    h = "Ouvre une fenêtre de graphique 3D complexe.";
                    break;
                case "Graphique polaire":
                    n = "";
                    h = "Ouvre une fenêtre de graphique polaire.";
                    break;
                case "Graphique paramétrique":
                    n = "";
                    h = "Ouvre une fenêtre de graphique paramétrique.";
                    break;
                case "Graphique polaire paramétrique":
                    n = "";
                    h = "Ouvre une fenêtre de graphique polaire paramétrique.";
                    break;
                case "Graphique de points":
                    n = "";
                    h = "Ouvre une fenêtre de graphique à partir d'une liste de points.";
                    break;

                case "Outils":
                    n = "";
                    h = "Outils de calcul divers.";
                    break;
                case "Calculer le côté d'un triangle rectangle":
                    n = "";
                    h = "Outil pour calculer un côté d'un triangle rectangle avec le théorème de Pythagore.";
                    break;


                case "MC":
                    p = false;
                    h = "Efface le contenu de la mémoire.";
                    break;
                case "MR":
                    p = false;
                    h = "Retourne le contenu de la mémoire.";
                    break;
                case "MS":
                    p = false;
                    h = "Définit le contenu de la mémoire.";
                    break;
                case "M+":
                    p = false;
                    h = "Ajoute le résultat à la mémoire.";
                    break;
                case "M-":
                    p = false;
                    h = "Soustrait le résultat à la mémoire.";
                    break;

                case "n!":
                    n = "fact";
                    a1 = "x";
                    h = "Calcule la factorielle de x.";
                    break;
                case "√":
                    a1 = "x";
                    h = "Calcule la racine carrée de x.";
                    break;

                case "x²":
                    p = false;
                    a1 = "x";
                    n = "";
                    h = "x² : Calcule le carré de x (x × x).";
                    break;
                case "x³":
                    p = false;
                    a1 = "x";
                    n = "";
                    h = "x³ : Calcule le cube de x (x × x × x).";
                    break;
                case "xⁿ":
                    p = false;
                    a1 = "x";
                    a2 = "n";
                    n = "";
                    h = "xⁿ : Élève x à la puissance n.";
                    break;

                case "cos":
                    a1 = "x";
                    h = "Calcule le cosinus de l'angle x.";
                    break;
                case "sin":
                    a1 = "x";
                    h = "Calcule le sinus de l'angle x.";
                    break;
                case "tan":
                    a1 = "x";
                    h = "Calcule la tangente de l'angle x.";
                    break;
                case "cos⁻¹":
                    a1 = "x";
                    h = "Retourne l'angle dont le cosinus est x. (-1 ≤ x ≤ 1)";
                    break;
                case "sin⁻¹":
                    a1 = "x";
                    h = "Retourne l'angle dont le sinus est x. (-1 ≤ x ≤ 1)";
                    break;
                case "tan⁻¹":
                    a1 = "x";
                    h = "Retourne l'angle dont la tangente est x.";
                    break;

                case "cosh":
                    a1 = "x";
                    h = "Calcule le cosinus hyperbolique de l'angle x.";
                    break;
                case "sinh":
                    a1 = "x";
                    h = "Calcule le sinus hyperbolique de l'angle x.";
                    break;
                case "tanh":
                    a1 = "x";
                    h = "Calcule la tangente hyperbolique de l'angle x.";
                    break;
                case "cosh⁻¹":
                    a1 = "x";
                    h = "Retourne l'angle dont le cosinus hyperbolique est x. (1 ≤ x ≤ ∞)";
                    break;
                case "sinh⁻¹":
                    a1 = "x";
                    h = "Retourne l'angle dont le sinus hyperbolique est x. (-∞ ≤ x ≤ ∞)";
                    break;
                case "tanh⁻¹":
                    a1 = "x";
                    h = "Retourne l'angle dont la tangente hyperbolique est x.";
                    break;

                case "sec":
                    a1 = "x";
                    h = "Calcule la sécante de l'angle x.";
                    break;
                case "csc":
                    a1 = "x";
                    h = "Calcule la cosécante de l'angle x.";
                    break;
                case "cot":
                    a1 = "x";
                    h = "Calcule la cotangente de l'angle x.";
                    break;
                case "sec⁻¹":
                    a1 = "x";
                    h = "Retourne l'angle dont la sécante est x.";
                    break;
                case "csc⁻¹":
                    a1 = "x";
                    h = "Retourne l'angle dont la cosécante est x.";
                    break;
                case "cot⁻¹":
                    a1 = "x";
                    h = "Retourne l'angle dont la cotangente est x.";
                    break;

                case "sech":
                    a1 = "x";
                    h = "Calcule la sécante hyperbolique de l'angle x.";
                    break;
                case "csch":
                    a1 = "x";
                    h = "Calcule la cosécante hyperbolique de l'angle x.";
                    break;
                case "coth":
                    a1 = "x";
                    h = "Calcule la cotangente hyperbolique de l'angle x.";
                    break;
                case "sech⁻¹":
                    a1 = "x";
                    h = "Retourne l'angle dont la sécante hyperbolique est x.";
                    break;
                case "csch⁻¹":
                    a1 = "x";
                    h = "Retourne l'angle dont la cosécante hyperbolique est x.";
                    break;
                case "coth⁻¹":
                    a1 = "x";
                    h = "Retourne l'angle dont la cotangente hyperbolique est x.";
                    break;

                case "∛":
                    a1 = "x";
                    h = "Calcule la racine cubique de x.";
                    break;
                case "ⁿ√":
                    n = "root";
                    a1 = "x";
                    a2 = "n";
                    h = "Calcule la racine n-ième de x.";
                    break;
                case "10ⁿ":
                    n = "";
                    a1 = "x";
                    p = false;
                    h = "10^x : Élève 10 à la puissance x.";
                    break;

                case "log":
                    a1 = "x";
                    a2 = "base";
                    h = "Calcule le logarithme de x à la base spécifiée.";
                    break;
                case "log₁₀":
                    a1 = "x";
                    h = "Calcule le logarithme de base 10 de x.";
                    break;
                case "ln":
                    a1 = "x";
                    h = "Calcule le logarithme naturel (de base e) de x.";
                    break;

                case "ℯⁿ":
                    a1 = "x";
                    n = "exp";
                    h = "Calcule l'exponentielle de x.";
                    break;
                case "ceil":
                    a1 = "x";
                    n = "ceil";
                    h = "Arrondit par excès x.";
                    break;
                case "floor":
                    a1 = "x";
                    n = "floor";
                    h = "Arrondit par défaut x.";
                    break;

                case "fibo":
                    a1 = "n";
                    h = "Retourne le n-ième nombre de la suite de Fibonacci.";
                    break;
                case "int":
                    a1 = "x";
                    h = "Retourne la partie entière de x.";
                    break;

                case "max":
                    a1 = "x";
                    a2 = "y";
                    h = "Retourne le plus grand nombre entre x et y.";
                    break;
                case "min":
                    a1 = "x";
                    a2 = "y";
                    h = "Retourne le plus petit nombre enter x et y.";
                    break;
                case "rand":
                    h = "Retourne un nombre aléatoire entre 0,0 et 1,0.";
                    break;

                case "gcd":
                    a1 = "x";
                    a2 = "y";
                    h = "Calcule le plus grand diviseur commun de x et y.";
                    break;
                case "lcm":
                    a1 = "x";
                    a2 = "y";
                    h = "Calcule le plus petit multiple commun de x et y.";
                    break;
                case "randn":
                    a1 = "x";
                    a2 = "y";
                    h = "Retourne un nombre aléatoire entre x et y exclus.";
                    break;

                default:
                    if (((Button) sender).Name == "btnInverse")
                    {
                        p = false;
                        n = "";
                        a1 = "x";
                        h = "1/x : Calcule l'inverse de x (1 / x).";
                    }
                    else if (((Button) sender).Name == "btnRnd")
                    {
                        a1 = "x";
                        a2 = "y";
                        n = "rnd";
                        h = "Arrondit x à y décimales.";
                    }
                    else if (((Button) sender).Name == "btnFloor")
                    {
                        a1 = "x";
                        n = "floor";
                        h = "Arrondit par défaut x.";
                    }
                    else if (((Button) sender).Name == "btnCeil")
                    {
                        a1 = "x";
                        n = "ceil";
                        h = "Arrondit par excès x.";
                    }
                    break;
            }

            lblHelp.Inlines.Clear();
            if (n != "")
            {
                lblHelp.Inlines.Add(new Run(n) {FontWeight = FontWeights.DemiBold});
                if (p)
                {
                    lblHelp.Inlines.Add(new Run("("));
                    if (a1 != "")
                    {
                        lblHelp.Inlines.Add(new Run(a1) {Foreground = new SolidColorBrush(Color.FromRgb(0, 102, 204))});
                    }
                    if (a2 != "")
                    {
                        lblHelp.Inlines.Add(new Run("; "));
                        lblHelp.Inlines.Add(new Run(a2)
                        {
                            Foreground = new SolidColorBrush(Color.FromRgb(224, 71, 71))
                        });
                    }
                    lblHelp.Inlines.Add(new Run(")"));
                }
                lblHelp.Inlines.Add(new Run(" : "));
            }

            if (h == "") h = "Pas d'aide disponible.";

            try
            {
                var h1 = h.Replace(a1, "☺");
                if (a2 != "") h1 = h1.Replace(a2, "☻");
                var tmp = "";
                int i = 0;
                foreach (var c in h1)
                {
                    if (c == '☺')
                    {
                        lblHelp.Inlines.Add(new Run(tmp));
                        tmp = "";
                        if (n == "exp" || n == "ceil")
                        {
                            if (i == h1.LastIndexOf(c))
                                lblHelp.Inlines.Add(new Run(a1)
                                {
                                    Foreground = new SolidColorBrush(Color.FromRgb(0, 102, 204))
                                });
                            else
                                lblHelp.Inlines.Add(new Run(a1));
                        }
                        else if (n == "fibo")
                        {
                            if (i == h1.IndexOf(c, 10))
                                lblHelp.Inlines.Add(new Run(a1)
                                {
                                    Foreground = new SolidColorBrush(Color.FromRgb(0, 102, 204))
                                });
                            else
                                lblHelp.Inlines.Add(new Run(a1));
                        }
                        else if (n == "randn")
                        {
                            if (i == h1.IndexOf(c))
                                lblHelp.Inlines.Add(new Run(a1)
                                {
                                    Foreground = new SolidColorBrush(Color.FromRgb(0, 102, 204))
                                });
                            else
                                lblHelp.Inlines.Add(new Run(a1));
                        }
                        else
                        {
                            lblHelp.Inlines.Add(new Run(a1)
                            {
                                Foreground = new SolidColorBrush(Color.FromRgb(0, 102, 204))
                            });
                        }

                        i++;
                        continue;
                    }
                    else if (c == '☻')
                    {
                        lblHelp.Inlines.Add(new Run(tmp));
                        tmp = "";
                        if (i == h1.LastIndexOf(c) || (sender as Button).Name == "btnRnd")
                            lblHelp.Inlines.Add(new Run(a2)
                            {
                                Foreground = new SolidColorBrush(Color.FromRgb(224, 71, 71))
                            });
                        else
                            lblHelp.Inlines.Add(new Run(a2));
                        i++;
                        continue;
                    }
                    else
                    {
                        tmp += c;
                    }
                    i++;
                }
                if (tmp != "") lblHelp.Inlines.Add(new Run(tmp));
            }
            catch
            {
                lblHelp.Inlines.Add(new Run(h));
            }
        }

        private void Toolhide(object sender, MouseEventArgs e)
        {
            lblHelp.Inlines.Clear();
        }

        private void btnPi_Click(object sender, RoutedEventArgs e)
        {
            txtOperation.Text += "π";
        }

        private void btnGraph3D_Click(object sender, RoutedEventArgs e)
        {
            var rsf = new Graph3DWindow();


            rsf.ShowDialog();
        }

        private void btnGraphComplex3D_Click(object sender, RoutedEventArgs e)
        {
            var rsf = new GraphComplex3DWindow();


            rsf.ShowDialog();
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
        }

        private string osi = "";

        private void txtOperation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (osi == (txtOperation.SelectedItem?.ToString() ?? "")) return;
            if (osi != txtOperation.Text) return;
            if (txtOperation.IsDropDownOpen) return;

            if (txtOperation.Text.Contains('('))
            {
                txtOperation.Text = txtOperation.Text.Substring(0, txtOperation.Text.IndexOf('('));
            }
        }

        private void txtOperation_DropDownClosed(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (txtOperation.SelectedItem == null) return;
            osi = txtOperation.SelectedItem.ToString();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            new Help().ShowDialog();
        }

        private void chkDegrees_Checked(object sender, RoutedEventArgs e)
        {
            EquationParser.UseDegrees = (bool) chkDegrees.IsChecked;
            Settings.Default.UseDegrees = (bool) chkDegrees.IsChecked;
            Settings.Default.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            chkDegrees.IsChecked = Settings.Default.UseDegrees;
        }

        private void btnPolarGraph_Click(object sender, RoutedEventArgs e)
        {
            new PolarGraphWindow().ShowDialog();
        }

        private void btnParamGraph_Click(object sender, RoutedEventArgs e)
        {
            new ParamGraphWindow().ShowDialog();
        }

        private void btnParamPolarGraph_Click(object sender, RoutedEventArgs e)
        {
            new PolarParamGraphWindow().ShowDialog();
        }

        private void btnCeil_Click(object sender, RoutedEventArgs e)
        {
            if (txtOperation.Text == "") txtOperation.Text += "ceil(ans)";
            else txtOperation.Text += "ceil(";
        }

        private void btnFloor_Click(object sender, RoutedEventArgs e)
        {
            if (txtOperation.Text == "") txtOperation.Text += "floor(ans)";
            else txtOperation.Text += "floor(";
        }

        private void btnRnd_Click(object sender, RoutedEventArgs e)
        {
            if (txtOperation.Text == "") txtOperation.Text += "rnd(ans)";
            else txtOperation.Text += "rnd(";
        }

        private void btnPointsGraph_Click(object sender, RoutedEventArgs e)
        {
            new PointsGraphWindow().ShowDialog();
        }

        private void btnCalcHyp_Click(object sender, RoutedEventArgs e)
        {
            new CalcPyth().ShowDialog();
        }
    }
}