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
            MenuItem result = null;
            var hierarchies = itemName.Split(Path.DirectorySeparatorChar);
            if (hierarchies.Length > 1) {
                if (parent != null)
                {
                    for (int i = 0; i < hierarchies.Length - 1; i++)
                    {
                        var existsParent = parent.Find(hierarchies[i]) as ItemsControl;
                        if (existsParent != null) parent = existsParent;
                        else parent = parent.AddMenuItem(hierarchies[i]);
                    }
                    result = new MenuItem() { Header = hierarchies[hierarchies.Length - 1] };
                    parent.Items.Add(result);
                } else
                {
                    result = Create(new MenuItem() { Header = hierarchies[0] },
                                   itemName.Substring(itemName.IndexOf(Path.DirectorySeparatorChar) + 1),
                                   inputGestureText);
                    parent.Items.Add(result);
                }
            }else
            {
                result = new MenuItem() { Header = itemName, InputGestureText = inputGestureText };
                if (parent != null) parent.Items.Add(result);
            }
            return result;
        }
    }
}
