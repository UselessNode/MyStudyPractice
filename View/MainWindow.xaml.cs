using AppDB.Model;
using AppDB.View;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AppDB
{
    public partial class MainWindow : Window
    {
        public DatabaseEntities Entities;
        public MainWindow()
        {
            InitializeComponent();
            ReadData();
        }

        public void CreateRecordButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = null;
            
            switch (TabContoler.SelectedIndex)
            {
                case 0:
                    window = new InvoiceWindow(this); break;
                case 1:
                    window = new ProductWindow(this); break;
                case 2:
                    window = new ForwardeerWindow(this); break;
                case 3:
                    window = new SupplierWindow(this); break;
                default: break;
            }
            window.Show();
        }

        public void ReadData()
        {
            Entities = new();
            InvoicesDataGrid.ItemsSource = Entities.Invoice.ToList();
            ProductDataGrid.ItemsSource = Entities.Product.ToList();
            ForwarderDataGrid.ItemsSource = Entities.Forwarder.ToList();
            SupplierDataGrid.ItemsSource = Entities.Supplier.ToList();
            Title = "Менеджер базы данных";
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = (sender as TextBox).Text.ToLower();
            int resultCount = 0;
            int totalCount = 0;
            switch (TabContoler.SelectedIndex)
            {
                case 0: //Invoice
                    resultCount = Entities.Invoice.Count(x => x.Product.Name.Contains(input) 
                    || x.Supplier.Name.Contains(input)
                    || x.Forwarder.Name.Contains(input));
                    totalCount = Entities.Invoice.ToList().Count();
                    InvoicesDataGrid.ItemsSource = Entities.Invoice.Where(x => x.Product.Name.Contains(input)
                    || x.Supplier.Name.Contains(input)
                    || x.Forwarder.Name.Contains(input)).ToList();
                    break;
                case 1: //Product
                    resultCount = Entities.Product.Count(x => x.Name.Contains(input)
                    || x.ProductType.Name.Contains(input)
                    || x.ProductCategory.Name.Contains(input));
                    totalCount = Entities.Product.ToList().Count();
                    ProductDataGrid.ItemsSource = Entities.Product.Where(x => x.Name.Contains(input)
                    || x.ProductType.Name.Contains(input)
                    || x.ProductCategory.Name.Contains(input)).ToList();
                    break;
                case 2: //Forwarder
                    resultCount = Entities.Forwarder.Count(x => x.Name.Contains(input) 
                    || x.Supplier.Name.Contains(input));
                    totalCount = Entities.Forwarder.ToList().Count();
                    ForwarderDataGrid.ItemsSource = Entities.Forwarder.Where(x => x.Name.Contains(input)
                    || x.Supplier.Name.Contains(input)).ToList();
                    break;
                case 3: //Supplier
                    resultCount = Entities.Supplier.Count(x => x.Name.Contains(input)
                    || x.Address.Contains(input)
                    || x.PhoneNumber.ToString().Contains(input)
                    || x.Email.Contains(input));
                    totalCount = Entities.Supplier.ToList().Count();
                    SupplierDataGrid.ItemsSource = Entities.Supplier.Where(x => x.Name.Contains(input)
                    || x.Address.Contains(input)
                    || x.PhoneNumber.ToString().Contains(input)
                    || x.Email.Contains(input)).ToList();
                    break;
                default: break;
            }           
            if (!(String.IsNullOrEmpty(input)))
            {
                Title = $"База данных | Поиск: {input} | Результатов: {resultCount} из {totalCount}";
            }
            else
                ReadData();
        }

        public void UpdateRecordButton_Click(object sender, RoutedEventArgs e)
        {
            object data = (sender as Button).DataContext;
            Window window = null;
            switch (data)
            {
                case Invoice:
                    window = new InvoiceWindow(data as Invoice, this); break;
                case Product:
                    window = new ProductWindow(data as Product, this); break;
                case Forwarder:
                    window = new ForwardeerWindow(data as Forwarder, this); break;
                case Supplier:
                    window = new SupplierWindow(data as Supplier, this); break;
                default: break;
            }
            window.Show();
            
        }

        public void DeleteRecordButton_Click(object sender, RoutedEventArgs e)
        {
            object data = (sender as Button).DataContext;
            string text = "";
            switch (data)
            {
                case Invoice:
                    Entities.Invoice.Remove(data as Invoice);
                    text = $"{(data as Invoice).Id}";break;
                case Product:
                    Entities.Product.Remove(data as Product);
                    text = $"{(data as Product).Id}"; break;
                case Forwarder:
                    Entities.Forwarder.Remove(data as Forwarder); 
                    text = $"{(data as Forwarder).Id}"; break;
                case Supplier:
                    Entities.Supplier.Remove(data as Supplier);
                    text = $"{(data as Supplier).Id}"; break;
                default: break;
            }
            if (MessageBox.Show($"Вы уверены, что хотите удалить запись {text}?",
                "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Entities.SaveChanges();
            }
            ReadData();
        }

        private void ResetSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = null;
            DatePickerStart.Text = null;
            DatePickerEnd.Text = null;
            ReadData();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePickerEnd.SelectedDate != null && DatePickerStart.SelectedDate != null)
            {
                if (DatePickerEnd.SelectedDate > DatePickerStart.SelectedDate)
                {
                    var start = DatePickerStart.SelectedDate;
                    var end = DatePickerEnd.SelectedDate;
                    var filteredData = Entities.Invoice
                        .Where(x => x.DepartureDate >= start && x.DepartureDate <= end)
                        .ToList();
                    InvoicesDataGrid.ItemsSource = filteredData;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void TabContoler_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabContoler.SelectedIndex != 0)
                FiltrationPanel.Visibility = Visibility.Collapsed;
            else
                FiltrationPanel.Visibility = Visibility.Visible;
        }
    }
}
