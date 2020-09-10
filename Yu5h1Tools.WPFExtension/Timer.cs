using System;
using System.Windows.Threading;

namespace Yu5h1Tools.WPFExtension
{
    public class Timer : DispatcherTimer
    {
        public double Duration;
        public DateTime previouseTime;
        public TimeSpan current => DateTime.Now - previouseTime;
        public bool IsCompleted => current.TotalSeconds > Duration;

        public Timer(double duration, Action<TimeSpan> updating, Action OnCompleted, double tickInterval = 0.1)
        {
            Duration = duration;
            Tick += (s, e) =>
            {
                Console.WriteLine("ticking..."+current.TotalSeconds.ToString());
                updating?.Invoke(current);
                if (current.TotalSeconds > Duration)
                {
                    Stop();
                    OnCompleted?.Invoke();
                }
            };
            Interval = TimeSpan.FromMilliseconds(tickInterval * 1000.0);
        }
        public Timer(double SecondsDuration,Action OnCompleted, double tickInterval = 0.1) :this(SecondsDuration,null,OnCompleted,tickInterval)
        {}
        public new void Start()
        {
            previouseTime = DateTime.Now;
            base.Start();
        }
    }
}
