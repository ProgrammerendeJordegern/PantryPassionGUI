using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using ConsoleAppClient.Utilities;
using PantryPassionGUI.Models;
using PantryPassionGUI.ViewModels;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace PantryPassionGUI.ViewModels
{
    public class AddItemViewModel : BindableBase
    {
        private BackendConnection _backendConnection;
        private ICommand _cancelCommand;
        private ICommand _okCommand;
        private ICommand _addInventoryItemCommand;
        private ICommand _upArrowCommand;
        private ICommand _downArrowCommand;
        private InventoryItem _inventoryItem;
        public ICameraViewModel CameraViewModel { get; private set; }

        public AddItemViewModel()
        {
            _backendConnection = new BackendConnection();
            _inventoryItem = new InventoryItem();
            CameraViewModel = new CameraViewModel();
            CameraViewModel.BarcodeFoundEventToViewModels += BarcodeAction;
        }

        public AddItemViewModel(ICameraViewModel cameraViewModel, BackendConnection backendConnection)
        {
            _backendConnection = backendConnection;
            _inventoryItem = new InventoryItem();
            CameraViewModel = cameraViewModel;
            CameraViewModel.BarcodeFoundEventToViewModels += BarcodeAction;
        }

        private async void BarcodeAction(object sender, EventArgs e)
        {
            Item test = new Item();

            try
            {
                test = await BackendConnection.CheckBarcode("123123");
            }
            catch (ApiException exception)
            {
                Debug.WriteLine(exception.StatusCode);
                ItemNotFound();
            }
            

            InventoryItem.Item = test;

            //Debug.WriteLine(InventoryItem.Item.Name);

            //InventoryItem.Item.Name = "test";

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
            _backendConnection.SetNewItem(InventoryItem);
            CameraViewModel.Camera.CameraOff();
            //Application.Current.Windows[Application.Current.Windows.Count - 2].Close();
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
            //Application.Current.Windows[Application.Current.Windows.Count - 2].Close();
        }

        public void ItemNotFound()
        {
            Debug.WriteLine("Error! Vare ikke fundet!");
            //DialogWindow win = new DialogWindow();
            //win.ShowDialog();

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

        private void AddInventoryItemHandler()
        {
            _backendConnection.SetNewItem(InventoryItem);
            InventoryItem = new InventoryItem();
        }
    }
}
