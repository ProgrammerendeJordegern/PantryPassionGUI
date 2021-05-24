using System;
using System.Drawing;
using System.Timers;
using AForge.Video;
using NSubstitute;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.Models.Interfaces;
using PantryPassionGUI.Utilities.Interfaces;
using PantryPassionGUI.ViewModels;
using PantryPassionGUI.ViewModels.Interfaces;

namespace PantryPassion.Test.Integration
{
    public class Step6
    {
        private RemoveItemViewModel _sut;
        private ICameraViewModel _cameraViewModel;
        private ICamera _camConnection;
        private IVideoSource _fakeVideoSource;
        private ITimer<Timer> _timer;
        private IBarcodeReader _barcodeReader;
        private ISoundPlayer _soundPlayer;
        private IOutput _fakeOutput;
        private IBackendConnection _fakeBackendConnection;
        private Bitmap myBitmap;
        private object _obj;


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

            _sut = new RemoveItemViewModel(_cameraViewModel, _fakeBackendConnection,5);
        }

        [Test]
        public void RemoveItemViewModel_BarcodeAction_CorrectBarcode()
        {
            Item item = new Item();
            item.ItemId = 8;

            _fakeBackendConnection.CheckBarcode("705632085943").Returns(item);
            _camConnection.SimulateEventFromVideoCaptureDevice(myBitmap);

            _fakeBackendConnection.Received(1).GetListOfInventoryItems(8);
        }

        [Test]
        public void AddItemViewModel_CancelCommand_CameraOff()
        {
            _sut.CancelCommand.Execute(_obj);

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("camera off")));
        }

        [Test]
        public void AddItemViewModel_OkCommand_UpdateInventoryItemAmountCalled()
        {
            _sut.OkCommand.Execute(_obj);

            _fakeBackendConnection.Received(1).SetQuantity(_sut.CurrentInventoryItem);
        }

        [Test]
        public void AddItemViewModel_OkCommand_CameraOff()
        {
            _sut.OkCommand.Execute(_obj);

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("camera off")));
        }

        [Test]
        public void AddItemViewModel_CurrentInventoryItem_SetOriginalQuantity()
        {
            InventoryItem inventoryItem = new InventoryItem();
            inventoryItem.Amount = 3;
            _sut.CurrentInventoryItem = inventoryItem;

            Assert.That(_sut.OriginalQuantity,Is.EqualTo(3));
        }
    }
}
