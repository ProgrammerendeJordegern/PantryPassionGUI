using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.ViewModels;

namespace PantryPassion.Test.Unit
{
    class AddItemViewModelTest
    {
        private AddItemViewModel _uut;
        private object _obj;

        [SetUp]
        public void Setup()
        {
            _uut = new AddItemViewModel();
            _obj = new object();
        }

        [Test]
        public void AddItemViewModel_UpArrowCommand_AddOneToAmount()
        {
            _uut.UpArrowCommand.Execute(_obj);
            Assert.That(_uut.InventoryItem.Amount,Is.EqualTo(1));
        }

        [Test]
        public void AddItemViewModel_DownArrowCommand_SubtractOneToAmount()
        {
            _uut.UpArrowCommand.Execute(_obj);
            _uut.UpArrowCommand.Execute(_obj);
            _uut.UpArrowCommand.Execute(_obj);

            _uut.DownArrowCommand.Execute(_obj);
            Assert.That(_uut.InventoryItem.Amount, Is.EqualTo(2));
        }

        [Test]
        public void AddItemViewModel_DownArrowCommand_CanNotExecute()
        {
            Assert.That(_uut.DownArrowCommand.CanExecute(_obj), Is.False);
        }

        [Test]
        public void AddItemViewModel_DownArrowCommand_CanExecute()
        {
            _uut.UpArrowCommand.Execute(_obj);
            Assert.That(_uut.DownArrowCommand.CanExecute(_obj), Is.True);
        }
    }
}
