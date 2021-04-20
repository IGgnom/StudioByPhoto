using System;
using System.Collections.Generic;
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
using static StudioByPhoto.PhotoDataContext;

namespace StudioByPhoto
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GuestOpenClick(object sender, RoutedEventArgs e)
        {
            ServiceWindow serviceWindow = new ServiceWindow("Гость");
            this.Hide();
            serviceWindow.Show();
        }

        private void ExitApp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(loginTextBox.Text) || string.IsNullOrEmpty(passwordTextBox.Password))
            {
                MessageBox.Show("Все поля должны быть заполнены!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (DBContext.Worker.Where(w => w.Login == loginTextBox.Text || w.Password == passwordTextBox.Password) != null)
                {
                    ServiceWindow serviceWindow = new ServiceWindow("Сотрудник");
                    this.Hide();
                    serviceWindow.Show();
                }
            }
        }
    }
}
