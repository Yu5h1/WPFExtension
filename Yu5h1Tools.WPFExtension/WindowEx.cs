using System.Windows;

namespace Yu5h1Tools.WPFExtension
{
    public static class WindowEx
    {
        public static void FocusControlWhenMouseDown(this Window window, FrameworkElement control)
        {
            control.Focusable = true;
            window.MouseDown += (s, e) => control.Focus();
        }
    }
}
