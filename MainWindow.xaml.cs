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
using Microsoft.VisualBasic;
using TodoLista.Scripts;

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

            TasksListsItemsControl.ItemsSource = tasksLists;
            
            UserNameTextBox.Text += State.User.Login;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            EditTaskWindow editTaskWindow = new EditTaskWindow();
            editTaskWindow.Show();
            Close();
        }

        private void SignOut(object sender, RoutedEventArgs e)
        {
            LoginAndRegistrationWindow loginAndRegistrationWindow = new LoginAndRegistrationWindow();
            loginAndRegistrationWindow.Show();
            State.SignOut();
            Close();
        }

        private void OpenTasksList(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            int id = int.Parse(clickedButton.Uid);
            State.SelectedTasksListId = id;
            TasksListBox.Items.Clear();
            foreach (TasksList tasksList in tasksLists)
            {
                if (tasksList.Id == State.SelectedTasksListId)
                {
                    foreach (Scripts.Tasks.Task task in tasksList.Tasks)
                    {
                        TasksListBox.Items.Add(task.Title);
                    };
                    break;
                }
            }
        }

        private void AddTasksListButton_Click(object sender, RoutedEventArgs e)
        {
            string newTasksListName = Interaction.InputBox("Wpisz nazwę listy zadań", "Dodawanie listy zadań", "");
            
            DatabaseManager.AddTasksList(State.User.Id, newTasksListName);
            new MainWindow().Show();
            Close();
        }
    }
}