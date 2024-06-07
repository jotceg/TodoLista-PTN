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
        public List<TasksList> tasksLists;
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            Instance = this;

            InitializeUserData();
            
        }

        private void InitializeUserData()
        {
            if (State.User != null && State.User.TasksLists != null)
            {
                tasksLists = State.User.TasksLists;
                TasksListsItemsControl.ItemsSource = tasksLists;
                UserNameTextBox.Text = $"Witaj, {State.User.Login}";

                if (tasksLists.Any())
                {
                   
                    State.SelectedTasksListId = tasksLists.First().Id;
                    LoadTasksForSelectedList();
                }
            }
            else
            {
                tasksLists = new List<TasksList>();
            }
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            EditTaskWindow.ShowWindow();
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
            
            LoadTasksForSelectedList();
        }

        public  void LoadTasksForSelectedList()
        {
            TasksDataGrid.ItemsSource = null;

            var selectedList = tasksLists.FirstOrDefault(tl => tl.Id == State.SelectedTasksListId);

            foreach (TasksList tasksList in tasksLists)
            {
                if (tasksList.Id == State.SelectedTasksListId)
                {
                    TasksDataGrid.ItemsSource = tasksList.Tasks;
                    TopNameTextBox.Text = $"Wybrana List to: {selectedList.Name}";
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

        

        private void RenameTasksListMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                ContextMenu contextMenu = menuItem.Parent as ContextMenu;
                if (contextMenu != null)
                {
                    Button button = contextMenu.PlacementTarget as Button;
                    if (button != null)
                    {

                        int id = int.Parse(button.Uid);
                        TasksList selectedTasksList = tasksLists.FirstOrDefault(t => t.Id == id);
                        if (selectedTasksList != null)
                        {
                            string newTasksListName = Microsoft.VisualBasic.Interaction.InputBox("Wpisz nową nazwę listy zadań", "Zmiana nazwy listy zadań", selectedTasksList.Name);
                            if (!string.IsNullOrEmpty(newTasksListName))
                            {
                                bool renameSuccessful = DatabaseManager.DoesListNameAlreadyExist(selectedTasksList.UserId, newTasksListName);

                                if (!renameSuccessful)
                                {
                                    DatabaseManager.RenameTasksList(selectedTasksList.Id, selectedTasksList.UserId, newTasksListName);
                                    selectedTasksList.Name = newTasksListName; 
                                    TasksListsItemsControl.Items.Refresh();
                                }
                                else
                                {
                                    
                                    MessageBox.Show("Zmiana nazwy nie powiodła się. Lista o takiej nazwie już istnieje!");
                                }

                            }
                            else
                            {
                                MessageBox.Show("Nie można zmienić nazwy na puste pole!");
                                return;
                            }
                        }
                    }
                }
            }
        }


        



        private void DeleteTasksListMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                ContextMenu contextMenu = menuItem.Parent as ContextMenu;
                if (contextMenu != null)
                {
                    Button button = contextMenu.PlacementTarget as Button;
                    if (button != null)
                    {
                        int id = int.Parse(button.Uid);
                        TasksList selectedTasksList = tasksLists.FirstOrDefault(t => t.Id == id);
                        if (selectedTasksList != null)
                        {
                            MessageBoxResult result = MessageBox.Show($"Czy na pewno chcesz usunąć listę zadań: {selectedTasksList.Name}?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                DatabaseManager.DeleteTasksList(State.User.Id, selectedTasksList.Id);
                                tasksLists.Remove(selectedTasksList);
                                TasksListsItemsControl.Items.Refresh();
                            }
                        }
                    }
                }
            }
        }

        private void TasksDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid.SelectedItem is Scripts.Tasks.Task selectedTask)
            {
                MainMenuEditTaskWindow taskDetailsWindow = new MainMenuEditTaskWindow(selectedTask);
                taskDetailsWindow.Show();
            }
        }


    }


}
