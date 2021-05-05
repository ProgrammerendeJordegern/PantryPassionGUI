using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PantryPassionGUI.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace PantryPassionGUI.ViewModels
{
    public class RemoveItemViewModel : BindableBase
    {
        public ICameraViewModel CameraViewModel { get; private set; }
        private BackendConnection _backendConnection;
        private ICommand _cancelCommand;
        private ICommand _okCommand;
        private ICommand _removeInventoryItemCommand;
        private ICommand _upArrowCommand;
        private ICommand _downArrowCommand;
        private InventoryItem _inventoryItem;
        public int OriginalQuantity { get; private set; }

        public RemoveItemViewModel()
        {
            CameraViewModel = new CameraViewModel();
            _backendConnection = new BackendConnection();
            _inventoryItem = new InventoryItem();
            CameraViewModel.BarcodeFoundEventToViewModels += BarcodeAction;

            //For testing
            OriginalQuantity = 5;
        }

        public RemoveItemViewModel(ICameraViewModel cameraViewModel, BackendConnection backendConnection , int originalQuantity)
        {
            CameraViewModel = cameraViewModel;
            _backendConnection = backendConnection;
            _inventoryItem = new InventoryItem();
            CameraViewModel.BarcodeFoundEventToViewModels += BarcodeAction;
            OriginalQuantity = originalQuantity;
        }

        private async void BarcodeAction(object sender, EventArgs e)
        {
            //_inventoryItem = await BackendConnection.CheckBarcode(CameraViewModel.Barcode);
            OriginalQuantity = _inventoryItem.Amount;
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
                return _okCommand ??= new DelegateCommand(OkHandler);
            }
        }

        private void OkHandler()
        {
            _backendConnection.SetQuantity(_inventoryItem.Item.Name, _inventoryItem.Amount);
            CameraViewModel.Camera.CameraOff();
            //Application.Current.Windows[Application.Current.Windows.Count - 2].Close();
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

        public ICommand RemoveInventoryItemCommand
        {
            get
            {
                return _removeInventoryItemCommand ??= new DelegateCommand(RemoveInventoryItemHandler);
            }
        }

        private void RemoveInventoryItemHandler()
        {
            _backendConnection.SetQuantity(InventoryItem.Item.Name,InventoryItem.Amount);
            InventoryItem = new InventoryItem();
        }
    }
}