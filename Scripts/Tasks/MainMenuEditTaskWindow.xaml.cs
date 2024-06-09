using System;
using System.Collections;
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

namespace TodoLista.Scripts.Tasks
{
    /// <summary>
    /// Interaction logic for MainMenuEditTaskWindow.xaml
    /// </summary>
    public partial class MainMenuEditTaskWindow : Window
    {
        private readonly Scripts.Tasks.Task _originalTask;
        

        public MainMenuEditTaskWindow(Scripts.Tasks.Task task)
        {
            InitializeComponent();

            _originalTask = task;
            PopulateTaskDetails();

            Width = SystemParameters.PrimaryScreenWidth / 2;
            Height = (SystemParameters.PrimaryScreenHeight - SystemParameters.WindowCaptionHeight) / 2;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void PopulateTaskDetails()
        {
            TitleTextBox.Text = _originalTask.Title;
            DescriptionTextBox.Text = _originalTask.Description;

            // Set priority
            PriorityComboBox.SelectedItem = PriorityComboBox.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == _originalTask.Priority);

            // Set date
            DatePickerBtn.SelectedDate = _originalTask.Date;

            // Set time
            TimeHourComboBox.SelectedItem = TimeHourComboBox.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == _originalTask.Date.Hour.ToString("00"));

            TimeMinutesComboBox.SelectedItem = TimeMinutesComboBox.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == _originalTask.Date.Minute.ToString("00"));
        }

        private void SaveTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string description = DescriptionTextBox.Text;
            string priority = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            DateTime? date = DatePickerBtn.SelectedDate;

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(priority) || date == null)
            {
                MessageBox.Show("All fields must be filled out.");
                return;
            }

            if (TimeHourComboBox.SelectedItem == null || TimeMinutesComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a time.");
                return;
            }

            int hour = int.Parse((TimeHourComboBox.SelectedItem as ComboBoxItem).Content.ToString());
            int minute = int.Parse((TimeMinutesComboBox.SelectedItem as ComboBoxItem).Content.ToString());

            DateTime realizationDate = date.Value.AddHours(hour).AddMinutes(minute);

            if (realizationDate <= DateTime.Now)
            {
                MessageBox.Show("The selected time cannot be in the past.");
                return;
            }

            _originalTask.Title = title;
            _originalTask.Description = description;
            _originalTask.Priority = priority;
            _originalTask.Date = realizationDate;

            DatabaseManager.UpdateTask(_originalTask.Id, title, description, priority, realizationDate);

            //MainWindow.Instance.LoadTasksForSelectedList();


            this.Close();
        }

        private void CancelTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy napewno chcesz usunąć to zadanie ?", "Usuń", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                DatabaseManager.DeleteSingleTask(_originalTask.Id);

                // Remove the task from the task list in the main window
                //TasksList parentList = MainWindow.Instance.tasksLists.FirstOrDefault(list => list.Tasks.Contains(_originalTask));
                //if (parentList != null)
                //{
                    //parentList.Tasks.Remove(_originalTask);
                //}

                //MainWindow.Instance.LoadTasksForSelectedList();

                this.Close();
            }
        }
    }
}
