using System;
using System.Drawing;
using System.Threading;
using AForge.Video;
using NSubstitute;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.Models.Interfaces;
using PantryPassionGUI.Utilities.Interfaces;
using Timer = System.Timers.Timer;

namespace PantryPassion.Test.Integration1
{
    public class Step1
    {
        private ICamera _sut;
        private IVideoSource _fakeVideoSource;
        private ITimer<Timer> _timer;
        private IBarcodeReader _barcodeReader;
        private IOutput _fakeOutput;
        private Bitmap myBitmap;

        [SetUp]
        public void Setup()
        {
            _fakeVideoSource = Substitute.For<IVideoSource>();
            _timer = new TimerClock(100);
            _barcodeReader = new ReadBarcode();
            _fakeOutput = Substitute.For<IOutput>();

            _sut = new CameraConnection(_timer, _barcodeReader, _fakeVideoSource, _fakeOutput);
        }

        [Test]
        public void CameraConnection_CameraOn_StartVideo()
        {
            _sut.CameraOn();
            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("camera on")));
            _fakeVideoSource.Received(1).Start();
        }


        [Test]
        public void CameraConnection_CameraOff_StopVideo()
        {
            _sut.CameraOn();
            _fakeVideoSource.IsRunning.Returns(true);
            _sut.CameraOff();
            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("camera off")));
            _fakeVideoSource.Received(1).SignalToStop();
            _fakeVideoSource.Received(1).WaitForStop();
        }

        [Test]
        public void CameraConnection_TimeHandler_ActivateReader()
        {
            _sut.CameraOn();
            Thread.Sleep(110);

            Assert.That(_barcodeReader.ActivateBool, Is.EqualTo(true));
        }

        [Test]
        public void CameraConnection_VideoCaptureDevice_NewFrame()
        {
            myBitmap = new Bitmap(Environment.CurrentDirectory + @"\barcode.png");
            myBitmap.Save("myBitmap.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

            _sut.SimulateEventFromVideoCaptureDevice(myBitmap);

            Assert.That(_barcodeReader.ActivateBool, Is.EqualTo(false));
        }
    }
}