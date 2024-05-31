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
using TodoLista.Scripts.Tasks;

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
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            {//Hiding Backgrounds
                UserNameTextBox.Background.Opacity = 0;
                UserNameTextBox.BorderBrush.Opacity = 0;
                TasksListBox.Background.Opacity = 0;
                TasksListBox.BorderBrush.Opacity = 0;
                TopNameTextBox.Background.Opacity = 0;
                TopNameTextBox.BorderBrush.Opacity = 0;
                TaskListsTreeView.Background.Opacity = 0;
                TaskListsTreeView.BorderBrush.Opacity = 0;
            }
            {//Font Test
                TasksListBox.Items.Add("1. Umyć naczynia");
                TasksListBox.Items.Add("    - Zacząć od talerzy");
                TopNameTextBox.Text = "Prace domowe";
                UserNameTextBox.Text = "Michał Kaniewski";
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            EditTaskWindow editTaskWindow = new EditTaskWindow();
            editTaskWindow.Show();
        }


        // Login And Registration Window 

        private void OpenLoginWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginAndRegistrationWindow loginAndRegistrationWindow = new LoginAndRegistrationWindow();
            loginAndRegistrationWindow.Show();
        }
    }
}