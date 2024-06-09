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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TodoLista.Scripts.LoginScripts;
using TodoLista.Scripts.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using TodoLista.Scripts;

namespace TodoLista.Pages.RegistrationAndLogin
{
    /// <summary>
    /// Logika interakcji dla klasy RegistrationAndLogin.xaml
    /// </summary>
    public partial class RegistrationAndLogin : Page
    {
        MainViewModel viewModel;

        public RegistrationAndLogin()
        {
            InitializeComponent();

            GetAndDisplayAllAvailableUsers(); // Loading information about current users in database to see them in window 

            viewModel = (MainViewModel)(App.Current.MainWindow.DataContext);
        }

        private void GetAndDisplayAllAvailableUsers()
        {
            dwgCustomers.ItemsSource = DatabaseManager.GetAllAvailableUsers().DefaultView; // See all Users in Window (Test purposes)
        }

        private void ClearData()
        {
            DatabaseManager.ClearData();

            // Odśwież widok danych, jeśli istnieje
            GetAndDisplayAllAvailableUsers();
            MessageBox.Show("Wyczyszczono bazę danych");
        }

        private void ClearLoginDataState()
        {
            LoginState.ClearLoginDataState();
            MessageBox.Show("Wyczyszczono bazę danych LoginState.txt");
        }

        //Button Logic
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
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

            if (!DatabaseManager.DoesUserExist(login))
            {
                MessageBox.Show("Użytkownik o podanym loginie nie istnieje, załóż nowe konto i spróbuj ponownie.", "Błąd");
                return;
            }

            if (!DatabaseManager.CheckPassword(login, password))
            {
                MessageBox.Show("Podano błędne hasło. Spróbuj ponownie", "Błąd");
                return;
            }

            DatabaseManager.LoginUser(login, password);

            if (RememberMyDataCheckBox.IsChecked == true)
            {
                LoginState.SaveLoginData(login, password); // Save login state to Login.txt
            }

            //viewModel.Navigate(new Home());
            ((MainWindow)App.Current.MainWindow).NavigateTo(new Home.Home());
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim(); ; //Login Input
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

            if (DatabaseManager.DoesUserExist(login))
            {
                MessageBox.Show("Użytkownik o podanym loginie już istnieje, zaloguj się i spróbuj ponownie.", "Błąd");
                return;
            }

            DatabaseManager.RegisterUser(login, password);

            if (RememberMyDataCheckBox.IsChecked == true)
            {
                LoginState.SaveLoginData(login, password);
            };

            //viewModel.Navigate(new Home());
            ((MainWindow)App.Current.MainWindow).NavigateTo(new Home.Home());
        }

        private void ClearDataBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        private void ClearLoginStateDataBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearLoginDataState();
        }
    }
}
