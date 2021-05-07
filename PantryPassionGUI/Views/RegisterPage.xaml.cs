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
using PantryPassionGUI.Operations;

namespace PantryPassionGUI.Views
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {
        public RegisterPage()
        {
            InitializeComponent();
           // WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Topmost = true;
        }

        /**
         * Register method to handle the Register Button
         * @param object sender
         * @param RoutedEventArgs e
         */
        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            string username = tbxUsername.Text;
            string password = pbxPassword.Password;
            string fullname = tbxFullName.Text;


            ApiOperations ops = new ApiOperations();
            User user = ops.RegisterUser(username, password, fullname);
            if (user == null)
            {
                MessageBox.Show("Username already exists");
                return;
            }

            Globals.LoggedInUser = user;
            MessageBox.Show("Registration successful");
            // NavigationService.Navigate(new DetailsPage());
            //NavigationService.GoBack(); // If page

        }

        /**
         * Method to handle going back to the previous screen
         * @param object  sender
         * @param RoutedEventArgs e
         */
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
         //   NavigationService.GoBack(); // if page
           Close(); // If window
        }
    }
}

