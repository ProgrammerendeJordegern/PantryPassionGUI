using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using AForge.Video.DirectShow;
using System.Drawing;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AForge.Video;
using PantryPassionGUI.Models.Interfaces;
using PantryPassionGUI.Utilities;
using PantryPassionGUI.Utilities.Interfaces;
using Prism.Mvvm;

namespace PantryPassionGUI.Models
{
    public class CameraConnection : BindableBase, ICamera
    {
        private FilterInfoCollection _filterInfoCollection;
        private IVideoSource _videoCaptureDevice;
        public ObservableCollection<string> CamerasList { get; private set; }
        private int _cameraListIndex;
        private BitmapImage _cameraFeed;
        private IBarcodeReader _reader;
        private ITimer<Timer> _timer;
        private IOutput _output;

        public event EventHandler<BarcodeFoundEventArgs> BarcodeFoundEvent;

        private static readonly object Padlock = new object();
        private static CameraConnection _instance = null;

        //Thread Safety Singleton
        public static CameraConnection Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new CameraConnection();
                    }
                    return _instance;
                }
            }
        }

        //Used to do dependency injection in testing
        public CameraConnection(ITimer<Timer> timer, IBarcodeReader barcodeReader, IVideoSource videoSource, IOutput output)
        {
            _timer = timer;
            _reader = barcodeReader;
            _videoCaptureDevice = videoSource;
            _output = output;
        }

        //private constructor used to prevent creating instances of the class
        private CameraConnection()
        {
            _cameraListIndex = 0;
            _reader = new ReadBarcode();
            _timer = new TimerClock(2000);
            _timer.GetTimer().Elapsed += new ElapsedEventHandler(TimeHandler);
            CamerasList = new ObservableCollection<string>();
            _filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            _output = new Output();

            foreach (FilterInfo device in _filterInfoCollection)
            {
                CamerasList.Add(device.Name);
            }

            _videoCaptureDevice = new VideoCaptureDevice(_filterInfoCollection[_cameraListIndex].MonikerString);
        }

        //Image from camera
        public BitmapImage CameraFeed
        {
            get
            {
                return _cameraFeed;
            }
            set
            {
                SetProperty(ref _cameraFeed, value);
            }
        }

        public void CameraOn()
        {
            _videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            _videoCaptureDevice.Start();

            //write line to output class for testing purpose
            _output.OutputLine("Camera on");
        }


        public void SetCameraListIndex(int index)
        {
            _cameraListIndex = index;
        }

        public int GetCameraListIndex()
        {
            return _cameraListIndex;
        }

        //Handler for event NewFrame in videoCaptureDevice
        private void VideoCaptureDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            
            if (_reader.ActivateBool)
            {
                //Gets the image from eventArgs
                Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

                //scan for barcode in image
                string barcode = _reader.GetBarcode(bitmap);


                //If a barcode was found
                if (barcode != null)
                {
                    BarcodeFound(new BarcodeFoundEventArgs { Barcode = barcode });
                    _reader.Deactivate();
                    _timer.Enable();
                }

                //invoke new action on main thread, because camera uses it own thread
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { CameraFeed = Convert(bitmap); }));
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { CameraFeed.Freeze(); }));
            }
        }

        //Function used in integration test 
        public void SimulateEventFromVideoCaptureDevice(Bitmap myBitmap)
        {
            string barcode = _reader.GetBarcode(myBitmap);

            if (barcode != null)
            {
                BarcodeFound(new BarcodeFoundEventArgs { Barcode = barcode });
                _reader.Deactivate();
                _timer.Enable();
            }
        }

        //Handler for event Elapsed in TimerClock
        private void TimeHandler(object source, ElapsedEventArgs e)
        {
            _reader.Activate();
        }

        //Function found on stackoverflow post:
        //https://stackoverflow.com/questions/5782913/how-to-convert-from-type-image-to-type-bitmapimage
        private BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        public void CameraOff()
        {
            if (_videoCaptureDevice != null)
            {
                if (_videoCaptureDevice.IsRunning)
                {
                    _videoCaptureDevice.SignalToStop();
                    _videoCaptureDevice.WaitForStop();
                }
            }

            //write line to output class for testing purpose
            _output.OutputLine("Camera off");
        }

        protected virtual void BarcodeFound(BarcodeFoundEventArgs e)
        {
            BarcodeFoundEvent?.Invoke(this, e);
        }
    }
}
