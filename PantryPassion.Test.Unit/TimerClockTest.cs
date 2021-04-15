using NUnit.Framework;
using PantryPassionGUI.Models;

namespace PantryPassion.Test.Unit
{
    public class TimerClockTest
    {
        private TimerClock _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new TimerClock(10);
        }

        [Test]
        public void Timer_Interval_CorrectValue()
        {
            Assert.That(_uut.GetTimer().Interval, Is.EqualTo(10));
        }

        [Test]
        public void Timer_Enabled_ValueIsTrue()
        {
            _uut.Enable();
            Assert.That(_uut.GetTimer().Enabled, Is.True);
        }

        [Test]
        public void Timer_Enabled_ValueIsFalse()
        {
            _uut.Disable();
            Assert.That(_uut.GetTimer().Enabled, Is.False);
        }
    }
}