using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace PantryPassionGUI.Views
{
    /// <summary>
    /// Interaction logic for AddItemView.xaml
    /// </summary>
    public partial class AddItemView : Window
    {
        public AddItemView()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void OkExitOnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CancelExitOnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
