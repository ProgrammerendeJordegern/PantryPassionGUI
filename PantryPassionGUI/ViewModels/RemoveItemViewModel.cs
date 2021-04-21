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
        private Items _item;
        private int _originalQuantity;

        public RemoveItemViewModel()
        {
            CameraViewModel = new CameraViewModel();
            _backendConnection = new BackendConnection();
            _item = new Items();
            CameraViewModel.BarcodeFoundEventToViewModels += BarcodeAction;
            _originalQuantity = 5;
        }

        private void BarcodeAction(object sender, EventArgs e)
        {
            _item = _backendConnection.CheckBarcode(CameraViewModel.Barcode);
            _originalQuantity = _item.Quantity;
        }

        public Items Item
        {
            get
            {
                return _item;
            }
            set
            {
                SetProperty(ref _item, value);
            }
        }

        public ICommand UpArrowCommand
        {
            get
            {
                return _upArrowCommand ??= new DelegateCommand(UpArrowHandler, UpArrowCanExecute)
                    .ObservesProperty(() => Item.Quantity);
            }
        }

        private void UpArrowHandler()
        {
            Item.Quantity++;
        }

        private bool UpArrowCanExecute()
        {
            if (Item.Quantity < _originalQuantity)
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
                    .ObservesProperty(() => Item.Quantity);
            }
        }

        private void DownArrowHandler()
        {
            Item.Quantity--;
        }

        private bool DownArrowCanExecute()
        {
            if (Item.Quantity >= 1)
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
            _backendConnection.SetQuantity(_item.Name, _item.Quantity);
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