using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using IMPression;
using IMPression.Parser;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace IMPressive
{
    /// <summary>
    /// Logique d'interaction pour GraphWindow.xaml
    /// </summary>
    public partial class ParamGraphWindow : Window
    {
        private EquationParser parser = new EquationParser();

        public PlotModel GraphModel
        {
            get { return (PlotModel) GetValue(GraphModelProperty); }
            set { SetValue(GraphModelProperty, value); }
        }

        public static readonly DependencyProperty GraphModelProperty =
            DependencyProperty.Register("GraphModel", typeof (PlotModel), typeof (ParamGraphWindow),
                new PropertyMetadata(null));


        public ParamGraphWindow()
        {
            InitializeComponent();
            GraphModel = new PlotModel();
            chartCanvas.InvalidatePlot();
            GraphModel.Axes.Add(new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
                MinorTickSize = 0,
                Title = "y"
            });
            GraphModel.Axes.Add(new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
                MinorTickSize = 0,
                Position = AxisPosition.Bottom,
                Title = "x"
            });
            txtOpX.ItemsSource = Funcs;
            txtOpY.ItemsSource = Funcs;
        }

        private void txtOperation_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Graph();
            }
        }

        public List<string> Funcs
        {
            get
            {
                var st = new List<string> {"t"};
                Function.FunctionsList.All(x =>
                {
                    st.Add(x.Contains('(') ? x.Substring(0, x.IndexOf('(')) : x);
                    return true;
                });
                st = st.OrderBy(x => x).Distinct().ToList();
                return st;
            }
        }

        private void Graph()
        {
            txtOpX.Text = parser.CleanUp(txtOpX.Text);
            txtOpY.Text = parser.CleanUp(txtOpY.Text);
            try
            {
                GraphModel.Series.Add(new FunctionSeries(Fx, Fy, parser.Calculate(txtFromT.Text),
                    parser.Calculate(txtToT.Text), parser.Calculate(txtPasT.Text), txtOpX.Text + " ; " + txtOpY.Text));
                chartCanvas.InvalidatePlot();
            }
            catch (Exception e)
            {
                e.Throw();
            }
        }

        private double Fx(double x)
        {
            return parser.Calculate(parser.DeCleanUp(txtOpX.Text), new Var("t", x));
        }

        private double Fy(double x)
        {
            return parser.Calculate(parser.DeCleanUp(txtOpY.Text), new Var("t", x));
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            Graph();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            GraphModel.Series.Clear();
            chartCanvas.InvalidatePlot();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        /*private void txtOperation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (osi == (txtOperation.SelectedItem?.ToString() ?? "")) return;
            if (osi != txtOperation.Text) return;
            if (txtOperation.IsDropDownOpen) return;

            if (txtOperation.Text.Contains('('))
            {
                txtOperation.Text = txtOperation.Text.Substring(0, txtOperation.Text.IndexOf('('));
            }
        }

        private string osi = "";
        private void txtOperation_DropDownClosed(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (txtOperation.SelectedItem == null) return;
            osi = txtOperation.SelectedItem.ToString();
        }*/
    }
}