using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PantryPassionGUI.ViewModels;

namespace PantryPassionGUI.Views
{
    /// <summary>
    /// Interaction logic for ScanEANWindow.xaml
    /// </summary>
    public partial class ScanEANWindow : Window
    {
        private ScanEANViewModel ENO1;
        public ScanEANWindow(FindItemViewModel ELA1)
        {
            ENO1 = new ScanEANViewModel(ELA1);
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}
