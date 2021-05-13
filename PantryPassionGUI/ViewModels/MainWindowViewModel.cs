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

        public tempUser T1
        {
            get;
            set;
        }

        public User CurrentUser { get; set; }
        public string CurrentUserFirstName { get; set; }

        public MainWindowViewModel()
        {
            T1 = new tempUser("Jesper");
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

        void FindItemExecute()
        {
            FindItemWindow FIWindow = new FindItemWindow();
            //FIWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //if (FIWindow.ShowDialog() == true)
            //{
            //    // Kan returnere det fundne item, msgbox er bare for debug
            //    MessageBox.Show((FIWindow.FindItemDataGrid.SelectedItem as Item).Name);
            //}
        }
    }
}
