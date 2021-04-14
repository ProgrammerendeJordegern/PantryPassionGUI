using System.Drawing;

namespace PantryPassionGUI.Models
{
    interface IBarcodeReader
    {
        string GetBarcode(Bitmap image);
        void Deactivate();
        void Activate();
    } 
    
}
