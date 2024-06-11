using Microsoft.VisualBasic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TodoLista.Scripts.LoginScripts;
using TodoLista.Scripts.Tasks;
using TodoLista.Scripts;
using TodoLista.Windows.EditTask;
using TodoLista.Windows;
using TodoLista.Windows.MainMenuEditTaskWindow;
using System.IO;
using Microsoft.Win32;

namespace TodoLista.Pages.Home
{
    /// <summary>
    /// Logika interakcji dla klasy Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public List<TasksList> tasksLists;

        public Home()
        {
            InitializeComponent();
            InitializeUserData();
        }

        // Acts as both an initializer as well as UI refresher for reactive-like behaviour of the app
        public void InitializeUserData()
        {
            // User validation
            if (State.User != null && State.User.TasksLists != null)
            {
                // Data & UI content setup
                tasksLists = State.User.TasksLists;
                TasksListsItemsControl.ItemsSource = tasksLists;
                UserNameTextBox.Text = $"Witaj, {State.User.Login}";
                ImageSource imageSource;
                UserImage.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFromString(Environment.CurrentDirectory + "/../../../Images/User/" + State.User.Image);

                // Loading tasks for a selected list (first in order by default)
                if (tasksLists.Any())
                {
                    State.SelectedTasksListId = tasksLists.First().Id;
                    LoadTasksForSelectedList();
                }

                // Setup task-counting functionality
                TaskCount();
            }
            else
            {
                tasksLists = new List<TasksList>();
            }
        }

        // Loads tasks for selected tasks list with support to only show unfinished tasks
        public void LoadTasksForSelectedList(bool showOnlyUnfinishedTasks = false)
        {
            TasksDataGrid.ItemsSource = null;

            var selectedList = tasksLists.FirstOrDefault(tl => tl.Id == State.SelectedTasksListId);

            foreach (TasksList tasksList in tasksLists)
            {
                if (tasksList.Id == State.SelectedTasksListId)
                {
                    // Get all the tasks by default
                    List<Scripts.Tasks.Task> tasks = tasksList.Tasks;
                    
                    // Only display tasks that are not yet completed
                    if (showOnlyUnfinishedTasks == true)
                    {
                        List<Scripts.Tasks.Task> _tasks = new List<Scripts.Tasks.Task>();

                        for (int i = 0; i < tasks.Count; i++)
                        {
                            if (tasks[i].isCompleted == false) _tasks.Add(tasks[i]);
                        }

                        tasks = _tasks;
                    }

                    // Fill data grid with tasks data
                    TasksDataGrid.ItemsSource = tasks;

                    // Update chosen list info
                    TopNameTextBox.Text = $"Wybrana lista zadań to: {selectedList?.Name}";
                    
                    // Finish the loop when the proper list is found
                    break;
                }
            }
        }

        // Handle adding new task functionality by opening a dedicated window
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            EditTaskWindow.ShowWindow();

            // Loading tasks for a selected list
            LoadTasksForSelectedList();
        }

        // Sign out and go back to the login page
        private void SignOut(object sender, RoutedEventArgs e)
        {
            ((MainWindow)App.Current.MainWindow).NavigateTo(new Login.Login());
            State.SignOut();
        }

        // Handle opening a given task list & refresh corresponding tasks data
        private void OpenTasksList(object sender, RoutedEventArgs e)
        {
            // Get a tasks list id from a clicked button
            Button clickedButton = (Button)sender;
            int id = int.Parse(clickedButton.Uid);

            // Update currently selected tasks list id
            State.SelectedTasksListId = id;

            // Loading tasks for a selected list
            LoadTasksForSelectedList();
        }

        // Handle adding new task list functionality
        private void AddTasksListButton_Click(object sender, RoutedEventArgs e)
        {
            // Opening an InputBox for entering new tasks list name
            string newTasksListName = Interaction.InputBox("Wpisz nazwę listy zadań", "Dodawanie listy zadań", "");

            // Input validation
            if (!string.IsNullOrEmpty(newTasksListName))
            {
                // Add tasks list
                DatabaseManager.AddTasksList(State.User.Id, newTasksListName);
                
                // Refresh user data & corresponding UI
                InitializeUserData();
                
                return;
            }
            else
            {
                // Handle improper data by rejecting the operation
                MessageBox.Show("Nie podano żadnej nazwy dla tworzonej listy zadań", "Błąd");
                
                return;
            }
        }

        // Handle renaming tasks list
        private void RenameTasksListMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve appropriate UI elements from sender object & validate them
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                ContextMenu contextMenu = menuItem.Parent as ContextMenu;
                if (contextMenu != null)
                {
                    Button button = contextMenu.PlacementTarget as Button;
                    if (button != null)
                    {
                        // Retrieve tasks list id
                        int id = int.Parse(button.Uid);
                        
                        // Find selected tasks list
                        TasksList selectedTasksList = tasksLists.FirstOrDefault(t => t.Id == id);
                        
                        if (selectedTasksList != null)
                        {
                            // Opening an InputBox for entering new tasks list name
                            string newTasksListName = Microsoft.VisualBasic.Interaction.InputBox("Wpisz nową nazwę listy zadań", "Zmiana nazwy listy zadań", selectedTasksList.Name);
                            
                            // Input validation
                            if (!string.IsNullOrEmpty(newTasksListName))
                            {
                                // Checking if the list name is not conflicting with another list
                                bool renameSuccessful = DatabaseManager.DoesListNameAlreadyExist(selectedTasksList.UserId, newTasksListName);

                                // Updating the name in the database
                                if (!renameSuccessful)
                                {
                                    DatabaseManager.RenameTasksList(selectedTasksList.Id, selectedTasksList.UserId, newTasksListName);
                                    selectedTasksList.Name = newTasksListName;
                                    TasksListsItemsControl.Items.Refresh();
                                }
                                else
                                {
                                    // Handling rejection of the operation
                                    MessageBox.Show("Zmiana nazwy nie powiodła się. Lista o takiej nazwie już istnieje!");

                                    return;
                                }
                            }
                            else
                            {
                                // Handling rejection of the operation
                                MessageBox.Show("Nie można zmienić nazwy na puste pole!");

                                return;
                            }
                        }
                    }
                }
            }
        }

        // Handling renaming a single task
        private void RenameTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve appropriate UI objects via sender object & validate them
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                ContextMenu contextMenu = menuItem.Parent as ContextMenu;
                if (contextMenu != null)
                {
                    // Retrieve task id
                    int taskId = int.Parse(contextMenu.Uid);

                    // Find selected task
                    Scripts.Tasks.Task currentlySelectedTask = tasksLists.FirstOrDefault(t => t.Id == State.SelectedTasksListId).Tasks.FirstOrDefault(t => t.Id == taskId);

                    // Allow user to input new task name
                    string newTaskName = Microsoft.VisualBasic.Interaction.InputBox("Wpisz nową nazwę zadania", "Zmiana nazwy zadania", currentlySelectedTask.Title);
                    
                    // Rename task if data validation is positive
                    if (!string.IsNullOrEmpty(newTaskName))
                    {
                        DatabaseManager.RenameSingleTask(taskId, newTaskName);
                        currentlySelectedTask.Title = newTaskName;
                        InitializeUserData();
                        TasksListsItemsControl.Items.Refresh();
                        TasksDataGrid.Items.Refresh();
                    }
                    else
                    {
                        // Handle improper data values
                        MessageBox.Show("Nie można zmienić nazwy na puste pole!");
                        
                        return;
                    }
                }
            }
        }

        // Delete task
        private void DeleteTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve appropriate UI objects via sender object & validate them
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                ContextMenu contextMenu = menuItem.Parent as ContextMenu;
                if (contextMenu != null)
                {
                    // Retrieve task id
                    int taskId = int.Parse(contextMenu.Uid);
                    
                    // Id validation
                    if (taskId != null)
                    {
                        // Allow user to confirm deletion
                        MessageBoxResult result = MessageBox.Show($"Czy na pewno chcesz usunąć to zadanie?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo);
                        
                        // Delete task & refresh UI
                        if (result == MessageBoxResult.Yes)
                        {
                            DatabaseManager.DeleteSingleTask(taskId);
                            InitializeUserData();
                            TasksListsItemsControl.Items.Refresh();
                            TasksDataGrid.Items.Refresh();
                        }
                    }
                }
            }
        }

        // Delete tasks list
        private void DeleteTasksListMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve appropriate UI objects via sender object & validate them
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                ContextMenu contextMenu = menuItem.Parent as ContextMenu;
                if (contextMenu != null)
                {
                    Button button = contextMenu.PlacementTarget as Button;
                    if (button != null)
                    {
                        // Retrieve task id
                        int id = int.Parse(button.Uid);

                        // Find selected tasks list
                        TasksList selectedTasksList = tasksLists.FirstOrDefault(t => t.Id == id);
                        
                        if (selectedTasksList != null)
                        {
                            // Allow user to confirm deletion
                            MessageBoxResult result = MessageBox.Show($"Czy na pewno chcesz usunąć listę zadań: {selectedTasksList.Name}?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo);

                            // Delete tasks list & refresh UI
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

        // Handle opening task-editing window
        private void TasksDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid.SelectedItem is Scripts.Tasks.Task selectedTask)
            {
                MainMenuEditTaskWindow taskDetailsWindow = new MainMenuEditTaskWindow(selectedTask);
                taskDetailsWindow.Show();
            }
        }

        // Handle marking task as done or undone
        private void TaskDoneCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox != null)
            {
                StackPanel stackPanel = checkBox.Parent as StackPanel;
                if (stackPanel != null)
                {
                    int taskId = int.Parse(stackPanel.Uid);
                    if (taskId != null)
                    {
                        DatabaseManager.MarkTaskAsCompleted(taskId, checkBox.IsChecked.Value);
                        InitializeUserData();
                        TasksListsItemsControl.Items.Refresh();
                        TasksDataGrid.Items.Refresh();
                    }
                }
            }
        }

        private void SidePanelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Equals(SidePanelButton.Content, "<"))
            {
                Sidebar.Width = new GridLength(0, GridUnitType.Star);
                SidePanelButton.Content = ">";
            }
            else
            {
                Sidebar.Width = new GridLength(2, GridUnitType.Star);
                SidePanelButton.Content = "<";
            }
        }
        
        // Handle opening avatar image choosing window
        private void UserImageButton_Click(object sender, RoutedEventArgs e)
        {
            new UserImageWindow().Show();
        }

        private void ShowOnlyUnfinishedTasksCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (ShowOnlyUnfinishedTasksCheckBox.IsChecked.Value)
            {
                LoadTasksForSelectedList(true);
            } else
            {
                LoadTasksForSelectedList(false);
            }
        }

        private void AddButton_Enter(object sender, RoutedEventArgs e)
        {
            AddButtonBackground.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFromString(Environment.CurrentDirectory + "/../../../Images/Core/PlusSignSelected.png");
        }

        private void AddButton_Leave(object sender, RoutedEventArgs e)
        {
            AddButtonBackground.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFromString(Environment.CurrentDirectory + "/../../../Images/Core/PlusSign.png");
        }

        // Import database data
        private void ImportDataBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                DefaultExt = ".accdb",
                Filter = "Access Database files (.accdb)|*.accdb"
            };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                DatabaseManager.ImportDatabase(filename);
                InitializeUserData();
                TasksListsItemsControl.Items.Refresh();
                TasksDataGrid.Items.Refresh();
                MessageBox.Show("Lista została zaimportowana poprawnie!", "Udało się", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Export database
        private void ExportDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                FileName = "TodoListDataBase",
                DefaultExt = ".accdb",
                Filter = "Access Database files (.accdb)|*.accdb"
            };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                DatabaseManager.ExportDatabase(filename);
                MessageBox.Show("Lista została wyeksportowana poprawnie!", "Udało się", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Handle counting finished and total amount of tasks
        public void TaskCount()
        {
            int totalTasks = tasksLists.Sum(list => list.Tasks.Count);
            int completedTasks = tasksLists.Sum(list => list.Tasks.Count(task => task.isCompleted));

            TasksCountTextBlock.Text = $"Łączna liczba zadań to: {totalTasks}, Zrealizowanych: {completedTasks}";
        }
    }

}
