using System.Drawing;

namespace PantryPassionGUI.Models
{
    public interface IBarcodeReader
    {
        bool ActivateBool { get; }
        string GetBarcode(Bitmap image);
        void Deactivate();
        void Activate();
    } 
    
}
