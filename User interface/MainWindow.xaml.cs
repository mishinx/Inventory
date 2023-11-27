using BusinessLogic;
using DB;
using System.Windows;
using Serilog;

namespace Wpf_Inventarium
{
    public partial class MainWindow : Window
    {
        public static string username;
        AdministratorRepository admin_repo = new AdministratorRepository();
        OperatorRepository operator_repo = new OperatorRepository();
        ILogger _logger = LoggerManager.Instance.Logger;

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

            _logger.Information("Спроба автентифікації");

            if (authenticationService.AuthenticateUser(username, password) == null)
            {
                _logger.Error("Помилка автентифікації");
                MessageBox.Show("Поле не заповнено. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (authenticationService.AuthenticateUser(username, password)[0] && authenticationService.AuthenticateUser(username, password)[1])
            {
                _logger.Information("Автентифікація адміністратора " + MainWindow.username + " успішна");
                MainWindowAdmin win = new MainWindowAdmin();
                win.Show();
                Close();
            }
            else if (!authenticationService.AuthenticateUser(username, password)[0] && authenticationService.AuthenticateUser(username, password)[1])
            {
                _logger.Information("Автентифікація адміністратора " + MainWindow.username + " успішна");
                MainWindowOperator win = new MainWindowOperator();
                win.Show();
                Close();
            }
            else
            {
                _logger.Error("Помилка автентифікації");
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
