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
        public AddEmployeeWindow()
        {
            InitializeComponent();
            this.MinHeight = 335; 
            this.MinWidth = 266;
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
    }
}
