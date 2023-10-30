using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Wpf_Inventarium
{
    /// <summary>
    /// Interaction logic for AddGoods.xaml
    /// </summary>

    public partial class AddGoods : Window
    {
        public AddGoods()
        {
            InitializeComponent();
            this.MinHeight = 500;
            this.MinWidth = 450;
        }
        private void ExampleTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Наприклад, iPhone 15")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }
        }

        private void ExampleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Наприклад, iPhone 15";
                textBox.Foreground = Brushes.Gray;
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

            if (selectedItem.Content.ToString() == "Додати")
            {

                TextBox textBox = new TextBox();
                textBox.Width = 200;
                textBox.VerticalAlignment = VerticalAlignment.Center;
                textBox.HorizontalAlignment = HorizontalAlignment.Center;
                textBox.Margin = new Thickness(0, 0, 0, 100);


                this.ContentPanel.Children.Add(textBox);


                textBox.KeyUp += (s, args) =>
                {
                    if (args.Key == Key.Enter)
                    {
                        string newItem = textBox.Text.Trim();
                        if (!string.IsNullOrEmpty(newItem) && !comboBox.Items.OfType<ComboBoxItem>().Any(item => item.Content.ToString() == newItem))
                        {
                            comboBox.Items.Insert(comboBox.Items.Count - 1, new ComboBoxItem { Content = newItem });

                            comboBox.SelectedItem = comboBox.Items[comboBox.Items.Count - 2];
                        }


                        this.ContentPanel.Children.Remove(textBox);
                    }
                };
            }
        }

        private void AddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Зображення|*.jpg;*.png;*.bmp;*.gif|Усі файли|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                Button addButton = (Button)sender;

                // Створюємо зображення
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(imagePath));

                // Створюємо Grid для розміщення зображення в центрі
                Grid grid = new Grid();
                grid.Children.Add(image);

                // Вирівнюємо зображення в центрі Grid
                grid.HorizontalAlignment = HorizontalAlignment.Center;
                grid.VerticalAlignment = VerticalAlignment.Center;

                // Встановлюємо Grid як вміст кнопки
                addButton.Content = grid;
            }
        }
    }
}
