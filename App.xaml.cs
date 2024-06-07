using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using TodoLista.Scripts;
using TodoLista.Scripts.LoginScripts;
using TodoLista.Scripts.Tasks;

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

            Window startupWindow;

            var (login, password) = LoginState.GetSavedLoginData();
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password) && DatabaseManager.TryLoginUserAutomatically(login, password))
            {
                var user = DatabaseManager.GetUserData(login, password);
                if (user != null)
                {
                    State.User = user;
                    startupWindow = new MainWindow();
                }
                else
                {
                    startupWindow = new LoginAndRegistrationWindow();
                }
            }
            else
            {
                startupWindow = new LoginAndRegistrationWindow();
            }


            startupWindow.Show();
        }

       
}

}
