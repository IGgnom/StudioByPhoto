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
using static StudioByPhoto.UserInformation;

namespace StudioByPhoto
{
    public partial class OrderWindow : Window
    {
        private Service orderService;

        public OrderWindow(Service serviceToOrder)
        {
            InitializeComponent();
            placeListView.ItemsSource = DBContext.Studio.ToList();
            workerListView.ItemsSource = DBContext.Worker.Where(w => w.IdRole == 1).ToList();

            orderService = serviceToOrder;
        }

        private void AddOrderClick(object sender, RoutedEventArgs e)
        {
            if (workerListView.SelectedItem is Worker && placeListView.SelectedItem is Studio)
            {
                UserName = customerNameTextBox.Text;
                UserPhone = customerPhoneTextBox.Text;

                int customerId;
                if (DBContext.Customer.SingleOrDefault(c => c.Name == UserName && c.Phone == UserPhone) != null)
                {
                    customerId = DBContext.Customer.Single(c => c.Name == UserName && c.Phone == UserPhone).IdCustomer;
                }
                else
                {
                    customerId = DBContext.Customer.Max(c => c.IdCustomer) + 1;
                    Customer newCustomer = new Customer()
                    {
                        IdCustomer = customerId,
                        Surname = null,
                        Name = UserName,
                        Phone = UserPhone,
                        Email = null
                    };
                    DBContext.Customer.Add(newCustomer);
                }

                Order newOrder = new Order()
                {
                    IdService = orderService.IdService,
                    IdCustomer = customerId,
                    DateTime = DateTime.Now,
                    IdDiscount = null,
                    FinalPrice = orderService.Price,
                    IdWorker = (workerListView.SelectedItem as Worker).IdWorker
                };
                DBContext.Order.Add(newOrder);
                DBContext.SaveChanges();
                MessageBox.Show("Заказ офомлен успешно!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("Выберите фотографа и место фотосесии!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
        }
    }
}
