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
        public static TreeViewItem SetHeader<T>(this TreeViewItem item,T control) where T : new()
        {
            if (control == null) control = new T();
            item.Header = control;
            return item;
        }
        public static TreeViewItem SetTextBlockHeader(this TreeViewItem item, string label, bool IsHitTestVisible = false)
        {
            item.SetHeader(new TextBlock
            {
                Text = label,
                IsHitTestVisible = IsHitTestVisible
            });
            return item;
        }
        public static T GetHeader<T>(this HeaderedItemsControl item) where T : UIElement => (T)item.Header;

        public static StackPanel CreateMixControls(object label,params UIElement[] controls)
        {
            var panel = new StackPanel() { Orientation = Orientation.Horizontal };
            foreach (var control in controls) panel.Children.Add(new TextBlock()
            {
                Text = label.ToString(),
                VerticalAlignment = VerticalAlignment.Center,
            }, control);
            return panel;
        }
        public static T GetMixControl<T>(this HeaderedItemsControl item,int index) where T : UIElement
        => (T)((StackPanel)item.Header).Children[index];
        public static T GetMixControl<T>(this ContentControl item, int index) where T : UIElement
        => (T)((StackPanel)item.Content).Children[index];


        static StackPanel GetField(object label, object value, double labelWidth = 50, double fieldWidth = 0)
        {
            return CreateMixControls(label, new TextBox()
            {
                Text = value.ToString(),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
            });
        }
        public static O SetControlLabel<O, T>(this O item, object label, T control = null,bool LabelRightSide = false)
                                                where T : UIElement, new()
                                                where O : Control
        {
            var panel = CreateMixControls(label, control == null ? new T() : control);
            if (LabelRightSide) panel.Switch(0, 1);
            switch (item)
            {
                case HeaderedItemsControl headeredItemsControl:
                    headeredItemsControl.Header = panel;
                    break;
                case ContentControl headeredItemsControl:
                    headeredItemsControl.Content = panel;
                    break;
            }
            return item;
        }
        static TextBox GetNameField(this Control item, string name,bool allowEmpty, Action<TextBox> OnconfirmModify, double fieldWidth = 0)
        {
            var field = new TextBox()
            {
                Background = null,
                BorderBrush = null,
                Text = name,
                Focusable = false,
                IsHitTestVisible = false,
                Width = fieldWidth == 0 ? item.Width : fieldWidth,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            Action<TextBox, bool> confirmModify = (tb, cancel) => {
                if (cancel || (!allowEmpty && tb.Text.Equals(string.Empty)))
                    tb.Text = tb.Tag as string;
                else OnconfirmModify(tb);
                tb.Background = null;
                tb.Focusable = false;
                tb.IsHitTestVisible = false;
                
            };

            field.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Escape) confirmModify((TextBox)s, true);
                if (e.Key == Key.Enter) confirmModify((TextBox)s, false);
            };
            field.LostFocus += (s, e) => confirmModify((TextBox)s, false);
            item.KeyDown += (s, e) => {
                if (e.Key == Key.F2)
                {
                    field.Tag = field.Text;
                    field.IsHitTestVisible = true;
                    field.Focusable = true;
                    field.Focus();
                    field.Background = Brushes.White;
                    field.SelectAll();
                }
            };
            item.HorizontalAlignment = HorizontalAlignment.Stretch;
            return field;
        }
        public static TreeViewItem SetNameField(this TreeViewItem treeItem, object name,bool allowEmpty, Action<TextBox> OnconfirmModify, double fieldWidth = 0)
        {
            treeItem.Header = GetNameField(treeItem, name.ToString(), allowEmpty, OnconfirmModify, fieldWidth);
            return treeItem;
        }
        public static TreeViewItem AddNameField(this TreeViewItem treeItem, object name, bool allowEmpty, Action<TextBox> OnconfirmModify, double fieldWidth = 0)
        {
            var result = new TreeViewItem().SetNameField(name, allowEmpty, OnconfirmModify, fieldWidth);
            treeItem.Items.Add(result);
            return result;
        }
        public static ListBoxItem SetNameField(this ListBoxItem listboxItem, object name,bool allowEmpty, Action<TextBox> OnconfirmModify, double fieldWidth = 0)
        {
            listboxItem.Content = GetNameField(listboxItem, name.ToString(), allowEmpty, OnconfirmModify, fieldWidth);
            return listboxItem;
        }
        public static TreeViewItem SetField(this TreeViewItem treeItem, object label, object value, double labelWidth = 50, double fieldWidth = 0)
        {
            treeItem.Header = GetField(label, value, labelWidth, fieldWidth);
            treeItem.KeyDown += (s, e) =>
            {
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.C)
                    Clipboard.SetText(treeItem.GetMixControl<TextBlock>(0).Text);
            };
            return treeItem;
        }
        public static TreeViewItem AddField(this TreeViewItem treeItem, object label, object value, double labelWidth = 50, double fieldWidth = 0)
        {
            var result = new TreeViewItem().SetField(label, value, labelWidth, fieldWidth);
            treeItem.Items.Add(result);
            return result;
        }
        public static TreeViewItem AddTreeNode(this TreeViewItem treeItem, object label)
        {
            var result = new TreeViewItem() { Header = label };
            return result;
        }
        public static TreeViewItem AddTreeNode<T>(this TreeViewItem treeItem, object label,T item = null)  where T : UIElement,new()
        {
            var result = new TreeViewItem();
            result.SetControlLabel(label, item);
            treeItem.Items.Add(result);
            return result;
        }
        public static ListBoxItem SetField(this ListBoxItem listBoxItem, object label, object value, double labelWidth = 50, double fieldWidth = 0)
        {
            listBoxItem.Content = GetField(label, value, labelWidth, fieldWidth);
            return listBoxItem;
        }
        public static object Find(this ItemsControl itemsControl, string name) => itemsControl.Items.Find(name);       
    }
}
