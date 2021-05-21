using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AForge.Video;
using NSubstitute;
using NUnit.Framework;
using PantryPassionGUI.Models;
using PantryPassionGUI.ViewModels;

namespace PantryPassion.Test.Integration1
{
    class Step2
    {
        private ICameraViewModel _sut;
        private ICamera _camConnection;
        private IVideoSource _fakeVideoSource; //Har vi den her?
        private ITimer<Timer> _timer;
        private IBarcodeReader _barcodeReader;
        private ISoundPlayer _soundPlayer;

        [SetUp]
        public void Setup()
        {
            _fakeVideoSource = Substitute.For<IVideoSource>();
            _timer = new TimerClock(100);
            _barcodeReader = new ReadBarcode();
            _camConnection = new CameraConnection(_timer, _barcodeReader, _fakeVideoSource);
            _soundPlayer = new SoundPlayer();

            _sut = new CameraViewModel(_camConnection, _soundPlayer);
        }

       // [Test]
      //  public void CameraVM_FoundBarcode_()
      //  {
       //     _sut.BarcodeFoundEventToViewModels += Raise.EventWith();
        //    _fakeVideoSource.Received(1).Start();
        //}
    }
}
