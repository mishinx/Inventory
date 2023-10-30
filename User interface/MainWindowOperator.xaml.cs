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
            this.MinWidth = 816;
            this.MinHeight = 470;
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
            CloseMenu();
        }

        private void buttonYourProfile_Click(object sender, RoutedEventArgs e)
        {

            EditProfileWindow win = new EditProfileWindow();
            win.Show();
            CloseMenu();
        }

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("buttonExit_Click Clicked");
            CloseMenu();
        }

        private void CloseMenu()
        {
            MenuPopup.IsOpen = false; 
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
