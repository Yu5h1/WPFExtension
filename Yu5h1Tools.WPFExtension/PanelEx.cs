using System.Windows.Controls;

namespace Yu5h1Tools.WPFExtension
{
    public static class PanelEx
    {
        public static void Switch(this Panel col, int from, int to) => col.Children.Switch(from, to);
    }
}
