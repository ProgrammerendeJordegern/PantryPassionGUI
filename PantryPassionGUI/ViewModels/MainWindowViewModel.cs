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
    class MainWindowViewModel
    {
        ICommand _addItemCommand;
        ICommand _removeItemCommand;

        public tempUser T1
        {
            get;
            set;
        }
        
        public MainWindowViewModel()
        {
            T1 = new tempUser("Jesper");
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

        ICommand _findItemCommand;

        public ICommand FindItemCommand
        {
            get { return _findItemCommand ?? (_findItemCommand = new DelegateCommand(FindItemExecute)); }
        }

        void FindItemExecute()
        {
            FindItemWindow FIWindow = new FindItemWindow();
            FIWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (FIWindow.ShowDialog() == true)
            {
                // Kan returnere det fundne item, msgbox er bare for debug
                MessageBox.Show((FIWindow.FindItemDataGrid.SelectedItem as Item).Name);
            }
        }
        //FindItem
    }
}
