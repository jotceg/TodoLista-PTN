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
using TodoLista.Scripts.LoginScripts;

namespace TodoLista
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    
    public partial class MainWindow : Window
    {
        public List<TasksList> tasksLists = State.User.TasksLists;
        public MainWindow()
        {
            InitializeComponent();
        }

            TasksListsItemsControl.ItemsSource = tasksLists;
            
            UserNameTextBox.Text += State.User.Login;
            
            {//Hiding Backgrounds
                UserNameTextBox.Background.Opacity = 0;
                UserNameTextBox.BorderBrush.Opacity = 0;
                TasksListBox.Background.Opacity = 0;
                TasksListBox.BorderBrush.Opacity = 0;
                TopNameTextBox.Background.Opacity = 0;
                TopNameTextBox.BorderBrush.Opacity = 0;
                //OptionsTreeView.Background.Opacity = 0;
                //OptionsTreeView.BorderBrush.Opacity = 0;
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

        private void OpenTasksList(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            int id = int.Parse(clickedButton.Uid);
        }
    }
}