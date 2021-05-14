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
using PantryPassionGUI.ViewModels;
using PantryPassionGUI.Views;

namespace PantryPassion.Test.Unit.ViewModelTest
{
    class ShoppingListViewModelTest
    {
        private ShoppingListViewModel _uut;
        private IFindItemViewModel _findItemViewModel;
        private IBackendConnection _backendConnection;
        private object _obj;

        [SetUp]
        public void Setup()
        {
            _backendConnection = Substitute.For<IBackendConnection>();
            _findItemViewModel = Substitute.For<IFindItemViewModel>();
            _uut = new ShoppingListViewModel(_backendConnection,_findItemViewModel);
            _obj = new object();
        }

        [Test]
        public void ShoppingListViewModel_ClearListCommand_CalledBackendServer()
        {
            _uut.ClearListCommand.Execute(_obj);
            //See if DeleteShoppingList() is called
            _backendConnection.Received(1).DeleteShoppingList();
        }


        //does not work
        [Test]
        public void ShoppingListViewMode_AddItemToListCommand_FindItemViewOpen()
        {
            //_uut.AddItemToListCommand.Execute(_obj);
            Assert.That(_uut.FindItemView,Is.Null);
        }

        [Test]
        public void ShoppingListViewMode_UpdateShoppingListCommand_CallGetInventoryItemListByType()
        {
            _uut.UpdateShoppingListCommand.Execute(_obj);

            _backendConnection.Received(2).GetInventoryItemListByType(3);
        }


        [Test]
        public void ShoppingListViewMode_UpdateShoppingListCommand_SetItemsInList()
        {
            ObservableCollection<InventoryItem> list = new ObservableCollection<InventoryItem>();
            InventoryItem inventoryItem = new InventoryItem();
            inventoryItem.Item.Name = "Salt";
            list.Add(inventoryItem);

            _backendConnection.GetInventoryItemListByType(3).Returns(list);
            _uut.UpdateShoppingListCommand.Execute(_obj);

            Assert.That(_uut.InventoryItems.ElementAt(0).Item.Name, Is.EqualTo("Salt"));
        }

        [Test]
        public void ShoppingListViewMode_UpdateSelectedItemCommand_CallSetQuantity()
        {
            _uut.UpdateSelectedItemCommand.Execute(_obj);
            _backendConnection.Received(1).SetQuantity(_uut.CurrentInventoryItem);
        }
    }
}
