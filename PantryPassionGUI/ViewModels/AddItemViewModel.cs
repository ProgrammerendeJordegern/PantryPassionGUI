using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using PantryPassionGUI.Models;
using PantryPassionGUI.Utilities;
using PantryPassionGUI.Utilities.Interfaces;
using PantryPassionGUI.ViewModels;
using PantryPassionGUI.ViewModels.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace PantryPassionGUI.ViewModels
{
    public class AddItemViewModel : BindableBase
    {
        private IBackendConnection _backendConnection;
        private ICommand _cancelCommand;
        private ICommand _okCommand;
        private ICommand _addInventoryItemCommand;
        private ICommand _upArrowCommand;
        private ICommand _downArrowCommand;
        private ICommand _findItemByNamCommand;
        private InventoryItem _inventoryItem;
        private bool _itemExistsInDatabase = true;
        public ICameraViewModel CameraViewModel { get; private set; }


        public AddItemViewModel()
        {
            _backendConnection = new BackendConnection();
            _inventoryItem = new InventoryItem();
            CameraViewModel = new CameraViewModel();
            CameraViewModel.BarcodeFoundEventToViewModels += BarcodeAction;
        }

        public AddItemViewModel(ICameraViewModel cameraViewModel, IBackendConnection backendConnection)
        {
            _backendConnection = backendConnection;
            _inventoryItem = new InventoryItem();
            CameraViewModel = cameraViewModel;
            CameraViewModel.BarcodeFoundEventToViewModels += BarcodeAction;
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
                _itemExistsInDatabase = false;
            }
            catch (HttpRequestException exception)
            {
                MessageBox.Show($"Der er ingen forbindele til serveren", "Error!");
            }

            InventoryItem.Amount++;
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

        public ICommand OkCommand
        {
            get
            {
                return _okCommand ??= new DelegateCommand(OkHandler, OkCommandCanExecute)
                    .ObservesProperty(() => InventoryItem.Amount).ObservesProperty(() => InventoryItem.Item.Name);
            }
        }

        private void OkHandler()
        {
            AddItemToDatabase();

            CameraViewModel.Camera.CameraOff();
        }

        private bool OkCommandCanExecute()
        {
            if (InventoryItem.Amount >= 1 && String.IsNullOrEmpty(InventoryItem.Item.Name) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
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

        private void ItemNotFound(int statusCode)
        {
            MessageBox.Show($"Fejl {statusCode}\nVare blev ikke fundet i systemet!\nIndtast venlist selv vares informationer","Error!");
        }

        public ICommand UpArrowCommand
        {
            get
            {
                return _upArrowCommand ??= new DelegateCommand(UpArrowHandler);
            }
        }

        private void UpArrowHandler()
        {
            InventoryItem.Amount++;
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

        public ICommand AddInventoryItemCommand
        {
            get
            {
                return _addInventoryItemCommand ??= new DelegateCommand(AddInventoryItemHandler,DownArrowCanExecute)
                    .ObservesProperty(() => InventoryItem.Amount); ;
            }
        }

        private  void AddInventoryItemHandler()
        {
            AddItemToDatabase();

            InventoryItem = new InventoryItem();
        }

        private async void AddItemToDatabase()
        {
            try
            {
                int StatusCode = await _backendConnection.SetNewItem(InventoryItem, _itemExistsInDatabase);
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
        }
    }
}
