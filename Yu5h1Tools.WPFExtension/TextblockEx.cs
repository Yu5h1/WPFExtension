using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Yu5h1Tools.WPFExtension
{
    public static class TextblockEx
    {
        public static void HighLight(this TextBlock target,string text,Brush background,Brush foreground)
        {
            if (target.Text == string.Empty) return;

            string content = target.Text;
            target.Inlines.Clear();
            if (text != null && text != string.Empty) {
                int index = content.IndexOf(text, System.StringComparison.CurrentCultureIgnoreCase);
                while (index >= 0)
                {
                    target.Inlines.AddRange(new Inline[] {
                    new Run(content.Substring(0, index)),
                    new Run(content.Substring(index, text.Length)) {Background = background,Foreground = foreground}
                    });
                    content = content.Substring(index + text.Length);
                    index = content.IndexOf(text, System.StringComparison.CurrentCultureIgnoreCase);
                }
            }
            if (content.Length > 0) target.Inlines.Add(new Run(content));
        }
    }
}
