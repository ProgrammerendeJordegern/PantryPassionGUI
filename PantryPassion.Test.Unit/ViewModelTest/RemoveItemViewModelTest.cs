using AForge.Video.DirectShow;
using NSubstitute;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.ViewModels;

namespace PantryPassion.Test.Unit.ViewModelTest
{
    public class RemoveItemViewModelTest
    {
        private RemoveItemViewModel _uut;
        private ICameraViewModel _cameraViewModel;
        private BackendConnection _backendConnection;
        private object _obj;

        [SetUp]
        public void Setup()
        {
            _cameraViewModel = Substitute.For<ICameraViewModel>();
            _backendConnection = new BackendConnection();
            _uut = new RemoveItemViewModel(_cameraViewModel, _backendConnection, 5);
            _obj = new object();
        }

        [Test]
        public void RemoveItemViewModel_UpArrowCommand_AddOneToAmount()
        {
            _uut.UpArrowCommand.Execute(_obj);
            Assert.That(_uut.InventoryItem.Amount, Is.EqualTo(1));
        }

        [Test]
        public void RemoveItemViewModel_UpArrowCommand_CanExecute()
        {
            Assert.That(_uut.UpArrowCommand.CanExecute(_obj),Is.True);
        }

        [Test]
        public void RemoveItemViewModel_UpArrowCommand_CanNotExecute()
        {
            for (int i = 0; i < 5; i++)
            {
                _uut.UpArrowCommand.Execute(_obj);
            }
            Assert.That(_uut.UpArrowCommand.CanExecute(_obj), Is.False);
        }

        [Test]
        public void RemoveItemViewModel_DownArrowCommand_SubtractOneToAmount()
        {
            _uut.UpArrowCommand.Execute(_obj);
            _uut.UpArrowCommand.Execute(_obj);

            _uut.DownArrowCommand.Execute(_obj);
            Assert.That(_uut.InventoryItem.Amount, Is.EqualTo(1));
        }

        [Test]
        public void RemoveItemViewModel_DownArrowCommand_CanNotExecute()
        {
            Assert.That(_uut.DownArrowCommand.CanExecute(_obj), Is.False);
        }

        [Test]
        public void RemoveItemViewModel_DownArrowCommand_CanExecute()
        {
            _uut.UpArrowCommand.Execute(_obj);
            Assert.That(_uut.DownArrowCommand.CanExecute(_obj), Is.True);
        }

        [Test]
        public void RemoveItemViewModel_OkCommand_CalledCameraOff()
        {
            _uut.OkCommand.Execute(_obj);
            _cameraViewModel.Camera.Received(1).CameraOff();
        }

        [Test]
        public void RemoveItemViewModel_CancelCommand_CalledCameraOff()
        {
            _uut.CancelCommand.Execute(_obj);
            _cameraViewModel.Camera.Received(1).CameraOff();
        }

        [Test]
        public void RemoveItemViewModel_RemoveInventoryItemCommand_SetNewInventoryItem()
        {
            _uut.RemoveInventoryItemCommand.Execute(_obj);
            Assert.That(_uut.InventoryItem.Item.Name, Is.Null);
            Assert.That(_uut.InventoryItem.Amount, Is.EqualTo(0));
        }

    }
}
