using System;
using System.Collections.Generic;
using System.IO;
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
using TodoLista.Pages.Home;
using TodoLista.Scripts;
using TodoLista.Windows.EditTask;

namespace TodoLista.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy UserImageWindow.xaml
    /// </summary>
    public partial class UserImageWindow : Window
    {
        string[] ImageFiles = Directory.GetFiles(Environment.CurrentDirectory + "/../../../Images/User","*.png",SearchOption.TopDirectoryOnly);

        public UserImageWindow()
        {
            InitializeComponent();
            foreach (string file in ImageFiles)
            {
                Image image = new Image();
                image.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(file);
                UserImageList.Items.Add(image);
            }
        }
        
        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserImageList.SelectedItems.Count != 1)
            {
                MessageBox.Show("Należy zaznaczyć 1 zdjęcie", "Błąd");
            }
            else
            {
                Image image = UserImageList.SelectedItem as Image;
                string fileName = System.IO.Path.GetFileName(image.Source.ToString());
                // Tu potrzeba jeszcze dodać: zmianę wartości kolumny UserImage w tabeli Users dla zalogowanego użytkownika na wartość zmiennej file
                DatabaseManager.SetUserImage(fileName);
                Close();

                (((MainWindow)(App.Current.MainWindow)).NavigationFrame.Content as Home).InitializeUserData();
            }
        }
    }
}
