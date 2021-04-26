using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.ViewModels;

namespace PantryPassion.Test.Unit
{
    class AddItemViewModelTest
    {
        private AddItemViewModel _uut;
        private CameraViewModel _cameraViewModel;
        private BackendConnection _backendConnection;
        private ICamera _camera;
        private ISoundPlayer _soundPlayer;
        private object _obj;

        [SetUp]
        public void Setup()
        {
            _camera = Substitute.For<ICamera>();
            _soundPlayer = Substitute.For<ISoundPlayer>();
            _backendConnection = new BackendConnection();
            _cameraViewModel = new CameraViewModel(_camera, _soundPlayer);
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
    }
}
