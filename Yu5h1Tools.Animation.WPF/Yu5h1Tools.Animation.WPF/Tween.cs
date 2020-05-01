using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Yu5h1Tools.Animation.WPF
{
    public static class Tween
    {        
        public static PowerEase defaultEaseFunction = null;
        static PowerEase checkEase(PowerEase easeFunction) {
            if (easeFunction == null)
            {
                easeFunction = defaultEaseFunction;
            }
            return easeFunction;
        }
        public static T Animate<T>(T timeline, double duration, bool autoReverse,
            int repeatCount, EventHandler updateing, EventHandler Completed) where T : AnimationTimeline
        {
            timeline.Duration = new Duration(TimeSpan.FromMilliseconds(duration * 1000.0));
            timeline.AutoReverse = autoReverse;
            if (repeatCount == 0)
                timeline.RepeatBehavior = RepeatBehavior.Forever;
            else
                timeline.RepeatBehavior = new RepeatBehavior(TimeSpan.FromMilliseconds(duration * repeatCount * 1000.0));
            if (updateing != null) timeline.CurrentTimeInvalidated += updateing;
            if (Completed != null) timeline.Completed += Completed;
            return timeline;
        }
        public static DoubleAnimation Animate(double duration, double from, double to, int repeatCount, EventHandler updating = null, EventHandler Completed = null
            , PowerEase easeFunction = null, bool autoReverse = false)
        {
            var result = new DoubleAnimation
            {
                From = new double?(double.IsNaN(from) ? 0.0 : from),
                To = new double?(double.IsNaN(to) ? 0.0 : to),
                EasingFunction = checkEase(easeFunction)
            };
            return Animate<DoubleAnimation>(result, duration, autoReverse, repeatCount, updating, Completed);
        }
        public static ThicknessAnimation Animate(double duration, Thickness from, Thickness to,
            int repeatCount = 1, EventHandler updating = null, EventHandler Completed = null, bool autoReverse = false, PowerEase easeFunction = null)
        {
            var result = new ThicknessAnimation
            {
                From = from,
                To = to,
                EasingFunction = checkEase(easeFunction)
            };
            return Animate<ThicknessAnimation>(result, duration, autoReverse, repeatCount, updating, Completed);
        }
        public static RectAnimation Animate(double duration, Rect from, Rect to,
            int repeatCount = 1, EventHandler updating = null, EventHandler Completed = null, bool autoReverse = false, PowerEase easeFunction = null)
        {
            var result = new RectAnimation
            {
                From = new Rect?(from),
                To = new Rect?(to),
                EasingFunction = checkEase(easeFunction)
            };
            return Animate<RectAnimation>(result, duration, autoReverse, repeatCount, updating, Completed);
        }

        public static AnimationTimeline[] scale(FrameworkElement target, double duration, Size size, EventHandler updating = null, EventHandler Completed = null, PowerEase easeFunction = null, bool autoReverse = false)
        {
            AnimationTimeline[] results = new AnimationTimeline[2];
            bool flag = target.Width != size.Width;
            if (flag)
            {
                results[0] = Animate(duration, target.Width, size.Width, 0, updating, Completed, easeFunction, autoReverse);
                target.BeginAnimation(FrameworkElement.WidthProperty, results[0]);
            }
            bool flag2 = target.Height != size.Width;
            if (flag2)
            {
                results[1] = Animate(duration, target.Height, size.Height, 0, updating, Completed, easeFunction, autoReverse);
                target.BeginAnimation(FrameworkElement.HeightProperty, results[1]);
            }
            return results;
        }
        public static AnimationTimeline[] scale(FrameworkElement target, double duration, double width, double height, EventHandler updating = null, EventHandler Completed = null, PowerEase easeFunction = null, bool autoReverse = false)
        { return scale(target, duration, new Size(width, height), updating, Completed, easeFunction, autoReverse); }
        public static void Move(FrameworkElement target, double duration, Point point,
            int repeatCount = 1, EventHandler updating = null, EventHandler Completed = null, PowerEase easeFunction = null, bool autoReverse = false)
        {
            target.BeginAnimation(FrameworkElement.MarginProperty,
                Animate(duration, target.Margin, new Thickness(point.X, point.Y, target.Margin.Right, target.Margin.Bottom),
                repeatCount, updating, Completed, autoReverse, easeFunction));
        }
        public static void Scale(RectangleGeometry target, double duration, Rect rect,
            int repeatCount = 1, EventHandler updating = null, EventHandler Completed = null, PowerEase easeFunction = null, bool autoReverse = false)
        {
            target.BeginAnimation(RectangleGeometry.RectProperty,
                Animate(duration, target.Rect, rect,
                repeatCount, updating, Completed, autoReverse, easeFunction));
        }

        public static void Fade(UIElement target, double duration, double opacity,
            int repeatCount = 1, EventHandler updating = null, EventHandler Completed = null, PowerEase easeFunction = null, bool autoReverse = false)
        {
            target.BeginAnimation(UIElement.OpacityProperty, Animate(duration, target.Opacity, opacity, repeatCount, updating, Completed, easeFunction, autoReverse));
        }
        public static void Content(ContentControl target, double duration, string text,
            int repeatCount = 1, EventHandler updating = null, EventHandler Completed = null, PowerEase easeFunction = null, bool autoReverse = false)
        {
            //contentani
            Storyboard storyboard = new Storyboard();
            
            //target.BeginAnimation(ContentControl.ContentProperty,
            //    Animate<AnimationTimeline>());
        }

        public static void StopAnimation(this FrameworkElement target, DependencyProperty property)
        {
            target.BeginAnimation(property, null);
        }

        public static void PauseAnimation(this FrameworkElement target)
        {
        }   
    }

}
