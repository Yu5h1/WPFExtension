using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace Yu5h1Tools.WPFExtension
{
    public static class FrameworkElementEx
    {
        public static void SetPosition(this FrameworkElement target, double x , double y)
        { target.Margin = target.Margin.LocateAt(x,y); }
        public static void SetPosition(this FrameworkElement target, Point p)
        { target.Margin = target.Margin.LocateAt(p); }
        public static Rect GetBounds(this FrameworkElement target, Visual refrom = null)
        {
            if (refrom == null) refrom = target.Parent as Visual;
            return target.TransformToVisual(refrom).TransformBounds(LayoutInformation.GetLayoutSlot(target));
        }
        public static MenuItem AddMenuItem(this FrameworkElement target, string itemName,string inputGestureText = "")
        {
            switch (target)
            {
                case MenuItem menuItem:
                    var curResult = new MenuItem() { Header = itemName };
                    menuItem.Items.Add(curResult);
                    return curResult;
            }
            if (target.ContextMenu == null) target.ContextMenu = new ContextMenu();
            return target.ContextMenu.AddMenuItem(itemName, inputGestureText);
        }
    }
}
