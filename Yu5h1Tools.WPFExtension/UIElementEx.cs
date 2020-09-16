using System;
using System.IO;
using System.Windows;

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
                        if (type == "folder" || type == "directory")
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
    }
}
