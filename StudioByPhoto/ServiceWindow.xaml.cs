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
using System.Windows.Shapes;
using static StudioByPhoto.PhotoDataContext;

namespace StudioByPhoto
{
    public partial class ServiceWindow : Window
    {
        public ServiceWindow(string loginMode)
        {
            InitializeComponent();
            loginModeTextBlock.Text = $"Вы вошли как: {loginMode}";
            serviceListView.ItemsSource = DBContext.Service.ToList();

            if (loginMode == "Гость")
            {
                addServiceButton.Visibility = Visibility.Hidden;
                editServiceButton.Visibility = Visibility.Hidden;
                deleteServiceButton.Visibility = Visibility.Hidden;
            }
        }

        private void ExitApp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AddServiceButtonClick(object sender, RoutedEventArgs e)
        {
            ServiceAddEdit serviceAddEdit = new ServiceAddEdit(ServiceAddEdit.OpenMode.Add);
            serviceAddEdit.ShowDialog();
            serviceListView.ItemsSource = DBContext.Service.ToList();
        }

        private void AddCartButtonClick(object sender, RoutedEventArgs e)
        {
            if (serviceListView.SelectedItem is Service selectedService)
            {
                OrderWindow orderWindow = new OrderWindow(selectedService);
                this.Hide();
                orderWindow.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Выберите услугу!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CartButtonClick(object sender, RoutedEventArgs e)
        {
            Cart cartWindow = new Cart();
            this.Hide();
            cartWindow.ShowDialog();
            this.Show();
        }

        private void EditServiceButtonClick(object sender, RoutedEventArgs e)
        {
            if (serviceListView.SelectedItem is Service selectedService)
            {
                ServiceAddEdit serviceAddEdit = new ServiceAddEdit(selectedService)
                {
                    ServiceName = selectedService.Name,
                    ServiceDescription = selectedService.Descriotion,
                    ServicePrice = Convert.ToString(selectedService.Price),
                    ServicePhoto = selectedService.Photo
                };
                serviceAddEdit.ShowDialog();
                serviceListView.ItemsSource = DBContext.Service.ToList();
            }
        }

        private void DeleteServiceButtonClick(object sender, RoutedEventArgs e)
        {
            if (serviceListView.SelectedItem is Service selectedService)
            {
                if (MessageBox.Show("Вы уверены что хотите удалить выбранную услугу?", "Удаление!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DBContext.Service.Remove(selectedService);
                    DBContext.SaveChanges();
                    MessageBox.Show("Услуга успешно удалена!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    serviceListView.ItemsSource = DBContext.Service.ToList();
                }
            }
        }
    }
}
