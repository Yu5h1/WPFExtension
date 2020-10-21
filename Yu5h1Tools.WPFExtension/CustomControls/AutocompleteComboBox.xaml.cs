using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Yu5h1Tools.WPFExtension.CustomControls
{
    /// <summary>
    /// Interaction logic for AautocompleteComboBox2.xaml
    /// </summary>
    public partial class AutocompleteComboBox : ComboBox
    {
        public string[] sourceItems = new string[0];
        public TextBox textBox => (TextBox)Template.FindName("PART_EditableTextBox", this);
        public event TextChangedEventHandler TextChanged
        {
            add { AddHandler(TextBoxBase.TextChangedEvent, value); }
            remove { RemoveHandler(TextBoxBase.TextChangedEvent, value); }
        }
        private new bool IsEditable {
            get => base.IsEditable; 
            set => base.IsEditable = value; 
        }

        public AutocompleteComboBox()
        {
            InitializeComponent();
            IsEditable = true;
            IsTextSearchEnabled = false;
            TextChanged += OnTextChanged;
            DropDownOpened += (s, ee) =>
            {
                var tb = textBox;
                tb.CaretIndex = tb.Text.Length;
            };
            SelectionChanged += AautocompleteComboBox_SelectionChanged;
            DropDownClosed += AautocompleteComboBox_DropDownClosed;
        }

        private void AautocompleteComboBox_DropDownClosed(object sender, EventArgs e)
        {
        }

        bool IsSelectionChangedCheck = false;
        private void AautocompleteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsSelectionChangedCheck = true;
            e.Handled = false;
        }
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsSelectionChangedCheck) {
                IsSelectionChangedCheck = false;
                return;
            }
            Items.Clear();
            if (Text == string.Empty) IsDropDownOpen = false;
            else if (sourceItems != null && sourceItems.Length > 0)
            {
                var results = sourceItems.Where(d => d.StartsWith(Text, StringComparison.OrdinalIgnoreCase));
                if (results.Count() > 0)
                {
                    foreach (var item in results) { Items.Add(new ComboBoxItem() { Content = item }); }
                    IsDropDownOpen = true;
                }
                else IsDropDownOpen = false;

            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Text = "";
            base.OnKeyDown(e);
        }
    }
}
