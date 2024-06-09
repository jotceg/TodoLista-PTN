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

        public void InitializeUserData()
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

        public void LoadTasksForSelectedList()
        {
            TasksDataGrid.ItemsSource = null;

            var selectedList = tasksLists.FirstOrDefault(tl => tl.Id == State.SelectedTasksListId);

            foreach (TasksList tasksList in tasksLists)
            {
                if (tasksList.Id == State.SelectedTasksListId)
                {
                    TasksDataGrid.ItemsSource = tasksList.Tasks;
                    TopNameTextBox.Text = $"Wybrana lista zadań to: {selectedList.Name}";
                    break;
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            EditTaskWindow.ShowWindow();
            LoadTasksForSelectedList();
        }

        private void SignOut(object sender, RoutedEventArgs e)
        {
            ((MainWindow)App.Current.MainWindow).NavigateTo(new RegistrationAndLogin.RegistrationAndLogin());
            State.SignOut();
        }

        private void OpenTasksList(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            int id = int.Parse(clickedButton.Uid);
            State.SelectedTasksListId = id;

            LoadTasksForSelectedList();
        }


        private void AddTasksListButton_Click(object sender, RoutedEventArgs e)
        {
            string newTasksListName = Interaction.InputBox("Wpisz nazwę listy zadań", "Dodawanie listy zadań", "");

            if (!string.IsNullOrEmpty(newTasksListName))
            {
                DatabaseManager.AddTasksList(State.User.Id, newTasksListName);
                InitializeUserData();
                return;
            }
            else
            {
                MessageBox.Show("Nie podano żadnej nazwy dla tworzonej listy zadań", "Błąd");
                return;
            }
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

        private void RenameTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                ContextMenu contextMenu = menuItem.Parent as ContextMenu;
                if (contextMenu != null)
                {
                    int taskId = int.Parse(contextMenu.Uid);

                    Scripts.Tasks.Task currentlySelectedTask = tasksLists.FirstOrDefault(t => t.Id == State.SelectedTasksListId).Tasks.FirstOrDefault(t => t.Id == taskId);

                    string newTaskName = Microsoft.VisualBasic.Interaction.InputBox("Wpisz nową nazwę zadania", "Zmiana nazwy zadania", currentlySelectedTask.Title);
                    
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
                        MessageBox.Show("Nie można zmienić nazwy na puste pole!");
                        return;
                    }
                }
            }
        }

        private void DeleteTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                ContextMenu contextMenu = menuItem.Parent as ContextMenu;
                if (contextMenu != null)
                {
                    int taskId = int.Parse(contextMenu.Uid);
                    
                    if (taskId != null)
                    {
                        MessageBoxResult result = MessageBox.Show($"Czy na pewno chcesz usunąć to zadanie?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo);
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
                        if (checkBox.IsChecked.Value)
                        {
                            DatabaseManager.DeleteSingleTask(taskId);
                            MessageBox.Show("Gratulujemy wykonania zadania", "Gratulacje!");
                            InitializeUserData();
                            TasksListsItemsControl.Items.Refresh();
                            TasksDataGrid.Items.Refresh();
                        }
                    }
                }
            }
        }

        private void DeleteTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                StackPanel stackPanel = button.Parent as StackPanel;
                if (stackPanel != null)
                {
                    int taskId = int.Parse(stackPanel.Uid);

                    if (taskId != null)
                    {
                        MessageBoxResult result = MessageBox.Show($"Czy na pewno chcesz usunąć to zadanie?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo);
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

        private void UserImageButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }

}
