using System;
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
        [Category("Common Properties")]
        public string label
        {
            get { return label_lb.Content as string; }
            set { label_lb.Content = value; }
        }
        [Category("Common Properties")]
        public string text
        {
            get { return textBox.Text; }

            set { textBox.Text = value; }
        }

        [Category("Common Properties")]
        public string DropTypes { get; set; } = "";
        public string[] DropTypesArray
        {
            get { return DropTypes.Split(','); }
            set => DropTypes = string.Join(",", value: value);
        }

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
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".nif"; // Default file extension
            dlg.Filter = "Nif File (.nif)|*.nif"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                textBox.Text = dlg.FileName;
            }
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
