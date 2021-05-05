using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PantryPassionGUI.Models;

namespace PantryPassionGUI.ViewModels
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
