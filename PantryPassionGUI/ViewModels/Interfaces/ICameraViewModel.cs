using System;
using System.Windows.Input;
using PantryPassionGUI.Models.Interfaces;

namespace PantryPassionGUI.ViewModels.Interfaces
{
    public interface ICameraViewModel
    {
        event EventHandler<EventArgs> BarcodeFoundEventToViewModels;
        string Barcode { get; set; }
        int CameraListIndex { get; set; }
        string CameraButtonText { get; set; }
        public ICommand TurnOffCamera { get; }
        public ICamera Camera { get; }
    }
}
