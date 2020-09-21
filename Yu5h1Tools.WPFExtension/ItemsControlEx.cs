using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System;
using System.Windows.Media;
using System.Windows.Input;
using System.IO;

namespace Yu5h1Tools.WPFExtension
{
    public static class ItemsControlEx
    {
        public static TreeViewItem SetTextBlockHeader(this TreeViewItem item, string txt, bool IsHitTestVisible = false)
        {
            var textBlock = new TextBlock
            {
                Text = txt,
                IsHitTestVisible = IsHitTestVisible
            };
            item.Header = textBlock;
            return item;
        }
        public static TreeViewItem AddTextBlock(this ItemsControl control, string header)
        {
            var result = new TreeViewItem().SetTextBlockHeader(header);
            control.Items.Add(result);
            return result;
        }
        public static T GetHeader<T>(this HeaderedItemsControl item) where T : UIElement => (T)item.Header;

        public static StackPanel CreateMixControls(object title,params UIElement[] controls)
        {
            var panel = new StackPanel() { Orientation = Orientation.Horizontal };
            foreach (var control in controls) panel.Children.Add(new TextBlock()
            {
                Text = title.ToString(),
                VerticalAlignment = VerticalAlignment.Center,
            }, control);
            return panel;
        }
        public static T GetMixControl<T>(this HeaderedItemsControl item,int index) where T : UIElement
        => (T)((StackPanel)item.Header).Children[index];

        static StackPanel GetField(object title, object value, double labelWidth = 50, double fieldWidth = 0)
        {
            return CreateMixControls(title, new TextBox()
            {
                Text = value.ToString(),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
            });
        }
        public static void SetTitleControl<T>(this HeaderedItemsControl item, object title, T control = null) where T : UIElement, new()
            => item.Header = CreateMixControls(title, control == null ? new T() : control);
        public static void SetTitleControl<T>(this ContentControl item, object title, T control = null) where T : UIElement, new()
            => item.Content = CreateMixControls(title, control == null ? new T() : control);


        static TextBox GetNameField(this Control item, string name, double fieldWidth = 0)
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
            Action<TextBox, bool> confirmModify = (textBox, cancel) => {
                if (cancel)
                {

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
                if (e.Key == Key.F2)
                {
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
        public static TreeViewItem SetNameField(this TreeViewItem treeItem, object name, double fieldWidth = 0)
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
        public static ListBoxItem SetNameField(this ListBoxItem listboxItem, object name, double fieldWidth = 0)
        {
            listboxItem.Content = GetNameField(listboxItem, name.ToString(), fieldWidth);
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
        public static TreeViewItem AddTitleControl<T>(this TreeViewItem treeItem, object title,T item = null)  where T : UIElement,new()
        {
            var result = new TreeViewItem();
            result.SetTitleControl(title, item);
            treeItem.Items.Add(result);
            return result;
        }
        public static ListBoxItem SetField(this ListBoxItem listBoxItem, object title, object value, double labelWidth = 50, double fieldWidth = 0)
        {
            listBoxItem.Content = GetField(title, value, labelWidth, fieldWidth);
            return listBoxItem;
        }
        public static object Find(this ItemsControl itemsControl, string name) => itemsControl.Items.Find(name);       
    }
}
