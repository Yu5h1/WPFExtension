using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        /// <summary>
        /// Image files (*.png;*.jpg;*.gif;*.jpeg)|*.png;*.jpg;*.gif;*.jpeg|All files (*.*)|*.*
        /// </summary>
        [Category("PathSelector")]
        public string Text
        {
            get { return textBox.Text; }

            set { textBox.Text = DisplayFileNameOnly ? Path.GetFileName(value) : value; }
        }
        [Category("PathSelector")]
        public bool DisplayFileNameOnly { get; set; } = false;

        [Category("PathSelector")]
        public string FileFilter { get; set; } = "Image files (*.png;*.jpg;*.gif;*.jpeg)|*.png;*.jpg;*.gif;*.jpeg|All files (*.*)|*.*";

        [Category("PathSelector")]
        public string InitialDirectory { get; set; }

        public string[] DropTypesArray { get { return GetTypesArray(FileFilter); } }

        public event TextChangedEventHandler TextChanged
        {
            add { textBox.TextChanged += value; }
            remove { textBox.TextChanged -= value; }
        }

        public PathSelector()
        {
            InitializeComponent();
            textBox.HandleDragDrop(files => {
                if (files[0].IsFileTypeMatches(DropTypesArray)) Text = files[0];
            }, DropTypesArray);
            textBox.KeyDown += (s, e) => {
                if (e.Key == Key.Escape) {
                    textBox.Text = "";
                    Keyboard.ClearFocus();
                }
            };
        }
        private void SelectDialog_btn_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (!string.Empty.Equals(InitialDirectory)) dlg.InitialDirectory = InitialDirectory;            
            
            dlg.FileName = Path.GetFileName(textBox.Text);
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
            List<string> filterList = filters.Split('|').Where( d => d.Contains("*") &&
                                                                !d.Contains("(") &&
                                                                !d.Contains("*.*")).
                                                                Select(d => d.Replace("*", "")).
                                                                Join().Split(';').ToList();
            return filterList.ToArray();
        }
        private void Label_lb_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Explorer(Text);
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
    }
}
