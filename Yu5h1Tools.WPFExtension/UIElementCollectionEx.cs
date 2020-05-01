using System.Windows;
using System.Windows.Controls;

namespace Yu5h1Tools.WPFExtension
{
    public static class UIElementCollectionEx
    {
        public static void Add(this UIElementCollection col,params UIElement[] elements) {
            foreach (UIElement ele in elements) col.Add(ele);
        }
    }
}
