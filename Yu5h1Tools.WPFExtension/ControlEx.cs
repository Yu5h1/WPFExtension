using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Yu5h1Tools.WPFExtension
{
    public static class ControlEx
    {
        public static Typeface GetTypeface(this Control control)
        {
            return new Typeface(control.FontFamily, control.FontStyle, control.FontWeight, control.FontStretch);
        }

        public static Size GetTextSize(this ContentControl contentControl)
        {
            FormattedText t = new FormattedText(
                contentControl.Content.ToString(), CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight, contentControl.GetTypeface(),
                contentControl.FontSize, contentControl.Foreground,new NumberSubstitution(),
                TextFormattingMode.Display);
            return new Size(t.Width,t.Height);
        }
        public static void UseTextSize(this ContentControl contentControl) {
            Size size = contentControl.GetTextSize();
            contentControl.Width = size.Width;
            //contentControl.Height = size.Height;
        }
    }
}
