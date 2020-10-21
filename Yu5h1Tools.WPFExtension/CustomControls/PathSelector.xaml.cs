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
            set {
                if (!string.IsNullOrEmpty(value) && OnlyAllowValueFromInitialDirectory)
                {
                    if (Path.GetPathRoot(value) != "" && !value.ToLower().Contains(InitialDirectory.ToLower()))
                    {
                        (value + "\n does not belong to this directory [ " + InitialDirectory + " ]").PromptWarnning();
                        return;
                    }
                }
                autoCompleteBox.Text = SetPathBy == null ? value : SetPathBy(value);
            }
        }

        [Category("PathSelector")]
        public string FileFilter { get; set; } = "Image files (*.png;*.jpg;*.gif;*.jpeg)|*.png;*.jpg;*.gif;*.jpeg|All files (*.*)|*.*";

        [Category("PathSelector")]
        public string InitialDirectory { get; set; } = "";
        [Category("PathSelector")]
        public bool ShowInitialDirectoryIfEmpty { get; set; }

        [Category("PathSelector")]
        public bool OnlyAllowValueFromInitialDirectory { get; set; }

        [Category("PathSelector")]
        public bool UseAutoComplete{ get; set; }

        [Category("PathSelector")]
        public string InvalidCharacters { get; set; } = ":*?\"<>|";

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
            AddInvalidCharactersHandler(autoCompleteBox,InvalidCharacters);
        }
        public void setFilters(bool AllFile,params FileTypeFilter[] filters) {
            FileFilter = filters.ToString(AllFile);
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
                    Text = ShowOpenFileDialog(false,Text,FileFilter,InitialDirectory);
                    break;
                case MouseButton.Right:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl))
                        ProcessUtil.Launch(Text);
                    else {
                        string pathForShow = Text;
                        if (ShowInitialDirectoryIfEmpty && Text == "")
                        {
                            pathForShow = InitialDirectory;
                        }
                        ProcessUtil.ShowInExplorer(pathForShow);
                    }
                        
                    break;
            }
        }
        public static string[] GetFileFilters(string filters)
        {
            List<string> filterList = filters.Split('|').Where( d => d.Contains("*") &&
                                                                !d.Contains("(") &&
                                                                !d.Contains("*.*")).
                                                                Select(d => d.Replace("*", "").Replace(" ","")).
                                                                Join().Split(';').Where(d=>d != "").ToList();
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
        public static void CheckInvalidCharactersInput(object s,TextCompositionEventArgs e)
        {
            var tb = (TextBox)s;
            var ttp = tb.ToolTip as ToolTip;
            string invalidCharacters = (ttp.Content as string).GetLines()[1].Replace(" ","");
            if (invalidCharacters.Contains(e.Text))
            {
                e.Handled = true;
                tb.ShowToolTip();
            }
        }
        public static void AddInvalidCharactersHandler(TextBox textBox,string invalidCharacters = ":*?\"<>|")
        {
            var toolTip = new ToolTip()
            {
                Content = "Invalid characters:\n" + invalidCharacters.ToArray().Join(" ").ToString(),
                
            };

            toolTip.Visibility = Visibility.Hidden;
            toolTip.Closed += (s, e) => ((ToolTip)s).Visibility = Visibility.Hidden;
            textBox.ToolTip = toolTip;
            textBox.PreviewTextInput += CheckInvalidCharactersInput;
        }
        public static void RemoveInvalidCharactersHandler(TextBox textBox)
                                        => textBox.PreviewTextInput -= CheckInvalidCharactersInput;
    }
}
