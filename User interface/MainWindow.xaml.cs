using System.Collections.Generic;
using System.Windows;
using BusinessLogic;
using DB;

namespace Wpf_Inventarium
{
    public partial class MainWindow : Window
    {
        public static string username;
        AdministratorRepository admin_repo = new AdministratorRepository();
        OperatorRepository operator_repo = new OperatorRepository();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            username = usernameTextBox.Text;
            string password = passwordBox.Password;
            AdministratorService admin_service = new AdministratorService(admin_repo);
            OperatorService operator_service = new OperatorService(operator_repo);
            AuthenticationService authenticationService = new AuthenticationService(admin_service, operator_service);
            
            if (authenticationService.AuthenticateUser(username, password) == null)
            {
                MessageBox.Show("Поле не заповнено. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (authenticationService.AuthenticateUser(username, password)[0] && authenticationService.AuthenticateUser(username, password)[1])
            {
                MainWindowAdmin win = new MainWindowAdmin();
                win.Show();
                Close();
            }
            else if (!authenticationService.AuthenticateUser(username, password)[0] && authenticationService.AuthenticateUser(username, password)[1])
            {
                MainWindowOperator win = new MainWindowOperator();
                win.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неправильний логін або пароль. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Registration_Window_Click(object sender, RoutedEventArgs e)
        {
            CreateWindow win = new CreateWindow();
            win.Show();
            Close();
        }
    }
}
    