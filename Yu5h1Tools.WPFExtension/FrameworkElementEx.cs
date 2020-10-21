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
            MenuItem result = null;
            switch (target)
            {
                case MenuItem menuItem:
                    result = new MenuItem() { Header = itemName };
                    menuItem.Items.Add(result );
                    return result ;
                case ContextMenu contextMenu:
                    result = new MenuItem() { Header = itemName };
                    contextMenu.Items.Add(result);
                    return result ;
            }
            if (target.ContextMenu == null) target.ContextMenu = new ContextMenu();
            return target.ContextMenu.AddMenuItem(itemName, inputGestureText);
        }
        public static void ShowToolTip(this FrameworkElement target)
        {
            if (target.ToolTip.GetType() == typeof(ToolTip)) {
                var ttp = target.ToolTip as ToolTip;
                ttp.Visibility = Visibility.Visible;
                ttp.IsOpen = true;
            }
                
        }
    }
}
