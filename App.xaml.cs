using System.Configuration;
using System.Data;
using System.Windows;

namespace TodoLista
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        { 

            if (LoginCurrentState.IsLoggedIn())
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                LoginAndRegistrationWindow loginAndRegistrationWindow = new LoginAndRegistrationWindow();
                loginAndRegistrationWindow.Show();
            }
        }
    }

}
