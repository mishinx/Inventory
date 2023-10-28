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
    /// Interaction logic for MainWindowAdmin.xaml
    /// </summary>
    public partial class MainWindowAdmin : Window
    {
        
        public MainWindowAdmin()
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
            
            MessageBox.Show("buttonHomePage_Click Clicked");
        }

        private void buttonYourProfile_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("buttonYourProfile_Click Clicked");
            CloseMenu();
        }

        private void buttonEmployes_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("buttonEmployes_Click Clicked");
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

        private void ButtonOpenWindowAddGoods_Click(object sender, RoutedEventArgs e)
        {

            AddGoods win = new AddGoods();
            win.Show();
        }
        private void AddGoodsButton_Click(object sender, RoutedEventArgs e) //це додавання самого товару в головне вікно 
        {
            Grid gridObject = new Grid();
            gridObject.HorizontalAlignment = HorizontalAlignment.Stretch;
            

            
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

            
            Label labelDescription = new Label();
            labelDescription.Content = "Новий текст";//треба замінити
            labelDescription.Width = double.NaN; 
            labelDescription.Height = imageGoodsBorder.Height; 

            
            Border labelDescriptionBorder = new Border();
            labelDescriptionBorder.Height = labelDescription.Height;
            labelDescriptionBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
            labelDescriptionBorder.Margin = new Thickness(123,0,10,0);
            labelDescriptionBorder.BorderThickness = new Thickness(0, 1, 1, 1);
            labelDescriptionBorder.BorderBrush = Brushes.Black;


            labelDescriptionBorder.Child = labelDescription;

            
            Grid gridLabelButtons = new Grid();
            gridLabelButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
            gridLabelButtons.Margin = new Thickness(0,0,0,0);
            gridLabelButtons.Children.Add(labelDescriptionBorder);

            
            Button buttonEdit = new Button();
            Image imageEdit = new Image();
            imageEdit.Source = new BitmapImage(new Uri("C:\\Users\\mishi\\source\\repos\\app\\Wpf_Inventarium\\edit.png"));
            imageEdit.HorizontalAlignment = HorizontalAlignment.Center;
            imageEdit.VerticalAlignment = VerticalAlignment.Center;
            buttonEdit.Content = imageEdit;
            buttonEdit.Width = 32;
            buttonEdit.Height = 32;
            buttonEdit.HorizontalAlignment = HorizontalAlignment.Right;
            buttonEdit.VerticalAlignment = VerticalAlignment.Bottom;
            buttonEdit.Margin = new Thickness(0, 0, 15, 1);
            buttonEdit.Background = Brushes.White; 
            buttonEdit.BorderThickness = new Thickness(0);
            buttonEdit.Style = (Style)FindResource("ButtonStyleCircle");
            Button buttonMinus = new Button();
            Image imageMinus = new Image();
            imageMinus.Source = new BitmapImage(new Uri("C:\\Users\\mishi\\source\\repos\\app\\Wpf_Inventarium\\minus.png"));
            imageMinus.HorizontalAlignment = HorizontalAlignment.Center;
            imageMinus.VerticalAlignment = VerticalAlignment.Center;
            buttonMinus.Content = imageMinus;
            buttonMinus.Width = 32;
            buttonMinus.Height = 32;
            buttonMinus.HorizontalAlignment = HorizontalAlignment.Right;
            buttonMinus.VerticalAlignment = VerticalAlignment.Bottom;
            buttonMinus.Margin = new Thickness(0, 0, 48, 1);
            buttonMinus.Background = Brushes.White;
            buttonMinus.BorderThickness = new Thickness(0);
            buttonMinus.Style = (Style)FindResource("ButtonStyleCircle");
            Button buttonPlus = new Button();
            Image imagePlus = new Image();
            imagePlus.Source = new BitmapImage(new Uri("C:\\Users\\mishi\\source\\repos\\app\\Wpf_Inventarium\\plus.png"));
            imagePlus.HorizontalAlignment = HorizontalAlignment.Center;
            imagePlus.VerticalAlignment = VerticalAlignment.Center;
            buttonPlus.Content = imagePlus;
            buttonPlus.Width = 32;
            buttonPlus.Height = 32;
            buttonPlus.HorizontalAlignment = HorizontalAlignment.Right;
            buttonPlus.VerticalAlignment = VerticalAlignment.Bottom;
            buttonPlus.Margin = new Thickness(0,0,83,1);
            buttonPlus.Background = Brushes.White; 
            buttonPlus.BorderThickness = new Thickness(0);
            buttonPlus.Style = (Style)FindResource("ButtonStyleCircle");

            Label CountText = new Label();
            CountText.Content = "К-сть:";
            CountText.Margin = new Thickness(0,0,133,0);
            CountText.HorizontalAlignment = HorizontalAlignment.Right;
            CountText.VerticalAlignment = VerticalAlignment.Bottom;
            CountText.Height = 32;
            CountText.Width = 40;

            Label Count = new Label();
            Count.Content = "5"; // треба замінити
            Count.Margin = new Thickness(0, 0,113, 0);
            Count.HorizontalAlignment = HorizontalAlignment.Right;
            Count.VerticalAlignment = VerticalAlignment.Bottom;
            Count.Height = 32;
            Count.Width = 20;
            

            Label labelInfo = new Label();
            Image imageInfo = new Image();
            imageInfo.Source = new BitmapImage(new Uri("C:\\Users\\mishi\\source\\repos\\app\\Wpf_Inventarium\\info.png"));
            labelInfo.Content = imageInfo;
            labelInfo.HorizontalAlignment = HorizontalAlignment.Right;
            labelInfo.VerticalAlignment = VerticalAlignment.Top;
            labelInfo.Margin = new Thickness(0,0,10,0);
            labelInfo.Width = 32; 
            labelInfo.Height = 32;

            // Встановлення тексту при наведенні на i
            labelInfo.ToolTip = "Текст, який ви бачите при наведенні";//треба замінити

            gridLabelButtons.Children.Add(buttonEdit);
            gridLabelButtons.Children.Add(buttonMinus);
            gridLabelButtons.Children.Add(buttonPlus);
            gridLabelButtons.Children.Add(Count);
            gridLabelButtons.Children.Add(CountText);
            gridLabelButtons.Children.Add(labelInfo);

            gridObject.Children.Add(imageGoodsBorder);
            gridObject.Children.Add(gridLabelButtons);
            gridObject.Margin = new Thickness(0, 15, 0, 0);

            PanelGoods.Children.Add(gridObject);
        }
    }
}
