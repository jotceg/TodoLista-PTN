using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TodoLista.Scripts.Tasks;
using TodoLista.Scripts.LoginScripts;
using Microsoft.VisualBasic;
using TodoLista.Scripts;
using TodoLista.Pages.Home;
using TodoLista.Pages.Login;

namespace TodoLista
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///

    public partial class MainWindow : Window
    {
        public Frame NavigationFrame;

        public MainWindow()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight - SystemParameters.WindowCaptionHeight;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowState = WindowState.Maximized;
            DataContext = new MainViewModel();
            NavigationFrame = MainFrame;

            var (login, password) = LoginState.GetSavedLoginData();

            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password) && DatabaseManager.TryLoginUserAutomatically(login, password))
            {
                var user = DatabaseManager.GetUserData(login, password);

                if (user != null)
                {
                    State.User = user;
                    MainFrame.Navigate(new Home());
                }
                else
                {
                    MainFrame.Navigate(new Login());
                };
            }
            else
            {
                MainFrame.Navigate(new Login());
            };
        }

        public void NavigateTo(Page page)
        {
            MainFrame.Navigate(page);
            
        }
    }
}
