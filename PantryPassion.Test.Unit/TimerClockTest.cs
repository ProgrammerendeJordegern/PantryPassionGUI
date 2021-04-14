using NUnit.Framework;
using PantryPassion.Models;

namespace PantryPassion.Test.Unit
{
    public class TimerClockTest
    {
        private TimerClock uut;

        [SetUp]
        public void Setup()
        {
            uut = new TimerClock(10);
        }

        [Test]
        public void Timer_Interval_CorrectValue()
        {
            Assert.That(uut.GetTimer().Interval, Is.EqualTo(10));
        }
    }
}