using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Yu5h1Tools.WPFExtension
{
    public static class ContextMenuEx
    {        
        public static MenuItem AddMenuItem(this ContextMenu contextMenu,string itemName,string inputGestureText = "")
        {
            var exists = contextMenu.Find(itemName);
            if (exists != null) return exists as MenuItem;
            return MenuItemEx.Create(contextMenu, itemName,  inputGestureText);
        }
    }
}
