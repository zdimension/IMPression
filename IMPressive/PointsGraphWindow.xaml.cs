using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
    public partial class PointsGraphWindow : Window
    {
        private EquationParser parser = new EquationParser();

        public PlotModel GraphModel
        {
            get { return (PlotModel) GetValue(GraphModelProperty); }
            set { SetValue(GraphModelProperty, value); }
        }

        public static readonly DependencyProperty GraphModelProperty =
            DependencyProperty.Register("GraphModel", typeof (PlotModel), typeof (PointsGraphWindow),
                new PropertyMetadata(null));

        private List<DataPoint> lbxpts = new List<DataPoint>();

        public PointsGraphWindow()
        {
            InitializeComponent();

            listBox.ItemsSource = lbxpts;
            GraphModel = new PlotModel();
            var sr = new LineSeries();
            sr.Color = OxyColors.Green;
            sr.MarkerType = MarkerType.Circle;
            sr.MarkerFill = OxyColors.SteelBlue;
            var sri = new LineSeries();
            sri.Color = OxyColors.Red;
            sri.Smooth = true;
            sri.IsVisible = false;
            GraphModel.Series.Add(sr);
            GraphModel.Series.Add(sri);
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
            txtX.ItemsSource = Funcs;
            txtY.ItemsSource = Funcs;
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

        private void Graph()
        {
            txtX.Text = parser.CleanUp(txtX.Text);
            txtY.Text = parser.CleanUp(txtY.Text);
            try
            {
                var p = new DataPoint(Fx(), Fy());
                if (!(GraphModel.Series[0] as LineSeries).Points.Any(x => x.X == p.X && x.Y == p.Y))
                {
                    (GraphModel.Series[0] as LineSeries).Points.Add(p);
                    (GraphModel.Series[1] as LineSeries).Points.Add(p);
                    lbxpts.Add(p);
                    listBox.Items.Refresh();
                }
                chartCanvas.InvalidatePlot(true);
            }
            catch (Exception e)
            {
                e.Throw();
            }
        }

        private double Fx()
        {
            return Functions.Round(parser.Calculate(parser.DeCleanUp(txtX.Text)), 15);
        }

        private double Fy()
        {
            return Functions.Round(parser.Calculate(parser.DeCleanUp(txtY.Text)), 15);
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


        private void cbxInterpolate_Checked(object sender, RoutedEventArgs e)
        {
            (GraphModel.Series[1] as LineSeries).IsVisible = true;
            chartCanvas.InvalidatePlot();
        }

        private void cbxInterpolate_Unchecked(object sender, RoutedEventArgs e)
        {
            (GraphModel.Series[1] as LineSeries).IsVisible = false;
            chartCanvas.InvalidatePlot();
        }

        private void btnDelPt_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var p = button.DataContext as DataPoint?;

                if (p != null)
                {
                    (GraphModel.Series[0] as LineSeries).Points.Remove((DataPoint) p);
                    (GraphModel.Series[1] as LineSeries).Points.Remove((DataPoint) p);
                    chartCanvas.InvalidatePlot();
                    lbxpts.Remove((DataPoint) p);
                }
                listBox.Items.Refresh();
            }
            else
            {
            }
        }

        private void cbxNormal_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                (GraphModel.Series[0] as LineSeries).IsVisible = true;
                chartCanvas.InvalidatePlot();
            }
            catch
            {
            }
        }

        private void cbxNormal_Unchecked(object sender, RoutedEventArgs e)
        {
            (GraphModel.Series[0] as LineSeries).IsVisible = false;
            chartCanvas.InvalidatePlot();
        }
    }
}