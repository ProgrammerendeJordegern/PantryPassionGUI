using System.Drawing;
using ZXing;

namespace PantryPassionGUI.Models
{
    public class ReadBarcode : IBarcodeReader
    {
        private BarcodeReader _reader;
        private bool _activate;

        public ReadBarcode()
        {
            _reader = new BarcodeReader();
            _activate = true;
        }
        
        public string GetBarcode(Bitmap image)
        {
            if (_activate == true)
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
            _activate = false;
        }

        public void Activate()
        {
            _activate = true;
        }
    }
}
