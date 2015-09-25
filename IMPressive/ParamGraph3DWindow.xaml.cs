using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using IMPression;

namespace IMPressive
{
    /// <summary>
    /// Logique d'interaction pour GraphWindow.xaml
    /// </summary>
    public partial class ParamGraph3DWindow : Window
    {
        private EquationParser parser = new EquationParser();

        public ParamGraph3DWindow()
        {
            InitializeComponent();
            txtX.ItemsSource = Funcs;
            txtY.ItemsSource = Funcs;
            txtZ.ItemsSource = Funcs;
            txtFromT.ItemsSource = Funcs;
            txtToT.ItemsSource = Funcs;
            txtPasT.ItemsSource = Funcs;
        }


        private void txtOperation_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Graph();
            }
        }

        public List<Point3D> Data { get; set; }
        public double[,] ColorValues { get; set; }

        public Model3DGroup Lights
        {
            get
            {
                var group = new Model3DGroup();
                group.Children.Add(new AmbientLight(Colors.White));
                return group;
            }
        }

        public Brush SurfaceBrush
            =>
                BrushHelper.CreateGradientBrush(Colors.Red, Colors.DarkRed, Colors.LightPink, Colors.OrangeRed,
                    Colors.Orange, Colors.Yellow, Colors.Green, Colors.LightSeaGreen, Colors.Blue);

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

        private int Rows = 100, Columns = 100;
        private Complex MinT, MaxT, PasT, MinX, MaxX, PasX, MinY, MaxY, PasY;

        private void Graph()
        {
            /*txtX.Text = parser.CleanUp(txtX.Text);
            txtY.Text = parser.CleanUp(txtY.Text);
            txtZ.Text = parser.CleanUp(txtZ.Text);
            txtFromT.Text = parser.CleanUp(txtFromT.Text);
            txtToT.Text = parser.CleanUp(txtToT.Text);
            txtPasT.Text = parser.CleanUp(txtPasT.Text);


            MinT = parser.Calculate(txtFromT.Text);
            MaxT = parser.Calculate(txtToT.Text);
            PasT = parser.Calculate(txtPasT.Text);


            Data = CreateDataArray();

            PasX = Data.Select(x => x.X).Zip(Data.Skip(1), (x, y) => y.X - x).Average();
            PasY = Data.Select(x => x.Y).Zip(Data.Skip(1), (x, y) => y.Y - x).Average();
            MaxY = Data.Max(x => x.Y);
            MinY = Data.Min(x => x.Y);
            MaxX = Data.Max(x => x.X);
            MinX = Data.Min(x => x.X);

            Rows = (int) ((MaxY - MinY) / PasY / 2);
            Columns = (int)((MaxX - MinX) / PasX / 2);

            /*Rows = (int)((Data.Max(x => x.Y) - Data.Min(x => x.Y)) / PasY / 2);
            Columns = (int)((MaxX - MinX) / PasX / 2);* /

            //ColorValues = FindGradientY(Data);
            RaisePropertyChanged("Data");
            RaisePropertyChanged("ColorValues");
            RaisePropertyChanged("SurfaceBrush");

            
            model3D.ColorValues = ColorValues;*/
        }

        public Point GetPointFromIndex(int i, int j)
        {
            double x = MinX + (double)j / (Columns - 1) * (MaxX - MinX);
            double y = MinY + (double)i / (Rows - 1) * (MaxY - MinY);
            return new Point(x, y);
        }

        /*public List<Point3D> CreateDataArray()
        {
            var data = new List<Point3D>();
            for(Complex t = MinT; t <= MaxT; t += PasT)
            {
                double? _x = Fx(t);
                double? _y = Fy(t);
                double? _z = Fz(t);
                if (_x == null || _y == null || _z == null) continue;
                data.Add(new Point3D((double)_x, (double)_y, (double)_z));
            }
            return data;
        }

        public Point3D[,] CreateDataArray()
        {
            var data = new Point3D[Rows, Columns];
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                {
                    var pt = GetPointFromIndex(i, j);
                    double? _x = Fx(t);
                    double? _y = Fy(t);
                    double? _z = Fz(t);
                    if (_x == null || _y == null || _z == null) continue;
                    if (res != null)
                        data[i, j] = new Point3D(pt.X, pt.Y, (double)res);
                }
            return data;
        }*/

        public double[,] FindGradientY(Point3D[,] data)
        {
            int n = data.GetUpperBound(0) + 1;
            int m = data.GetUpperBound(0) + 1;
            var K = new double[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    var p10 = data[i + 1 < n ? i + 1 : i, j - 1 > 0 ? j - 1 : j];
                    var p00 = data[i - 1 > 0 ? i - 1 : i, j - 1 > 0 ? j - 1 : j];
                    var p11 = data[i + 1 < n ? i + 1 : i, j + 1 < m ? j + 1 : j];
                    var p01 = data[i - 1 > 0 ? i - 1 : i, j + 1 < m ? j + 1 : j];
                    double dy = p10.Y - p00.Y;
                    double dz = p10.Z - p00.Z;

                    K[i, j] = dz / dy;
                }
            return K;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(property));
        }


        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            Graph();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Title = "";
            center();
            Data = null;
            ColorValues = null;
            model3D.Points = null;
            model3D.ColorValues = null;
            model3D.UpdateModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            center();
        }

        private void center()
        {
            camera.Position = new Point3D(5, 30, 40);
            camera.LookDirection = new Vector3D(-5, -30, -40);
            camera.UpDirection = new Vector3D(0, 0, 1);
            camera.FieldOfView = 45;
        }

        private double? Fx(Complex t)
        {
            try
            {
                Complex ret = Complex.Zero;

                ret = parser.Calculate(txtX.Text, new Var("t", t));

                return ret.IsReal() ? ret.Real : (double?) null;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private double? Fy(Complex t)
        {
            try
            {
                Complex ret = Complex.Zero;

                ret = parser.Calculate(txtY.Text, new Var("t", t));

                return ret.IsReal() ? ret.Real : (double?)null;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private double? Fz(Complex t)
        {
            try
            {
                Complex ret = Complex.Zero;

                ret = parser.Calculate(txtZ.Text, new Var("t", t));

                return ret.IsReal() ? ret.Real : (double?)null;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}