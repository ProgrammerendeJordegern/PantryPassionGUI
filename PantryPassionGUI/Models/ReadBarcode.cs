﻿using System.Diagnostics;
using System.Drawing;
using ZXing;

namespace PantryPassionGUI.Models
{
    public class ReadBarcode : IBarcodeReader
    {
        private BarcodeReader _reader;
        public bool ActivateBool { get; private set; }

        public ReadBarcode()
        {
            _reader = new BarcodeReader();
            ActivateBool = true;
        }

        public string GetBarcode(Bitmap image)
        {
            
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
