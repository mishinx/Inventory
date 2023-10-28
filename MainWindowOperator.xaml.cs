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

namespace Wpf_Inventarium
{
    /// <summary>
    /// Interaction logic for MainWindowOperator.xaml
    /// </summary>
    public partial class MainWindowOperator : Window
    {

        public MainWindowOperator()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void MenuToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            MenuPopup.IsOpen = true;
        }

        private void buttonHomePage_Click(object sender, RoutedEventArgs e)
        {
            // Handle the click event for Option 1 here
            MessageBox.Show("buttonHomePage_Click Clicked");
        }

        private void buttonYourProfile_Click(object sender, RoutedEventArgs e)
        {
            // Handle the click event for Option 2 here
            MessageBox.Show("buttonYourProfile_Click Clicked");
            CloseMenu();
        }

        private void buttonEmployes_Click(object sender, RoutedEventArgs e)
        {
            // Handle the click event for Option 3 here
            MessageBox.Show("buttonEmployes_Click Clicked");
            CloseMenu();
        }
        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            // Handle the click event for Option 4 here
            MessageBox.Show("buttonSettings_Click Clicked");
            CloseMenu();
        }

        private void CloseMenu()
        {
            MenuPopup.IsOpen = false; // Close the menu
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Пошук")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Пошук";
                textBox.Foreground = Brushes.Gray;
            }
        }
    }
}
