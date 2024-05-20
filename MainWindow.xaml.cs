using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TodoLista
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            {//Hiding Backgrounds
                UserNameTextBox.Background.Opacity = 0;
                UserNameTextBox.BorderBrush.Opacity = 0;
                TasksListBox.Background.Opacity = 0;
                TasksListBox.BorderBrush.Opacity = 0;
                TopNameTextBox.Background.Opacity = 0;
                TopNameTextBox.BorderBrush.Opacity = 0;
                OptionsTreeView.Background.Opacity = 0;
                OptionsTreeView.BorderBrush.Opacity = 0;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("TEST");
        }
    }
}