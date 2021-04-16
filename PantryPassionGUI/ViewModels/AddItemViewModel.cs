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
        public CameraViewModel _cameraViewModel { get; private set; }

        public AddItemViewModel()
        {
            _backendConnection = new BackendConnection();
            _item = new Items();
            _cameraViewModel = new CameraViewModel();
            _cameraViewModel._modelState = CameraViewModel.ViewModelState.AddItem;
            _cameraViewModel._viewModelItem = item;
        }

        public Items item
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
                    .ObservesProperty(() => item.Quantity).ObservesProperty(() => item.Name));
            }
        }

        private void OkHandler()
        {
            _backendConnection.SetNewItem("Test", "Test", "Test");
            _cameraViewModel.Camera.CameraOff();
            Application.Current.Windows[Application.Current.Windows.Count - 2].Close();
        }

        private bool OkCommandCanExecute()
        {
            if (item.Quantity >= 1 && String.IsNullOrEmpty(item.Name) == false)
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
            _cameraViewModel.Camera.CameraOff();
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
            item.Quantity++;
        }

        public ICommand DownArrowCommand
        {
            get
            {
                return _okCommand ??= new DelegateCommand(DownArrowHandler, DownArrowCanExecute)
                    .ObservesProperty(() => item.Quantity);
            }
        }

        private void DownArrowHandler()
        {
            item.Quantity--;
        }

        private bool DownArrowCanExecute()
        {
            if (item.Quantity >=1)
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
