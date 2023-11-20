using BusinessLogic;
using Inventory_Context;
using System.Windows;
using System.Windows.Controls;

namespace Wpf_Inventarium
{
    /// <summary>
    /// Interaction logic for EditProfileAdminWindow.xaml
    /// </summary>
    public partial class EditProfileAdminWindow : Window
    {
        AdministratorRepository admin_repo = new AdministratorRepository();
        Administrator admin_to_edit;
        public EditProfileAdminWindow()
        {
            AdministratorService admin_service = new AdministratorService(admin_repo);
            admin_to_edit = admin_service.GetAdministratorByEmail(MainWindow.username);

            InitializeComponent();
            textBoxCompanyName.Text = admin_to_edit.company_name;
            textBoxFullName.Text = admin_to_edit.full_name;
            textBoxLogin.Text = admin_to_edit.email_address;
            textBoxPhoneNumber.Text = admin_to_edit.phone_number;
        }

        private void textBoxCompanyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (InputValidator.IsCompanyNameValid(textBoxCompanyName.Text))
            {
                admin_to_edit.company_name = textBoxCompanyName.Text;
            }
            else
            {
                MessageBox.Show("Назва компанії повинна мати довжину від 2 до 18 символів." +
                "\nНе може містити спецільних символів." +
                "\nНе може містити цифр.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                textBoxCompanyName.Text = admin_to_edit.company_name;
            }
        }

        private void textBoxFullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            admin_to_edit.full_name = textBoxFullName.Text;
        }

        private void textBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (InputValidator.IsEmailValid(textBoxLogin.Text))
            {
                admin_to_edit.email_address = textBoxLogin.Text;
            }
            else
            {
                MessageBox.Show("Ім'я користувача повинне бути у формі електронної пошти (example@example.com).", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                textBoxLogin.Text = admin_to_edit.email_address;
            }
        }

        private void textBoxPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            admin_to_edit.phone_number = textBoxPhoneNumber.Text;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            AdministratorService admin_service = new AdministratorService(admin_repo);
            if (InputValidator.IsPasswordValid(passwordBox.Password))
            {
                admin_to_edit.admin_password = PasswordHasher.HashPassword(passwordBox.Password);
                MessageBoxResult result = MessageBox.Show("Ви справді хочете змінити дані користувача?", "Запитання", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    admin_service.UpdateAdministrator(admin_to_edit);
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
}
