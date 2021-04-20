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
    public partial class Cart : Window
    {
        public Cart()
        {
            InitializeComponent();
            cartListView.ItemsSource = DBContext.Order.Where(o => o.Customer.Name == UserName && o.Customer.Phone == UserPhone).ToList();
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
