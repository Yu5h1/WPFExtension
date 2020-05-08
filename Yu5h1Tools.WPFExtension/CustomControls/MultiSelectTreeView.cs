using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;
using Yu5h1Tools.WPFExtension;

namespace Yu5h1Tools.WPFExtension.CustomControls
{
    public class MultiSelectTreeView : TreeView
    {
        [Category("Custom Properties")]
        public bool EnableDeleteNode { get; set; }
        public TreeViewItem selectedNode {
            get {
                if (SelectedItem == null) return null;
                return (TreeViewItem)SelectedItem;
            }
        }
        public List<TreeViewItem> selectedNodes = new List<TreeViewItem>();

        TreeViewItem previouseSelected;

        bool IsMouseInSelectedHeader 
        {
            get {
                if (selectedNode == null) return false;
                return ((FrameworkElement)selectedNode.Header).GetBounds(this).Contains(Mouse.GetPosition(this));
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }


        void AppendToSelection(TreeViewItem item) {
            if (selectedNodes.Contains(item)) return;
            item.Background = Brushes.SkyBlue;
            selectedNodes.Add(item);
        }
        void AppendToSelection(object item) {
            if (item.GetType() != typeof(TreeViewItem)) return;
            AppendToSelection((TreeViewItem)item);
        }

        public List<TreeViewItem> GetAllTreeViewItems(bool IncludeUnExpanded = false) {
            var result = new List<TreeViewItem>();
            foreach (var item in Items)
                result.AddRange(GetAllChildren((TreeViewItem)item, IncludeUnExpanded));
            return result;
        }
        public List<TreeViewItem> GetAllChildren(TreeViewItem node, bool IncludeUnExpanded = false )
        {
            List<TreeViewItem> result = new List<TreeViewItem>();
            result.Add(node);
            if (node.IsExpanded || IncludeUnExpanded )
            {
                foreach (var item in node.Items)
                {
                    if (item == null) continue;
                    var curItem = item as TreeViewItem;                    
                    result.AddRange(GetAllChildren(curItem, IncludeUnExpanded));
                }
            }
            return result;
        }
        public void SelectItems(List<TreeViewItem> items) {
            foreach (var item in items) AppendToSelection(item);
        }
        public void SelectAllChildren(TreeViewItem node,bool IncludeUnExpanded = false) {
            SelectItems(GetAllChildren(node, IncludeUnExpanded));
        }
        public void SelectAllChildren(object item)
        {
            if (item.GetType() != typeof(TreeViewItem)) return;
            SelectAllChildren((TreeViewItem)item);
        }

        TreeViewItem getRootItem(TreeViewItem item) {
            while (item.Parent.GetType() == typeof(TreeViewItem)) {
                item = item.Parent as TreeViewItem;
            }
            return item;
        }
        public void SetTreeViewItemsIsExpended(List<TreeViewItem> items,bool IsExpanded)
        {
            foreach (var item in items) item.IsExpanded = IsExpanded;
        }
        public void ShowItems(string filter,Func<TreeViewItem,string> getName) {
            var items = GetAllTreeViewItems(true);
            List<TreeViewItem> visibleItems = new List<TreeViewItem>();
            foreach (var item in items)
            {
                if (getName(item).Match(filter, StringComparison.Ordinal)) visibleItems.Add(item);
                else item.Visibility = Visibility.Collapsed;
            }
            foreach (var item in visibleItems)
            {
                var curNode = item as TreeViewItem;
                while (curNode != null )
                {
                    curNode.Visibility = Visibility.Visible;
                    curNode.IsExpanded = true;
                    if (curNode.Parent.GetType() == typeof(TreeViewItem)) curNode = curNode.Parent as TreeViewItem;
                    else curNode = null;
                }
            }
            if (visibleItems.Count > 0)
                visibleItems[visibleItems.Count - 1].BringIntoView();
        }

        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            previouseSelected = e.OldValue as TreeViewItem;
            base.OnSelectedItemChanged(e);
        }
        #region InputEvent
        
        void CustomDoubleClickMouse(MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                var nodeChildren = GetAllChildren(selectedNode, true);
                if (InputEx.KeysDown(Key.LeftCtrl, Key.LeftAlt))
                {
                    if (e.ChangedButton == MouseButton.Left)
                        SetTreeViewItemsIsExpended(nodeChildren, true);
                    else if (e.ChangedButton == MouseButton.Right)
                        SetTreeViewItemsIsExpended(nodeChildren, false);
                }else {
                    if (e.ChangedButton == MouseButton.Left)
                        SelectAllChildren(selectedNode);
                }
                
                e.Handled = true;
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = true;
                CustomMouseDownEvent(e);
            }
            if (e.ClickCount > 1) {             
                if (IsMouseInSelectedHeader)
                {
                    if (selectedNode != null) CustomDoubleClickMouse(e);
                }
                e.Handled = true;
            }
            base.OnPreviewMouseLeftButtonDown(e);
        }
        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                //treeViewItem.IsSelected = true;
                if (e.ClickCount > 1) CustomDoubleClickMouse(e);
                else CustomMouseDownEvent(e);
                e.Handled = true;
            }
            base.OnPreviewMouseRightButtonDown(e);
        }
        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }
        void CustomMouseDownEvent(MouseButtonEventArgs e)
        {
            if (!IsMouseInSelectedHeader) return;
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {

            }
            else if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                if (previouseSelected != null)
                {
                    var displayitems = GetAllTreeViewItems();
                    int oldIndex = displayitems.IndexOf(previouseSelected);
                    int newIndex = displayitems.IndexOf(selectedNode);

                    int start = oldIndex < newIndex ? oldIndex : newIndex;
                    int end = oldIndex < newIndex ? newIndex : oldIndex;

                    if (end - start > 1)
                    {
                        for (int i = start + 1; i < end; i++)
                        {
                            AppendToSelection(displayitems[i]);
                        }
                    }
                }
            }
            else
            {
                foreach (var item in selectedNodes)
                {
                    item.Background = Brushes.White;
                }
                selectedNodes.Clear();
            }
            if (selectedNode != null)
            {
                selectedNode.Background = selectedNodes.Count == 0 ? Brushes.Orange : Brushes.SkyBlue;
                AppendToSelection(selectedNode);
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Delete && EnableDeleteNode)
            {
                var parent = ((ItemsControl)SelectedItem).Parent as ItemsControl;
                parent.Items.Remove(SelectedItem);
            }
            base.OnKeyDown(e);
        }
        
        #endregion
    }
}
