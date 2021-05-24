using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using PantryPassionGUI.Models;
using PantryPassionGUI.Utilities;
using PantryPassionGUI.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace PantryPassionGUI.ViewModels
{
    public class EditItemsViewModel : BindableBase
    {
        private ICommand _findItemCommand;
        private BackendConnection _backendConnection = new BackendConnection();

        //Ok button
        public ICommand FindItemCommand
        {
            get
            {
                return _findItemCommand ??= new DelegateCommand(FindItemHandler);
            }
        }
        private void FindItemHandler()
        {
            FindItemView FIW = new FindItemView();
            if (FIW.ShowDialog() == true)
            {
                MessageBox.Show("FIW IS TRUE");
            }
            // FoundItem is found in the FindItemView and saved to globals
            TheItem = Globals.FoundItem;

        }

        private ICommand _saveItemCommand;

        //Ok button
        public ICommand SaveItemCommand
        {
            get
            {
                return _saveItemCommand ??= new DelegateCommand(SaveItemHandler);
            }
        }
        private async void SaveItemHandler()
        {
            try
            {
                Item ItemToSend = TheItem;
                //InventoryItem inventoryItemToSend = TheItem;
                await _backendConnection.EditItem(ItemToSend);
                //await _backendConnection.SetQuantity(inventoryItemToSend);
            }
            catch (ApiException e)
            {
                MessageBox.Show($"Fejl {e.StatusCode}", "Error!");
            }
            catch (HttpRequestException exception)
            {
                MessageBox.Show($"Der er ingen forbindele til serveren", "Error!");
            }
            finally
            {
                MessageBox.Show("Item Updated Successfully");
                TheItem = null;
            }

            Globals.FoundItem = null;
            
        }


        private Item theitem;

        public Item TheItem
        {
            get { return theitem; }
            set
            {
                SetProperty(ref theitem, value);
                RaisePropertyChanged("TheItem");
            }
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
