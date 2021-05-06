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

namespace PantryPassionGUI.Views
{
    /// <summary>
    /// Interaction logic for ShoppinglistView.xaml
    /// </summary>
    public partial class ShoppinglistView : Window
    {
        public ShoppinglistView()
        {
            InitializeComponent();
        }

        private void CloseWindowOnOK(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CloseWindowOnCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
