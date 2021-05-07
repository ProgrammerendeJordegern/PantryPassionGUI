using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.ViewModels;

namespace PantryPassion.Test.Unit.ViewModelTest
{
    [TestFixture]
    class FindItemViewModelTest
    {
        private FindItemViewModel _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new FindItemViewModel();
        }

        [Test]
        public void FindItemViewModel_NameFilter_SetsCorrect()
        {
            _uut.NameFilter = "ll";
            Assert.That(_uut.NameFilter, Is.EqualTo("ll"));
        }

        [Test]
        public void FindItemViewModel_EANFilter_SetsCorrect()
        {
            _uut.EANFilter = "1235";
            Assert.That(_uut.EANFilter, Is.EqualTo("1235"));
        }

        [Test]
        public void FindItemViewModel_ScanEANCommand_CanExecute()
        {
            Assert.That(_uut.ScanEANCommand.CanExecute(null), Is.True);
        }

        [Test]
        public void FindItemViewModel_AddsItemCorrectly()
        {
            var currentItemCount = _uut.Items.Count;
            Item testItem = new Item("Test Item", "9988776655", 99, 98);
            _uut.Items.Add(testItem);
            Assert.That(_uut.Items.Count, Is.EqualTo(currentItemCount + 1));
            Assert.That(_uut.Items.Last().Name, Is.EqualTo("Test Item"));
            Assert.That(_uut.Items.Last().Ean, Is.EqualTo("9988776655"));
            Assert.That(_uut.Items.Last().AverageLifespanDays, Is.EqualTo(99));
            Assert.That(_uut.Items.Last().Size, Is.EqualTo(98));
        }
    }
}
