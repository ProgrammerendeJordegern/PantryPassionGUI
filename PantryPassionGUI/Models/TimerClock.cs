    using System.Timers;

namespace PantryPassionGUI.Models
{
    public class TimerClock : ITimer<Timer>
    {
        private Timer _timer;
        public TimerClock(int interval)
        {
            _timer = new Timer();
            _timer.Interval = interval;
        }

        public void Enable()
        {
            _timer.Enabled = true;
        }

        public void Disable()
        {
            _timer.Enabled = false;
        }

        public Timer GetTimer()
        {
            return _timer;
        }
    }
}
