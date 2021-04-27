using System;
using System.Drawing;
using NUnit.Framework;
using PantryPassionGUI.Models;

namespace PantryPassion.Test.Unit.ModelTest
{
    public class ReadBarcodeTest
    {
        private ReadBarcode _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new ReadBarcode();
        }

        [Test]
        public void ReadBarcode_GetBarcode_CorrectValue()
        {
            Bitmap myBitmap = new Bitmap(Environment.CurrentDirectory + @"\barcode.png");
            myBitmap.Save("myBitmap.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            Assert.That(_uut.GetBarcode(myBitmap), Is.EqualTo("705632085943"));
        }

        [Test]
        public void ReadBarcode_GetBarcode_RetunsNull()
        {
            _uut.Deactivate();
            Bitmap myBitmap = new Bitmap(Environment.CurrentDirectory + @"\barcode.png");
            myBitmap.Save("myBitmap.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            Assert.That(_uut.GetBarcode(myBitmap), Is.Null);
        }



        [Test]
        public void ReadBarcode_Activate_Correct()
        {
            _uut.Activate();
            Assert.That(_uut.ActivateBool, Is.EqualTo(true));
        }

        [Test]
        public void ReadBarcode_Deactivate_Correct()
        {
            _uut.Deactivate();
            Assert.That(_uut.ActivateBool, Is.EqualTo(false));
        }
    }
}
