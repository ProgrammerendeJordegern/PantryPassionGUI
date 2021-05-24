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
using AForge.Video.DirectShow;
using PantryPassionGUI.Models;
using PantryPassionGUI.Utilities;
using PantryPassionGUI.Utilities.Interfaces;
using PantryPassionGUI.ViewModels.Interfaces;
using Prism.Commands;
using Prism.Mvvm;

namespace PantryPassionGUI.ViewModels
{
    public class RemoveItemViewModel : BindableBase
    {
        public ICameraViewModel CameraViewModel { get; private set; }
        private IBackendConnection _backendConnection;
        private ICommand _cancelCommand;
        private ICommand _okCommand;
        private ICommand _removeInventoryItemCommand;
        private ICommand _upArrowCommand;
        private ICommand _downArrowCommand;
        private ICommand _findItemByNamCommand;
        private InventoryItem _inventoryItem;
        private InventoryItem _currentItem;

        public int OriginalQuantity { get; private set; }

        private ObservableCollection<InventoryItem> _inventoryItemsList;

        public RemoveItemViewModel()
        {
            CameraViewModel = new CameraViewModel();
            _backendConnection = new BackendConnection();
            _inventoryItem = new InventoryItem();
            CameraViewModel.BarcodeFoundEventToViewModels += BarcodeAction;
            _currentItem = new InventoryItem();
            _inventoryItemsList = new ObservableCollection<InventoryItem>();
        }

        public RemoveItemViewModel(ICameraViewModel cameraViewModel, IBackendConnection backendConnection , int originalQuantity)
        {
            CameraViewModel = cameraViewModel;
            _backendConnection = backendConnection;
            _inventoryItem = new InventoryItem();
            _inventoryItemsList = new ObservableCollection<InventoryItem>();
            _currentItem = new InventoryItem();
            CameraViewModel.BarcodeFoundEventToViewModels += BarcodeAction;
            OriginalQuantity = originalQuantity;
        }

        private async void BarcodeAction(object sender, EventArgs e)
        {
            InventoryItem.Item.Ean = CameraViewModel.Barcode;

            try
            {
                InventoryItem.Item = await _backendConnection.CheckBarcode(InventoryItem.Item.Ean);
            }
            catch (ApiException exception)
            {
                ItemNotFound(exception.StatusCode);
            }
            catch (HttpRequestException exception)
            {
                MessageBox.Show($"Der er ingen forbindele til serveren", "Error!");
            }

            GetInventoryItemsList();
        }

        public ObservableCollection<InventoryItem> InventoryItemsList
        {
            get
            {
                return _inventoryItemsList;
            }
            set
            {
                SetProperty(ref _inventoryItemsList, value);
            }
        }

        public InventoryItem InventoryItem
        {
            get
            {
                return _inventoryItem;
            }
            set
            {
                SetProperty(ref _inventoryItem, value);
            }
        }

        public ICommand UpArrowCommand
        {
            get
            {
                return _upArrowCommand ??= new DelegateCommand(UpArrowHandler, UpArrowCanExecute)
                    .ObservesProperty(() => InventoryItem.Amount);
            }
        }

        private void UpArrowHandler()
        {
            InventoryItem.Amount++;
        }

        private bool UpArrowCanExecute()
        {
            if (InventoryItem.Amount < OriginalQuantity)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ICommand DownArrowCommand
        {
            get
            {
                return _downArrowCommand ??= new DelegateCommand(DownArrowHandler, DownArrowCanExecute)
                    .ObservesProperty(() => InventoryItem.Amount);
            }
        }

        private void DownArrowHandler()
        {
            InventoryItem.Amount--;
        }

        private bool DownArrowCanExecute()
        {
            if (InventoryItem.Amount >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ICommand OkCommand
        {
            get
            {
                return _okCommand ??= new DelegateCommand(OkHandler,RemoveAndOkCanExecute)
                    .ObservesProperty(() => InventoryItem.Item.Ean);
            }
        }

        private void OkHandler()
        {
            UpdateInventoryItemAmount();

            CameraViewModel.Camera.CameraOff();
        }

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ??= new DelegateCommand(CancelHandler);
            }
        }


        private void CancelHandler()
        {
            CameraViewModel.Camera.CameraOff();
        }

        public ICommand RemoveInventoryItemCommand
        {
            get
            {
                return _removeInventoryItemCommand ??= new DelegateCommand(RemoveInventoryItemHandler,RemoveAndOkCanExecute)
                    .ObservesProperty(() => InventoryItem.Item.Ean);
            }
        }

        private void RemoveInventoryItemHandler()
        {
            UpdateInventoryItemAmount();

            InventoryItem = new InventoryItem();
            InventoryItemsList.Clear();
        }

        private bool RemoveAndOkCanExecute()
        {
            if (String.IsNullOrEmpty(InventoryItem.Item.Ean) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ICommand FindItemByNameCommand
        {
            get
            {
                return _findItemByNamCommand ??= new DelegateCommand(FindItemByNameHandler);
            }
        }

        private async void FindItemByNameHandler()
        {
            try
            {
                InventoryItem.Item = await _backendConnection.GetItemByName(InventoryItem.Item.Name);
            }
            catch (ApiException exception)
            {
                ItemNotFound(exception.StatusCode);
            }
            catch (HttpRequestException exception)
            {
                MessageBox.Show($"Der er ingen forbindele til serveren", "Error!");
            }

            GetInventoryItemsList();
        }

        public InventoryItem CurrentInventoryItem
        {
            get
            {
                return _currentItem;
            }
            set
            {
                if (value != null)
                {
                    OriginalQuantity = value.Amount;
                    InventoryItem.Amount = value.Amount;
                }
                
                SetProperty(ref _currentItem, value);
            }
        }
        private void ItemNotFound(int statusCode)
        {
            MessageBox.Show($"Fejl {statusCode}\nVare blev ikke fundet i systemet!\nIndtast venlist selv vares informationer", "Error!");
        }

        private async void GetInventoryItemsList()
        {
            if (InventoryItem.Item.ItemId != 0)
            {
                InventoryItemsList = await _backendConnection.GetListOfInventoryItems(InventoryItem.Item.ItemId);
                OriginalQuantity = InventoryItemsList.ElementAt(0).Amount;
                InventoryItem.Amount = InventoryItemsList.ElementAt(0).Amount;
            }
        }

        private async void UpdateInventoryItemAmount()
        {
            CurrentInventoryItem.Amount = InventoryItem.Amount;

            try
            {
                int statusCode = await _backendConnection.SetQuantity(CurrentInventoryItem);
            }
            catch (ApiException exception)
            {
                MessageBox.Show($"Fejl {exception.StatusCode}", "Error!");
            }
            catch (HttpRequestException exception)
            {
                MessageBox.Show($"Der er ingen forbindele til serveren", "Error!");
            }
        }
    }
}