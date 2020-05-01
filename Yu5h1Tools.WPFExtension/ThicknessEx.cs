using System.Windows;

namespace Yu5h1Tools.WPFExtension
{
    public static class ThicknessEx
    {
        public static Point Location(this Thickness t,double x = 0, double y = 0)
        { return new Point(t.Left+x, t.Top+y); }
        public static Point Location(this Thickness t, Point p = default(Point))
        { return t.Location(p.X,p.Y); }
        public static Thickness LocateAt(this Thickness t, double x ,double y)
        { return new Thickness(x,y,t.Right,t.Bottom); }
        public static Thickness LocateAt(this Thickness t, Point p)
        { return t.LocateAt(p.X,p.Y); }
    }
}
