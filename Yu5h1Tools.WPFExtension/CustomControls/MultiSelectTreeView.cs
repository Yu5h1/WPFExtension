using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace Yu5h1Tools.WPFExtension.CustomControls
{
    public class MultiSelectTreeView : TreeView
    {
        public TreeViewItem selectedNode { get { return (TreeViewItem)SelectedItem; } }
        public List<TreeViewItem> selectedNodes = new List<TreeViewItem>();

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            SelectedItemChanged += MultiSelectTreeView_SelectedItemChanged; ;

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
        public void SelectAllChildren(TreeViewItem node,bool visible = false) {
            SelectItems(GetAllChildren(node, visible));
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
        private void MultiSelectTreeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            
            if (Keyboard.IsKeyDown(Key.LeftCtrl)){

            } else if (Keyboard.IsKeyDown(Key.LeftShift)) {
                if (e.OldValue != null) {
                    if (e.OldValue != null) {

                        TreeViewItem oldItem = e.OldValue as TreeViewItem;
                        TreeViewItem newItem = e.NewValue as TreeViewItem;

                        var displayitems = GetAllTreeViewItems();
                        int oldIndex = displayitems.IndexOf(oldItem);
                        int newIndex = displayitems.IndexOf(newItem);

                        int start = oldIndex < newIndex ? oldIndex : newIndex;
                        int end = oldIndex < newIndex ? newIndex : oldIndex;

                        if (end - start > 1) {
                            for (int i = start + 1; i < end; i++)
                            {
                                AppendToSelection(displayitems[i]);
                            }
                        }
                        

                    }
                }

            } else {
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
    }
}
