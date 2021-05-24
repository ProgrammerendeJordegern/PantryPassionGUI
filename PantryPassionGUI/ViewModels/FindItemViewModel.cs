using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using PantryPassionGUI.Models;
using PantryPassionGUI.Utilities;
using PantryPassionGUI.Utilities.Interfaces;
using PantryPassionGUI.ViewModels.Interfaces;
using PantryPassionGUI.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace PantryPassionGUI.ViewModels
{

    public class FindItemViewModel : BindableBase, IFindItemViewModel
    {
        private ICommand _okCommand;
        private ICommand _addToShoppingCommand;
        private ICommand _scanEANCommand;
        private ICollectionView ViewFilter;

        private string _namefilter;
        private string _eanfilter;
        private int _currentIndex = -1;

        private Item _currentItem = null;
        private IBackendConnection _backendConnection;
        private ObservableCollection<Item> _items;
        public ICameraViewModel CameraViewModel { get; private set; }

        public FindItemViewModel()
        {
            Items = new ObservableCollection<Item>();


            _backendConnection = new BackendConnection();

            GetInventoryForFindItem();

            //Camera
            CameraViewModel = new CameraViewModel();

        }

        public FindItemViewModel(IBackendConnection backendConnection, ICameraViewModel cameraViewModel)
        {
            Items = new ObservableCollection<Item>();


            _backendConnection = backendConnection;
            CameraViewModel = cameraViewModel;
            GetInventoryForFindItem();
        }

        private bool UserFilter(object item)
        {
            if (!(String.IsNullOrEmpty(EANFilter)))
            {
                if ((item as Item).Ean != null)
                {
                    return ((item as Item).Ean.IndexOf(EANFilter, StringComparison.OrdinalIgnoreCase) >= 0);
                }
                else
                {
                    return false;
                }
            }
            else if (!(String.IsNullOrEmpty(NameFilter)))
            {
                return ((item as Item).Name.IndexOf(NameFilter, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            
            return true;
        }


        public string NameFilter
        {
            get { return _namefilter; }
            set
            {
                if (value != _namefilter)
                {
                    _namefilter = value;
                    ViewFilter.Refresh();
                    RaisePropertyChanged("NameFilter");
                }

            }
        }

        public string EANFilter
        {
            get { return _eanfilter; }
            set
            {
                if (value != _eanfilter)
                {
                    _eanfilter = value;
                    ViewFilter.Refresh();
                    RaisePropertyChanged("EANFilter");
                }

            }
        }

        public ObservableCollection<Item> Items
        {
            get
            {
                return _items;
            }
            set
            {
                SetProperty(ref _items, value);
            }
        }

        public ICommand ScanEANCommand
        {
            get { return _scanEANCommand ?? (_scanEANCommand = new DelegateCommand(ScanEANExecute)); }
        }

        void ScanEANExecute()
        {
            ScanEANWindow scanEanWindow = new ScanEANWindow(this);
            scanEanWindow.ShowDialog();
        }

        //Ok button
        public ICommand OkCommand
        {
            get
            {
                return _okCommand ??= new DelegateCommand(OkHandler);
            }
        }
        private async void OkHandler()
        {
            Globals.FoundItem = Items.ElementAt(CurrentIndex);
        }

        //AddToShoppingList button
        public ICommand AddToShoppingListCommand
        {
            get
            {
                return _addToShoppingCommand ??= new DelegateCommand(AddToShoppingListHandler);
            }
        }
        private async void AddToShoppingListHandler()
        {

            InventoryItem inventoryItemToShoppingList = new InventoryItem();

            inventoryItemToShoppingList.Item = Items.ElementAt(CurrentIndex);
            inventoryItemToShoppingList.Amount = 1;
            inventoryItemToShoppingList.InventoryType = 3;

            try
            {
                await _backendConnection.SetNewItem(inventoryItemToShoppingList, true);
            }
            catch (ApiException e)
            {
                MessageBox.Show($"Fejl {e.StatusCode}", "Error!");
            }
            catch (HttpRequestException exception)
            {
                MessageBox.Show($"Der er ingen forbindele til serveren", "Error!");
            }

            CameraViewModel.Camera.CameraOff();
        }

        public Item CurrentItem
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

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set
            {
                SetProperty(ref _currentIndex, value);
            }
        }

        private async void GetInventoryForFindItem()
        {
            Items = await _backendConnection.GetListOfItems();
            ViewFilter = (CollectionView)CollectionViewSource.GetDefaultView(Items);
            ViewFilter.Filter = UserFilter;
        }
    }
}
