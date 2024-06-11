﻿using System;
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

namespace TodoLista.Pages.Login
{
    /// <summary>
    /// Logika interakcji dla klasy RegistrationAndLogin.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        // Handling login button functionality
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            // Preventing whitespaces
            string login = LoginTextBox.Text.Trim();
            
            string password = PasswordTextBox.Password;

            // Inputs validations
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


            // Database content validation
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
                // Save login state to Login.txt for auto sign-in functionality
                LoginState.SaveLoginData(login, password);
            }

            // Navigating to home page
            ((MainWindow)App.Current.MainWindow).NavigateTo(new Home.Home());
        }

        // Handling button to show or hide typed password
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

        // Handling navigation to register page if the user doesn't have an account yet
        private void NavigateToRegister(object sender, RoutedEventArgs e)
        {
            ((MainWindow)App.Current.MainWindow).NavigateTo(new Register.Register());
        }
    }
}
