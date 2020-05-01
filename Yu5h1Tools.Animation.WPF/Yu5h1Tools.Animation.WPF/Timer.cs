using System;
using System.Windows;
using System.Windows.Threading;

namespace Yu5h1Tools.Animation.WPF
{
    public struct TimerInfo
    {
        public double time { get; set; }
        public double totalTime { get; set; }
        public double normalized { get; set; }
        public int repeatCount { get; set; }
        public override string ToString()
        {
            return "time:" + time.ToString("0.00") + "\n" +
                    "totalTime:" + totalTime.ToString("0.00") + "\n" +
                    "normalized:" + normalized.ToString("0.00") + "\n" +
                    "repeatCount:" + repeatCount.ToString() + "\n";
        }
    }
    public static class Timer
    {
        public static DispatcherTimer Create(double duration, Action<TimerInfo> updating, int repeatCount = 1, double interval = 0.1)
        {
            double totalDuration = Math.Abs(repeatCount) * duration;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            DateTime previouseTime = DateTime.Now;
            dispatcherTimer.Tick += (s, e) => {
                TimeSpan currentTime = DateTime.Now - previouseTime;

                double progress = currentTime.TotalSeconds % duration;

                updating(new TimerInfo()
                {
                    time = progress,
                    totalTime = currentTime.TotalSeconds,
                    normalized = progress / duration,
                    repeatCount = (int)(currentTime.TotalSeconds / duration)
                });

                if (repeatCount > 0 && currentTime.TotalSeconds > totalDuration)
                {
                    dispatcherTimer.Stop();
                }
            };
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(interval * 1000.0);
            dispatcherTimer.Start();
            return dispatcherTimer;
        }
        public static DispatcherTimer Move(FrameworkElement target, double duration, Point from,Point to
            , Action<TimerInfo> updating = null, int repeatCount = 1, double interval = 0.03) {
            return Create(duration, (info) => {
                Point p = MathE.Lerp(from, to, info.normalized);
                Thickness t = target.Margin;
                target.Margin = new Thickness(p.X,p.Y,t.Right,t.Bottom);
                if (updating != null) updating(info);
            },repeatCount,interval);
        }
    }
}
