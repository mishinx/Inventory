using System.Windows;
using System.Windows.Controls;

namespace Wpf_Inventarium
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            CreateWindow win = new CreateWindow();
            win.Show();
        }

        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindowAdmin win = new MainWindowAdmin();
            win.Show();
        }
    }
}
