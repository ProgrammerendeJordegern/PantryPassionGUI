    using System.Timers;

namespace PantryPassionGUI.Models
{
    public class TimerClock : ITimer<Timer>
    {
        //Uses timer in System.Timers
        private Timer _timer;

        //Interval is in milliseconds [ms]
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

        //uses to get the timer so other classes can implement the event handler
        public Timer GetTimer()
        {
            return _timer;
        }
    }
}
