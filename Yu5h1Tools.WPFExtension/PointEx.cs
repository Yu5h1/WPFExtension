using static System.Math;
using System.Windows;

namespace Yu5h1Tools.WPFExtension
{
    public static class PointEx
    {
        /// <returns>Point[Left,Top]</returns>        
        public static double Distance(this Point p1,Point p2) {
            return Sqrt(Pow(p2.X-p1.X,2)+ Pow(p2.Y-p1.Y, 2));
        }
        public static string ToString(this Point p, string format)
        {
            return " X : " +p.X.ToString(format) + " Y : " + p.Y.ToString(format);
        }
    }
}
