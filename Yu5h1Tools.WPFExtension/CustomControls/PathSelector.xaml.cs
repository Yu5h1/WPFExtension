using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;


namespace Yu5h1Tools.WPFExtension.CustomControls
{
    /// <summary>
    /// Interaction logic for PathSelector.xaml
    /// </summary>
    public partial class PathSelector : UserControl
    {
        [Category("PathSelector")]
        public string label
        {
            get { return label_lb.Content as string; }
            set { label_lb.Content = value; }
        }
        [Category("PathSelector")]
        public string text
        {
            get { return textBox.Text; }

            set { textBox.Text = value; }
        }

        [Category("PathSelector")]
        public string FileFilter { get; set; } = "";

        public string[] DropTypesArray { get { return GetTypesArray(FileFilter); } }

        public event TextChangedEventHandler TextChanged
        {
            add { textBox.TextChanged += value; }
            remove { textBox.TextChanged -= value; }
        }

        public PathSelector()
        {
            InitializeComponent();
            Drop += TextBox_Drop;
            PreviewDragEnter += checkDragEvent;
            PreviewDragOver += checkDragEvent;
        }
        private void SelectDialog_btn_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = Path.GetFileName(textBox.Text);
            //dlg.DefaultExt = ".???";
            dlg.Filter = FileFilter;

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                textBox.Text = dlg.FileName;
            }
        }
        public static string[] GetTypesArray(string filters)
        {
            List<string> result = new List<string>();
            foreach (var item in filters.Split('|', ';'))
            {
                if (item.StartsWith("*"))
                {
                    string curtype = item.Remove(0, 2).ToLower();
                    if (!result.Contains(curtype) && curtype != "*")
                    {
                        result.Add(curtype);
                    }
                }
            }
            return result.ToArray();
        }
        void checkDragEvent(object sender,DragEventArgs e)
        {
            var fileDrop = e.Data.GetData(DataFormats.FileDrop);
            if (fileDrop == null) return;
            //e.Effects = DragDropEffects.Move;
            string[] files = (string[])fileDrop;
            if (files.Length == 1)
            {
                foreach (var type in DropTypesArray)
                {
                    if (type == "folder" || type == "directory")
                    {
                        if (File.GetAttributes(files[0]) == FileAttributes.Directory)
                        {
                            e.Handled = true;
                        }
                    }
                }
                if (IsFileTypeMatchWith(files[0], DropTypesArray))
                {
                    e.Handled = true;
                }
            }
        }
        private void TextBox_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (IsFileTypeMatchWith(files[0], DropTypesArray))
            {
                text = files[0];
            }
        }

        private void Label_lb_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Explorer(text);
        }
        void Explorer(string path)
        {
            if (path != string.Empty && (Directory.Exists(path) || File.Exists(path)))
            {
                string cmdPath = "\"" + path + "\"";
                string stringArgs = File.GetAttributes(path) != FileAttributes.Directory ?
                                    " /select, " + cmdPath : cmdPath;
                System.Diagnostics.Process.Start("explorer.exe", stringArgs);
            }
            else
            {
                string description = path == string.Empty ? "Empty Path ! " : path + " \n does not exist ! ";
                MessageBox.Show(description, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public static bool IsFileTypeMatchWith(string path, params string[] types)
        {
            //if (!System.Uri.IsWellFormedUriString(path,System.UriKind.Absolute)) {
            //    return false;
            //}
            string ext = Path.GetExtension(path).ToLower();
            foreach (var t in types)
            {
                string CONDITION = t.ToLower();
                if (!t.StartsWith(".")) CONDITION = "." + CONDITION;
                if (ext == CONDITION) return true;
            }
            return false;
        }
    }
}
