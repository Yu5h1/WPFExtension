using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Yu5h1Tools.WPFExtension
{
    public static class ShapeEx
    {
        public static Line CreateLine(Point p1, Point p2,double Thickness = 3, SolidColorBrush color = default(SolidColorBrush))
        {
            var l = new Line();
            if (color == default(SolidColorBrush)) color = Brushes.White;
            l.Stroke = color; 
            l.HorizontalAlignment = HorizontalAlignment.Left;
            l.VerticalAlignment = VerticalAlignment.Top;
            l.X1 = p1.X; l.Y1 = p1.Y;
            l.X2 = p2.X; l.Y2 = p2.Y;
            l.StrokeThickness = Thickness;
            return l;
        }
        public static Line[] CreateCrossLine(Grid grid,double Thickness = 3, SolidColorBrush color = default(SolidColorBrush))
        {
            Point center = new Point(grid.ActualWidth/2, grid.ActualHeight/2);
            Line line1 = CreateLine(new Point(center.X, 0), new Point(center.X, grid.ActualHeight));
            Line line2 = CreateLine(new Point(0, center.Y), new Point(grid.ActualWidth, center.Y));
            grid.Children.Add(line1);
            grid.Children.Add(line2);
            return new Line[] {
                line1,
                line2
            };
        }
   }
}
