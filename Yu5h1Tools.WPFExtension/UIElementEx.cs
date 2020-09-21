using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Yu5h1Tools.WPFExtension
{
    public static class UIElementEx
    {
        public static void HandleDragDrop(this UIElement target,Action<string[]> dropHandle, params string[] filters)
        {
            target.AllowDrop = true;
            var CheckDrag = new DragEventHandler((sender, e) =>
            {
                var fileDrop = e.Data.GetData(DataFormats.FileDrop);
                if (fileDrop == null) return;
                //e.Effects = DragDropEffects.Move;
                string[] files = (string[])fileDrop;
                if (files.Length == 1)
                {
                    foreach (var type in filters)
                    {
                        if (type.ToLower() == "folder" || type.ToLower() == "directory")
                        {
                            if (File.GetAttributes(files[0]) == FileAttributes.Directory)
                            {
                                e.Handled = true;
                            }
                        }
                    }
                    if (files[0].IsFileTypeMatches(filters))
                    {
                        e.Handled = true;
                    }
                }
            });
            target.PreviewDragEnter += CheckDrag;
            target.PreviewDragOver += CheckDrag;
            target.Drop += new DragEventHandler((s,e)=> dropHandle((string[])e.Data.GetData(DataFormats.FileDrop)));
        }
        internal static bool DisplayTextEquals(this object target, string txt,
    StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            switch (target)
            {
                case TextBlock tbk: return txt.Equals(tbk.Text, stringComparison);
                case TextBox tbx: return txt.Equals(tbx.Text, stringComparison);
                case ContentControl contentcontrol:
                    if (contentcontrol.Content.GetType().IsSubclassOf(typeof(UIElement)))
                        return DisplayTextEquals(contentcontrol.Content, txt, stringComparison);
                    return txt.Equals(contentcontrol.Content.ToString(), stringComparison);
                case HeaderedItemsControl headerControl:
                    if (headerControl.Header.GetType().IsSubclassOf(typeof(UIElement)))
                        return DisplayTextEquals(headerControl.Header, txt, stringComparison);
                    return txt.Equals(headerControl.Header.ToString(), stringComparison);
                case string displayText:
                    return txt.Equals(displayText, stringComparison);
            }
            return false;
        }
        public static bool DisplayTextEquals<T>(this T target, string txt,
    StringComparison stringComparison = StringComparison.OrdinalIgnoreCase) where T : UIElement
        {
            switch (target)
            {
                case TextBlock tbk: return txt.Equals(tbk.Text, stringComparison);
                case TextBox tbx: return txt.Equals(tbx.Text, stringComparison);
                case ContentControl contentcontrol:
                    if (contentcontrol.Content.GetType().IsSubclassOf(typeof(UIElement)))
                        return DisplayTextEquals(contentcontrol.Content, txt, stringComparison);
                    return txt.Equals(contentcontrol.Content.ToString(), stringComparison);
                case HeaderedItemsControl headerControl:
                    if (headerControl.Header.GetType().IsSubclassOf(typeof(UIElement)))
                        return DisplayTextEquals(headerControl.Header, txt, stringComparison);
                    return txt.Equals(headerControl.Header.ToString(), stringComparison);
            }
            return false;
        }
    }
}
