using NUnit.Framework;
using PantryPassionGUI.Models;

namespace PantryPassion.Test.Unit
{
    class SoundPlayerTest
    {
        private SoundPlayer _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new SoundPlayer();
        }

        [Test]
        public void Mute_SetDefult_False()
        {
            Assert.That(_uut.Mute, Is.False);
        }

        [Test]
        public void Mute_Set_True()
        {
            _uut.Mute = true;
            Assert.That(_uut.Mute,Is.True);
        }
    }
}
