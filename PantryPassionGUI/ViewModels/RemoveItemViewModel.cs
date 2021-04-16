//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Input;
//using PantryPassionGUI.Models;
//using Prism.Commands;
//using Prism.Mvvm;

//namespace PantryPassionGUI.ViewModels
//{
//    public class RemoveItemViewModel : BindableBase
//    {
//        private string _barcode;
//        private BackendConnection _backendConnection;
//        private ICommand _cancelCommand;
//        private ICommand _okCommand;
//        private ICommand _turnOffCamera;
//        public CameraConnection Camera { get; private set; }
//        private string _cameraButtonText;
//        private ISoundPlayer _soundPlayer;
//        private int _cameraListIndex;
//        private Items _item;
//        private CameraState _stateForCamera;

//        public ObservableCollection<string> CameraList { get; private set; }

//        public RemoveItemViewModel()
//        {
//            Camera = CameraConnection.Instance;
//            Camera.CameraOn();
//            Camera.BarcodeFoundEvent += FoundBarcode;
//            _cameraButtonText = "Sluk kamera";
//            _soundPlayer = new SoundPlayer();
//            CameraList = new ObservableCollection<string>();
//            CameraList = Camera.CamerasList;
//            _backendConnection = new BackendConnection();
//            _item = new Items();
//        }

//        public string Barcode
//        {
//            get
//            {
//                return _barcode;
//            }
//            set
//            {
//                SetProperty(ref _barcode, value);
//            }
//        }

//        private void FoundBarcode(object sender, BarcodeFoundEventArgs e)
//        {
//            Barcode = e.Barcode;
//            _soundPlayer.Play();
//        }

//        public string CameraButtonText
//        {
//            get
//            {
//                return _cameraButtonText;
//            }
//            set
//            {
//                SetProperty(ref _cameraButtonText, value);
//            }
//        }

//        public ICommand TurnOffCamera
//        {
//            get
//            {
//                return _turnOffCamera ?? (_turnOffCamera = new DelegateCommand(TurnOffCamHandler));
//            }
//        }


//        private void TurnOffCamHandler()
//        {
//            switch (_stateForCamera)
//            {
//                case AddItemViewModel.CameraState.CameraOn:
//                    _stateForCamera = AddItemViewModel.CameraState.CameraOff;
//                    CameraButtonText = "Tænd kamera";
//                    Camera.CameraOff();
//                    Application.Current.Dispatcher.BeginInvoke(new Action(() => { Camera.CameraFeed = null; }));
//                    break;
//                case AddItemViewModel.CameraState.CameraOff:
//                    _stateForCamera = AddItemViewModel.CameraState.CameraOn;
//                    CameraButtonText = "Sluk kamera";
//                    Camera.CameraOn();
//                    break;
//            }
//        }
//    }
//}
