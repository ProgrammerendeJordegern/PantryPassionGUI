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
        public CameraViewModel CameraViewModel { get; private set; }
        private BackendConnection _backendConnection;
        private ICommand _cancelCommand;
        private ICommand _okCommand;
        private ICommand _upArrowCommand;
        private ICommand _downArrowCommand;
        private InventoryItem _inventoryItem;
        private int _originalQuantity;

        public RemoveItemViewModel()
        {
            CameraViewModel = CameraViewModel.Instance;
            _backendConnection = new BackendConnection();
            _inventoryItem = new InventoryItem();
            CameraViewModel.BarcodeFoundEventToViewModels += BarcodeAction;
            _originalQuantity = 5;
        }

        private async void BarcodeAction(object sender, EventArgs e)
        {
            _inventoryItem = await BackendConnection.CheckBarcode(CameraViewModel.Barcode);
            _originalQuantity = _inventoryItem.Amount;
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
            if (InventoryItem.Amount < _originalQuantity)
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
            Application.Current.Windows[Application.Current.Windows.Count - 2].Close();
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
            Application.Current.Windows[Application.Current.Windows.Count - 2].Close();
        }
    }
}