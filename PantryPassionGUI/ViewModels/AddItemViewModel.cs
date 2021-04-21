using System;
using System.Collections.ObjectModel;
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
        private Items _item;
        public CameraViewModel CameraViewModel { get; private set; }

        public AddItemViewModel()
        {
            _backendConnection = new BackendConnection();
            _item = new Items();
            CameraViewModel = new CameraViewModel();
            CameraViewModel.BarcodeFoundEventToViewModels += BarcodeAction;
        }

        private void BarcodeAction(object sender, EventArgs e)
        {
            Item = _backendConnection.CheckBarcode(CameraViewModel.Barcode);
            Item.Quantity++;
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

        public ICommand OkCommand
        {
            get
            {
                return _okCommand ?? (_okCommand = new DelegateCommand(OkHandler, OkCommandCanExecute)
                    .ObservesProperty(() => Item.Quantity).ObservesProperty(() => Item.Name));
            }
        }

        private void OkHandler()
        {
            _backendConnection.SetNewItem("Test", "Test", "Test");
            CameraViewModel.Camera.CameraOff();
            Application.Current.Windows[Application.Current.Windows.Count - 2].Close();
        }

        private bool OkCommandCanExecute()
        {
            if (Item.Quantity >= 1 && String.IsNullOrEmpty(Item.Name) == false)
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
                return _cancelCommand ?? (_cancelCommand = new DelegateCommand(CancelHandler));
            }
        }


        private void CancelHandler()
        {
            CameraViewModel.Camera.CameraOff();
            Application.Current.Windows[Application.Current.Windows.Count - 2].Close();
        }

        public void ItemNotFound()
        {
            Console.WriteLine("sadf");
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
            Item.Quantity++;
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
    }
}
