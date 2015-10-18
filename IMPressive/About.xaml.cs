using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IMPression;
using IMPression.Parser;
using IMPressive.Properties;

namespace IMPressive
{
    /// <summary>
    /// Logique d'interaction pour About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            lblLst.ToolTip = new TextBlock {Text = new EquationParser().CleanUp(string.Join(",\n", Funcs))};
            lblConst.ToolTip = new TextBlock
            {
                Text =
                    "π (pi) ≈ 3.14159265" +
                    "\nℯ ≈ 2.71828183" +
                    "\nφ (phi, nombre d'or) ≈ 1.61803399"+
                    "\nγ (euler gamma) ≈ 0.57721566" +
                    "\ncatalan (constante de Catalan) ≈ 0.91596559" +
                    "\nc (vitesse de la lumière) ≈ 299792458 m/s" +
                    "\ndegree (radians dans 1 degré) ≈ 0.01745329" +
                    "\nglaisher (constante de Glaisher) ≈ 1.28242713" +
                    "\nkhinchin (constante de Khinchin) ≈ 2.68545200" +
                    "\nτ (tau, 2π) ≈ 6.28318530" +
                    "\ni = √-1"
            };

            int count = Function.FunctionsList.Count - Constant.cs.Count;
            lblNbFunc.Text = "Il y a actuellement " + count + " fonctions implémentées\net " + Constant.cs.Count +
                             " constantes définies.";
            switch (Settings.Default.Theme)
            {
                case "Windows 7":
                    cbxTheme.SelectedIndex = 0;
                    break;
                case "Windows 8":
                    cbxTheme.SelectedIndex = 1;
                    break;
                case "Système":
                    cbxTheme.SelectedIndex = 2;
                    break;
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
                st.RemoveRange(0, 6);
                st = st.OrderBy(x => x).Distinct().ToList();
                return st;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cbxTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string res;
            switch (cbxTheme.SelectedIndex)
            {
                case 1:
                    res = "Windows 8";
                    break;
                case 2:
                    res = "Système";
                    break;
                default:
                    res = "Windows 7";
                    break;
            }
            Settings.Default.Theme = res;
            Settings.Default.Save();
            App.SwitchTheme(res);
        }
    }
}