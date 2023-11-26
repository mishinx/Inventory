using BusinessLogic;
using DB;
using Inventory_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wpf_Inventarium
{
    /// <summary>
    /// Interaction logic for MainWindowOperator.xaml
    /// </summary>
    public partial class MainWindowOperator : Window
    {
        OperatorRepository operator_repo = new OperatorRepository();
        GoodsRepository goods_repo = new GoodsRepository();

        public MainWindowOperator()
        {
            InitializeComponent();
            GoodsService goods_service = new GoodsService(goods_repo);
            OperatorService operator_service = new OperatorService(operator_repo);
            List<Goods> allGoods = goods_service.GetAllGoodsForOperator(operator_service.GetOperatorByEmail(MainWindow.username).operator_id);

            foreach (var goods in allGoods)
            {
                AddGoodsGrid(goods);
            }
            this.MinWidth = 816;
            this.MinHeight = 470;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Close();
        }

        private void MenuToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            MenuPopup.IsOpen = true;
        }

        private void buttonHomePage_Click(object sender, RoutedEventArgs e)
        {
            CloseMenu();
        }

        private void buttonYourProfile_Click(object sender, RoutedEventArgs e)
        {
            EditProfileOperatorWindow win = new EditProfileOperatorWindow();
            win.Show();
            CloseMenu();
        }

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();
            Close();
        }

        private void CloseMenu()
        {
            MenuPopup.IsOpen = false;
        }

        private void ToggleButtonFilter_Checked(object sender, RoutedEventArgs e)
        {
            FilterPopup.IsOpen = true;
        }

        private void CloseMenuFilter()
        {
            FilterPopup.IsOpen = false;
        }

        private void buttonFromAtoZ_Click(object sender, RoutedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            OperatorService operator_service = new OperatorService(operator_repo);
            List<Goods> goods = goods_service.GetAllGoodsForOperator(operator_service.GetOperatorByEmail(MainWindow.username).operator_id);
            DisplayGoods(goods.OrderBy(g => g.full_name).ToList());
            CloseMenuFilter();
        }

        private void buttonFromZtoA_Click(object sender, RoutedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            OperatorService operator_service = new OperatorService(operator_repo);
            List<Goods> goods = goods_service.GetAllGoodsForOperator(operator_service.GetOperatorByEmail(MainWindow.username).operator_id);
            DisplayGoods(goods.OrderByDescending(g => g.full_name).ToList());
            CloseMenuFilter();
        }

        private void buttonCountFromLess_Click(object sender, RoutedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            OperatorService operator_service = new OperatorService(operator_repo);
            List<Goods> goods = goods_service.GetAllGoodsForOperator(operator_service.GetOperatorByEmail(MainWindow.username).operator_id);
            DisplayGoods(goods.OrderBy(g => g.quantity).ToList());
            CloseMenuFilter();
        }

        private void buttonCountFromMore_Click(object sender, RoutedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            OperatorService operator_service = new OperatorService(operator_repo);
            List<Goods> goods = goods_service.GetAllGoodsForOperator(operator_service.GetOperatorByEmail(MainWindow.username).operator_id);
            DisplayGoods(goods.OrderByDescending(g => g.quantity).ToList());
            CloseMenuFilter();
        }

        private void buttonPriceFromLess_Click(object sender, RoutedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            OperatorService operator_service = new OperatorService(operator_repo);
            List<Goods> goods = goods_service.GetAllGoodsForOperator(operator_service.GetOperatorByEmail(MainWindow.username).operator_id);
            DisplayGoods(goods.OrderBy(g => g.price).ToList());
            CloseMenuFilter();
        }

        private void buttonPriceFromMore_Click(object sender, RoutedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            OperatorService operator_service = new OperatorService(operator_repo);
            List<Goods> goods = goods_service.GetAllGoodsForOperator(operator_service.GetOperatorByEmail(MainWindow.username).operator_id);
            DisplayGoods(goods.OrderByDescending(g => g.price).ToList());
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
                OperatorService operator_service = new OperatorService(operator_repo);
                PanelGoods.Children.Clear();

                var allGoods = goods_service.GetAllGoodsForOperator(operator_service.GetOperatorByEmail(MainWindow.username).operator_id);
                DisplayGoods(allGoods);
            }
        }

        private void ButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            GoodsService goods_service = new GoodsService(goods_repo);
            Button button = sender as Button;
            GoodsButtonTag tag = button.Tag as GoodsButtonTag;

            tag.GoodsObject.quantity--;

            if (tag.GoodsObject.quantity < 0)
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

        public void AddGoodsGrid(Goods goods)
        {
            GoodsService goods_service = new GoodsService(goods_repo);

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
            imageGoods.Source = ImageFormatter.ByteArrayToBitmapImage(goods.photo);
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
            buttonMinus.Margin = new Thickness(0, 0, 26, 2);
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
            buttonPlus.Margin = new Thickness(0, 0, 61, 2);
            buttonPlus.Background = Brushes.White;
            buttonPlus.BorderThickness = new Thickness(0);
            buttonPlus.Style = (Style)FindResource("ButtonStyleCircle");
            buttonPlus.Click += ButtonPlus_Click;


            Label CountText = new Label();
            CountText.Content = "К-сть:";
            CountText.Margin = new Thickness(0, 0, 112, 0);
            CountText.HorizontalAlignment = HorizontalAlignment.Right;
            CountText.VerticalAlignment = VerticalAlignment.Bottom;
            CountText.Height = 32;
            CountText.Width = 40;

            Label Count = new Label();
            Count.Content = goods.quantity;
            Count.Margin = new Thickness(0, 0, 92, 0);
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

            labelInfo.ToolTip = goods.full_name + ": " + goods.price + "$/шт";

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

        public class GoodsButtonTag
        {
            public Label CountLabel { get; set; }
            public Goods GoodsObject { get; set; }
        }
    }
}
