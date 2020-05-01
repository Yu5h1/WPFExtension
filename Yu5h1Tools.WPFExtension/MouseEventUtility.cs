using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Yu5h1Tools.WPFExtension
{
    public static class MouseEventUtility
    {
        public static Point previouseDragPoint;
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="DragStart">boolean confirm start</param>
        /// <param name="Draging">Vector draged distance.</param>
        /// <param name="DragEnd"></param>
        /// <param name="RMBDown">boolean IsCancel</param>
        public static void AddMouseDragEvent<T>(T target,
            Func<bool> DragStart, Action<Vector> Draging, Action DragEnd, Action<bool> RMBDown)
            where T : UIElement
        {
            target.MouseLeftButtonDown += (s,e) => {
                var sender = (UIElement)s;
                previouseDragPoint = e.GetPosition(sender);
                if (DragStart()) {
                    sender.CaptureMouse();
                }
                
            };
            target.MouseMove += (s, e) => {
                var sender = (UIElement)s;
                if (sender.IsMouseCaptured)
                {
                    var v = e.GetPosition(sender) - previouseDragPoint ;
                    Draging(v);
                }
            };
            target.MouseLeftButtonUp += (s, e) => {
                var sender = (UIElement)s;
                if (sender.IsMouseCaptured)
                {
                    DragEnd();
                    sender.ReleaseMouseCapture();
                }
            };
            //Cancel Drag
            target.MouseRightButtonDown += (s, e) => {
                var sender = (UIElement)s;
                RMBDown(sender.IsMouseCaptured);
                if (sender.IsMouseCaptured)
                {
                    sender.ReleaseMouseCapture();
                }
            };
        }
    }
}
