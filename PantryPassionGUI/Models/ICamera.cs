using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace PantryPassionGUI.Models
{
    public class BarcodeFoundEventArgs : EventArgs
    {
        public string Barcode { get; set; }
    }

    public interface ICamera
    {
        event EventHandler<BarcodeFoundEventArgs> BarcodeFoundEvent;
        void CameraOn();
        void CameraOff();
        void SetCameraListIndex(int index);
        int GetCameraListIndex();
        BitmapImage CameraFeed { get; set; }
        ObservableCollection<string> CamerasList { get; }
    }
}
