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
using PantryPassionGUI.Views;

namespace PantryPassionGUI
{
    /// <summary>
    /// Interaction logic for FindItemWindow.xaml
    /// </summary>
    public partial class FindItemWindow : UserControl
    {
        public FindItemWindow()
        {
            InitializeComponent();
            //this.PreviewKeyDown += new KeyEventHandler(CloseOnEscape);
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            
        }

        //ikke den foretrukkede metode, men det andet virkede ikke :( 
        // = MVVM overholdes ikke 
        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    DialogResult = true;
        //}

        //private void CloseOnEscape(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Escape)
        //        Close();
        //}
    } 
}
