using System.Windows;
using PantryPassionGUI.Models;

namespace PantryPassionGUI.Views
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
