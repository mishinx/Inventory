using BusinessLogic;
using System.Windows;

namespace Wpf_Inventarium
{
    /// <summary>
    /// Interaction logic for Create_window.xaml
    /// </summary>
    public partial class CreateWindow : Window
    {
        AdministratorRepository admin_repo = new AdministratorRepository();
        public CreateWindow()
        {
            InitializeComponent();
        }

        private void ButtonAuthorization_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();
            Close();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Close();
        }

        private void Confirm_Registration_Button_Click(object sender, RoutedEventArgs e)
        {
            string companyName = textBoxOrganization.Text;
            string username = textBoxLogin.Text;
            string password = passwordBox.Password;
            AdministratorService admin_service = new AdministratorService(admin_repo);
            if (!InputValidator.IsCompanyNameValid(companyName))
            {
                MessageBox.Show("Назва компанії повинна мати довжину від 2 до 18 символів." +
                "\nНе може містити спецільних символів." +
                "\nНе може містити цифр.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!InputValidator.IsEmailValid(username))
            {
                MessageBox.Show("Ім'я користувача повинне бути у формі електронної пошти (example@example.com).", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!InputValidator.IsPasswordValid(password))
            {
                MessageBox.Show("Пароль повинен мати довжину від 8 до 20 символів." +
                    "\nПовинен містити як мінімум 1 велику літеру." +
                    "\nПовинен містити як мінімум 1 малу літеру." +
                    "\nПовинен містити як мінімум 1 цифру.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (admin_service.RegisterAdministrator(companyName, username, password) != null)
            {   
                MessageBox.Show("Вхід в систему успішний!");
                MainWindowAdmin win = new MainWindowAdmin();
                Close();
                win.Show();
            }
            else
            {
                MessageBox.Show("Спробуйте ще раз.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}