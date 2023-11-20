using BusinessLogic;
using DB;
using Inventory_Context;
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
    /// Interaction logic for AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        AdministratorRepository admin_repo = new AdministratorRepository();
        OperatorRepository operator_repo = new OperatorRepository();
        WarehouseRepository warehouse_repo = new WarehouseRepository();
        GoodsRepository goods_repo = new GoodsRepository();
        private List<string> subcategories;
        Operator new_operator = new Operator();
        public MainWindowEmployes ParentMainWindowEmployee { get; set; }
        public AddEmployeeWindow()
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            AdministratorService admin_service = new AdministratorService(admin_repo);
            WarehouseService warehouse_service = new WarehouseService(warehouse_repo);

            InitializeComponent();
            this.MinHeight = 335; 
            this.MinWidth = 266;
            foreach (Warehouse warehouse in warehouse_service.GetWarehousesForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id))
            {
                comboBoxWarehouse.Items.Add(warehouse.addres);
            }
            comboBoxWarehouse.DisplayMemberPath = ".";
        }

        private string GetSelectedItemComboBoxWarehouse()
        {
            return comboBoxWarehouse.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
        }
        private void ComboBox_Warehouse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedAddress = GetSelectedItemComboBoxWarehouse();
            if (selectedAddress == "Додати")
            {
                ComboBox_SelectionChanged(sender, e);
            }
            else
            {
                comboBoxWarehouse.SelectedItem = selectedAddress;
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

            if (selectedItem.Content.ToString() == "Додати")
            {
                TextBox textBox = new TextBox();
                textBox.VerticalAlignment = VerticalAlignment.Center;
                textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                textBox.Margin = new Thickness(10, 0, 10, 102);

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

        private void checkBoxAdminPowers_Checked(object sender, RoutedEventArgs e)
        {
            comboBoxWarehouse.IsEnabled = false;
        }
        private void checkBoxAdminPowers_Unchecked(object sender, RoutedEventArgs e)
        {
            comboBoxWarehouse.IsEnabled = true;
        }

        private void textBoxNameEmmployee_TextChanged(object sender, TextChangedEventArgs e)
        {
            new_operator.full_name = textBoxNameEmmployee.Text;
        }

        private void textBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            new_operator.email_address = textBoxLogin.Text;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            AdministratorService admin_service = new AdministratorService(admin_repo);
            string password = passwordBox.Password;
            if (!InputValidator.IsPasswordValid(password))
            {
                MessageBox.Show("Пароль повинен мати довжину від 8 до 20 символів." +
                    "\nПовинен містити як мінімум 1 велику літеру." +
                    "\nПовинен містити як мінімум 1 малу літеру." +
                    "\nПовинен містити як мінімум 1 цифру.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if ((bool)!checkBoxAdminPowers.IsChecked)
                {
                    OperatorService operator_service = new OperatorService(operator_repo);
                    WarehouseService warehouse_service = new WarehouseService(warehouse_repo);
                    string selectedAddress = GetSelectedItemComboBoxWarehouse();
                    if (selectedAddress != null)
                    {
                        if (warehouse_service.GetWarehouseByAddress(GetSelectedItemComboBoxWarehouse()) == null)
                        {
                            Warehouse warehouse = new Warehouse { addres = selectedAddress, admin_id_ref = admin_service.GetAdministratorByEmail(MainWindow.username).admin_id };
                            warehouse_service.CreateWarehouse(warehouse);
                            new_operator.warehouse_id_ref = warehouse.warehouse_id;
                        }
                        else
                        {
                            new_operator.warehouse_id_ref = warehouse_service.GetWarehouseByAddress(GetSelectedItemComboBoxWarehouse()).warehouse_id;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Поле не заповнено. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    operator_service.RegisterOperator(new_operator.email_address, password, new_operator.full_name, new_operator.warehouse_id_ref, admin_service.GetAdministratorByEmail(MainWindow.username).admin_id);

                    Close();
                }
                else
                {
                    admin_service.RegisterAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).company_name, new_operator.email_address, password);
                    Close();
                }
            }
        }        
    }
}
