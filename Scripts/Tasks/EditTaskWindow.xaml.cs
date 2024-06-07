using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
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
using TodoLista.Scripts.LoginScripts;

namespace TodoLista.Scripts.Tasks
{
    /// <summary>
    /// Interaction logic for EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        private static EditTaskWindow Instance;

        private OleDbConnection conn;
        private OleDbCommand cmd;
        private OleDbDataAdapter adapter;
        private DataTable dt;

        //DataBase NAme and Connection Location Information  
        private const string DataBaseName = "TasksDataBase.accdb";
        private const string connectionAndDataString = "Provider=Microsoft.ACE.OleDb.16.0; Data Source=" + DataBaseName;

        private const string userDataBase = "UserDataBase.accdb";
        private const string tasksListsDataBase = "TasksListsDataBase.accdb";
        private const string tasksDataBase = "TasksDataBase.accdb";
        private const string userDataBaseConnectionAndDataSetting = "Provider=Microsoft.ACE.OleDb.16.0; Data Source=" + userDataBase;
        private const string tasksListDataBaseConnectionAndDataSetting = "Provider=Microsoft.ACE.OleDb.16.0; Data Source=" + tasksListsDataBase;
        private const string tasksDataBaseConnectionAndDataSetting = "Provider=Microsoft.ACE.OleDb.16.0; Data Source=" + tasksDataBase;

        public EditTaskWindow()
        {
            InitializeComponent();

            GetTimeTableData();

            this.Closed += EditTaskWindow_Closed;
        }

        public static void ShowWindow()
        {
            if (Instance == null)
            {
                Instance = new EditTaskWindow();
                Instance.Show();
            }
            else
            {
                Instance.Activate();
            }
        }

        private void EditTaskWindow_Closed(object sender, EventArgs e)
        {
            Instance = null;
        }

        public void RetrieveUserData()
        {
            int userId = 0;
            string userLogin = "", userPassword = "";

            using (conn = new OleDbConnection(userDataBaseConnectionAndDataSetting))
            {
                string query = "SELECT Id, Name, UserPassword FROM Users WHERE Name = @Name AND UserPassword = @UserPassword";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", State.User.Login);
                    cmd.Parameters.AddWithValue("@UserPassword", State.User.Password);

                    conn.Open();

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = reader.GetInt32(reader.GetOrdinal("Id"));
                            userLogin = reader.GetString(reader.GetOrdinal("Name"));
                            userPassword = reader.GetString(reader.GetOrdinal("UserPassword"));
                        }
                    }
                }
            }

            List<TasksList> tasksLists = new List<TasksList>();

            using (conn = new OleDbConnection(tasksListDataBaseConnectionAndDataSetting))
            {
                string query = "SELECT * FROM Lists where UserId = @UserId";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Assuming your table has columns like "Id", "Task", "Completed"
                            string tasksListName = reader["ListName"].ToString();
                            int tasksListId = (int)reader["ListId"]; // Adjust data type based on actual column
                            List<Scripts.Tasks.Task> tasks = new List<Scripts.Tasks.Task>();

                            tasksLists.Add(new TasksList(userId, tasksListId, tasksListName, tasks));
                        }
                    }
                }
            }

            for (int i = 0; i < tasksLists.Count(); i++)
            {
                using (conn = new OleDbConnection(tasksDataBaseConnectionAndDataSetting))
                {
                    string query = "SELECT * FROM Tasks where ListId = @ListId";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ListId", tasksLists[i].Id);

                        conn.Open();

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Assuming your table has columns like "Id", "Task", "Completed"
                                string taskName = reader["Title"].ToString();
                                int taskId = (int)reader["TaskId"]; // Adjust data type based on actual column
                                string description = reader["Description"].ToString();
                                string priority = (string)reader["Priority"];
                                DateTime realizationDate = (DateTime)reader["RealizationDate"];

                                tasksLists[i].Tasks.Add(new Scripts.Tasks.Task(taskId, tasksLists[i].Id, taskName, description, priority, realizationDate));
                            }
                        }
                    }
                }
            }

            State.User = new User(userId, userLogin, userPassword, tasksLists);
            MainWindow.Instance.LoadTasksForSelectedList();
        }

        private void AddTask(string priority, string title, string description, DateTime date)
        {


            if (date <= DateTime.Now)
            {
                MessageBox.Show("Nie można wybrać daty z przeszłości.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DatabaseManager.DoesTaskNameAlreadyExist(State.SelectedTasksListId, title))
            {
                MessageBox.Show("Zadanie o takiej nazwie już istnieje!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (conn = new OleDbConnection(connectionAndDataString))
            {
                string query = "INSERT INTO Tasks (ListId, Title, Description, Priority, RealizationDate) VALUES (@ListId, @Title, @Description, @Priority, @RealizationDate)";

                using (cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ListId", State.SelectedTasksListId);
                    cmd.Parameters.AddWithValue("@Title", title); // Add Parameters name to DataBase
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Priority", priority);
                    cmd.Parameters.AddWithValue("@RealizationDate", date);
                    
                    conn.Open(); // Open Connetction With DataBase
                    cmd.ExecuteNonQuery(); // Allows to Insert Changes in DataBase
                }
            }
            ClearUI();

            RetrieveUserData();
            MessageBox.Show($"Wprowadzono zadanie!");
            
            MainWindow.Instance.LoadTasksForSelectedList();
            
           
           
        }

        private void ClearUI()
        {
            PriorityComboBox.SelectedIndex = -1; 
            TitleTextBox.Clear(); 
            DescriptionTextBox.Clear();
            DatePickerBtn.SelectedDate = null;
            TimeHourComboBox.SelectedIndex = -1;
            TimeMinutesComboBox.SelectedIndex = -1; 
        }

        public void MarkAsCompleted(int id)
        {
            conn = new OleDbConnection(connectionAndDataString);

            string query = "UPDATE Tasks SET IsCompleted = @IsCompleted WHERE Id = @Id";

            cmd = new OleDbCommand(query, conn);

            cmd.Parameters.AddWithValue("@IsCompleted", true);
            cmd.Parameters.AddWithValue("@Id", id);


            conn.Open(); // Open Connetction With DataBase
            cmd.ExecuteNonQuery(); // Allows to Insert Changes in DataBase
            conn.Close(); // Close Connetction With DataBase
        }

        public void DeleteTask(int id)
        {
            conn = new OleDbConnection(connectionAndDataString);


            string deleteQuery = "DELETE FROM Tasks WHERE Id = @Id";

            cmd = new OleDbCommand(deleteQuery, conn);
            cmd.Parameters.AddWithValue("@Id", id);


            conn.Open(); // Open Connetction With DataBase
            cmd.ExecuteNonQuery(); // Allows to Insert Changes in DataBase
            conn.Close(); // Close Connetction With DataBase


        }

        private void AddTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            string priority = PriorityComboBox.Text;
            string title = TitleTextBox.Text;
            string description = DescriptionTextBox.Text;
            DateTime? date = DatePickerBtn.SelectedDate;


            

            if (string.IsNullOrWhiteSpace(priority) || string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) || date == null)
            {
                MessageBox.Show($"Podaj Priority, Title oraz Destctiption oraz Date!");
                return;
            }


            if (TimeHourComboBox.SelectedItem == null || TimeMinutesComboBox.SelectedItem == null)
            {
                MessageBox.Show("Wybierz godzinę i minutę realizacji zadania!");
                return;
            }
            int hour = int.Parse((TimeHourComboBox.SelectedItem as ComboBoxItem).Content.ToString());
            int minute = int.Parse((TimeMinutesComboBox.SelectedItem as ComboBoxItem).Content.ToString());


            DateTime dateAndTime = date.Value.AddHours(hour).AddMinutes(minute);

            if (dateAndTime <= DateTime.Now)
            {
                MessageBox.Show("Nie można wybrać godziny z przeszłości.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AddTask(priority, title, description, dateAndTime);

            
        }

        private void DeleteTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = int.Parse(DeleteTextBox.Text);
                DeleteTask(id);
                MessageBox.Show($"Usunięto Zadanie!");

                ClearUI();

                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Podaj poprawne Id zadania.");
            }
        }
        private void DeleteAllTasks()
        {
            conn = new OleDbConnection(connectionAndDataString);
            

            string query = "DELETE FROM Tasks";

            cmd = new OleDbCommand(query, conn);

            MessageBox.Show("Wszystkie zadania Usunięte!");

            conn.Open(); // Open Connetction With DataBase
            cmd.ExecuteNonQuery(); // Allows to Insert Changes in DataBase
            conn.Close(); // Close Connetction With DataBase
        }

        private void DeleteAllTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                DeleteAllTasks();
                MessageBox.Show($"Usunięto Zadanie!");

                ClearUI();

                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Podaj poprawne Id zadania.");
            }
        }



        private void MarkAsCompletedTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = int.Parse(MarkAsCompletedTaskTextBox.Text);

                if (!IsTaskExists(id)) { MessageBox.Show($"Nie ma takiego zadania albo zostało już zakończone!"); return; }

                MarkAsCompleted(id);
                MessageBox.Show($"Zadanie Ukończone!");
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Podaj poprawne Id zadania.");
            }
        }

        private bool IsTaskExists(int id)
        {
            conn = new OleDbConnection(connectionAndDataString);

            string query = "SELECT COUNT(*) FROM Tasks WHERE Id = @Id AND IsCompleted = @IsCompleted";
            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@IsCompleted", false);

            conn.Open();
            int taskCount = (int)cmd.ExecuteScalar();
            conn.Close();

            return taskCount > 0;
        }

        public void GetTimeTableData()
        {

            conn = new OleDbConnection(connectionAndDataString);
            dt = new DataTable();
            adapter = new OleDbDataAdapter("SELECT *FROM Tasks", conn);

            conn.Open();
            adapter.Fill(dt);

            DataBase.ItemsSource = dt.DefaultView; // See all Users in Window (Test porposes)
            conn.Close();
        }

        private void DeleteTaskBtn_Kopiuj_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
           
        }
    }
}

