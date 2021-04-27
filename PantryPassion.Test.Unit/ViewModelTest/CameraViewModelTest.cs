using NSubstitute;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.ViewModels;

namespace PantryPassion.Test.Unit
{
    public class CameraViewModelTest
    {
        private CameraViewModel _uut;
        private object _obj;
        private ICamera _camera;
        private ISoundPlayer _soundPlayer;

        [SetUp]
        public void Setup()
        {
            _camera = Substitute.For<ICamera>();
            _soundPlayer = Substitute.For<ISoundPlayer>();
            _uut = new CameraViewModel(_camera,_soundPlayer);
            _obj = new object();
        }

        [Test]
        public void CameraViewModel_TurnOffCamera_CalledCameraOff()
        {
            _uut.TurnOffCamera.Execute(_obj);
            _camera.Received(1).CameraOff();
        }


        [Test]
        public void CameraViewModel_TurnOffCamera_CalledCameraOn()
        {
            _uut.TurnOffCamera.Execute(_obj);
            _uut.TurnOffCamera.Execute(_obj);
            _camera.Received(1).CameraOn();
        }

        [Test]
        public void CameraViewModel_CameraListIndex_SetCorrect()
        {
            _uut.CameraListIndex = 5;
            _camera.Received(1).SetCameraListIndex(5);
        }

        [Test]
        public void CameraViewModel_BarcodeFound_BarcodeCorrect()
        {
            _camera.BarcodeFoundEvent += Raise.EventWith(new BarcodeFoundEventArgs() { Barcode = "123456"});

            Assert.That(_uut.Barcode, Is.EqualTo("123456"));
        }

        [Test]
        public void CameraViewModel_BarcodeFound_CalledPlay()
        {
            _camera.BarcodeFoundEvent += Raise.EventWith(new BarcodeFoundEventArgs() { Barcode = "123456" });

           _soundPlayer.Received(1).Play();
        }

        [Test]
        public void CameraViewModel_CameraButtonText_IsSlukkamera()
        {
            Assert.That(_uut.CameraButtonText,Is.EqualTo("Sluk kamera"));
        }

        [Test]
        public void CameraViewModel_CameraButtonText_IsTændkamera()
        {
            _uut.TurnOffCamera.Execute(_obj);
            Assert.That(_uut.CameraButtonText, Is.EqualTo("Tænd kamera"));
        }

        [Test]
        public void CameraViewModel_CameraList_NotNull()
        {
            Assert.That(_uut.CameraList, Is.Not.Null);
        }


    }
}
