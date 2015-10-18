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
using IMPression.Parser;

namespace IMPressive
{
    /// <summary>
    /// Logique d'interaction pour GraphWindow.xaml
    /// </summary>
    public partial class GraphComplex3DWindow : Window
    {
        private EquationParser parser = new EquationParser();

        public GraphComplex3DWindow()
        {
            InitializeComponent();
            txtOperation.ItemsSource = Funcs;

            //Graph();
        }


        private void txtOperation_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Graph();
            }
        }

        public Point3D[,] Data { get; set; }
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
                var st = new List<string> {"x"};
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
        private Complex MinX, MaxX, MinY, MaxY, PasX, PasY;

        private void Graph()
        {
            txtOperation.Text = parser.CleanUp(txtOperation.Text);

            MinX = parser.Calculate(txtFromX.Text);
            MaxX = parser.Calculate(txtToX.Text);
            MinY = parser.Calculate(txtFromY.Text);
            MaxY = parser.Calculate(txtToY.Text);
            PasX = parser.Calculate(txtPasX.Text);
            PasY = parser.Calculate(txtPasY.Text);

            Rows = (int) ((MaxY - MinY) / PasY / 2);
            Columns = (int) ((MaxX - MinX) / PasX / 2);

            Data = CreateDataArray(F);
            ColorValues = FindGradientY(Data);
            RaisePropertyChanged("Data");
            RaisePropertyChanged("ColorValues");
            RaisePropertyChanged("SurfaceBrush");

            model3D.Points = Data;
            model3D.ColorValues = ColorValues;
        }

        public Point GetPointFromIndex(int i, int j)
        {
            double x = MinX + (double) j / (Columns - 1) * (MaxX - MinX);
            double y = MinY + (double) i / (Rows - 1) * (MaxY - MinY);
            return new Point(x, y);
        }

        public Point3D[,] CreateDataArray(Func<double, double, double?> f)
        {
            var data = new Point3D[Rows, Columns];
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                {
                    var pt = GetPointFromIndex(i, j);
                    double? res = f(pt.X, pt.Y);
                    if (res != null)
                        data[i, j] = new Point3D(pt.X, pt.Y, (double) res);
                }
            return data;
        }

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

        private double? F(double x, double y)
        {
            try
            {
                Complex ret = Complex.Indeterminate;
                ret = parser.Calculate(txtOperation.Text, new Var("x", new Complex(x, y)));

                return ret.Module;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public class SurfaceComplexPlotVisual3D : ModelVisual3D
    {
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof (Point3D[,]), typeof (SurfaceComplexPlotVisual3D),
                new UIPropertyMetadata(null, ModelChanged));

        public static readonly DependencyProperty ColorValuesProperty =
            DependencyProperty.Register("ColorValues", typeof (double[,]), typeof (SurfaceComplexPlotVisual3D),
                new UIPropertyMetadata(null, ModelChanged));

        public static readonly DependencyProperty SurfaceBrushProperty =
            DependencyProperty.Register("SurfaceBrush", typeof (Brush), typeof (SurfaceComplexPlotVisual3D),
                new UIPropertyMetadata(null, ModelChanged));

        private readonly ModelVisual3D visualChild;

        public SurfaceComplexPlotVisual3D()
        {
            IntervalX = 1;
            IntervalY = 1;
            IntervalZ = 0.5;
            FontSize = 0.06;
            LineThickness = 0.025;

            visualChild = new ModelVisual3D();
            Children.Add(visualChild);
        }

        /// <summary>
        /// Gets or sets the points defining the surface.
        /// </summary>
        public Point3D[,] Points
        {
            get { return (Point3D[,]) GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color values corresponding to the Points array.
        /// The color values are used as Texture coordinates for the surface.
        /// Remember to set the SurfaceBrush, e.g. by using the BrushHelper.CreateGradientBrush method.
        /// If this property is not set, the z-value of the Points will be used as color value.
        /// </summary>
        public double[,] ColorValues
        {
            get { return (double[,]) GetValue(ColorValuesProperty); }
            set { SetValue(ColorValuesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush used for the surface.
        /// </summary>
        public Brush SurfaceBrush
        {
            get { return (Brush) GetValue(SurfaceBrushProperty); }
            set { SetValue(SurfaceBrushProperty, value); }
        }


        // todo: make Dependency properties
        public double IntervalX { get; set; }
        public double IntervalY { get; set; }
        public double IntervalZ { get; set; }
        public double FontSize { get; set; }
        public double LineThickness { get; set; }

        private static void ModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SurfaceComplexPlotVisual3D) d).UpdateModel();
        }

        public void UpdateModel()
        {
            //var mod = CreateModel();
            /*if (mod != null)*/
            visualChild.Content = CreateModel();
        }

        private Model3D CreateModel()
        {
            if (Points == null || Points.Length == 0) return null;


            var plotModel = new Model3DGroup();

            int rows = Points.GetUpperBound(0) + 1;
            int columns = Points.GetUpperBound(1) + 1;
            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;
            double minZ = double.MaxValue;
            double maxZ = double.MinValue;
            double minColorValue = double.MaxValue;
            double maxColorValue = double.MinValue;
            minX = Points.OfType<Point3D>().Min(x => x.X);
            maxX = Points.OfType<Point3D>().Max(x => x.X);
            minY = Points.OfType<Point3D>().Min(x => x.Y);
            maxY = Points.OfType<Point3D>().Max(x => x.Y);
            minZ = Points.OfType<Point3D>().Min(x => x.Z);
            maxZ = Points.OfType<Point3D>().Max(x => x.Z);
            if (ColorValues != null) minColorValue = ColorValues.OfType<double>().Min(x => x);
            if (ColorValues != null) maxColorValue = ColorValues.OfType<double>().Max(x => x);

            if (Functions.Abs(minColorValue) < Functions.Abs(maxColorValue))
                minColorValue = -maxColorValue;
            else
                maxColorValue = -minColorValue;

            BitmapPixelMaker bm_maker =
                new BitmapPixelMaker(columns, rows);

            for (int ix = 0; ix < columns; ix++)
            {
                for (int iy = 0; iy < rows; iy++)
                {
                    try
                    {
                        byte red, green, blue;
                        MapRainbowColor(Points[ix, iy].Z, minZ, maxZ,
                            out red, out green, out blue);
                        bm_maker.SetPixel(ix, iy, red, green, blue, 255);
                    }
                    catch
                    {

                    }
                }
            }

            var texcoords = new Point[rows, columns];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    texcoords[i, j] = new Point(i, j);
                }

            var surfaceMeshBuilder = new MeshBuilder();
            surfaceMeshBuilder.AddRectangularMesh(Points, texcoords);
            var surfaceModel = new GeometryModel3D(surfaceMeshBuilder.ToMesh(),
                MaterialHelper.CreateMaterial(new ImageBrush(bm_maker.MakeBitmap(128, 128)), null, null, 1, 0));
            surfaceModel.BackMaterial = surfaceModel.Material;

            var axesMeshBuilder = new MeshBuilder();
            for (double x = minX; x <= maxX; x += IntervalX)
            {
                double j = (x - minX) / (maxX - minX) * (columns - 1);
                var path = new List<Point3D> {new Point3D(x, minY, minZ)};
                for (int i = 0; i < rows; i++)
                {
                    path.Add(BilinearInterpolation(Points, i, j));
                }
                path.Add(new Point3D(x, maxY, minZ));

                axesMeshBuilder.AddTube(path, LineThickness, 9, false);
                GeometryModel3D label = TextCreator.CreateTextLabelModel3D(x.ToString(), Brushes.Black, true,
                    FontSize * 3,
                    new Point3D(x, minY - FontSize * 2.5, minZ),
                    new Vector3D(1, 0, 0), new Vector3D(0, 1, 0));
                plotModel.Children.Add(label);
            }
            {
                GeometryModel3D label = TextCreator.CreateTextLabelModel3D("Axe réel", Brushes.Black, true,
                    FontSize * 10,
                    new Point3D((minX + maxX) * 0.25,
                        minY - FontSize * 6 - 0.5, minZ),
                    new Vector3D(1, 0, 0), new Vector3D(0, 1, 0));
                plotModel.Children.Add(label);
            }
            for (double y = minY; y <= maxY; y += IntervalY)
            {
                double i = (y - minY) / (maxY - minY) * (rows - 1);
                var path = new List<Point3D> {new Point3D(minX, y, minZ)};
                for (int j = 0; j < columns; j++)
                {
                    path.Add(BilinearInterpolation(Points, i, j));
                }
                path.Add(new Point3D(maxX, y, minZ));

                axesMeshBuilder.AddTube(path, LineThickness, 9, false);
                GeometryModel3D label = TextCreator.CreateTextLabelModel3D(y.ToString(), Brushes.Black, true,
                    FontSize * 3,
                    new Point3D(minX - FontSize * 3, y, minZ),
                    new Vector3D(0, 1, 0), new Vector3D(1, 0, 0));
                plotModel.Children.Add(label);
            }
            {
                GeometryModel3D label = TextCreator.CreateTextLabelModel3D("Axe imaginaire", Brushes.Black, true,
                    FontSize * 10,
                    new Point3D(minX - FontSize * 10,
                        (minY + maxY) * 0.5, minZ),
                    new Vector3D(0, 1, 0), new Vector3D(1, 0, 0));
                plotModel.Children.Add(label);
            }
            double z0 = (int) (minZ / IntervalZ) * IntervalZ;
            for (double z = z0; z <= maxZ + double.Epsilon; z += IntervalZ)
            {
                GeometryModel3D label = TextCreator.CreateTextLabelModel3D(z.ToString(), Brushes.Black, true,
                    FontSize * 3,
                    new Point3D(minX - FontSize * 3, maxY, z),
                    new Vector3D(1, 0, 0), new Vector3D(0, 0, 1));
                plotModel.Children.Add(label);
            }
            {
                GeometryModel3D label = TextCreator.CreateTextLabelModel3D("Module", Brushes.Black, true, FontSize * 10,
                    new Point3D(minX - FontSize * 10 - 1.5, maxY,
                        (minZ + maxZ) * 0.5),
                    new Vector3D(1, 0, 0), new Vector3D(0, 0, 1));
                plotModel.Children.Add(label);
            }
            var bb = new Rect3D(minX, minY, minZ, maxX - minX, maxY - minY, (maxZ - minZ));
            axesMeshBuilder.AddBoundingBox(bb, LineThickness);

            var axesModel = new GeometryModel3D(axesMeshBuilder.ToMesh(), Materials.Black);

            plotModel.Children.Add(surfaceModel);
            plotModel.Children.Add(axesModel);

            return plotModel;
        }

        private void MapRainbowColor(double value,
            double min_value, double max_value,
            out byte red, out byte green, out byte blue)
        {
            // Convert into a value between 0 and 1023.
            int int_value = (int) (1023 * (value - min_value) /
                                   (max_value - min_value));
            int_value = 1023 - int_value;

            // Map different color bands.
            if (int_value < 256)
            {
                // Red to yellow. (255, 0, 0) to (255, 255, 0).
                red = 255;
                green = (byte) int_value;
                blue = 0;
            }
            else if (int_value < 512)
            {
                // Yellow to green. (255, 255, 0) to (0, 255, 0).
                int_value -= 256;
                red = (byte) (255 - int_value);
                green = 255;
                blue = 0;
            }
            else if (int_value < 768)
            {
                // Green to aqua. (0, 255, 0) to (0, 255, 255).
                int_value -= 512;
                red = 0;
                green = 255;
                blue = (byte) int_value;
            }
            else
            {
                // Aqua to blue. (0, 255, 255) to (0, 0, 255).
                int_value -= 768;
                red = 0;
                green = (byte) (255 - int_value);
                blue = 255;
            }
        }

        private static Point3D BilinearInterpolation(Point3D[,] p, double i, double j)
        {
            int n = p.GetUpperBound(0);
            int m = p.GetUpperBound(1);
            var i0 = (int) i;
            var j0 = (int) j;
            if (i0 + 1 >= n) i0 = n - 2;
            if (j0 + 1 >= m) j0 = m - 2;

            if (i < 0) i = 0;
            if (j < 0) j = 0;
            double u = i - i0;
            double v = j - j0;
            Vector3D v00 = p[i0, j0].ToVector3D();
            Vector3D v01 = p[i0, j0 + 1].ToVector3D();
            Vector3D v10 = p[i0 + 1, j0].ToVector3D();
            Vector3D v11 = p[i0 + 1, j0 + 1].ToVector3D();
            Vector3D v0 = v00 * (1 - u) + v10 * u;
            Vector3D v1 = v01 * (1 - u) + v11 * u;
            return (v0 * (1 - v) + v1 * v).ToPoint3D();
        }
    }
}