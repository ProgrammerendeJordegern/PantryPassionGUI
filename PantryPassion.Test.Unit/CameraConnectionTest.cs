using System.Drawing;
using System.Timers;
using AForge.Video;
using NSubstitute;
using NUnit.Framework;
using PantryPassionGUI.Models;

namespace PantryPassion.Test.Unit
{
    public class CameraConnectionTest
    {
        private CameraConnection _uut;
        private IBarcodeReader _barcodeReader;
        private ITimer<Timer> _timer;
        private IVideoSource _video;

            [SetUp]
        public void Setup()
        {
            _barcodeReader = Substitute.For<IBarcodeReader>();
            _timer = Substitute.For<ITimer<Timer>>();
            _video = Substitute.For<IVideoSource>();
            _uut = new CameraConnection(_timer, _barcodeReader,_video);
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
