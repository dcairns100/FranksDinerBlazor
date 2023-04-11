using System.Timers;

namespace FranksDinerBlazor.Shared.Utilities
{
    public class FranksTimer
    {
        private System.Timers.Timer _timer = new System.Timers.Timer();

        public event EventHandler<ElapsedEventArgs>? TimerElapsed;

        public FranksTimer(double period)
           => SetTimer(period);

        private void SetTimer(double period)
        {
            _timer = new System.Timers.Timer(period);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimedEvent(object? source, ElapsedEventArgs e)
            => this.TimerElapsed?.Invoke(this, e);
    }
}
