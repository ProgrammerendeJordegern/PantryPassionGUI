using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using PantryPassionGUI.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace PantryPassionGUI.ViewModels
{
    public class AddItemViewModel : BindableBase
    {
        private string _name;
        private int _quantity;
        private string _category;
        private string _barcode;
        private BackendConnection _backendConnection;
        private ICommand _cancelCommand;
        private ICommand _okCommand;
        private ICommand _turnOffCamera;
        public CameraConnection Camera { get; private set; }
        private string _cameraButtonText;
        private ISoundPlayer _soundPlayer;
        private int _cameraListIndex;
        private Items _item;

        public enum CameraState
        {
            CameraOn,
            CameraOff
        }

        private CameraState _stateForCamera;

        public ObservableCollection<string> CameraList { get; private set; }

        public AddItemViewModel()
        {
            Camera = CameraConnection.Instance;
            Camera.CameraOn();
            Camera.BarcodeFoundEvent += FoundBarcode;
            _cameraButtonText = "Sluk kamera";
            _soundPlayer = new SoundPlayer();
            _stateForCamera = CameraState.CameraOn;
            CameraList = new ObservableCollection<string>();
            CameraList = Camera.CamerasList;
            _backendConnection = new BackendConnection();
            _item = new Items();
        }

        public Items item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
            }
            set
            {
                SetProperty(ref _item, value);
            }
        }

        public int CameraListIndex
        {
            get { return _cameraListIndex; }
            set
            {
                Camera.SetCameraListIndex(value);
                SetProperty(ref _cameraListIndex, value);
            }
        }

        public string Barcode
        {
            get { return _barcode; }
            set { SetProperty(ref _barcode, value); }
        }

        private void FoundBarcode(object sender, BarcodeFoundEventArgs e)
        {
            Barcode = e.Barcode;
            _soundPlayer.Play();

            item.Quantity++;
        }

        public string CameraButtonText
        {
            get { return _cameraButtonText; }
            set { SetProperty(ref _cameraButtonText, value); }
        }

        public ICommand TurnOffCamera
        {
            get { return _turnOffCamera ?? (_turnOffCamera = new DelegateCommand(TurnOffCamHandler)); }
            }
        }

        private void TurnOffCamHandler()
        {
            switch (_stateForCamera)
            {
                case CameraState.CameraOn:
                    _stateForCamera = CameraState.CameraOff;
                    CameraButtonText = "Tænd kamera";
                    Camera.CameraOff();
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => { Camera.CameraFeed = null; }));
                    break;
                case CameraState.CameraOff:
                    _stateForCamera = CameraState.CameraOn;
                    CameraButtonText = "Sluk kamera";
                    Camera.CameraOn();
                    break;
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
            Camera.CameraOff();
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
            get { return _cancelCommand ?? (_cancelCommand = new DelegateCommand(CancelHandler)); }
            }
        }

        private void CancelHandler()
        {
            Camera.CameraOff();
            Application.Current.Windows[Application.Current.Windows.Count - 2].Close();
        }

        public void ItemNotFound()
        {
            Console.WriteLine("sadf");
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                SetProperty(ref _quantity, value);

            }
        }

        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                SetProperty(ref _category, value);
            }
        }
    }
}
