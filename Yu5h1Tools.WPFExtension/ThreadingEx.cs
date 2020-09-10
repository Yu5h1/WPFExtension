using System;
using System.Windows;
using System.Windows.Threading;

namespace Yu5h1Tools.WPFExtension
{
    public static class ThreadingEx
    {
        public static void DelayInvoke(this Window window, double seconds, Action action )
        {
            window.Dispatcher.BeginInvoke(new Action(action), TimeSpan.FromSeconds(seconds));
        }
    }
}
