using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.Utilities.Interfaces;
using PantryPassionGUI.ViewModels;

namespace PantryPassion.Test.Integration
{
    public class Step7
    {
        private IBackendConnection _backendConnection;
        private ViewInventoriesViewModel _sut;

        [SetUp]
        public void Setup()
        {
            _backendConnection = Substitute.For<IBackendConnection>();

            ObservableCollection<InventoryItem> list = new ObservableCollection<InventoryItem>();

            InventoryItem inventoryItem1 = new InventoryItem();
            inventoryItem1.InventoryType = 1;

            InventoryItem inventoryItem2 = new InventoryItem();
            inventoryItem2.InventoryType = 2;

            list.Add(inventoryItem1);
            list.Add(inventoryItem2);

            _backendConnection.GetInventory().Returns(list);
            _sut = new ViewInventoriesViewModel(_backendConnection);
        }

        [Test]
        public void ViewInventoriesViewModel_FridgeItems_CountOne()
        {
            Assert.That(_sut.FridgeItems.Count,Is.EqualTo(1));
        }


        [Test]
        public void ViewInventoriesViewModel_AllItems_CountTwo()
        {
            Assert.That(_sut.AllItems.Count, Is.EqualTo(2));
        }
    }
}
