using System;
using System.Collections.Generic;
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
using PantryPassionGUI.Utilities.Interfaces;
using PantryPassionGUI.ViewModels;
using PantryPassionGUI.ViewModels.Interfaces;

namespace PantryPassion.Test.Integration
{
    public class Step8
    {
        private ShoppingListViewModel _sut;
        private IFindItemViewModel _findItemViewModel;
        private ICameraViewModel _cameraViewModel;
        private ICamera _camConnection;
        private IVideoSource _fakeVideoSource;
        private ITimer<Timer> _timer;
        private IBarcodeReader _barcodeReader;
        private ISoundPlayer _soundPlayer;
        private IOutput _fakeOutput;
        private IBackendConnection _fakeBackendConnection;
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
            _findItemViewModel = new FindItemViewModel(_fakeBackendConnection, _cameraViewModel);
            _obj = new object();

            _sut = new ShoppingListViewModel(_fakeBackendConnection,_findItemViewModel);
        }

        
        [Test]
        public void ShoppingListViewModel_UpdateShoppingListCommand_CallsGetInventoryItemListByType()
        {
            _sut.UpdateShoppingListCommand.Execute(_obj);

            _fakeBackendConnection.Received(2).GetInventoryItemListByType(3);
        }

        [Test]
        public void ShoppingListViewModel_DeleteItemInListCommand_CallsSetQuantity()
        {
            _sut.DeleteItemInListCommand.Execute(_obj);

            _fakeBackendConnection.Received(1).SetQuantity(_sut.CurrentInventoryItem);
        }


        [Test]
        public void ShoppingListViewModel_ClearListCommand_CallsDeleteShoppingList()
        {
            _sut.ClearListCommand.Execute(_obj);

            _fakeBackendConnection.Received(1).DeleteShoppingList();
        }


    }
}
