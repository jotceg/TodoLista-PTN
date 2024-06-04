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

namespace TodoLista.Scripts.Tasks
{
    /// <summary>
    /// Interaction logic for EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        private OleDbConnection conn;
        private OleDbCommand cmd;
        private OleDbDataAdapter adapter;
        private DataTable dt;

        //DataBase NAme and Connection Location Information  
        private const string DataBaseName = "TasksDataBase.accdb";
        private const string connectionAndDataString = "Provider=Microsoft.ACE.OleDb.16.0; Data Source=" + DataBaseName;
        public EditTaskWindow()
        {
            InitializeComponent();

            GetTimeTableData();
        }

        private void AddTask(string priority, string title, string description, DateTime date)
        {
            conn = new OleDbConnection(connectionAndDataString);

            string query = "INSERT INTO Tasks (Priority,Title, Description,[RealizationDate]) VALUES (@Priority,@Title, @Description,@RealizationDate)";

            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@Priority", priority);
            cmd.Parameters.AddWithValue("@Title", title); // Add Parameters name to DataBase
            cmd.Parameters.AddWithValue("@Description", description);
            cmd.Parameters.AddWithValue("@RealizationDate", date);


            conn.Open(); // Open Connetction With DataBase
            cmd.ExecuteNonQuery(); // Allows to Insert Changes in DataBase
            conn.Close(); // Close Connetction With DataBase
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


            int hour = TimeHourComboBox.SelectedIndex;
            int minute = TimeMinutesComboBox.SelectedIndex;


            if (string.IsNullOrWhiteSpace(priority) || string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) || date == null)
            {
                MessageBox.Show($"Podaj Priority, Title oraz Destctiption oraz Date!");
                return;
            }


            if (hour == -1 || minute == -1)
            {
                MessageBox.Show("Wybierz godzinę i minutę realizacji zadania!");
                return;
            }


            DateTime dateAndTime = date.Value.AddHours(hour).AddMinutes(minute);

            AddTask(priority, title, description, dateAndTime);
            MessageBox.Show($"Wprowadzono zadanie!");
            this.Close();
        }

        private void DeleteTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = int.Parse(DeleteTextBox.Text);
                DeleteTask(id);
                MessageBox.Show($"Usunięto Zadanie!");
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

        private void GetTimeTableData()
        {

            conn = new OleDbConnection(connectionAndDataString);
            dt = new DataTable();
            adapter = new OleDbDataAdapter("SELECT *FROM Tasks", conn);

            conn.Open();
            adapter.Fill(dt);

            DataBase.ItemsSource = dt.DefaultView; // See all Users in Window (Test porposes)
            conn.Close();
        }
    }
}

