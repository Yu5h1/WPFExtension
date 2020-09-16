using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;
using Yu5h1Tools.WPFExtension;
using static InformationViewer;


namespace Yu5h1Tools.WPFExtension.CustomControls
{
    public class MultiSelectTreeView : TreeView
    {
        public List<object> DeleteIgnoreList = new List<object>();
        [Category("Custom Properties")]
        public bool AllowDeleteNode { get; set; }
        //public event SelectionChangedEventHandler SelectionChanged
        //{
        //    add { SelectionChanged += value; }
        //    remove { SelectionChanged -= value; }
        //}

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
                if (selectedNode == null) return true;
                if (selectedNode.Header.GetType() != typeof(FrameworkElement)) return true;
                return ((FrameworkElement)selectedNode.Header).GetBounds(this).Contains(Mouse.GetPosition(this));
            }
        }        
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }
        void AppendToSelection(TreeViewItem item) {
            if (item == null || selectedNodes.Contains(item)) return;
            //item.Foreground = Brushes.DarkGray;            
            item.Background = selectedNodes.Count == 0 ? Brushes.Orange : SystemColors.HighlightBrush;
            selectedNodes.Add(item);
        }
        void AppendToSelection(object item) {
            if (item.GetType() != typeof(TreeViewItem)) return;
            AppendToSelection((TreeViewItem)item);
        }
        void DeSelect(TreeViewItem item)
        {
            item.Background = Brushes.White;
            item.IsSelected = false;
            selectedNodes.Remove(item);
        }
        public List<TreeViewItem> GetAllTreeViewItems(bool IncludeUnExpanded = false) {
            var result = new List<TreeViewItem>();
            foreach (var item in Items)
                result.AddRange(GetAllChildren((TreeViewItem)item, IncludeUnExpanded));
            return result;
        }
        public List<TreeViewItem> GetAllChildren(TreeViewItem node, bool IncludeUnExpanded = false ,bool containSelf = true)
        {
            List<TreeViewItem> result = new List<TreeViewItem>();
            if (containSelf) result.Add(node);
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
 
        public void ClearSelection()
        {
            foreach (var item in selectedNodes) {
                //item.Foreground = SystemColors.ControlTextBrush;
                item.Background = Brushes.White;
            } 
            selectedNodes.Clear();
            previouseSelected = null;
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
            if (filter == null || filter == string.Empty) {
                foreach (var item in items) {
                    getName(item);
                    item.Visibility = Visibility.Visible;
                }
                return;
            }

            List<TreeViewItem> visibleItems = new List<TreeViewItem>();
            foreach (var item in items)
            {
                if (getName(item).Match(filter, StringComparison.Ordinal)) visibleItems.Add(item);
                else {
                    item.IsExpanded = false;
                    item.Visibility = Visibility.Collapsed;
                }
                
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
            if (e.ClickCount > 1 && selectedNode != null)
            {
                var nodeChildren = GetAllChildren(selectedNode,true,!Key.LeftShift.IsPressed());
                if (nodeChildren.Count > 0) {
                    if (e.ChangedButton == MouseButton.Left)
                    {
                        if (Key.LeftCtrl.IsPressed())
                        {
                            SelectAllChildren(selectedNode, true);
                        }
                        else
                        {
                            ClearSelection();
                            SetTreeViewItemsIsExpended(nodeChildren, !nodeChildren[0].IsExpanded);
                        }
                        
                    }
                }
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
                    e.Handled = true;
                }
            }
            base.OnPreviewMouseLeftButtonDown(e);
        }
        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);
            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = true;
                CustomMouseDownEvent(e);
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

            if (!IsMouseInSelectedHeader) {
                previouseSelected?.Focus();
                return;
            }
            
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {                
                AppendToSelection(previouseSelected);
            }
            else if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                if (previouseSelected != null)
                {
                    AppendToSelection(previouseSelected);
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
            else ClearSelection();
            AppendToSelection(selectedNode);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Delete && AllowDeleteNode)
            {
                var nodes = new List<TreeViewItem>(selectedNodes);
                foreach (var item in nodes)
                {
                    if (!DeleteIgnoreList.Contains(item)) {
                        var parent = (ItemsControl)item.Parent;
                        parent.Items.Remove(item);
                    }
                }
                ClearSelection();
            }
            
            base.OnKeyDown(e);
        }

        #endregion
    }
}
