using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PantryPassionGUI.Models;
using Prism.Mvvm;
using Prism.Commands;

namespace PantryPassionGUI.ViewModels
{
    public class CameraViewModel : BindableBase
    {
        private ICommand _turnOffCamera;
        public CameraConnection Camera { get; private set; }
        private string _cameraButtonText;
        private ISoundPlayer _soundPlayer;
        private int _cameraListIndex;
        private string _barcode;
        public Items _viewModelItem { get; set; }
        public ViewModelState _modelState { get; set; }

        public enum CameraState
        {
            CameraOn,
            CameraOff,
        }

        public enum ViewModelState
        {
            AddItem,
            RemoveItem
        }

        private CameraState _stateForCamera;

        public ObservableCollection<string> CameraList { get; private set; }

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
            _viewModelItem = new Items();
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

            switch (_modelState)
            {
                case ViewModelState.AddItem:
                    _viewModelItem.Quantity++;
                    _soundPlayer.Play();
                    break;
                case ViewModelState.RemoveItem:
                    _soundPlayer.Play();
                    break;
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
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => { Camera.CameraFeed = null; }));
                    break;
                case CameraState.CameraOff:
                    _stateForCamera = CameraState.CameraOn;
                    CameraButtonText = "Sluk kamera";
                    Camera.CameraOn();
                    break;
            }
        }
    }
}
