using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Yu5h1Tools.WPFExtension
{
    public static class TreeViewItemEx
    {
         public static TreeViewItem SetTextBlockHeader(this TreeViewItem item,string txt,bool IsHitTestVisible = false)
        {
            var textBlock = new TextBlock
            {                
                Text = txt,
                IsHitTestVisible = IsHitTestVisible
            };
            item.Header = textBlock;
            return item;
        }
        public static TreeViewItem AddTextBlock(this ItemsControl control,string header)
        {
            var result = new TreeViewItem().SetTextBlockHeader(header);
            control.Items.Add(result);
            return result;
        }
        public static StackPanel GetField(object title, object value, double labelWidth = 50,double fieldWidth = 0)
        {
            var panel = new StackPanel() { Orientation = Orientation.Horizontal };
            var tb = new TextBox()
            {
                Text = value.ToString(),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
            };
            if (fieldWidth > labelWidth) tb.Width = fieldWidth - labelWidth;
            panel.Children.Add( new TextBlock() {
                Text = title.ToString(),
                VerticalAlignment = VerticalAlignment.Center,
                Width = labelWidth
            },tb);
            return panel;
        }
        static TextBox GetNameField(this Control item,string name,double fieldWidth = 0)
        {
            var tb = new TextBox()
            {
                Background = null,
                BorderBrush = null,
                Text = name,
                Focusable = false,
                IsHitTestVisible = false,
                Width = fieldWidth == 0 ? item.Width : fieldWidth,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            Action<TextBox,bool> confirmModify = (textBox,cancel) => {
                if (cancel) {

                }
                textBox.Background = null;
                textBox.Focusable = false;
                textBox.IsHitTestVisible = false;
            };

            tb.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Escape) confirmModify((TextBox)s, true);
                if (e.Key == Key.Enter) confirmModify((TextBox)s, false);
            };
            tb.LostFocus += (s, e) => confirmModify((TextBox)s, false);
            item.KeyDown += (s, e) => {
                if (e.Key == Key.F2) {
                    tb.IsHitTestVisible = true;
                    tb.Focusable = true;
                    tb.Focus();
                    tb.Background = Brushes.White;
                    tb.SelectAll();
                }
            };
            item.HorizontalAlignment = HorizontalAlignment.Stretch;
            return tb;
        }
        public static TreeViewItem SetNameField(this TreeViewItem treeItem,object name, double fieldWidth = 0)
        {
            treeItem.Header = GetNameField(treeItem, name.ToString(), fieldWidth);
            return treeItem;
        }
        public static TreeViewItem AddNameField(this TreeViewItem treeItem, object name, double fieldWidth = 0)
        {
            var result = new TreeViewItem().SetNameField(name, fieldWidth);
            treeItem.Items.Add(result);
            return result;
        }
        public static ListBoxItem SetNameField(this ListBoxItem listboxItem, object name,double fieldWidth = 0)
        {
            listboxItem.Content = GetNameField(listboxItem, name.ToString(),fieldWidth);
            return listboxItem;
        }

        public static TreeViewItem SetField(this TreeViewItem treeItem, object title, object value, double labelWidth = 50, double fieldWidth = 0)
        {
            treeItem.Header = GetField(title, value, labelWidth, fieldWidth);
            return treeItem;
        }
        public static TreeViewItem AddField(this TreeViewItem treeItem, object title, object value, double labelWidth = 50, double fieldWidth = 0)
        {
            var result = new TreeViewItem().SetField(title, value, labelWidth, fieldWidth);
            treeItem.Items.Add(result);
            return result;
        }
        public static ListBoxItem SetField(this ListBoxItem listBoxItem, object title, object value, double labelWidth = 50, double fieldWidth = 0)
        {
            listBoxItem.Content = GetField(title, value, labelWidth, fieldWidth) ;
            return listBoxItem;
        }
    }
}
