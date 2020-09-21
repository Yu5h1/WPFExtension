using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System;
using System.IO;

namespace Yu5h1Tools.WPFExtension
{
    public static class ItemCollectionEx
    {
        public static object Find(this ItemCollection items, string name)
        {
            var Hierarchies = name.Split(Path.DirectorySeparatorChar);
            var subPath = Hierarchies.Length > 1 ? name.Substring(name.IndexOf(Path.DirectorySeparatorChar) + 1) : string.Empty;
            name = Hierarchies.Length > 1 ? name.Remove(name.IndexOf(Path.DirectorySeparatorChar)) : name;
            foreach (var item in items)
            {
                switch (item)
                {
                    case HeaderedItemsControl headerControl:
                        if (headerControl.Header.DisplayTextEquals(name))
                        {
                            if (subPath != string.Empty) return headerControl.Items.Find(subPath);
                            else return item;
                        }
                        break;
                    case ContentControl contentControl:
                        if (contentControl.Content.DisplayTextEquals(name))
                        {
                            if (subPath != string.Empty && contentControl.Content.GetType().IsSubclassOf(typeof(ItemsControl)))
                                return ((ItemsControl)contentControl.Content).Items.Find(subPath);
                            else return item;
                        }
                        break;
                }
            }
            return null;
        }

    }
}
