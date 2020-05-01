using System.Windows;

namespace Yu5h1Tools.WPFExtension
{
    public static class SizeEx
    {
        public static string ToString(this Size size,string format) {
            return " Width : " + size.Width.ToString(format) + " Height : " + size.Height.ToString(format);
        }
    }
}
