using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Yu5h1Tools.WPFExtension
{
    /// <summary>
    /// Interaction logic for VisualPoint.xaml
    /// </summary>
    public partial class VisualPoint : UserControl
    {
        public enum Style {
            Default,
            CrossLine,
            Square
        }
        Grid grid { get { return (Grid)Content; } }

        public VisualPoint()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            ShapeEx.CreateCrossLine(grid);
        }
    }
}
