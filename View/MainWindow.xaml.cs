using AppDB.Data;
using AppDB.View;
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

namespace AppDB
{
    public partial class MainWindow : Window
    {
        public DatabaseEntities Entities;
        public List<Invoice> TableItemSource;

        public MainWindow()
        {
            InitializeComponent();
            ReadData();
        }

        public void CreateRecordButton_Click(object sender, RoutedEventArgs e)
        {
            AddingWindow addingWindow = new AddingWindow(this);
            addingWindow.Show();
        }

        public void ReadData()
        {
            Entities = new();
            InvoicesDataGrid.ItemsSource = Entities.Invoice.ToList();
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
            var selectedInvoice = (sender as Button).DataContext as Invoice;
            EditingWindow editingWindow = new EditingWindow(selectedInvoice, this);
            editingWindow.Show();
        }

        public void DeleteRecordButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedInvoice = ((sender as Button).DataContext as Invoice);
            if (MessageBox.Show($"Вы уверены, что хотите удалить запись под номером {selectedInvoice.Id}?",
                "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Entities.Invoice.Remove(selectedInvoice);
                Entities.SaveChanges();
                ReadData();
            }
        }

        private void ResetSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            ReadData();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePickerEnd.SelectedDate != null && DatePickerStart.SelectedDate != null)
            {
                if(DatePickerEnd.SelectedDate > DatePickerStart.SelectedDate)
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

        private void ThemeToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            //Смена темы

            //var app = Application.Current as App;
            //// Проверяем, какое значение записано в App.xaml
            //if (BaseTheme == "Light")
            //{
            //    // Меняем значение на Dark
            //    app.Theme = "Dark";
            //}
            //else
            //{
            //    // Меняем значение на Light
            //    app.Theme = "Light";
            //}
        }
    }
}
