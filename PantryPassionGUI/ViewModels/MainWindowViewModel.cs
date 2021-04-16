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
            AddItemView addItem = new AddItemView();

            addItem.ShowDialog();
        }

        //FindItem
    }
}
