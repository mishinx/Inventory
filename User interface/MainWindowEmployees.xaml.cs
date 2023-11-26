using BusinessLogic;
using DB;
using Inventory_Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wpf_Inventarium
{
    /// <summary>
    /// Interaction logic for MainWindowEmployes.xaml
    /// </summary>
    public partial class MainWindowEmployees : Window
    {
        AdministratorRepository admin_repo = new AdministratorRepository();
        OperatorRepository operator_repo = new OperatorRepository();
        WarehouseRepository warehouse_repo = new WarehouseRepository();
        GoodsRepository goods_repo = new GoodsRepository();

        public MainWindowEmployees()
        {
            InitializeComponent();

            OperatorService operator_service = new OperatorService(operator_repo);
            AdministratorService admin_service = new AdministratorService(admin_repo);

            List<Operator> operator_employees = operator_service.GetAllOperatorsForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id);
            List<Administrator> admin_employees = admin_service.GetAdministratorsByCompanyName(admin_service.GetAdministratorByEmail(MainWindow.username).company_name);

            foreach (var admin in admin_employees)
            {
                if (admin.email_address != MainWindow.username)
                {
                    AddEmployeesGrid(admin);
                }
            }

            foreach (var @operator in operator_employees)
            {
                AddEmployeesGrid(@operator);
            }
            this.MinWidth = 816;
            this.MinHeight = 470;
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Close();
        }

        private void MenuToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            MenuPopup.IsOpen = true;
        }

        private void buttonHomePage_Click(object sender, RoutedEventArgs e)
        {
            MainWindowAdmin win = new MainWindowAdmin();
            win.Height = this.ActualHeight;
            win.Width = this.ActualWidth;
            win.Show();
            Close();
            CloseMenu();
        }

        private void buttonYourProfile_Click(object sender, RoutedEventArgs e)
        {
            EditProfileAdminWindow win = new EditProfileAdminWindow();
            win.Show();
            CloseMenu();
        }

        private void buttonEmployes_Click(object sender, RoutedEventArgs e)
        {
            CloseMenu();
        }

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();
            Close();
            CloseMenu();
        }

        private void CloseMenu()
        {
            MenuPopup.IsOpen = false;
        }

        private void CloseMenuFilter()
        {
            FilterPopup.IsOpen = false;
        }

        private void ToggleButtonFilter_Checked(object sender, RoutedEventArgs e)
        {
            FilterPopup.IsOpen = true;
        }

        private void buttonFromAtoZ_Click(object sender, RoutedEventArgs e)
        {
            PanelEmployes.Children.Clear();

            AdministratorService admin_service = new AdministratorService(admin_repo);
            OperatorService operator_service = new OperatorService(operator_repo);
            var operators = operator_service.GetAllOperatorsForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id);
            var admins = admin_service.GetAdministratorsByCompanyName(admin_service.GetAdministratorByEmail(MainWindow.username).company_name).Where(a => a.email_address != MainWindow.username).ToList();

            var sortedAdmins = admins.OrderBy(a => a.full_name).Cast<IEmployee>().ToList();
            var sortedOperators = operators.OrderBy(a => a.full_name).Cast<IEmployee>().ToList();
            DisplayEmployees(sortedAdmins);
            DisplayEmployees(sortedOperators);

            CloseMenuFilter();
        }

        private void buttonFromZtoA_Click(object sender, RoutedEventArgs e)
        {
            PanelEmployes.Children.Clear();

            AdministratorService admin_service = new AdministratorService(admin_repo);
            OperatorService operator_service = new OperatorService(operator_repo);
            var operators = operator_service.GetAllOperatorsForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id);
            var admins = admin_service.GetAdministratorsByCompanyName(admin_service.GetAdministratorByEmail(MainWindow.username).company_name).Where(a => a.email_address != MainWindow.username).ToList();

            var sortedAdmins = admins.OrderByDescending(a => a.full_name).Cast<IEmployee>().ToList();
            var sortedOperators = operators.OrderByDescending(a => a.full_name).Cast<IEmployee>().ToList();
            DisplayEmployees(sortedAdmins);
            DisplayEmployees(sortedOperators);
            CloseMenuFilter();
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

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string searchTerm = SearchTextBox.Text.Trim();
                DisplayFilteredEmployees(searchTerm);
            }
        }

        private void DisplayEmployees(List<IEmployee> employees_to_display)
        {
            foreach (var employee in employees_to_display)
            {
                AddEmployeesGrid(employee);
            }
        }

        private void DisplayFilteredEmployees(string searchTerm)
        {
            OperatorService operator_service = new OperatorService(operator_repo);
            AdministratorService admin_service = new AdministratorService(admin_repo);
            PanelEmployes.Children.Clear();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var filteredOperators = operator_service.GetFilteredOperatorsForAdministrator(searchTerm, admin_service.GetAdministratorByEmail(MainWindow.username).admin_id);
                var filteredAdmins = admin_service.GetFilteredAdmins(searchTerm, admin_service.GetAdministratorByEmail(MainWindow.username).company_name).Where(a => a.email_address != MainWindow.username).ToList();
                DisplayEmployees(filteredAdmins.Cast<IEmployee>().ToList());
                DisplayEmployees(filteredOperators.Cast<IEmployee>().ToList());
            }
            else
            {
                var operators_for_admin = operator_service.GetAllOperatorsForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id);
                var admins_for_admin = admin_service.GetAdministratorsByCompanyName(admin_service.GetAdministratorByEmail(MainWindow.username).company_name).Where(a => a.email_address != MainWindow.username).ToList();
                DisplayEmployees(admins_for_admin.Cast<IEmployee>().ToList());
                DisplayEmployees(operators_for_admin.Cast<IEmployee>().ToList());
            }
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow win = new AddEmployeeWindow();
            win.ParentMainWindowEmployee = this;
            win.Show();
        }

        public void AddEmployeesGrid(IEmployee employee)
        {
            Operator operatorEmployee = employee as Operator;

            WarehouseService warehouse_service = new WarehouseService(warehouse_repo);

            Grid gridEmployee = new Grid();
            gridEmployee.HorizontalAlignment = HorizontalAlignment.Stretch;

            Label labelDescriptionEmployee = new Label();
            Border photoEmployeeBorder = new Border();
            photoEmployeeBorder.Width = 112;
            photoEmployeeBorder.Height = 112;
            photoEmployeeBorder.Margin = new Thickness(11, 0, 0, 0);
            photoEmployeeBorder.BorderThickness = new Thickness(1);
            photoEmployeeBorder.BorderBrush = Brushes.Black;
            photoEmployeeBorder.HorizontalAlignment = HorizontalAlignment.Left;
            Image photoOperator = new Image();

            photoOperator.Source = new BitmapImage(new Uri("pack://application:,,,/icons/employee_icon.png"));
            photoOperator.HorizontalAlignment = HorizontalAlignment.Center;

            if (operatorEmployee != null)
            {
                photoOperator.HorizontalAlignment = HorizontalAlignment.Center;

                labelDescriptionEmployee.Content = operatorEmployee.full_name + " - " + warehouse_service.GetWarehouseById(operatorEmployee.warehouse_id_ref).addres;

            }
            else
            {
                Administrator adminEmployee = employee as Administrator;
                labelDescriptionEmployee.Content = adminEmployee.full_name + " - " + adminEmployee.phone_number;
                gridEmployee.Margin = new Thickness(0, 15, 0, 0);
            }

            photoEmployeeBorder.Child = photoOperator;
            photoEmployeeBorder.Child = photoOperator;
            labelDescriptionEmployee.Width = double.NaN;
            labelDescriptionEmployee.Height = photoEmployeeBorder.Height;

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


            gridEmployee.Margin = new Thickness(0, 15, 0, 0);

            gridEmployee.Children.Add(photoEmployeeBorder);
            gridEmployee.Children.Add(gridForlabelDescriptionEmployee);
            PanelEmployes.Children.Add(gridEmployee);
        }

        public static ImageSource ConvertByteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                return image;
            }
        }
    }
}
