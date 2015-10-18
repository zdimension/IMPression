using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IMPression;
using IMPression.Parser;

namespace IMPressive.Outils
{
    /// <summary>
    /// Logique d'interaction pour CalcPyth.xaml
    /// </summary>
    public partial class CalcPyth : Window
    {
        public CalcPyth()
        {
            InitializeComponent();
            parser = new EquationParser();
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
                st = st.OrderBy(x => x).Distinct().ToList();
                return st;
            }
        }

        private bool calculating = false;

        private int numwt => (txtA.Text != "" ? 1 : 0) + (txtB.Text != "" ? 1 : 0) + (txtHyp.Text != "" ? 1 : 0);

        private void txtA_TextChanged(object sender, RoutedEventArgs e)
        {
            if (calculating) return;
            if (numwt == 2)
            {
                var res = new List<AutoCompleteBox>() {txtA, txtB, txtHyp};
                res.First(x => x.Text == "").IsEnabled = false;
            }
        }

        private EquationParser parser;

        public void Calculate()
        {
            var tx1 = new AutoCompleteBox();
            if (txtA.Text != "" && txtA.IsEnabled) tx1 = txtA;
            else if (txtB.Text != "" && txtB.IsEnabled) tx1 = txtB;
            else if (txtHyp.Text != "" && txtHyp.IsEnabled) tx1 = txtHyp;

            var tx2 = new AutoCompleteBox();
            if (txtA.Text != "" && txtA.IsEnabled && tx1 != txtA) tx2 = txtA;
            else if (txtB.Text != "" && txtB.IsEnabled && tx1 != txtB) tx2 = txtB;
            else if (txtHyp.Text != "" && txtHyp.IsEnabled && tx1 != txtHyp) tx2 = txtHyp;

            Complex tx1c = parser.Calculate(tx1.Text);
            Complex tx2c = parser.Calculate(tx2.Text);

            Complex result = 0;

            if (tx1 == txtHyp)
            {
                result = Functions.Sqrt(tx1c.Square() - tx2c.Square());
            }
            else if (tx2 == txtHyp)
            {
                result = Functions.Sqrt(tx2c.Square() - tx1c.Square());
            }
            else
            {
                result = Functions.Sqrt(tx1c.Square() + tx2c.Square());
            }

            getinv(tx1, tx2).Text = result.ToString();
        }

        private AutoCompleteBox getinv(AutoCompleteBox a, AutoCompleteBox b)
        {
            var res = new List<AutoCompleteBox>() {txtA, txtB, txtHyp};
            res.Remove(a);
            res.Remove(b);
            return res[0];
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
        }
    }
}