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
        public static O SetControlWithLabel<O, T>(this O item, object title, T control = null,bool LabelRightSide = false)
                                                where T : UIElement, new()
                                                where O : Control
        {
            var panel = CreateMixControls(title, control == null ? new T() : control);
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
        public static TreeViewItem SetField(this TreeViewItem treeItem, object title, object value, double labelWidth = 50, double fieldWidth = 0)
        {
            treeItem.Header = GetField(title, value, labelWidth, fieldWidth);
            treeItem.KeyDown += (s, e) =>
            {
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.C)
                    Clipboard.SetText(treeItem.GetMixControl<TextBlock>(0).Text);
            };
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
            result.SetControlWithLabel(title, item);
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
