using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using AForge.Video;
using NSubstitute;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.Models.Interfaces;
using PantryPassionGUI.Utilities;
using PantryPassionGUI.Utilities.Interfaces;
using PantryPassionGUI.ViewModels;
using PantryPassionGUI.ViewModels.Interfaces;

namespace PantryPassion.Test.Integration1
{
    class Step3
    {
        private ScanEANViewModel _sut;
        private ICameraViewModel _cameraViewModel;
        private ICamera _camConnection;
        private IVideoSource _fakeVideoSource;
        private ITimer<Timer> _timer;
        private IBarcodeReader _barcodeReader;
        private ISoundPlayer _soundPlayer;
        private IOutput _fakeOutput;
        private IBackendConnection _fakeBackendConnection;
        private FindItemViewModel _findItemViewModel;
        private Bitmap myBitmap;
        private Item _item;


        [SetUp]
        public void Setup()
        {
            _fakeVideoSource = Substitute.For<IVideoSource>();
            _timer = new TimerClock(100);
            _barcodeReader = new ReadBarcode();
            _fakeOutput = Substitute.For<IOutput>();
            _camConnection = new CameraConnection(_timer, _barcodeReader, _fakeVideoSource, _fakeOutput);
            _soundPlayer = new SoundPlayer(_fakeOutput);
            _cameraViewModel = new CameraViewModel(_camConnection, _soundPlayer);
            _fakeBackendConnection = Substitute.For<IBackendConnection>();

            ObservableCollection<Item> itemList = new ObservableCollection<Item>();
            _item = new Item("Jordbær", "123", 7, 1);
            itemList.Add(_item);
            _fakeBackendConnection.GetListOfItems().Returns(itemList);

            _findItemViewModel = new FindItemViewModel(_fakeBackendConnection, _cameraViewModel);


            _sut = new ScanEANViewModel(_findItemViewModel, _fakeOutput, _cameraViewModel);
        }

        [Test]
        public void ScanEANViewModel_CloseScanWindow_CloseCorrect()
        {
            if (System.Windows.Application.Current == null)
            { new System.Windows.Application { ShutdownMode = ShutdownMode.OnExplicitShutdown }; }

            myBitmap = new Bitmap(Environment.CurrentDirectory + @"\barcode.png");
            myBitmap.Save("myBitmap.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

            _camConnection.SimulateEventFromVideoCaptureDevice(myBitmap);

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("scaneanwindow closed")));
        }
    }
}
