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

namespace TodoLista.Pages.Register
{
    /// <summary>
    /// Logika interakcji dla klasy RegistrationAndLogin.xaml
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        private void ShowPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PasswordTextBoxVisible.Text = PasswordTextBox.Password;
            PasswordTextBoxVisible.Visibility = Visibility.Visible;
            PasswordTextBox.Visibility = Visibility.Collapsed;
        }

        private void ShowPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Password = PasswordTextBoxVisible.Text;
            PasswordTextBoxVisible.Visibility = Visibility.Collapsed;
            PasswordTextBox.Visibility = Visibility.Visible;
        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (ShowPasswordCheckBox.IsChecked == true)
            {
                PasswordTextBoxVisible.Text = PasswordTextBox.Password;
            }
        }

        private void PasswordTextBoxVisible_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ShowPasswordCheckBox.IsChecked == true)
            {
                PasswordTextBox.Password = PasswordTextBoxVisible.Text;
            }
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim(); ; //Login Input
            string password = PasswordTextBox.Password; // Password Input

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

            if (password.Count() < 8)
            {
                MessageBox.Show("Podane hasło musi mieć co najmniej 8 znaków. Spróbuj ponownie", "Błąd");
                return;
            }

            if (!password.Any(char.IsUpper))
            {
                MessageBox.Show("Podane hasło musi mieć co najmniej jedną dużą literę. Spróbuj ponownie", "Błąd");
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

            ((MainWindow)App.Current.MainWindow).NavigateTo(new Home.Home());
        }

        private void NavigateToLogin(object sender, RoutedEventArgs e)
        {
            ((MainWindow)App.Current.MainWindow).NavigateTo(new Login.Login());
        }
    }
}
