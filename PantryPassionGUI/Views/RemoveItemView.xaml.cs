using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace PantryPassionGUI.Views
{
    /// <summary>
    /// Interaction logic for RemoveItemView.xaml
    /// </summary>
    public partial class RemoveItemView : Window
    {
        public RemoveItemView()
        {
            InitializeComponent();
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
