using System.Drawing;

namespace PantryPassionGUI.Models
{
    public interface IBarcodeReader
    {
        string GetBarcode(Bitmap image);
        void Deactivate();
        void Activate();
    } 
    
}
