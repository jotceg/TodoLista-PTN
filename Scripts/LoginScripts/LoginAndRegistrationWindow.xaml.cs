using System;
using System.Collections.Generic;
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
using System.Data.OleDb;
using System.IO;
using TodoLista.Scripts.LoginScripts;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Controls.Primitives;
using TodoLista.Scripts.Tasks;

namespace TodoLista
{
    /// <summary>
    /// Interaction logic for LoginAndRegistrationWindow.xaml
    /// </summary>
    public partial class LoginAndRegistrationWindow : Window
    {
        //Connection References
        private OleDbConnection conn;
        private OleDbCommand cmd;
        private OleDbDataAdapter adapter;
        private DataTable dt;

        //DataBase Name and Connection Location Information  
        private const string userDataBase = "UserDataBase.accdb";
        private const string tasksListsDataBase = "TasksListsDataBase.accdb";
        private const string tasksDataBase = "TasksDataBase.accdb";
        private const string userDataBaseConnectionAndDataSetting = "Provider=Microsoft.ACE.OleDb.16.0; Data Source=" + userDataBase;
        private const string tasksListDataBaseConnectionAndDataSetting = "Provider=Microsoft.ACE.OleDb.16.0; Data Source=" + tasksListsDataBase;
        private const string tasksDataBaseConnectionAndDataSetting = "Provider=Microsoft.ACE.OleDb.16.0; Data Source=" + tasksDataBase;

        public LoginAndRegistrationWindow()
        {
            InitializeComponent();

            GetAllAvailableUsers(); // Loading information about current users in database to see them in window 
        }

        private void RegisterUser(string name, string password)
        {
            string userId;

            using (conn = new OleDbConnection(userDataBaseConnectionAndDataSetting))
            {
                string query = "INSERT INTO Users (Name, UserPassword) VALUES (@Name, @UserPassword)";

                using (cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name); // Add Parameters name to DataBase
                    cmd.Parameters.AddWithValue("@UserPassword", password);

                    conn.Open(); // Open Connetction With DataBase
                    cmd.ExecuteNonQuery(); // Allows to Insert Changes in DataBase
                    //conn.Close(); // Close Connetction With DataBase

                    cmd.CommandText = "SELECT @@IDENTITY";
                    userId = cmd.ExecuteScalar().ToString();
                }
            }

            using (conn = new OleDbConnection(tasksListDataBaseConnectionAndDataSetting))
            {
                string query = "INSERT INTO Lists (ListName, UserId) VALUES (@ListName, @UserId)";

                using (cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ListName", "Dzisiaj");
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            LoginUser(name, password);
        }

        private void GetAllAvailableUsers()
        {
            conn = new OleDbConnection(userDataBaseConnectionAndDataSetting);
            dt = new DataTable();
            adapter = new OleDbDataAdapter("SELECT * FROM Users", conn);

            conn.Open();
            adapter.Fill(dt);

            dwgCustomers.ItemsSource = dt.DefaultView; // See all Users in Window (Test purposes)
            conn.Close();
        }
        private bool DoesUserExist(string name)
        {
            conn = new OleDbConnection(userDataBaseConnectionAndDataSetting);
            string query = "SELECT COUNT(*) FROM Users WHERE  StrComp(Name ,@Name,0) = 0";

            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", name);

            conn.Open();
            int userCount = (int)cmd.ExecuteScalar(); // return the overall value of Users in DataBase  
            conn.Close();

            return userCount > 0;

        }
        private bool CheckPassword(string userName, string password)
        {
            conn = new OleDbConnection(userDataBaseConnectionAndDataSetting);

            string query = "SELECT COUNT(*) FROM Users WHERE  StrComp(Name, @Name,0) = 0 AND  StrComp(UserPassword ,@UserPassword, 0) = 0";
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", userName);
            cmd.Parameters.AddWithValue("@UserPassword", password);

            conn.Open();
            int userCount = (int)cmd.ExecuteScalar();
            conn.Close();

            return userCount > 0;

        }
        public void LoginUser(string name, string password)
        {
            int userId = 0;
            string userLogin = "", userPassword = "";

            using (conn = new OleDbConnection(userDataBaseConnectionAndDataSetting))
            {
                string query = "SELECT Id, Name, UserPassword FROM Users WHERE Name = @Name AND UserPassword = @UserPassword";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@UserPassword", password);

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

                            tasksLists.Add(new TasksList(userId, tasksListId, tasksListName));
                        }
                    }
                }
            }

            State.User = new User(userId, userLogin, userPassword, tasksLists);
        }
        private void ClearData()
        {
            conn = new OleDbConnection(userDataBaseConnectionAndDataSetting);

            string query = "DELETE FROM Users";
            cmd = new OleDbCommand(query, conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            // Odśwież widok danych, jeśli istnieje
            GetAllAvailableUsers();
            MessageBox.Show("Wyczyszczono bazę danych");
        }

        //Button Logic
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(login) && string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Nie podano żadnego loginu ani hasła. Spróbuj ponownie", "Błąd");
                return;
            }

            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Nie podano żadnego loginu. Spróbuj ponownie", "Błąd");
                return;
            };

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Nie podano żadnego hasła. Spróbuj ponownie", "Błąd");
                return;
            };

            if (!DoesUserExist(login))
            {
                MessageBox.Show("Użytkownik o podanym loginie nie istnieje, załóż nowe konto i spróbuj ponownie.", "Błąd");
                return;
            }

            if (!CheckPassword(login, password))
            {
                MessageBox.Show("Podano błędne hasło. Spróbuj ponownie", "Błąd");
                return;
            }

            LoginUser(login, password);
           
            if (RememberMyDataCheckBox.IsChecked == true)
            {
                LoginState.SaveLoginData(login, password); // Save login state to Login.txt
            }

            MainWindow mainWindow = new MainWindow();

            Close();

            mainWindow.Show();
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text; //Login Input
            string password = PasswordBox.Password; // Password Input

            if (string.IsNullOrWhiteSpace(login) && string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Nie podano żadnego loginu ani hasła. Spróbuj ponownie", "Błąd");
                return;
            }

            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Nie podano żadnego loginu. Spróbuj ponownie", "Błąd");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Nie podano żadnego hasła. Spróbuj ponownie", "Błąd");
                return;
            }

            if (DoesUserExist(login))
            {
                MessageBox.Show("Użytkownik o podanym loginie już istnieje, zaloguj się i spróbuj ponownie.", "Błąd");
                return;
            }

            RegisterUser(login, password);

            if (RememberMyDataCheckBox.IsChecked == true)
            {
                LoginState.SaveLoginData(login, password);
            };

            MainWindow mainWindow = new MainWindow();
            
            Close();

            mainWindow.Show();
        }

        private void ClearDataBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }
    }
}
