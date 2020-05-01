using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yu5h1Tools.Animation.WPF
{
    public static class MathE
    {
        public static double Clamp(double value, double min,double max) {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
        public static double Lerp(double from,double to,double n) {
            return from + (to - from) * n;
        }
        public static Point Lerp(Point from, Point to, double n)
        {
            return new Point(Lerp(from.X,to.X,n), Lerp(from.Y, to.Y, n));
        }
        public static Point Lerp(Size from, Size to, double n)
        {
            return new Point(Lerp(from.Width, to.Width, n), Lerp(from.Height, to.Height, n));
        }
        public static Rect Lerp(Rect from, Rect to, double n)
        {            
            return new Rect(Lerp(from.Location, to.Location,n),Lerp(from.Size, to.Size, n));
        }
    }
}
