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
    public partial class PolarParamGraphWindow : Window
    {
        private EquationParser parser = new EquationParser();

        public PlotModel GraphModel
        {
            get { return (PlotModel) GetValue(GraphModelProperty); }
            set { SetValue(GraphModelProperty, value); }
        }

        public static readonly DependencyProperty GraphModelProperty =
            DependencyProperty.Register("GraphModel", typeof (PlotModel), typeof (PolarParamGraphWindow),
                new PropertyMetadata(null));


        public PolarParamGraphWindow()
        {
            InitializeComponent();
            GraphModel = new PlotModel();
            chartCanvas.InvalidatePlot();
            GraphModel.Axes.Add(new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
                MinorTickSize = 0,
                Unit = "rad",
                Title = "θ"
            });
            GraphModel.Axes.Add(new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
                MinorTickSize = 0,
                Position = AxisPosition.Bottom,
                Title = "ρ"
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
                GraphModel.Series.Add(new FunctionSeries(x => Fx(x) * Functions.Cos(x),
                    y => Fy(y) * Functions.Sin(y), parser.Calculate(txtFromT.Text), parser.Calculate(txtToT.Text),
                    parser.Calculate(txtPasT.Text), txtOpX.Text + " ; " + txtOpY.Text));
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
    }
}