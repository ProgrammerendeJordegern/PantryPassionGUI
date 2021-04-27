using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using PantryPassionGUI.Models;
using PantryPassionGUI.ViewModels;
using Prism.Commands;
using Prism.Mvvm;

namespace PantryPassionGUI.ViewModels
{
    public class AddItemViewModel : BindableBase
    {
        private BackendConnection _backendConnection;
        private ICommand _cancelCommand;
        private ICommand _okCommand;
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
            //InventoryItem = await BackendConnection.CheckBarcode(CameraViewModel.Barcode);
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
            _backendConnection.SetNewItem("Test", "Test", "Test");
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
            Console.WriteLine("Error! Vare ikke fundet!");
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
    }
}
