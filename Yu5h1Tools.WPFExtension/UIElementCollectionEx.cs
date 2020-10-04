using System.Windows;
using System.Windows.Controls;

namespace Yu5h1Tools.WPFExtension
{
    public static class UIElementCollectionEx
    {
        public static void Add(this UIElementCollection col,params UIElement[] elements) {
            foreach (UIElement ele in elements) col.Add(ele);
        }
        public static void Switch(this UIElementCollection col, int from ,int to)
        {
            var smallestIndex = from;
            var biggestIndex = to;
            if (from > to) {
                smallestIndex = to;
                biggestIndex = from;
            }
            var ObjA = col[smallestIndex];
            var ObjB = col[biggestIndex];
            col.RemoveAt(biggestIndex);
            col.RemoveAt(smallestIndex);
            col.Insert(smallestIndex, ObjB);
            col.Insert(biggestIndex, ObjA);            
        }
    }
}
