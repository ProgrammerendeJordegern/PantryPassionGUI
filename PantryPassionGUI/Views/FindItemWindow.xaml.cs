using System.Windows;
using System.Windows.Controls;

namespace PantryPassionGUI.Views
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

        private void CloseWindowOK(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
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
