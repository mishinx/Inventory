using Microsoft.Win32;
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
    /// Interaction logic for EditEmployeeWindow.xaml
    /// </summary>
    public partial class EditProfileWindow : Window
    {
        public EditProfileWindow()
        {
            InitializeComponent();
            this.MinHeight = 217;
            this.MinWidth = 496;
        }

        private void buttonPhotoProfile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Зображення|*.jpg;*.png;*.bmp;*.gif|Усі файли|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                Button addButton = (Button)sender;

                
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(imagePath));

                
                Grid grid = new Grid();
                grid.Children.Add(image);

                
                grid.HorizontalAlignment = HorizontalAlignment.Center;
                grid.VerticalAlignment = VerticalAlignment.Center;

                
                addButton.Content = grid;
            }
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            textBoxPhoneNumber.IsEnabled = true;
        }
    }
}
