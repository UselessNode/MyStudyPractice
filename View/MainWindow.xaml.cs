using AppDB.Model;
using AppDB.View;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web.UI;
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
            var input = (sender as TextBox).Text.ToLower();
            int resultCount = Entities.Invoice.Count(x => x.Product.Name.Contains(input));
            if (!(String.IsNullOrEmpty(input)))
            {
                InvoicesDataGrid.ItemsSource = Entities.Invoice.Where(x => x.Product.Name.Contains(input)).ToList();
                Title = $"База данных | Поиск: {input} | Результатов: {resultCount} из {Entities.Invoice.ToList().Count()}";
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
            Application.Current.Shutdown();
        }
    }
}
