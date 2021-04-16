using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PantryPassionGUI.Models;
using Prism.Commands;


namespace PantryPassionGUI.ViewModels
{
    class MainWindowViewModel
    {
        public tempUser t1
        {
            get;
            set;
        }
        
        public MainWindowViewModel()
        {
            t1 = new tempUser("Jesper");
        }

        ICommand _addItemCommand;

        public ICommand AddItemCommand
        {
            get { return _addItemCommand ?? (_addItemCommand = new DelegateCommand(AddItemExecute)); }
        }

        void AddItemExecute()
        {
            MessageBox.Show("This is where u add an item...");
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
