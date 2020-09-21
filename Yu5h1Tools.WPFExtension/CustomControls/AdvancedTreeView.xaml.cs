using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for AdvancedTreeView.xaml
    /// </summary>
    public partial class AdvancedTreeView : UserControl
    {
        [Category("Common")]
        public ItemCollection items => multiSelectTreeView.Items;
        public event RoutedPropertyChangedEventHandler<object> SelectedItemChanged
        {
            add => multiSelectTreeView.SelectedItemChanged += value;
            remove => multiSelectTreeView.SelectedItemChanged -= value;
        }
        public AdvancedTreeView()
        {
            InitializeComponent();
        }

    }
    class TreeViewLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TreeViewItem item = (TreeViewItem)value;
            ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
            return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }
}
