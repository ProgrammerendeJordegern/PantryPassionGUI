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
using PantryPassionGUI.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace PantryPassionGUI.ViewModels
{

    public class FindItemViewModel : BindableBase, IFindItemViewModel
    {
        private ICommand _okCommand;
        private ICommand _scanEANCommand;
        private ICollectionView ViewFilter;

        private string _namefilter;
        private string _eanfilter;
        private int _currentIndex = -1;

        private InventoryItem _currentItem = null;
        private IBackendConnection _backendConnection;
        private ObservableCollection<InventoryItem> _inventoryItems;
        public ICameraViewModel CameraViewModel { get; private set; }

        public FindItemViewModel()
        {
            InventoryItems = new ObservableCollection<InventoryItem>();

            ViewFilter = (CollectionView)CollectionViewSource.GetDefaultView(InventoryItems);
            ViewFilter.Filter = UserFilter;
            _backendConnection = new BackendConnection();

            GetInventoryForFindItem();

            //Camera
            CameraViewModel = new CameraViewModel();

        }

        public FindItemViewModel(IBackendConnection backendConnection, ICameraViewModel cameraViewModel)
        {
            InventoryItems = new ObservableCollection<InventoryItem>();

            ViewFilter = (CollectionView)CollectionViewSource.GetDefaultView(InventoryItems);
            ViewFilter.Filter = UserFilter;
            _backendConnection = backendConnection;
            CameraViewModel = cameraViewModel;
            GetInventoryForFindItem();
        }

        private bool UserFilter(object item)
        {
            if (!(String.IsNullOrEmpty(EANFilter)))
                return ((item as InventoryItem).Item.Ean.IndexOf(EANFilter, StringComparison.OrdinalIgnoreCase) >= 0);
            else if (!(String.IsNullOrEmpty(NameFilter)))
                return ((item as InventoryItem).Item.Name.IndexOf(NameFilter, StringComparison.OrdinalIgnoreCase) >= 0);
            else return true;
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

        public ObservableCollection<InventoryItem> InventoryItems
        {
            get
            {
                return _inventoryItems;
            }
            set
            {
                SetProperty(ref _inventoryItems, value);
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
            InventoryItems.ElementAt(CurrentIndex).InventoryType = 3;
            InventoryItems.ElementAt(CurrentIndex).Amount = 1;

            try
            {
                await _backendConnection.SetNewItem(InventoryItems.ElementAt(CurrentIndex), true);
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
        
        public InventoryItem CurrentItem
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
            InventoryItems = await _backendConnection.GetInventoryItemListByType(2);
        }
    }
}
