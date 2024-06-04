using System.Configuration;
using System.Data;
using System.Windows;
using TodoLista.Scripts.LoginScripts;

namespace TodoLista
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Window startupWindow = new LoginAndRegistrationWindow();


            /* if (LoginCurrentState.IsLoggedIn())
            {
                startupWindow = new MainWindow();
            }
            else
            {
                startupWindow = new LoginAndRegistrationWindow();
            } */

            startupWindow.Show();
        }
    }

}
