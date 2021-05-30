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
    class Step2
    {
        private ICameraViewModel _sut;
        private ICamera _camConnection;
        private IVideoSource _fakeVideoSource;
        private ITimer<Timer> _timer;
        private IBarcodeReader _barcodeReader;
        private ISoundPlayer _soundPlayer;
        private IOutput _fakeOutput;
        private Object _obj;
        private Bitmap myBitmap;


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

            myBitmap = new Bitmap(Environment.CurrentDirectory + @"\barcode.png");
            myBitmap.Save("myBitmap.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

            _sut = new CameraViewModel(_camConnection, _soundPlayer);
        }

        [Test]
        public void CameraVM_FoundBarcode_CorrectBarcode()
        {
            _camConnection.SimulateEventFromVideoCaptureDevice(myBitmap);

            Assert.That(_sut.Barcode, Is.EqualTo("705632085943"));
        }

        [Test]
        public void CameraVM_FoundBarcode_SoundPlayed()
        {
            _camConnection.SimulateEventFromVideoCaptureDevice(myBitmap);
            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("sound played")));
        }

        [Test]
        public void CameraVM_TurnOffCamHandler_CamerStateOff()
        {
            _fakeVideoSource.IsRunning.Returns(true);
            _sut.TurnOffCamera.Execute(_obj);
            
            _fakeVideoSource.Received(1).SignalToStop();
            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("camera off")));

        }

        [Test]
        public void CameraVM_TurnOffCamHandler_CamerStateOn()
        {
            _fakeVideoSource.IsRunning.Returns(true);
            _sut.TurnOffCamera.Execute(_obj);
            _sut.TurnOffCamera.Execute(_obj);

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("camera on")));

        }
    }
}
