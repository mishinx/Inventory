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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Serilog;

namespace Wpf_Inventarium
{
    /// <summary>
    /// Interaction logic for MainWindowAdmin.xaml
    /// </summary>
    public partial class MainWindowAdmin : Window
    {
        AdministratorRepository admin_repo = new AdministratorRepository();
        GoodsRepository goods_repo = new GoodsRepository();
        private bool isMenuOpen = true;
        private bool isFilterOpen = true;
        ILogger _logger = LoggerManager.Instance.Logger;

        public MainWindowAdmin()
        {
            InitializeComponent();
            
            GoodsService goods_service = new GoodsService(goods_repo);
            AdministratorService admin_service = new AdministratorService(admin_repo);
            if (goods_service.GetAllGoodsForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id) != null)
            {
                List<Goods> allGoods = goods_service.GetAllGoodsForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id);

               DisplayGoods(allGoods);
            }

            this.MinWidth = 816;
            this.MinHeight = 470;
        }

        private void MenuClick(object sender, RoutedEventArgs e)
        {
            DoubleAnimation Anim = new DoubleAnimation();
            Anim.Duration = TimeSpan.FromSeconds(1);
            Anim.EasingFunction = new QuadraticEase();

            if (isMenuOpen)
            {
                Anim.From = 0;
                Anim.To = 380;
            }

            isMenuOpen = !isMenuOpen;
            MenuPopup.BeginAnimation(HeightProperty, Anim);
        }
        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isMenuOpen && !IsMouseOverPopup(e.GetPosition(MenuPopup)) && !MenuButton.IsMouseOver)
            {
                CloseMenu();
            }
            if (!isFilterOpen && !IsMouseOverFilterPopup(e.GetPosition(FilterPopup)) && !ButtonFilter.IsMouseOver)
            {
                CloseFilter();
            }
        }

        private bool IsMouseOverPopup(Point mousePosition)
        {
            Point popupPosition = MenuPopup.PointToScreen(new Point(0, 0));

            Rect popupRect = new Rect(popupPosition.X, popupPosition.Y, MenuPopup.ActualWidth, MenuPopup.ActualHeight);

            return popupRect.Contains(mousePosition);
        }

        private void CloseMenu()
        {
            DoubleAnimation Anim = new DoubleAnimation();
            Anim.Duration = TimeSpan.FromSeconds(1);
            Anim.EasingFunction = new QuadraticEase();
            Anim.From = 380;
            Anim.To = 0;
            isMenuOpen = !isMenuOpen;
            MenuPopup.BeginAnimation(HeightProperty, Anim);
        }

        private void FilterClick(object sender, RoutedEventArgs e)
        {
            DoubleAnimation Anim = new DoubleAnimation();
            Anim.Duration = TimeSpan.FromSeconds(1);
            Anim.EasingFunction = new QuadraticEase();

            if (isFilterOpen)
            {
                Anim.From = 0;
                Anim.To = 143;
            }

            isFilterOpen = !isFilterOpen;
            FilterPopup.BeginAnimation(WidthProperty, Anim);
        }

        private bool IsMouseOverFilterPopup(Point mousePosition)
        {
            Point popupPosition = FilterPopup.PointToScreen(new Point(0, 0));

            Rect popupRect = new Rect(popupPosition.X, popupPosition.Y, FilterPopup.ActualWidth, FilterPopup.ActualHeight);

            return popupRect.Contains(mousePosition);
        }

        private void CloseFilter()
        {
            DoubleAnimation Anim = new DoubleAnimation();
            Anim.Duration = TimeSpan.FromSeconds(1);
            Anim.EasingFunction = new QuadraticEase();
            Anim.From = 143;
            Anim.To = 0;
            Anim.FillBehavior = FillBehavior.Stop;
            isFilterOpen = !isFilterOpen;
            FilterPopup.BeginAnimation(WidthProperty, Anim);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MenuPopup.IsOpen = true;
            FilterPopup.IsOpen = true;
        }

        private void HomePageButton_Click(object sender, RoutedEventArgs e)
        {            
        }

        private void OwnInformationButton_Click(object sender, RoutedEventArgs e)
        {
            EditProfileAdminWindow win = new EditProfileAdminWindow();
            win.Show();            
        }

        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowEmployees win = new MainWindowEmployees();
            win.Height = this.ActualHeight;
            win.Width = this.ActualWidth;
            win.Show();
            Close();            
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Information("Користувач " + MainWindow.username + " вийшов з профілю");
            MainWindow win = new MainWindow();
            win.Show();
            Close();            
        }

        private void buttonFromAtoZ_Click(object sender, RoutedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            AdministratorService admin_service = new AdministratorService(admin_repo);
            List<Goods> goods = goods_service.GetAllGoodsForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id);
            DisplayGoods(goods.OrderBy(g => g.full_name).ToList());            
        }

        private void buttonCountFromLess_Click(object sender, RoutedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            AdministratorService admin_service = new AdministratorService(admin_repo);
            List<Goods> goods = goods_service.GetAllGoodsForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id);
            DisplayGoods(goods.OrderBy(g => g.quantity).ToList());            
        }

        private void buttonPriceFromLess_Click(object sender, RoutedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            AdministratorService admin_service = new AdministratorService(admin_repo);
            List<Goods> goods = goods_service.GetAllGoodsForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id);
            DisplayGoods(goods.OrderBy(g => g.price).ToList());            
        }

        private void buttonFromZtoA_Click(object sender, RoutedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            AdministratorService admin_service = new AdministratorService(admin_repo);
            List<Goods> goods = goods_service.GetAllGoodsForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id);
            DisplayGoods(goods.OrderByDescending(g => g.full_name).ToList());            
        }

        private void buttonCountFromMore_Click(object sender, RoutedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            AdministratorService admin_service = new AdministratorService(admin_repo);
            List<Goods> goods = goods_service.GetAllGoodsForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id);
            DisplayGoods(goods.OrderByDescending(g => g.quantity).ToList());            
        }

        private void buttonPriceFromMore_Click(object sender, RoutedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            AdministratorService admin_service = new AdministratorService(admin_repo);
            List<Goods> goods = goods_service.GetAllGoodsForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id);
            DisplayGoods(goods.OrderByDescending(g => g.price).ToList());            
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Пошук")
            {
                textBox.Text = string.Empty;
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Пошук";
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string searchTerm = SearchTextBox.Text.Trim();
                DisplayFilteredGoods(searchTerm);
            }
        }

        private void DisplayGoods(List<Goods> goods_to_display)
        {
            PanelGoods.Children.Clear();

            foreach (var goods in goods_to_display)
            {
                AddGoodsGrid(goods);
            }
        }

        private void DisplayFilteredGoods(string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                GoodsService goods_service = new GoodsService(goods_repo);
                PanelGoods.Children.Clear();

                var filteredGoods = goods_service.GetFilteredGoods(searchTerm);
                DisplayGoods(filteredGoods);
            }
            else
            {
                GoodsService goods_service = new GoodsService(goods_repo);
                AdministratorService admin_service = new AdministratorService(admin_repo);
                PanelGoods.Children.Clear();

                var allGoods = goods_service.GetAllGoodsForAdministrator(admin_service.GetAdministratorByEmail(MainWindow.username).admin_id);
                DisplayGoods(allGoods);
            }
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Close();
        }

        private void Create_Item_Button_Click(object sender, RoutedEventArgs e)
        {
            AddGoods win = new AddGoods();
            win.ParentMainWindowAdmin = this;
            win.Show();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Goods goods_to_edit = button.Tag as Goods;
            EditGoodsWindow win = new EditGoodsWindow(goods_to_edit);
            win.ParentMainWindowAdmin = this;
            win.Show();
        }

        private void ButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            Button button = sender as Button;
            GoodsButtonTag tag = button.Tag as GoodsButtonTag;

            tag.GoodsObject.quantity--;

            if (tag.GoodsObject.quantity <= 0)
            {
                tag.GoodsObject.quantity = 0;
            }

            tag.CountLabel.Content = tag.GoodsObject.quantity.ToString();
            goods_service.UpdateGoods(tag.GoodsObject);
        }

        private void ButtonPlus_Click(object sender, RoutedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            Button button = sender as Button;
            GoodsButtonTag tag = button.Tag as GoodsButtonTag;

            tag.GoodsObject.quantity++;

            tag.CountLabel.Content = tag.GoodsObject.quantity.ToString();
            goods_service.UpdateGoods(tag.GoodsObject);
        }

        private void LabelInfo_Click(object sender, RoutedEventArgs e)
        {
            Label label = sender as Label;
            Goods goods = label.Tag as Goods;

            QRCodeWindow win = new QRCodeWindow(goods);
            win.Show();
        }

        public void AddGoodsGrid(Goods goods)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            AdministratorService admin_service = new AdministratorService(admin_repo);

            Grid gridObject = new Grid();
            gridObject.HorizontalAlignment = HorizontalAlignment.Stretch;

            Border imageGoodsBorder = new Border();
            imageGoodsBorder.Width = 112;
            imageGoodsBorder.Height = 112;
            imageGoodsBorder.Margin = new Thickness(11, 0, 0, 0);
            imageGoodsBorder.BorderThickness = new Thickness(1);
            imageGoodsBorder.BorderBrush = Brushes.Black;
            imageGoodsBorder.HorizontalAlignment = HorizontalAlignment.Left;

            Image imageGoods = new Image();
            imageGoods.Source = ConvertByteArrayToImage(goods.photo);
            imageGoods.HorizontalAlignment = HorizontalAlignment.Center;

            imageGoodsBorder.Child = imageGoods;

            Label labelDescription = new Label();
            labelDescription.Content = goods.full_name + "\n" + goods.short_description;
            labelDescription.Width = double.NaN;
            labelDescription.Height = imageGoodsBorder.Height;

            Border labelDescriptionBorder = new Border();
            labelDescriptionBorder.Height = labelDescription.Height;
            labelDescriptionBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
            labelDescriptionBorder.Margin = new Thickness(123, 0, 10, 0);
            labelDescriptionBorder.BorderThickness = new Thickness(0, 1, 1, 1);
            labelDescriptionBorder.BorderBrush = Brushes.Black;

            labelDescriptionBorder.Child = labelDescription;

            Grid gridLabelButtons = new Grid();
            gridLabelButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
            gridLabelButtons.Margin = new Thickness(0, 0, 0, 0);
            gridLabelButtons.Children.Add(labelDescriptionBorder);

            Button buttonEdit = new Button();
            Image imageEdit = new Image();
            imageEdit.Source = new BitmapImage(new Uri("pack://application:,,,/icons/edit.png"));
            imageEdit.HorizontalAlignment = HorizontalAlignment.Center;
            imageEdit.VerticalAlignment = VerticalAlignment.Center;
            buttonEdit.Content = imageEdit;
            buttonEdit.Width = 32;
            buttonEdit.Height = 32;
            buttonEdit.HorizontalAlignment = HorizontalAlignment.Right;
            buttonEdit.VerticalAlignment = VerticalAlignment.Bottom;
            buttonEdit.Margin = new Thickness(0, 0, 15, 2);
            buttonEdit.Background = Brushes.White;
            buttonEdit.BorderThickness = new Thickness(0);
            buttonEdit.Style = (Style)FindResource("ButtonStyleCircle");
            buttonEdit.Tag = goods;
            buttonEdit.Click += ButtonEdit_Click;

            Button buttonMinus = new Button();
            Image imageMinus = new Image();
            imageMinus.Source = new BitmapImage(new Uri("pack://application:,,,/icons/minus.png"));
            imageMinus.HorizontalAlignment = HorizontalAlignment.Center;
            imageMinus.VerticalAlignment = VerticalAlignment.Center;
            buttonMinus.Content = imageMinus;
            buttonMinus.Width = 32;
            buttonMinus.Height = 32;
            buttonMinus.HorizontalAlignment = HorizontalAlignment.Right;
            buttonMinus.VerticalAlignment = VerticalAlignment.Bottom;
            buttonMinus.Margin = new Thickness(0, 0, 48, 2);
            buttonMinus.Background = Brushes.White;
            buttonMinus.BorderThickness = new Thickness(0);
            buttonMinus.Style = (Style)FindResource("ButtonStyleCircle");
            buttonMinus.Click += ButtonMinus_Click;

            Button buttonPlus = new Button();
            Image imagePlus = new Image();
            imagePlus.Source = new BitmapImage(new Uri("pack://application:,,,/icons/plus.png"));
            imagePlus.HorizontalAlignment = HorizontalAlignment.Center;
            imagePlus.VerticalAlignment = VerticalAlignment.Center;
            buttonPlus.Content = imagePlus;
            buttonPlus.Width = 32;
            buttonPlus.Height = 32;
            buttonPlus.HorizontalAlignment = HorizontalAlignment.Right;
            buttonPlus.VerticalAlignment = VerticalAlignment.Bottom;
            buttonPlus.Margin = new Thickness(0, 0, 83, 2);
            buttonPlus.Background = Brushes.White;
            buttonPlus.BorderThickness = new Thickness(0);
            buttonPlus.Style = (Style)FindResource("ButtonStyleCircle");
            buttonPlus.Click += ButtonPlus_Click;

            Label CountText = new Label();
            CountText.Content = "К-сть:";
            CountText.Margin = new Thickness(0, 0, 133, 0);
            CountText.HorizontalAlignment = HorizontalAlignment.Right;
            CountText.VerticalAlignment = VerticalAlignment.Bottom;
            CountText.Height = 32;
            CountText.Width = 40;

            Label Count = new Label();
            Count.Content = goods.quantity;
            Count.Margin = new Thickness(0, 0, 113, 0);
            Count.HorizontalAlignment = HorizontalAlignment.Right;
            Count.VerticalAlignment = VerticalAlignment.Bottom;
            Count.Height = 32;
            Count.Width = 20;

            GoodsButtonTag tag = new GoodsButtonTag
            {
                CountLabel = Count,
                GoodsObject = goods
            };

            buttonMinus.Tag = tag;
            buttonPlus.Tag = tag;

            Label labelInfo = new Label();
            Image imageInfo = new Image();
            imageInfo.Source = new BitmapImage(new Uri("pack://application:,,,/icons/info.png"));
            labelInfo.Content = imageInfo;
            labelInfo.HorizontalAlignment = HorizontalAlignment.Right;
            labelInfo.VerticalAlignment = VerticalAlignment.Top;
            labelInfo.Margin = new Thickness(0, 0, 10, 0);
            labelInfo.Width = 32;
            labelInfo.Height = 32;
            labelInfo.Tag = goods;
            labelInfo.MouseLeftButtonDown += LabelInfo_Click;
            labelInfo.ToolTip = goods.full_name + ": " + goods.price + "$/шт";

            gridLabelButtons.Children.Add(buttonEdit);
            gridLabelButtons.Children.Add(buttonMinus);
            gridLabelButtons.Children.Add(buttonPlus);
            gridLabelButtons.Children.Add(Count);
            gridLabelButtons.Children.Add(CountText);
            gridLabelButtons.Children.Add(labelInfo);

            gridObject.Children.Add(imageGoodsBorder);
            gridObject.Children.Add(gridLabelButtons);
            gridObject.Margin = new Thickness(0, 15, 0, 0);

            PanelGoods.Children.Add(gridObject);
        }

        private ImageSource ConvertByteArrayToImage(byte[] byteArrayIn)
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

        public class GoodsButtonTag
        {
            public Label CountLabel { get; set; }

            public Goods GoodsObject { get; set; }
        }
    }
}
