using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using PantryPassionGUI.Models;
using Prism.Mvvm;
using Prism.Commands;

namespace PantryPassionGUI.ViewModels
{
    public class CameraViewModel : BindableBase
    {
        private ICommand _turnOffCamera;
        public ICamera Camera { get; private set; }
        private string _cameraButtonText;
        private ISoundPlayer _soundPlayer;
        private int _cameraListIndex;
        private string _barcode;
        private CameraState _stateForCamera;

        public event EventHandler<EventArgs> BarcodeFoundEventToViewModels;

        public ObservableCollection<string> CameraList { get; private set; }

        public enum CameraState
        {
            CameraOn,
            CameraOff,
        }

        public CameraViewModel(ICamera camera, ISoundPlayer soundPlayer)
        {
            Camera = camera;
            _soundPlayer = soundPlayer;
            _stateForCamera = CameraState.CameraOn;
            CameraList = new ObservableCollection<string>();
            Camera.BarcodeFoundEvent += FoundBarcode;
            _cameraButtonText = "Sluk kamera";
            Camera.CameraFeed = new BitmapImage();
        }

        public CameraViewModel()
        {
            Camera = CameraConnection.Instance;
            Camera.CameraOn();
            Camera.BarcodeFoundEvent += FoundBarcode;
            _cameraButtonText = "Sluk kamera";
            _soundPlayer = new SoundPlayer();
            _stateForCamera = CameraState.CameraOn;
            CameraList = new ObservableCollection<string>();
            CameraList = Camera.CamerasList;
        }

        public string Barcode
        {
            get
            {
                return _barcode;
            }
            set
            {
                SetProperty(ref _barcode, value);
            }
        }

        private void FoundBarcode(object sender, BarcodeFoundEventArgs e)
        {
            Barcode = e.Barcode;

            BarcodeFoundEventViewModels(new EventArgs());
            _soundPlayer.Play();
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

        public string CameraButtonText
        {
            get
            {
                return _cameraButtonText;
            }
            set
            {
                SetProperty(ref _cameraButtonText, value);
            }
        }

        public ICommand TurnOffCamera
        {
            get
            {
                return _turnOffCamera ?? (_turnOffCamera = new DelegateCommand(TurnOffCamHandler));
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
                    Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => { Camera.CameraFeed = null; }));
                    break;
                case CameraState.CameraOff:
                    _stateForCamera = CameraState.CameraOn;
                    CameraButtonText = "Sluk kamera";
                    Camera.CameraOn();
                    break;
            }
        }

        protected virtual void BarcodeFoundEventViewModels(EventArgs e)
        {
            BarcodeFoundEventToViewModels?.Invoke(this, e);
        }
    }
}
