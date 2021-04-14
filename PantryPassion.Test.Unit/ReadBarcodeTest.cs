using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PantryPassionGUI.Models;

namespace PantryPassion.Test.Unit
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
            Bitmap myBitmap = new Bitmap(@"C:\Users\Kathrine\OneDrive - Aarhus Universitet\AU\4. Semester\PRJ4_Kode\PantryPassionGUI_V2\PantryPassion.Test.Unit\barcode.png");
            myBitmap.Save("myBitmap.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            Assert.That(_uut.GetBarcode(myBitmap), Is.EqualTo("705632085943"));
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
