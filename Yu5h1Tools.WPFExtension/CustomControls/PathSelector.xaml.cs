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
    public struct SelectionDialogFilter
    {
        public string DisplayName;
        public string[] types;
        public SelectionDialogFilter(string displayName, params string[] fileTypes)
        {
            DisplayName = displayName;
            types = fileTypes;
        }
        public override string ToString()
        {
            string typesTxT = types.Select(d => {
                var item = d;
                if (!item.StartsWith("*."))
                {
                    if (item.StartsWith(".")) item = "*" + item;
                    else item = "*." + item;
                }
                return item;
            }).Join("; ");
            return DisplayName + "(" + typesTxT + ")|" + typesTxT;
        }
    }
    /// <summary>
    /// Interaction logic for PathSelector.xaml
    /// </summary>
    public partial class PathSelector : UserControl
    {
        public Label labelControl => label_lb;
        [Category("PathSelector")]
        public string label
        {
            get { return label_lb.Content as string; }
            set { label_lb.Content = value; }
        }

        [Category("PathSelector")]
        public double labelWidth
        {
            get => label_column.Width.Value;
            set => label_column.Width = new GridLength(value, GridUnitType.Pixel);
        }

        public Func<string, string> GetPathBy;
        public Func<string, string> SetPathBy;

        /// <summary>
        /// Image files (*.png;*.jpg;*.gif;*.jpeg)|*.png;*.jpg;*.gif;*.jpeg|All files (*.*)|*.*
        /// </summary>
        [Category("PathSelector")]
        public string Text
        {
            get => GetPathBy == null ? autoCompleteBox.Text : GetPathBy(autoCompleteBox.Text);
            set => autoCompleteBox.Text = SetPathBy == null ? value : SetPathBy(value);
        }

        [Category("PathSelector")]
        public string FileFilter { get; set; } = "Image files (*.png;*.jpg;*.gif;*.jpeg)|*.png;*.jpg;*.gif;*.jpeg|All files (*.*)|*.*";

        [Category("PathSelector")]
        public string InitialDirectory { get; set; }

        [Category("PathSelector")]
        public bool UseAutoComplete{ get; set; }

        public string AutoCompleteLocation = "";

        public bool DisplayFullPath { get; set; }        

        public event TextChangedEventHandler TextChanged
        {
            add { autoCompleteBox.TextChanged += value; }
            remove { autoCompleteBox.TextChanged -= value; }
        }

        public string fileName => Path.GetFileName(Text);        

        public PathSelector()
        {
            InitializeComponent();
            Loaded += PathSelector_Loaded;
            autoCompleteBox.KeyDown += (s, keyEvent) => {
                if (keyEvent.Key == Key.Escape)
                {
                    autoCompleteBox.Text = "";
                    Keyboard.ClearFocus();
                }
            };

        }
        private void PathSelector_Loaded(object sender, RoutedEventArgs e)
        {
            autoCompleteBox.HandleDragDrop(files => {
                Text = files[0];
            }, GetFileFilters(FileFilter));
        }
        public void setFilters(bool AllFile,params SelectionDialogFilter[] filters) {
            FileFilter = filters.Select(d=>d.ToString()).Join("|");
            if (AllFile) FileFilter+= "|All files (*.*)|*.*";
        }
        public static string ShowOpenFileDialog(bool Multiselect = false,string filePath = "", string fileFilter = "All files (*.*)|*.*", string InitialDirectory = "") {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = Multiselect;
            if (File.Exists(filePath)) dlg.InitialDirectory = Path.GetDirectoryName(filePath);
            else if (!string.Empty.Equals(InitialDirectory)) dlg.InitialDirectory = InitialDirectory;
            if (!string.Empty.Equals(filePath)) dlg.FileName = Path.GetFileName(filePath);
            dlg.Filter = fileFilter;

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                return dlg.FileName;
            }
            return "";
        }
        private void SelectDialog_btn_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    ShowOpenFileDialog(false,Text,FileFilter,InitialDirectory);
                    break;
                case MouseButton.Right:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl))
                        ProcessUtil.Launch(Text);
                    else
                        ProcessUtil.ShowInExplorer(Text,false);
                    break;
            }
        }
        public static string[] GetFileFilters(string filters)
        {
            List<string> filterList = filters.Split('|').Where( d => d.Contains("*") &&
                                                                !d.Contains("(") &&
                                                                !d.Contains("*.*")).
                                                                Select(d => d.Replace("*", "").Replace(" ","")).
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
