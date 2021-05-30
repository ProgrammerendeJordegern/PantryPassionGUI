using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using NSubstitute;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.Utilities.Interfaces;
using PantryPassionGUI.ViewModels;

namespace PantryPassion.Test.Integration
{
    public class Step9
    {
        private MainWindowViewModel _sut;

        [SetUp]
        public void Setup()
        {
            User user = new User();
            user.FullName = "Bente Hansen";

            Globals.LoggedInUser = user;

            _sut = new MainWindowViewModel();
        }

        [Test]
        public void MainWindowViewModel_CurrentUserFirstName_correct()
        {
            Assert.That(_sut.CurrentUserFirstName, Is.EqualTo("Bente"));
        }
    }
}
