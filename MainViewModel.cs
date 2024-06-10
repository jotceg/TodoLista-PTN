using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoLista.Pages.Home;
using TodoLista.Pages.Login;

namespace TodoLista
{
    public class MainViewModel
    {
        public ICommand NavigateCommand { get; private set; }

        public MainViewModel()
        {
            NavigateCommand = new RelayCommand(Navigate);
        }

        public void Navigate(object parameter)
        {
            if (parameter is string pageName)
            {
                switch (pageName)
                {
                    case "Home":
                        ((MainWindow)App.Current.MainWindow).NavigateTo(new Home());
                        break;
                    case "RegistrationAndLogin":
                        ((MainWindow)App.Current.MainWindow).NavigateTo(new Login());
                        break;
                }
            }
        }
    }
}
