using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using BusinessLogic;
using DB;
using Inventory_Context;

namespace Wpf_Inventarium
{
    /// <summary>
    /// Interaction logic for EditEmployeeWindow.xaml
    /// </summary>
    public partial class EditProfileOperatorWindow : Window
    {
        OperatorRepository operator_repo = new OperatorRepository();
        Operator operator_to_edit;
        public EditProfileOperatorWindow()
        {
            OperatorService operator_service = new OperatorService(operator_repo);
            operator_to_edit = operator_service.GetOperatorByEmail(MainWindow.username);
            InitializeComponent();
            textBoxFullName.Text = operator_to_edit.full_name;
            textBoxPhoneNumber.Text = operator_to_edit.phone_number;
            textBoxLogin.Text = operator_to_edit.email_address;
            textBoxPhoneNumber.Text = operator_to_edit.phone_number;

            byte[] photoBytes = operator_to_edit.photo;
            BitmapImage bitmapImage = ImageFormatter.ByteArrayToBitmapImage(photoBytes);
            Image photo = new Image();
            photo.Source = bitmapImage;

            buttonPhotoProfile.Content = photo;

            MinHeight = 217;
            MinWidth = 496;
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

        private void textBoxFullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            operator_to_edit.full_name = textBoxFullName.Text;
        }

        private void textBoxPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            operator_to_edit.phone_number = textBoxPhoneNumber.Text;
        }

        private void textBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (InputValidator.IsEmailValid(textBoxLogin.Text))
            {
                operator_to_edit.email_address = textBoxLogin.Text;
            }
            else
            {
                MessageBox.Show("Ім'я користувача повинне бути у формі електронної пошти (example@example.com).", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                textBoxLogin.Text = operator_to_edit.email_address;
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            OperatorService operator_service = new OperatorService(operator_repo);
            if (InputValidator.IsPasswordValid(passwordBox.Password))
            {
                operator_to_edit.operator_password = PasswordHasher.HashPassword(passwordBox.Password); 
                
                MessageBoxResult result = MessageBox.Show("Ви справді хочете змінити дані користувача?", "Запитання", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    operator_service.UpdateOperator(operator_to_edit); 
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Пароль повинен мати довжину від 8 до 20 символів." +
                    "\nПовинен містити як мінімум 1 велику літеру." +
                    "\nПовинен містити як мінімум 1 малу літеру." +
                    "\nПовинен містити як мінімум 1 цифру.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    class ImageFormatter
    { 
        public static BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;

            using (var stream = new System.IO.MemoryStream(byteArray))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                return image;
            }
        }
    }
}
