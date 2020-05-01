using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Yu5h1Tools.WPFExtension
{
    /// <summary>
    /// Interaction logic for SearchBar.xaml
    /// </summary>
    public partial class SearchBar : UserControl
    {
        private int _Win32IconID = 209;

        [Category("Common Properties")]
        public ImageSource Icon
        {
            get { return image.Source; }
            set { image.Source = value; }
        }

        [Category("Common Properties")]
        public string Text
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }
        [Category("Common Properties")]
        public double TextLength
        {
            get { return textBox.Width; }
            set { textBox.Width = value; }
        }
        
        public event TextChangedEventHandler TextChanged
        {
            add { textBox.TextChanged += value; }
            remove { textBox.TextChanged -= value; }
        }

        public SearchBar()
        {
            InitializeComponent();
        }
        public static bool Match(string searchText, string target,bool ToLower = true)
        {
            if (searchText == "" || searchText == null) return true;
            string[] searchTextDatas = searchText.Split('_', ' ');
            for (int i = 0; i < searchTextDatas.Length; i++) {
                if (ToLower)
                {
                    if (!target.ToLower().Contains(searchTextDatas[i].ToLower())) return false;
                }
                else {
                    if (!target.Contains(searchTextDatas[i])) return false;
                }
            }
                
            return true;
        }
        

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
         
        }

        private void textBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                textBox.Text = "";
            }
        }
    }
}
