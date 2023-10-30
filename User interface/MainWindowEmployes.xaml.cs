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
    /// Interaction logic for MainWindowEmployes.xaml
    /// </summary>
    public partial class MainWindowEmployes : Window
    {
        public MainWindowEmployes()
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
            this.Hide();
            MainWindowAdmin win = new MainWindowAdmin();
            win.Height = this.ActualHeight;
            win.Width = this.ActualWidth;
            win.Show();
            CloseMenu();
        }

        private void buttonYourProfile_Click(object sender, RoutedEventArgs e)
        {
            EditProfileWindow win = new EditProfileWindow();
            win.Show();
            CloseMenu();
        }

        private void buttonEmployes_Click(object sender, RoutedEventArgs e)
        {
            CloseMenu();
        }
        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("buttonSettings_Click Clicked");
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            AddEmployeeWindow win = new AddEmployeeWindow();
            win.Show();
        }
        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            Grid gridEmployee = new Grid();
            gridEmployee.HorizontalAlignment = HorizontalAlignment.Stretch;



            Border imageGoodsBorder = new Border();
            imageGoodsBorder.Width = 112;
            imageGoodsBorder.Height = 112;
            imageGoodsBorder.Margin = new Thickness(11, 0, 0, 0);
            imageGoodsBorder.BorderThickness = new Thickness(1);
            imageGoodsBorder.BorderBrush = Brushes.Black;
            imageGoodsBorder.HorizontalAlignment = HorizontalAlignment.Left;

            Image imageGoods = new Image();
            imageGoods.Source = new BitmapImage(new Uri("C:\\Users\\mishi\\source\\repos\\app\\Wpf_Inventarium\\filter.png"));//треба замінити
            imageGoods.HorizontalAlignment = HorizontalAlignment.Center;

            imageGoodsBorder.Child = imageGoods;


            Label labelDescriptionEmployee = new Label();
            labelDescriptionEmployee.Content = "Новий текст";//треба замінити
            labelDescriptionEmployee.Width = double.NaN;
            labelDescriptionEmployee.Height = imageGoodsBorder.Height;


            Border labelDescriptionEmployeeBorder = new Border();
            labelDescriptionEmployeeBorder.Height = labelDescriptionEmployee.Height;
            labelDescriptionEmployeeBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
            labelDescriptionEmployeeBorder.Margin = new Thickness(123, 0, 10, 0);
            labelDescriptionEmployeeBorder.BorderThickness = new Thickness(0, 1, 1, 1);
            labelDescriptionEmployeeBorder.BorderBrush = Brushes.Black;


            labelDescriptionEmployeeBorder.Child = labelDescriptionEmployee;


            Grid gridForlabelDescriptionEmployee = new Grid();
            gridForlabelDescriptionEmployee.HorizontalAlignment = HorizontalAlignment.Stretch;
            gridForlabelDescriptionEmployee.Margin = new Thickness(0, 0, 0, 0);
            gridForlabelDescriptionEmployee.Children.Add(labelDescriptionEmployeeBorder);


            gridEmployee.Children.Add(imageGoodsBorder);
            gridEmployee.Children.Add(gridForlabelDescriptionEmployee);
            gridEmployee.Margin = new Thickness(0, 15, 0, 0);

            PanelEmployes.Children.Add(gridEmployee);
        }
    }
}
