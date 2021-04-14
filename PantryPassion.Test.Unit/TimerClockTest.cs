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
    }
}