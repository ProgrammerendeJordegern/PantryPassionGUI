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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PantryPassionGUI.Models;
using PantryPassionGUI.Views;

namespace PantryPassionGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        // Makes sure the camera is turned of when the program is closed. So the program can close properly
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            CameraConnection.Instance.CameraOff();
        }
    }
}
