using System;
using NSubstitute;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.ViewModels;

namespace PantryPassion.Test.Unit.ViewModelTest
{
    class AddItemViewModelTest
    {
        private AddItemViewModel _uut;
        private ICameraViewModel _cameraViewModel;
        private BackendConnection _backendConnection;
        private object _obj;

        [SetUp]
        public void Setup()
        {
            _backendConnection = new BackendConnection();
            _cameraViewModel = Substitute.For<ICameraViewModel>();
            _uut = new AddItemViewModel(_cameraViewModel, _backendConnection);
            _obj = new object();
        }

        [Test]
        public void AddItemViewModel_UpArrowCommand_AddOneToAmount()
        {
            _uut.UpArrowCommand.Execute(_obj);
            Assert.That(_uut.InventoryItem.Amount, Is.EqualTo(1));
        }

        [Test]
        public void AddItemViewModel_DownArrowCommand_SubtractOneToAmount()
        {
            _uut.UpArrowCommand.Execute(_obj);
            _uut.UpArrowCommand.Execute(_obj);
            _uut.UpArrowCommand.Execute(_obj);

            _uut.DownArrowCommand.Execute(_obj);
            Assert.That(_uut.InventoryItem.Amount, Is.EqualTo(2));
        }

        [Test]
        public void AddItemViewModel_DownArrowCommand_CanNotExecute()
        {
            Assert.That(_uut.DownArrowCommand.CanExecute(_obj), Is.False);
        }

        [Test]
        public void AddItemViewModel_DownArrowCommand_CanExecute()
        {
            _uut.UpArrowCommand.Execute(_obj);
            Assert.That(_uut.DownArrowCommand.CanExecute(_obj), Is.True);
        }

        [Test]
        public void AddItemViewModel_OkCommand_CalledCamaraOff()
        {
            _uut.OkCommand.Execute(_obj);
            _cameraViewModel.Camera.Received(1).CameraOff();
        }

        [Test]
        public void AddItemViewModel_OkCommand_CanNotExecute()
        {
            Assert.That(_uut.OkCommand.CanExecute(_obj), Is.False);
        }

        [Test]
        public void AddItemViewModel_OkCommand_CanExecute()
        {
            _uut.InventoryItem.Item.Name = "Test";
            _uut.InventoryItem.Amount = 1;
            Assert.That(_uut.OkCommand.CanExecute(_obj), Is.True);
        }

        [Test]
        public void AddItemViewModel_CancelCommand_CalledCamaraOff()
        {
            _uut.CancelCommand.Execute(_obj);
            _cameraViewModel.Camera.Received(1).CameraOff();
        }

        [Test]
        public void AddItemViewModel_BarcodeFoundEventToViewModels_AddOneToAmount()
        {
            _cameraViewModel.BarcodeFoundEventToViewModels += Raise.EventWith(new EventArgs());
            Assert.That(_uut.InventoryItem.Amount,Is.EqualTo(1));
        }

        [Test]
        public void AddItemViewModel_InventoryItem_SetCorrect()
        {
            _uut.InventoryItem = new InventoryItem() {Amount = 10};
            Assert.That(_uut.InventoryItem.Amount,Is.EqualTo(10));
        }


    }
}
