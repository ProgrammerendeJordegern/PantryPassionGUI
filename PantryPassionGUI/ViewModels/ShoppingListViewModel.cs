using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PantryPassionGUI.Models;
using PantryPassionGUI.Utilities;
using PantryPassionGUI.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace PantryPassionGUI.ViewModels
{
    public class ShoppingListViewModel : BindableBase
    {
        private BackendConnection _backendConnection;
        private InventoryItem _currentItem;
        public FindItemViewModel FindItemViewModel { get; private set; }
        public SharedOberserverableCollectionOfInventoryItems SharedOberserverableCollection { get; private set; }
        
        private ICommand _addItemToListCommand;
        private ICommand _deleteItemInListCommand;
        private ICommand _clearListCommand;
        private ICommand _updateListCommand;

        public ShoppingListViewModel()
        {
            _backendConnection = new BackendConnection();
            FindItemViewModel = new FindItemViewModel();
            SharedOberserverableCollection = SharedOberserverableCollectionOfInventoryItems.Instance();
            SharedOberserverableCollection.UpdateShoppingList += UpdateHandler;
            SharedOberserverableCollection.SharedInventoryItems.Clear();
            _currentItem = new InventoryItem();
            GetShoppingList();
        }

        private void UpdateHandler(object sender, EventArgs e)
        {
            GetShoppingList();
        }

        public ICommand DeleteItemInListCommand
        {
            get
            {
                return _deleteItemInListCommand ??= new DelegateCommand(DeleteItemInListHandler);
            }
        }

        private void DeleteItemInListHandler()
        {
            Debug.WriteLine(CurrentInventoryItem.InventoryType);
            CurrentInventoryItem.Amount = 0;
            UpdateInventoryItemQuantity();
            SharedOberserverableCollection.SharedInventoryItems.Clear();
            GetShoppingList();
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
            //Brug find funktion fra finditemview
            FindItemView findItemView = new FindItemView();
            findItemView.ShowDialog();
        }

        public InventoryItem CurrentInventoryItem
        {
            get
            {
                return _currentItem;
            }
            set
            {
                SetProperty(ref _currentItem, value);
            }
        }

        public ICommand ClearListCommand
        {
            get
            {
                return _clearListCommand ??= new DelegateCommand(ClearListHandler);
            }
        }

        private async void ClearListHandler()
        {
            //Clear list in view

            SharedOberserverableCollection.SharedInventoryItems.Clear();

            //Clear list in db

            try
            {
                await _backendConnection.DeleteShoppingList();
            }
            catch (ApiException e)
            {
                MessageBox.Show($"Fejl {e.StatusCode}", "Error!");
            }
            catch (HttpRequestException exception)
            {
                MessageBox.Show($"Der er ingen forbindele til serveren", "Error!");
            }
        }

        public async void GetShoppingList()
        {
            var temp = new ObservableCollection<InventoryItem>();
            try
            {
                temp = await _backendConnection.GetInventoryItemListByType(3);
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
                foreach (var inventoryItem in temp)
                {
                    SharedOberserverableCollection.SharedInventoryItems.Add(inventoryItem);
                }
            }
        }

        public ICommand UpdateSelectedItemCommand
        {
            get
            {
                return _updateListCommand ??= new DelegateCommand(UpdateSelectedItemHandler);
            }
        }

        private void UpdateSelectedItemHandler()
        {
            //_backendConnection.SendInformationToBackendServer("Test", "Test", "Test");
            //Update to db
            UpdateInventoryItemQuantity();
        }

        private async void UpdateInventoryItemQuantity()
        {
            try
            {
                int temp = await _backendConnection.SetQuantity(CurrentInventoryItem);
                foreach (var inventoryItem in SharedOberserverableCollection.SharedInventoryItems)
                {
                    Debug.WriteLine($"InventoryItem: {inventoryItem}");
                }
            }
            catch (ApiException e)
            {
                MessageBox.Show($"Fejl {e.StatusCode}", "Error!");
            }
            catch (HttpRequestException exception)
            {
                MessageBox.Show($"Der er ingen forbindele til serveren", "Error!");
            }
        }
    }
}
