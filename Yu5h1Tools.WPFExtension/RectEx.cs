using System;
using System.Windows;
using System.Windows.Input;

namespace Yu5h1Tools.WPFExtension
{
    public static class RectEx
    {
        /// <summary></summary>
        /// <param name="r">Rect</param>
        /// <returns></returns>
        public static Anchor2D ShowBorderSizeIcon(this Rect r, FrameworkElement target, Point point,Cursor innerIcon = null)
        {
            double t = 3;
            if (innerIcon == null) innerIcon = Cursors.Arrow;
            Anchor2D result = Anchor2D.none;
            Rect inner = new Rect(
                    r.Width > (t + t) ? r.X + t : r.X , r.Height > (t+t) ? r.Y+t:r.Y,
                    r.Width > (t + t) ? r.Width - (t + t) : r.Width, r.Height > (t + t) ? r.Height - (t + t) : r.Height
                );
            Rect outer = new Rect(r.X - t, r.Y - t, r.Width + (t + t), r.Height + (t + t));

            if (outer.Contains(point)) {
                if (inner.Contains(point)) target.Cursor = innerIcon;
                else {
                    bool OnLeft = point.X < inner.X;
                    bool OnRight = point.X > inner.X + inner.Width;
                    bool OnTop = point.Y < inner.Y;
                    bool OnBottom = point.Y > inner.Y + inner.Height;
                    if (OnLeft || OnRight)
                    {
                        if (OnTop || OnBottom)
                        {
                            if ((OnLeft && OnTop) || (OnRight && OnBottom))
                            {
                                if (OnTop) result = Anchor2D.TopLeft;
                                else result = Anchor2D.BottomRight;
                                target.Cursor = Cursors.SizeNWSE;
                            }
                            else
                            {
                                if (OnTop) result = Anchor2D.TopRight;
                                else result = Anchor2D.BottomLeft;
                                target.Cursor = Cursors.SizeNESW;
                            }
                        }
                        else
                        {
                            if (OnLeft) result = Anchor2D.Left;
                            else result = Anchor2D.Right;
                            target.Cursor = Cursors.SizeWE;
                        }
                    }
                    else
                    {
                        if (OnTop) result = Anchor2D.Top;
                        else result = Anchor2D.Bottom;
                        target.Cursor = Cursors.SizeNS;
                    }
                }
            } else target.Cursor = Cursors.Arrow;
            return result;
        }
        public static Rect DragRectangle(this Point from,Point to,bool square = false) {
            double x = to.X > from.X ? from.X : to.X;
            double y = to.Y > from.Y ? from.Y : to.Y;
            double w = Math.Abs(from.X - to.X);
            double h = Math.Abs(from.Y - to.Y);
            if (square) {
                if (w > h)
                {
                    w = h;
                    x = to.X > from.X ? from.X : from.X - w;
                }
                else if (h > w) {
                    h = w;
                    y = to.Y > from.Y ? from.Y : from.Y - h;
                } 
            }
            return new Rect( x, y, w, h);
        }
        public static Rect Add(this Rect r, Rect v)
        {
            return new Rect(r.X+v.X,r.Y+v.Y,r.Width+v.Width,r.Height+v.Height);
        }
        public static Rect Move(this Rect r, Point v)
        {
            return new Rect(r.X + v.X, r.Y + v.Y, r.Width , r.Height );
        }
        public static string ToString(this Rect r,string format)
        {
            return r.Location.ToString(format) + "\n" + r.Size.ToString(format);
        }
        public static Rect MaxSquare(this Rect rect)
        {
            Size size = rect.Size;
            if (rect.Width > rect.Height)
            {
                rect.Y = -((rect.Width / 2) - (size.Height / 2));
                rect.Height = rect.Width;

            }
            else
            {
                rect.X = -((rect.Height / 2) - (size.Width / 2));
                rect.Width = rect.Height;
            }
            return rect;
        }

    }
}
