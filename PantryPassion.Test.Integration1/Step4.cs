using System;
using System.Collections.ObjectModel;
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
    class Step4
    {
        private FindItemViewModel _sut;
        private ICameraViewModel _cameraViewModel;
        private ICamera _camConnection;
        private IVideoSource _fakeVideoSource;
        private ITimer<Timer> _timer;
        private IBarcodeReader _barcodeReader;
        private ISoundPlayer _soundPlayer;
        private IOutput _fakeOutput;
        private IBackendConnection _fakeBackendConnection;
        private Object _obj;
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
            _obj = new object();

            _cameraViewModel = new CameraViewModel(_camConnection, _soundPlayer);
            _fakeBackendConnection = Substitute.For<IBackendConnection>();

            ObservableCollection<Item> itemList = new ObservableCollection<Item>();
            _item = new Item("Jordbær", "123", 7, 1);
            itemList.Add(_item);
            _fakeBackendConnection.GetListOfItems().Returns(itemList);

            _sut = new FindItemViewModel(_fakeBackendConnection, _cameraViewModel);
        }

        [Test]
        public void FindItemViewModel_GetListOfItems_ReturnCorrectNumber()
        {
            Assert.That(_sut.Items.Count, Is.EqualTo(1));
        }

        //[Test]
        //public void FindItemViewModel_AddToShoppingListHandler_AddCorrectly()
        //{
        //    _sut.CurrentIndex = 0;
        //    _sut.AddToShoppingListCommand.Execute(_obj);
        //    InventoryItem iitem = new InventoryItem();
        //    iitem.Item = _item;
        //    iitem.Amount = 1;
        //    iitem.InventoryType = 3;

        //    _fakeBackendConnection.Received(1).SetNewItem(iitem, true);
        //}

        [Test]
        public void FindItemViewModel_AddToShoppingListHandler_CameraOff()
        {
            _sut.CurrentIndex = 0;
            _sut.AddToShoppingListCommand.Execute(_obj);

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("camera off")));
        }
    }
}
