using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PantryPassionGUI.Models;
using PantryPassionGUI.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace PantryPassionGUI.ViewModels
{
    public class EditItemsViewModel : BindableBase
    {
        private Item theitem;

        public Item theItem
        {
            get { return theitem; }
            set
            {
                SetProperty(ref theitem, value);
            }
        }

        public EditItemsViewModel()
        {
            theItem = new Item("");
        }

        //ICommand _findItemCommand;

        //public ICommand FindItemCommand
        //{
        //    get { return _findItemCommand ?? (_findItemCommand = new DelegateCommand(FindItemExecute)); }
        //}

        //void FindItemExecute()
        //{
        //    FindItemWindow FIW1 = new FindItemWindow();
        //    FIW1.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //    if (FIW1.ShowDialog() == true)
        //    {
        //        theItem = (FIW1.FindItemDataGrid.SelectedItem as Item);
        //    }
        //}


    }
}
