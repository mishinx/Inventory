using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;
using BusinessLogic;
using DB;
using Inventory_Context;


namespace Wpf_Inventarium
{
    /// <summary>
    /// Interaction logic for AddGoods.xaml
    /// </summary>

    public partial class AddGoods : Window
    {
        AdministratorRepository admin_repo = new AdministratorRepository();
        OperatorRepository operator_repo = new OperatorRepository();
        WarehouseRepository warehouse_repo = new WarehouseRepository();
        GoodsRepository goods_repo = new GoodsRepository();
        private List<string> subcategories;
        private Goods new_goods = new Goods();
        public MainWindowAdmin ParentMainWindowAdmin { get; set; }

        public MainWindowOperator ParentMainWindowOperator { get; set; }
        private string previousCategory = "Додайте чи Виберіть з існуючих";
        
        public AddGoods()
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            AdministratorService admin_service = new AdministratorService(admin_repo);
            WarehouseService warehouse_service = new WarehouseService(warehouse_repo);

            InitializeComponent();

            foreach (string category in goods_service.GetCategoriesForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id))
            {
                ComboBoxCategory.Items.Add(category);
            }
            ComboBoxCategory.DisplayMemberPath = ".";
            foreach (Warehouse warehouse in warehouse_service.GetWarehousesForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id))
            {
                ComboBoxAddress.Items.Add(warehouse.addres);
            }
            ComboBoxAddress.DisplayMemberPath = ".";

            this.Closed += AddWindow_AddWindowClosed;
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

        private string GetSelectedItemComboBoxCountGoods()
        {
            return ComboBoxCountGoods.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
        }

        private string GetSelectedItemComboBoxAddress()
        {
            return ComboBoxAddress.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
        }

        private string GetSelectedItemComboBoxCategory()
        {
            return ComboBoxCategory.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
        }

        private string GetSelectedItemComboBoxSubcategory()
        {
            return ComboBoxSubcategory.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
        }

        private void TextBoxGoodsName_TextChanged(object sender, object e)
        {
            new_goods.full_name = TextBoxGoodsName.Text;
        }

        private void ComboBox_CategoryChanged(object sender, SelectionChangedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            AdministratorService admin_service = new AdministratorService(admin_repo);
            string selectedCategory = GetSelectedItemComboBoxCategory();
            if (!string.IsNullOrEmpty(selectedCategory))
            {
                if (selectedCategory == "Додати")
                {
                    ComboBox_SelectionChanged(sender, e);
                }
                else
                {
                    bool categoryChanged = selectedCategory != previousCategory;
                    if (categoryChanged)
                    {
                        //ComboBoxSubcategory = new ComboBox();
                        subcategories = new List<string>();
                        previousCategory = selectedCategory;
                        subcategories = goods_service.GetSubCategoriesForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id, selectedCategory);
                        foreach (string sub_category in subcategories)
                        {
                            ComboBoxSubcategory.Items.Add(sub_category);
                        }
                        ComboBoxCategory.SelectedItem = selectedCategory;
                    }
                }
            }
        }

        private void ComboBox_SubcategoryChanged(object sender, SelectionChangedEventArgs e) 
        {
            string selectedSubcategory = GetSelectedItemComboBoxSubcategory();
            if (selectedSubcategory == "Додати")
            {
                ComboBox_SelectionChanged(sender, e);
            }
            else
            {
                ComboBoxSubcategory.SelectedItem = selectedSubcategory;
            }
        }

        private void ComboBox_AddressChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedAddress = GetSelectedItemComboBoxAddress();
            if (selectedAddress == "Додати")
            {
                ComboBox_SelectionChanged(sender, e);
            }
            else
            {
                ComboBoxAddress.SelectedItem = selectedAddress;
            }
        }

        private void ComboBox_CountGoodsChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedCount = GetSelectedItemComboBoxCountGoods();
            if (selectedCount == "Додати")
            {
                ComboBox_SelectionChanged(sender, e);
            }
            else
            {
                ComboBoxCountGoods.SelectedItem = selectedCount;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedItem = (comboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (selectedItem != null && selectedItem == "Додати")
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
            else
            {
                comboBox.SelectedItem = selectedItem;
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
                string fileExtension = Path.GetExtension(imagePath).ToLower();

                ImageFormat imageFormat = ImageFormat.Jpeg;

                if (fileExtension == ".jpg" || fileExtension == ".jpeg")
                {
                    imageFormat = ImageFormat.Jpeg;
                }
                else if (fileExtension == ".png")
                {
                    imageFormat = ImageFormat.Png;
                }
                else if (fileExtension == ".bmp")
                {
                    imageFormat = ImageFormat.Bmp;
                }
                else if (fileExtension == ".gif")
                {
                    imageFormat = ImageFormat.Gif;
                }

                Image image = new Image();
                image.Source = new BitmapImage(new Uri(imagePath));
                new_goods.photo = ImageConverter.ConvertImageToByteArray(imagePath, imageFormat);

                Grid grid = new Grid();
                grid.Children.Add(image);

                grid.HorizontalAlignment = HorizontalAlignment.Center;
                grid.VerticalAlignment = VerticalAlignment.Center;

                addButton.Content = grid;
            }
        }

        private void TextBoxPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (decimal.TryParse(TextBoxPrice.Text, out decimal result))
            {
                new_goods.price = result;
            }
            else
            {
                MessageBox.Show("Некоректне значення ціни.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                TextBoxPrice.Text = "";
            }
        }

        private void TextBoxDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            new_goods.short_description = TextBoxDescription.Text;
        }

        private void Save_Click(object sender, object e)
        {
            AdministratorService admin_service = new AdministratorService(admin_repo);
            string selectedQuantity = GetSelectedItemComboBoxCountGoods();
            if (selectedQuantity != null)
            {
                if (int.TryParse(selectedQuantity, out int quantity))
                {
                    new_goods.quantity = quantity;
                }
            }
            string selectedCategory = GetSelectedItemComboBoxCategory();
            if (selectedCategory != null)
            {
                new_goods.category = selectedCategory;
            }
            string selectedSubcategory = GetSelectedItemComboBoxSubcategory();
            if (selectedSubcategory != null)
            {
                new_goods.subcategory = selectedSubcategory;
            }

            string selectedAddress = GetSelectedItemComboBoxAddress();
            if (selectedAddress != null)
            {
                WarehouseService warehouse_service = new WarehouseService(warehouse_repo);
                if (warehouse_service.GetWarehouseByAddress(GetSelectedItemComboBoxAddress()) == null)
                {
                    Warehouse warehouse = new Warehouse { addres = selectedAddress, admin_id_ref = admin_service.GetAdministratorByEmail(MainWindow.username).admin_id };
                    warehouse_service.CreateWarehouse(warehouse);
                    new_goods.warehouse_id_ref = warehouse.warehouse_id;
                }
                else
                {
                    new_goods.warehouse_id_ref = warehouse_service.GetWarehouseByAddress(GetSelectedItemComboBoxAddress()).warehouse_id;
                }
            }
            else
            {
                MessageBox.Show("Поле не заповнено. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (new_goods.photo == null)
            {
                MessageBox.Show("Фото повинно бути вибране. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            GoodsService goods_service = new GoodsService(goods_repo);
            goods_service.CreateGoods(new_goods);
            if (ParentMainWindowAdmin != null)
            {
                ParentMainWindowAdmin.AddGoodsGrid(new_goods);
            }
            else if (ParentMainWindowOperator != null) 
            {
                ParentMainWindowOperator.AddGoodsGrid(new_goods);
            }
            Close();
        }
        private void AddWindow_AddWindowClosed(object sender, EventArgs e)
        {
            Close();
        }
    }
}