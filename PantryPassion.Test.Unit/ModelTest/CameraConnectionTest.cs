using System.Timers;
using AForge.Video;
using NSubstitute;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.Models.Interfaces;
using PantryPassionGUI.Utilities;
using PantryPassionGUI.Utilities.Interfaces;

namespace PantryPassion.Test.Unit.ModelTest
{
    public class CameraConnectionTest
    {
        private CameraConnection _uut;
        private IBarcodeReader _barcodeReader;
        private ITimer<Timer> _timer;
        private IVideoSource _video;
        private IOutput _output;

        [SetUp]
        public void Setup()
        {
            _barcodeReader = Substitute.For<IBarcodeReader>();
            _timer = Substitute.For<ITimer<Timer>>();
            _video = Substitute.For<IVideoSource>();
            _output = Substitute.For<IOutput>();
            _uut = new CameraConnection(_timer, _barcodeReader,_video,_output);
        }

        [Test]
        public void CameraOn_Set_VideoTunedOn()
        {
            _uut.CameraOn();

            _video.Received(1).Start();
        }

        [Test]
        public void CameraOFF_Calls_SignalToStop()
        {

            _video.IsRunning.Returns(true);

            _uut.CameraOff();

            _video.Received(1).SignalToStop();
        }

        [Test]
        public void CameraOFF_Calls_WaitForStop()
        {

            _video.IsRunning.Returns(true);

            _uut.CameraOff();

            _video.Received(1).WaitForStop();
        }

        [Test]
        public void CameraListIndex_set_ToCorrecValue()
        {
            _uut.SetCameraListIndex(1);
            Assert.That(_uut.GetCameraListIndex(),Is.EqualTo(1));
        }
    }
}
