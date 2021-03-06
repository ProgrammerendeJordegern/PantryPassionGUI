using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.Utilities;
using PantryPassionGUI.Utilities.Interfaces;
using PantryPassionGUI.ViewModels;
using PantryPassionGUI.ViewModels.Interfaces;

namespace PantryPassion.Test.Unit.ViewModelTest
{
    [TestFixture]
    class FindItemViewModelTest
    {
        private FindItemViewModel _uut;
        private ICameraViewModel _cameraViewModel;
        private IBackendConnection _backendConnection;

        [SetUp]
        public void Setup()
        {
            _cameraViewModel = Substitute.For<ICameraViewModel>();
            _backendConnection = Substitute.For<IBackendConnection>();

            ObservableCollection<Item> list = new ObservableCollection<Item>();
            Item item = new Item();
            item.Name = "mælk";
            item.Ean = "123";
            list.Add(item);

            _backendConnection.GetListOfItems().Returns(list);

            _uut = new FindItemViewModel(_backendConnection, _cameraViewModel);
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

        //[Test]
        //public void FindItemViewModel_AddsItemCorrectly()
        //{

        //    _backendConnection.GetInventoryItemListByType(2).Returns(new ObservableCollection<InventoryItem>());
        //    var currentItemCount = _uut.Items.Count;
        //    InventoryItem inventoryItem = new InventoryItem();
        //    inventoryItem.Item = new Item("Test Item", "9988776655", 99, 98);
        //    _uut.Items.Add(inventoryItem);
        //    Assert.That(_uut.Items.Count, Is.EqualTo(currentItemCount + 1));
        //    Assert.That(_uut.Items.Last().Item.Name, Is.EqualTo("Test Item"));
        //    Assert.That(_uut.Items.Last().Item.Ean, Is.EqualTo("9988776655"));
        //    Assert.That(_uut.Items.Last().Item.AverageLifespanDays, Is.EqualTo(99));
        //    Assert.That(_uut.Items.Last().Item.Size, Is.EqualTo(98));
        //}
    }
}
