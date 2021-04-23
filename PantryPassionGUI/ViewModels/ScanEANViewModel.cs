using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PantryPassionGUI.Views;

namespace PantryPassionGUI.ViewModels
{
    public class ScanEANViewModel
    {
        public CameraViewModel CameraViewModel { get; private set; }
        private FindItemViewModel FIV1;


        public ScanEANViewModel(FindItemViewModel FIV2)
        {
            FIV1 = FIV2;
            CameraViewModel = new CameraViewModel();
            CameraViewModel.BarcodeFoundEventToViewModels += CloseScanWindow;
        }

        public void CloseScanWindow(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => { FIV1.EANFilter = CameraViewModel.Barcode; }));
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Application.Current.Windows[Application.Current.Windows.Count - 2].Close();
            }));
        }
    }
}
