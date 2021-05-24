using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    class Step5
    {
        private AddItemViewModel _sut;
        private ICameraViewModel _cameraViewModel;
        private ICamera _camConnection;
        private IVideoSource _fakeVideoSource;
        private ITimer<Timer> _timer;
        private IBarcodeReader _barcodeReader;
        private ISoundPlayer _soundPlayer;
        private IOutput _fakeOutput;
        private IBackendConnection _fakeBackendConnection;
        private Bitmap myBitmap;
        private Object _obj;


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
            _obj = new object();

            myBitmap = new Bitmap(Environment.CurrentDirectory + @"\barcode.png");
            myBitmap.Save("myBitmap.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

            _sut = new AddItemViewModel(_cameraViewModel, _fakeBackendConnection);
        }

        [Test]
        public void AddItemViewModel_BarcodeAction_CheckBarcode()
        {
            _camConnection.SimulateEventFromVideoCaptureDevice(myBitmap);

            _fakeBackendConnection.Received(1).CheckBarcode("705632085943");
        }

        [Test]
        public void AddItemViewModel_BarcodeAction_ItemNotFound()
        {
            
        }

        [Test]
        public void AddItemViewModel_FindItemByNameHandler_GetItemByName()
        {
            InventoryItem iitem = new InventoryItem();
            iitem.Item.Name = "Jordbær";
            _sut.InventoryItem = iitem;
            _sut.FindItemByNameCommand.Execute(_obj);

            _fakeBackendConnection.Received(1).GetItemByName("Jordbær");
        }

        [Test]
        public void AddItemViewModel_CancelCommand_CameraOff()
        {
            _sut.CancelCommand.Execute(_obj);

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("camera off")));
        }

        //[Test]
        //public void AddItemViewModel_OkCommand_OkHandler()
        //{
        //    _sut.OkCommand.Execute(_obj);

        //    InventoryItem iitem = new InventoryItem();
        //    iitem.Item.Name = "Jordbær";
        //    _sut.InventoryItem = iitem;

        //    _fakeBackendConnection.Received(1).SetNewItem(iitem, true);
        //    _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("camera off")));
        //}
    }
}
