using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PantryPassionGUI.Models;
using PantryPassionGUI.Views;
using Prism.Commands;


namespace PantryPassionGUI.ViewModels
{
    public class MainWindowViewModel
    {
        private ICommand _addItemCommand;
        private ICommand _removeItemCommand;
        private ICommand _shoppingListCommand;
        private ICommand _viewInventoriesCommand;

        public User CurrentUser { get; set; }
        public string CurrentUserFirstName { get; set; }

        public MainWindowViewModel()
        {
            CurrentUser = Globals.LoggedInUser;

            if (CurrentUser.FullName.Contains(" "))
            {
                string[] splitName = CurrentUser.FullName.Split(" ");
                CurrentUserFirstName = splitName[0];
            }
            else CurrentUserFirstName = CurrentUser.FullName;
        }

        public ICommand AddItemCommand
        {
            get { return _addItemCommand ??= new DelegateCommand(AddItemExecute); }
        }

        private void AddItemExecute()
        {
            AddItemView addItem = new AddItemView();

            addItem.ShowDialog();
        }
        public ICommand RemoveItemCommand
        {
            get { return _removeItemCommand ??= new DelegateCommand(RemoveItemExecute); }
        }

        private void RemoveItemExecute()
        {
            RemoveItemView removeItem = new RemoveItemView();

            removeItem.ShowDialog();
        }

        public ICommand ShoppingListCommand
        {
            get { return _shoppingListCommand ??= new DelegateCommand(ShoppingListExecute); }
        }

        private void ShoppingListExecute()
        {
            ShoppinglistView shoppinglist = new ShoppinglistView();
            shoppinglist.ShowDialog();
        }

        ICommand _findItemCommand;

        public ICommand FindItemCommand
        {
            get { return _findItemCommand ??= new DelegateCommand(FindItemExecute); }
        }

        public ICommand ViewInventoriesCommand
        {
            get { return _viewInventoriesCommand ??= new DelegateCommand(ViewInventoriesExecute); }
        }

        void ViewInventoriesExecute()
        {
            ViewInventories viewInventories = new ViewInventories();
            viewInventories.ShowDialog();
        }


        void FindItemExecute()
        {
            FindItemView FIWindow = new FindItemView();
            FIWindow.ShowDialog();
        }

        ICommand _editItemCommand;

        public ICommand EditItemCommand
        {
            get { return _editItemCommand ??= new DelegateCommand(EditItemExecute); }
        }

        void EditItemExecute()
        {
            EditItemsView EIW = new EditItemsView();
            EIW.ShowDialog();
        }
    }
}
