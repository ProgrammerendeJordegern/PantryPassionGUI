using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PantryPassionGUI.Utilities;
using PantryPassionGUI.Utilities.Interfaces;
using PantryPassionGUI.ViewModels.Interfaces;
using PantryPassionGUI.Views;

namespace PantryPassionGUI.ViewModels
{
    public class ScanEANViewModel
    {
        public ICameraViewModel CameraViewModel { get; private set; }
        private FindItemViewModel FIV1;
        private IOutput _output;


        public ScanEANViewModel(FindItemViewModel FIV2)
        {
            FIV1 = FIV2;
            CameraViewModel = new CameraViewModel();
            CameraViewModel.BarcodeFoundEventToViewModels += CloseScanWindow;
            _output = new Output();
        }

        public ScanEANViewModel(FindItemViewModel FIV2, IOutput output, ICameraViewModel cameraViewModel)
        {
            FIV1 = FIV2;
            CameraViewModel = cameraViewModel;
            CameraViewModel.BarcodeFoundEventToViewModels += CloseScanWindow;
            _output = output;
        }

        public void CloseScanWindow(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => { FIV1.EANFilter = CameraViewModel.Barcode; }));
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Application.Current.Windows[Application.Current.Windows.Count - 1].Close();
            }));

            _output.OutputLine("ScanEANWindow closed");
        }
    }
}
