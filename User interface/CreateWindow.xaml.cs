using System.Windows;

namespace Wpf_Inventarium
{
    /// <summary>
    /// Interaction logic for Create_window.xaml
    /// </summary>
    public partial class CreateWindow : Window
    {
        public CreateWindow()
        {
            InitializeComponent();
        }

        private void ButtonAuthorization_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();
            Close();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
