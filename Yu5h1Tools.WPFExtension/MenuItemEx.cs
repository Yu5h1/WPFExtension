using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Yu5h1Tools.WPFExtension
{
    public static class MenuItemEx
    {
        public static MenuItem Create(ItemsControl parent,string itemName,string inputGestureText = "")
        {
            var hierarchies = itemName.Split(Path.DirectorySeparatorChar);
            if (hierarchies.Length > 1) {
                var root = new MenuItem() { Header = hierarchies[0] };
                if (parent != null) parent.Items.Add(root);
                return Create(root, itemName.Substring( itemName.IndexOf(Path.DirectorySeparatorChar) + 1),inputGestureText);
            }
            var result = new MenuItem() { Header = itemName ,InputGestureText = inputGestureText};
            if (parent != null) parent.Items.Add(result);
            return result;
        }
    }
}
