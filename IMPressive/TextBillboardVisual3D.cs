using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace HelixToolkit.Wpf
{
    /// <summary>
    /// A visual element that contains a text billboard.
    /// </summary>
    public class TextBillboardVisual3D : BillboardVisual3D
    {
        #region Constants and Fields

        /// <summary>
        /// The font family property.
        /// </summary>
        public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register(
            "FontFamily", typeof (FontFamily), typeof (TextBillboardVisual3D), new UIPropertyMetadata(null, TextChanged));

        /// <summary>
        /// The font size property.
        /// </summary>
        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
            "FontSize", typeof (double), typeof (TextBillboardVisual3D), new UIPropertyMetadata(0.0, TextChanged));

        /// <summary>
        /// The font weight property.
        /// </summary>
        public static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register(
            "FontWeight", typeof (FontWeight), typeof (TextBillboardVisual3D),
            new UIPropertyMetadata(FontWeights.Normal, TextChanged));

        /// <summary>
        /// The foreground property.
        /// </summary>
        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
            "Foreground", typeof (Brush), typeof (TextBillboardVisual3D), new UIPropertyMetadata(null, TextChanged));

        /// <summary>
        /// The text property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof (string), typeof (TextBillboardVisual3D), new UIPropertyMetadata(null, TextChanged));

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the font family.
        /// </summary>
        /// <value>The font family.</value>
        public FontFamily FontFamily
        {
            get { return (FontFamily) GetValue(FontFamilyProperty); }

            set { SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        ///   Gets or sets the size of the font.
        /// </summary>
        /// <value>The size of the font.</value>
        public double FontSize
        {
            get { return (double) GetValue(FontSizeProperty); }

            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        ///   Gets or sets the font weight.
        /// </summary>
        /// <value>The font weight.</value>
        public FontWeight FontWeight
        {
            get { return (FontWeight) GetValue(FontWeightProperty); }

            set { SetValue(FontWeightProperty, value); }
        }

        /// <summary>
        ///   Gets or sets the foreground brush.
        /// </summary>
        /// <value>The foreground.</value>
        public Brush Foreground
        {
            get { return (Brush) GetValue(ForegroundProperty); }

            set { SetValue(ForegroundProperty, value); }
        }

        /// <summary>
        ///   Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return (string) GetValue(TextProperty); }

            set { SetValue(TextProperty, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The text changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TextBillboardVisual3D) d).OnTextChanged();
        }

        /// <summary>
        /// The on text changed.
        /// </summary>
        private void OnTextChanged()
        {
            var tb = new TextBlock(new Run(Text));
            if (Foreground != null)
            {
                tb.Foreground = Foreground;
            }

            if (FontFamily != null)
            {
                tb.FontFamily = FontFamily;
            }

            tb.FontWeight = FontWeight;

            if (FontSize > 0)
            {
                tb.FontSize = FontSize;
            }

            Material = new DiffuseMaterial(new VisualBrush(tb));

            tb.Measure(new Size(1000, 1000));
            Width = tb.DesiredSize.Width;
            Height = tb.DesiredSize.Height;
        }

        #endregion
    }
}