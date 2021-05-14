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
    class ShoppingListViewModelTest
    {
        private ShoppingListViewModel _uut;
        private object _obj;

        [SetUp]
        public void Setup()
        {
            _uut = new ShoppingListViewModel();
            _obj = new object();
        }

        [Test]
        public void ShoppingListViewModel_ClearListCommand_CalledBackendServer()
        {
            _uut.ClearListCommand.Execute(_obj);
            //See if DeleteShoppingList() is called
        }

        [Test]
        public void ShoppingListViewModel_UpdateSelectedItemCommand_CalledSetQuantity()
        {
            _uut.UpdateSelectedItemCommand.Execute(_obj);
            //see if SetQuantity() is called
        }

        [Test]
        public void ShoppingListViewModel_ClearListCommand_CalledDeleteShoppingList()
        {
            _uut.ClearListCommand.Execute(_obj);
            //see if DeleteShoppingList() is called
        }

        [Test]
        public void ShoppingListViewMode_AddItemToListCommand_FindItemViewOpen()
        {
            //make findItem view public get and private get
        }
    }
}
