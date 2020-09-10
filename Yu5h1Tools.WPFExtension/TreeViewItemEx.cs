using System.Windows.Controls;

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
        public static TreeViewItem AddTextBlockTreeItem(this ItemsControl control,string header)
        {
            var result = new TreeViewItem().SetTextBlockHeader(header);
            control.Items.Add(result);
            return result;
        } 
    }
}
