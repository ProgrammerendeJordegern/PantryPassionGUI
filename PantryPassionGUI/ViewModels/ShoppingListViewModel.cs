using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PantryPassionGUI.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace PantryPassionGUI.ViewModels
{
    public class ShoppingListViewModel : BindableBase
    {
        private BackendConnection _backendConnection;
        private ICommand _autoGenerateListCommand;
        private ICommand _addItemToListCommand;
        private ICommand _clearListCommand;
        private ICommand _addItemsOnListToOwnedItemsCommand;
        private ICommand _updateListCommand;
        private ICommand _cancelCommand;

        public ShoppingListViewModel()
        {
            _backendConnection = new BackendConnection();
        }

        public ICommand AutoGenerateListCommand
        {
            get
            {
                return _autoGenerateListCommand ??= new DelegateCommand(AutoGenerateListHandler);
            }
        }

        private void AutoGenerateListHandler()
        {

        }

        public ICommand AddItemToListCommand
        {
            get
            {
                return _addItemToListCommand ??= new DelegateCommand(AddItemToListHandler);
            }
        }

        private void AddItemToListHandler()
        {

        }

        public ICommand ClearListCommand
        {
            get
            {
                return _clearListCommand ??= new DelegateCommand(ClearListHandler);
            }
        }

        private void ClearListHandler()
        {
            //Clear list in db

            //Clear list in view

        }

        public ICommand AddItemsOnListToOwnedItemsCommand
        {
            get
            {
                return _addItemsOnListToOwnedItemsCommand ??= new DelegateCommand(AddItemsOnListToOwnedItemsHandler);
            }
        }

        private void AddItemsOnListToOwnedItemsHandler()
        {

        }

        public ICommand UpdateListCommand
        {
            get
            {
                return _updateListCommand ??= new DelegateCommand(UpdateListHandler);
            }
        }

        private void UpdateListHandler()
        {
            _backendConnection.SetNewItem("Test", "Test", "Test");
            //Update to db

            Application.Current.Windows[Application.Current.Windows.Count - 2].Close();
        }

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ??= new DelegateCommand(CancelHandler);
            }
        }

        private void CancelHandler()
        {
            Application.Current.Windows[Application.Current.Windows.Count - 2].Close();
        }
    }
}
