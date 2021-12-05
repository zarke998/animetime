using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnimeTime.WPF.Views.Controls
{
    /// <summary>
    /// Interaction logic for ImageButton.xaml
    /// </summary>
    public partial class ImageButton : Button
    {
        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public int RotateDegrees
        {
            get { return (int)GetValue(RotateDegreesProperty); }
            set { SetValue(RotateDegreesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RotateDegrees.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RotateDegreesProperty =
            DependencyProperty.Register("RotateDegrees", typeof(int), typeof(ImageButton), new PropertyMetadata(0));
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(string), typeof(ImageButton), new PropertyMetadata(String.Empty));

        public ImageButton()
        {
            InitializeComponent();
        }
    }
}
