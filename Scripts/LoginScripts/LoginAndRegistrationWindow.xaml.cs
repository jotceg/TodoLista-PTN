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
        private const string DataBaseName = "UserDataBase.accdb";
        private const string connectionAndDataStting = "Provider=Microsoft.ACE.OleDb.16.0; Data Source=" + DataBaseName;

        public LoginAndRegistrationWindow()
        {
            InitializeComponent();

            GetUser(); // Loading information about current users in database to see them in window 
        }

        private void InsertUser(string name, string password)
        {
            conn = new OleDbConnection(connectionAndDataStting);

            string query = "INSERT INTO Users (Name, UserPassword) VALUES (@Name, @UserPassword)";

            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", name); // Add Parameters name to DataBase
            cmd.Parameters.AddWithValue("@UserPassword", password);

            conn.Open(); // Open Connetction With DataBase
            cmd.ExecuteNonQuery(); // Allows to Insert Changes in DataBase
            conn.Close(); // Close Connetction With DataBase
        }
        private void GetUser()
        {

            conn = new OleDbConnection(connectionAndDataStting);
            dt = new DataTable();
            adapter = new OleDbDataAdapter("SELECT *FROM Users", conn);

            conn.Open();
            adapter.Fill(dt);

            dwgCustomers.ItemsSource = dt.DefaultView; // See all Users in Window (Test porposes)
            conn.Close();
        }
        private bool UserExists(string name)
        {
            conn = new OleDbConnection(connectionAndDataStting);
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
            conn = new OleDbConnection(connectionAndDataStting);

            string query = "SELECT COUNT(*) FROM Users WHERE  StrComp(Name, @Name,0) = 0 AND  StrComp(UserPassword ,@UserPassword, 0) = 0";
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", userName);
            cmd.Parameters.AddWithValue("@UserPassword", password);

            conn.Open();
            int userCount = (int)cmd.ExecuteScalar();
            conn.Close();

            return userCount > 0;

        }
        public bool LoginUser(string name, string password)
        {
            conn = new OleDbConnection(connectionAndDataStting);

            string query = "SELECT COUNT(*) FROM Users WHERE Name = @Name AND UserPassword = @UserPassword";
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@UserPassword", password);

            conn.Open();
            int userCount = (int)cmd.ExecuteScalar();
            conn.Close();

            return userCount > 0;

        }
        private void ClearData()
        {
            conn = new OleDbConnection(connectionAndDataStting);

            string query = "DELETE FROM Users";
            cmd = new OleDbCommand(query, conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();



            // Odśwież widok danych, jeśli istnieje
            GetUser();
            MessageBox.Show("Wyczyszczono bazę danych");
        }

        //Button Logic
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = RegisterTextBox.Text;
            string password = PasswordBox.Password;

            if (!UserExists(name))
            {
                MessageBox.Show($"Użytkownik nie istnieje, załóż nowe konto.");
                return;
            }

            if (!CheckPassword(name, password))
            {
                MessageBox.Show($"Błędne Hasło");
                return;
            }

            MessageBox.Show($"Zalogowano pomyślnie!");

           
            LoginCurrentState.SetLoggedIn(true); // Save login state to Login.txt

            this.Close();
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = RegisterTextBox.Text; //Login Input
            string password = PasswordBox.Password; // Password Input

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show($"Podaj Login oraz Hasło.");
                return;
            }

            if (UserExists(name))
            {
                MessageBox.Show($"Użytkownik jest już zarejestrowany.");
                return;
            }


            InsertUser(name, password);
            MessageBox.Show($"Rejestracja Powiodła się");
            this.Close();
        }

        private void ClearDataBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }
    }
}
