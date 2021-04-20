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
using System.IO;
using Microsoft.Win32;
using static StudioByPhoto.PhotoDataContext;

namespace StudioByPhoto
{
    public partial class ServiceAddEdit : Window
    {
        public string ServiceName { set { serviceNameTextBox.Text = value; } }
        public string ServicePrice { set { servicePriceTextBox.Text = value; } }
        public byte[] ServicePhoto 
        { 
            set 
            { 
                using (MemoryStream photoMemoryStream = new MemoryStream(value))
                {
                    BitmapImage servicePhoto = new BitmapImage();
                    servicePhoto.BeginInit();
                    servicePhoto.CacheOption = BitmapCacheOption.OnLoad;
                    servicePhoto.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    servicePhoto.StreamSource = photoMemoryStream;
                    servicePhoto.EndInit();
                    serviceImage.Source = servicePhoto;
                }
            } 
        }
        public string ServiceDescription
        {
            set
            {
                serviceDescriptTextBox.Document.Blocks.Clear();
                serviceDescriptTextBox.Document.Blocks.Add(new Paragraph(new Run(value)));
            }
        }

        private Service editingService;

        public enum OpenMode
        {
            Add = 1,
            Edit = 2
        }

        public ServiceAddEdit()
        {
            InitializeComponent();
        }

        public ServiceAddEdit(OpenMode openMode)
        {
            InitializeComponent();
            switch (openMode)
            {
                case OpenMode.Add:
                    addEditButton.Content = "Добавить";
                    this.Title = "Добавить услугу";
                    break;
                case OpenMode.Edit:
                    addEditButton.Content = "Изменить";
                    this.Title = "Изменить услугу";
                    break;
            }
        }

        public ServiceAddEdit(Service serviceToEdit) : this(OpenMode.Edit)
        {
            editingService = serviceToEdit;
        }

        private void AddEditButtonClick(object sender, RoutedEventArgs e)
        {
            switch (Convert.ToString((sender as Button).Content))
            {
                case "Добавить":
                    Service addService = new Service
                    {
                        Name = serviceNameTextBox.Text,
                        Price = Convert.ToDecimal(servicePriceTextBox.Text),
                        Descriotion = new TextRange(serviceDescriptTextBox.Document.ContentStart, serviceDescriptTextBox.Document.ContentEnd).Text,
                        Photo = GetPhotoByteArray()
                    };
                    DBContext.Service.Add(addService);
                    break;
                case "Изменить":
                    editingService.Name = serviceNameTextBox.Text;
                    editingService.Price = Convert.ToDecimal(servicePriceTextBox.Text);
                    editingService.Descriotion = new TextRange(serviceDescriptTextBox.Document.ContentStart, serviceDescriptTextBox.Document.ContentEnd).Text;
                    editingService.Photo = GetPhotoByteArray();
                    break;
            }
            DBContext.SaveChanges();
            this.Close();
        }

        private void ChoseImageClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog choosePhoto = new OpenFileDialog
            {
                Title = "Выбрать документ",
                Filter = "Все файлы |*.*"
            };
            if (choosePhoto.ShowDialog() == true)
            {
                using (FileStream DocumentFileStream = new FileStream(choosePhoto.FileName, FileMode.Open))
                {
                    byte[] imageArray = new byte[DocumentFileStream.Length];
                    DocumentFileStream.Read(imageArray, 0, imageArray.Length);
                    this.ServicePhoto = imageArray;
                }
            }
        }

        private byte[] GetPhotoByteArray()
        {
            MemoryStream photoMemoryStream = new MemoryStream();
            JpegBitmapEncoder photoBitmapEncoder = new JpegBitmapEncoder();
            photoBitmapEncoder.Frames.Add(BitmapFrame.Create(serviceImage.Source as BitmapImage));
            photoBitmapEncoder.Save(photoMemoryStream);

            return photoMemoryStream.ToArray();
        }
    }
}
