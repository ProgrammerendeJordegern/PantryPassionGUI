using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.Operations;

namespace PantryPassion.Test.Unit
{
    class ApiOperationsTest
    {
        private ApiOperations _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new ApiOperations();
        }

        [Test]
        public void Api_CheckAdmin_AdminIsFalse()
        {
           Assert.That(_uut.CheckIfAdmin("Ad", "Ad"),Is.EqualTo(false));
        }

        [Test]
        public void Api_CheckAdmin_PaswwordIsWrongAdminIsFalse()
        {
            Assert.That(_uut.CheckIfAdmin("Admin", "Ad"), Is.EqualTo(false));
        }
        [Test]
        public void Api_CheckAdmin_UsernameIsWrongAdminIsFalse()
        {
            Assert.That(_uut.CheckIfAdmin("Ad", "Admin"), Is.EqualTo(false));
        }
        [Test]
        public void Api_CheckAdmin_AdminIsCorrect()
        {
            Assert.That(_uut.CheckIfAdmin("Admin", "Admin"), Is.EqualTo(true));
        }
    }
}
