using Microsoft.Win32;
using System;
using System.IO;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BusinessLogic;
using DB;
using Inventory_Context;
using System.Collections.Generic;

namespace Wpf_Inventarium
{
    /// <summary>
    /// Interaction logic for EditGoodsWindow.xaml
    /// </summary>
    public partial class EditGoodsWindow : Window
    {
        AdministratorRepository admin_repo = new AdministratorRepository();
        WarehouseRepository warehouse_repo = new WarehouseRepository();
        GoodsRepository goods_repo = new GoodsRepository();
        List<string> subcategories;
        string previousCategory;
        public MainWindowAdmin ParentMainWindowAdmin { get; set; }

        private Goods goods_to_edit;
        public EditGoodsWindow(Goods goods_to_edit_)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            AdministratorService admin_service = new AdministratorService(admin_repo);
            WarehouseService warehouse_service = new WarehouseService(warehouse_repo);

            goods_to_edit = goods_to_edit_;
            previousCategory = goods_to_edit_.category;
            InitializeComponent();

            ComboBoxAddress.SelectedItem = warehouse_service.GetWarehouseById(goods_to_edit_.warehouse_id_ref).addres;
            ComboBoxCategory.SelectedItem = goods_to_edit_.category;
            ComboBoxSubcategory.SelectedItem = goods_to_edit_.subcategory;
            ComboBoxCountGoods.Items.Add(goods_to_edit_.quantity.ToString());
            ComboBoxCountGoods.SelectedItem = goods_to_edit_.quantity.ToString();
            TextBoxGoodsName.Text = goods_to_edit_.full_name;
            TextBoxDescription.Text = goods_to_edit_.short_description;
            TextBoxPrice.Text = goods_to_edit_.price.ToString();
            byte[] photoBytes = goods_to_edit.photo;
            BitmapImage bitmapImage = ImageFormatter.ByteArrayToBitmapImage(photoBytes);
            Image photo = new Image();
            photo.Source = bitmapImage;

            AddPhoto.Content = photo;
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
            this.Closed += EditWindow_EditWindowClosed;
        }
        private void EditWindow_EditWindowClosed(object sender, EventArgs e)
        {
            if (ParentMainWindowAdmin != null)
            {
                MainWindowAdmin refreshedMainWindowAdmin = new MainWindowAdmin();
                refreshedMainWindowAdmin.Width = ParentMainWindowAdmin.ActualWidth;
                refreshedMainWindowAdmin.Height = ParentMainWindowAdmin.ActualHeight;
                refreshedMainWindowAdmin.Show();
                ParentMainWindowAdmin.Close();
            }

            this.Close();
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

        private void ExampleTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == goods_to_edit.full_name)
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
                textBox.Text = goods_to_edit.full_name;
                textBox.Foreground = Brushes.Gray;
            }
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
                        categoryChanged = false;
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
            GoodsService goods_service = new GoodsService(goods_repo);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Зображення|*.jpg;*.png;*.bmp;*.gif|Усі файли|*.*";
            
            byte[] imageData = goods_to_edit.photo;

            BitmapImage imageSource = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(imageData))
            {
                stream.Position = 0;
                imageSource.BeginInit();
                imageSource.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.UriCachePolicy = null;
                imageSource.StreamSource = stream;
                imageSource.EndInit();
            }
            Image image = new Image();
            image.Source = imageSource;

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                Button addButton = (Button)sender;
                string fileExtension = System.IO.Path.GetExtension(imagePath).ToLower();

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

                image.Source = new BitmapImage(new Uri(imagePath));
                goods_to_edit.photo = ImageConverter.ConvertImageToByteArray(imagePath, imageFormat);

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
                goods_to_edit.price = result;
            }
            else
            {
                MessageBox.Show("Некоректне значення ціни.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                TextBoxPrice.Text = ""; 
            }
        }

        private void TextBoxDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            goods_to_edit.short_description = TextBoxDescription.Text;
        }

        private void Save_Click(object sender, object e)
        {
            AdministratorService admin_service = new AdministratorService(admin_repo);

            string selectedQuantity = GetSelectedItemComboBoxCountGoods();
            if (selectedQuantity != null)
            {
                if (int.TryParse(selectedQuantity, out int quantity))
                {
                    goods_to_edit.quantity = quantity;
                }
            }

            string selectedCategory = GetSelectedItemComboBoxCategory();
            if (selectedCategory != null)
            {
                goods_to_edit.category = selectedCategory;
            }

            string selectedSubcategory = GetSelectedItemComboBoxSubcategory();
            if (selectedSubcategory != null)
            {
                goods_to_edit.subcategory = selectedSubcategory;
            }

            string selectedAddress = GetSelectedItemComboBoxAddress();
            if (selectedAddress != null)
            {
                WarehouseService warehouse_service = new WarehouseService(warehouse_repo);
                if (warehouse_service.GetWarehouseByAddress(GetSelectedItemComboBoxAddress()) == null)
                {
                    Warehouse warehouse = new Warehouse { addres = selectedAddress, admin_id_ref = admin_service.GetAdministratorByEmail(MainWindow.username).admin_id };
                    warehouse_service.CreateWarehouse(warehouse);
                    goods_to_edit.warehouse_id_ref = warehouse.warehouse_id;
                }
                else
                {
                    goods_to_edit.warehouse_id_ref = warehouse_service.GetWarehouseByAddress(GetSelectedItemComboBoxAddress()).warehouse_id;
                }
            }
            else
            {
                MessageBox.Show("Поле не заповнено. Спробуйте ще раз.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            GoodsService goods_service = new GoodsService(goods_repo);
            goods_service.UpdateGoods(goods_to_edit);
            Close();
        }

        private void buttonTrash_Click(object sender, RoutedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo); 
            MessageBoxResult result = MessageBox.Show("Ви справді хочете видалити товар?", "Запитання", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                goods_service.DeleteGoods(goods_to_edit.goods_id);
                Close();
            }         
        }
    }
}
