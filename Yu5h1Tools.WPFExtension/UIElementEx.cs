using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Yu5h1Tools.WPFExtension
{
    public static class UIElementEx
    {
        private static readonly Action<UIElement> RefreshElement = ui =>
        {
            ui.UpdateLayout();
            ui.InvalidateVisual();
        };
        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, new Action(() => RefreshElement(uiElement)));
            uiElement.UpdateLayout();
            uiElement.InvalidateVisual();
        }
        public static DragEventHandler CheckDrag(string[] filters)
        {
            return new DragEventHandler((sender, e) =>
            {
                var fileDrop = e.Data.GetData(DataFormats.FileDrop);
                if (fileDrop == null) return;
                string[] files = (string[])fileDrop;
                if (files.Length == 1)
                {
                    foreach (var type in filters)
                    {
                        if (type.ToLower() == "folder" || type.ToLower() == "directory")
                        {
                            if (File.GetAttributes(files[0]).HasFlag(FileAttributes.Directory)) e.Handled = true;
                        }
                    }
                    if (files[0].IsFileTypeHasAny(filters)) e.Handled = true;
                }
            });
        }
        public static void HandleDragDrop(this UIElement target,Action<string[]> dropHandle, params string[] filters)
        {
            target.AllowDrop = true;
            target.PreviewDragOver += CheckDrag(filters);
            target.Drop += new DragEventHandler((s, e) => dropHandle((string[])e.Data.GetData(DataFormats.FileDrop)));
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
