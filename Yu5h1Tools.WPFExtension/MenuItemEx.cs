using System;
using System.IO;
using System.Windows.Controls;

namespace Yu5h1Tools.WPFExtension
{
    public static class MenuItemEx
    {
        public static MenuItem Create(string itemName, ItemsControl parent)
        {
            var hierarchies = itemName.Split(Path.DirectorySeparatorChar);
            if (hierarchies.Length > 1) {
                var root = new MenuItem() { Header = hierarchies[0] };
                if (parent != null) parent.Items.Add(root);
                return Create(itemName.Substring(itemName.IndexOf(Path.DirectorySeparatorChar) + 1), root);
            }
            var result = new MenuItem() { Header = itemName };
            if (parent != null) parent.Items.Add(result);
            return result;
        }
    }
}
