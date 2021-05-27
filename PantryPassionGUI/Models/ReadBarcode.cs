using System.Diagnostics;
using System.Drawing;
using ZXing;
using IBarcodeReader = PantryPassionGUI.Models.Interfaces.IBarcodeReader;

namespace PantryPassionGUI.Models
{
    public class ReadBarcode : IBarcodeReader
    {
        //uses BarcodeReader in ZXing library 
        private BarcodeReader _reader;
        public bool ActivateBool { get; private set; }

        public ReadBarcode()
        {
            _reader = new BarcodeReader();
            ActivateBool = true;
        }

        //Get Barcode in image and return it as a string
        public string GetBarcode(Bitmap image)
        {
            //It reads only the barcode if it is activated
            if (ActivateBool == true)
            {
              
                var barcode = _reader.Decode(image);

                if (barcode != null)
                {
                    return barcode.ToString();
                }
            }

            return null;
        }

        public void Deactivate()
        {
            ActivateBool = false;
        }

        public void Activate()
        {
            ActivateBool = true;
        }
    }
}
