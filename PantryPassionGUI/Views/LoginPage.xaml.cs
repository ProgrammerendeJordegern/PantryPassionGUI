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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        /**
                * Login Method to handle Login Button
                * @param  object sender
                * @param  RoutedEventArgs e
                */
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            ApiOperations ops = new ApiOperations();
            string username = tbxUsername.Text;
            string password = pbxPassword.Password;


            // Kan slettes, bruges blot til test
            if (ops.CheckIfAdmin(username,password))
            {
                ShowMainMenu();
                return;
            }
            //NavigationService.Navigate(new MainWindow());

            User user = ops.AuthenticateUser(username, password);
            if (user == null)
            {
                MessageBox.Show("Invalid username or password");
                return;
            }

            Globals.LoggedInUser = user;
            MessageBox.Show("Login successful");
            ShowMainMenu();
            //NavigationService.Navigate(new DetailsPage());

            //her indsættes kode til at åbne MainWindow
        }

        /**
         * Method to direct user to Register Page
         * @param object sender
         * @param RoutedEventArgs e
         */
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
           // NavigationService.Navigate(new RegisterPage());
            var RP =new RegisterPage();
            RP.Top = this.Top;
            RP.Left = this.Left;
            RP.ShowDialog();
        }

        public void ShowMainMenu()
        {
            MainWindow MW = new MainWindow();
            Hide();
            MW.ShowDialog();
            ApiOperations op = new ApiOperations();
            op.Logout();
            Show();
            tbxUsername.Text = "";
            pbxPassword.Password = "";
        }
    }
    
}

