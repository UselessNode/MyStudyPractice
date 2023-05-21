using AppDB.Model;
using AppDB.View;
using AppDB.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;

namespace AppDB
{
    public partial class MainWindow : Window
    {
        public Data Data { get; set; }
        public DatabaseEntities Entities;
        public MainWindow()
        {
            InitializeComponent();
            Data = new Data();
            this.Language = XmlLanguage.GetLanguage("ru-RU");
            ReadData();
        }

        public void ReadData()
        {
            Entities = new();
            InvoicesDataGrid.ItemsSource = Entities.Invoice.ToList();
            ProductDataGrid.ItemsSource = Entities.Product.ToList();
            ForwarderDataGrid.ItemsSource = Entities.Forwarder.ToList();
            SupplierDataGrid.ItemsSource = Entities.Supplier.ToList();

            Title = "Менеджер базы данных";

            Data.Invoices = Entities.Invoice;
            Data.Products = Entities.Product;
            Data.Suppliers = Entities.Supplier;
            Data.Forwarders = Entities.Forwarder;

            var minCost = Entities.Invoice.Min(x => x.Cost);
            LowerBoundSlider.Minimum = (double)minCost;
            UpperBoundSlider.Minimum = (double)minCost + 1d;
            var maxCost = Entities.Invoice.Max(x => x.Cost);
            LowerBoundSlider.Maximum = (double)maxCost - 1d;
            UpperBoundSlider.Maximum = (double)maxCost;

            LowerBoundSlider.Value = (double)minCost;
            UpperBoundSlider.Value = (double)maxCost;
        }

        private void CreateRecordButton_Click(object sender, RoutedEventArgs e)
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

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = (sender as TextBox).Text.ToLower();
            int resultCount = 0;
            int totalCount = 0;
            if (!(String.IsNullOrEmpty(input)))
            {
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
                Title = $"База данных | Поиск: {input} | Результатов: {resultCount} из {totalCount}";
            }
            else
                ReadData();
        }

        private void UpdateRecordButton_Click(object sender, RoutedEventArgs e)
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

        private void DeleteRecordButton_Click(object sender, RoutedEventArgs e)
        {

            object data = (sender as Button).DataContext;
            string text = "";
            switch (data)
            {
                case Invoice:
                    Entities.Invoice.Remove(data as Invoice);
                    text = $"{(data as Invoice).Id}"; break;
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

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e) => FiltrationByDate();

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.LastTabIndex = TabContoler.SelectedIndex;
            Properties.Settings.Default.IsDarkTheme = new AppTheme().IsDarkTheme();
            Properties.Settings.Default.Save();
            System.Windows.Application.Current.Shutdown();
        }

        private void TabContoler_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int currTab = Properties.Settings.Default.SelectedTab = TabContoler.SelectedIndex;

            switch (TabContoler.SelectedIndex)
            {
                case 0: //Вкладка с таблицей Invoice
                    ChangeInvoiceFiltrationFieldsVisibility(Visibility.Visible);
                    break;
                case 1: //Вкладка с таблицей Product
                    ChangeInvoiceFiltrationFieldsVisibility(Visibility.Collapsed);
                    break;
                case 2: //Вкладка с таблицей Forwarder
                    ChangeInvoiceFiltrationFieldsVisibility(Visibility.Collapsed);
                    break;
                case 3: //Вкладка с таблицей Supplier
                    ChangeInvoiceFiltrationFieldsVisibility(Visibility.Collapsed);
                    break;
                default: break;
            }
        }

        private void ChangeInvoiceFiltrationFieldsVisibility(Visibility comboBoxVisibility, Visibility datePickersVisibility = Visibility.Collapsed)
        {
            InvoiceFiltrationTypeComboBox.Visibility = comboBoxVisibility;

            FiltrationByDatePanel.Visibility = datePickersVisibility;
            FiltrationByCostPanel.Visibility = datePickersVisibility;

            if (comboBoxVisibility == Visibility.Visible)
                if (InvoiceFiltrationTypeComboBox.SelectedIndex == 2)
                {
                    FiltrationByDatePanel.Visibility = Visibility.Collapsed;
                    FiltrationByCostPanel.Visibility = Visibility.Visible;
                }
        }

        private void ExportAsButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение текущей выбранной вкладки
            TabItem selectedTab = (TabItem)TabContoler.SelectedItem;
            // Поиск элемента DataGrid на выбранной вкладке
            DataGrid dataGrid = null;
            string tabName = "null";

            switch (TabContoler.SelectedIndex)
            {
                case 0: //Invoice
                    tabName = "Inovoice";
                    dataGrid = InvoicesDataGrid;
                    break;
                case 1: //Product
                    tabName = "Product";
                    dataGrid = ProductDataGrid;
                    break;
                case 2: //Forwarder
                    tabName = "Forwarder";
                    dataGrid = ForwarderDataGrid;
                    break;
                case 3: //Supplier
                    tabName = "Supplier";
                    dataGrid = SupplierDataGrid;
                    break;
                default:
                    MessageBox.Show($"TabContoler.SelectedIndex: [{TabContoler.SelectedIndex}]",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }
            Exporter exporter = new Exporter();
            exporter.ExportAsExcel(dataGrid, tabName);
        }

        private void DarkThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            bool currTheme = (bool)(sender as ToggleButton).IsChecked.Value;
            SwithTheme(currTheme);
        }

        private static void SwithTheme(bool IsDark)
        {
            if (IsDark)
                new AppTheme().ChangeTheme(Theme.Dark);
            else
                new AppTheme().ChangeTheme(Theme.Light);
        }

        private void FiltrationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int filtrationType = (sender as ComboBox).SelectedIndex;
            if (filtrationType != 2) // Фильтрация по дате
            {
                FiltrationByCostPanel.Visibility = Visibility.Collapsed;
                FiltrationByDatePanel.Visibility = Visibility.Visible;
                FiltrationByDate();
            }
            else if (filtrationType == 2) // Фильтрация по стоимости
            {
                FiltrationByDatePanel.Visibility = Visibility.Collapsed;
                FiltrationByCostPanel.Visibility = Visibility.Visible;
                FiltrationByCost();
            }
        }

        private void FiltrationByDate()
        {
            if (InvoicesDataGrid is null) return;
            var filteredData = InvoicesDataGrid.ItemsSource;
            if (DatePickerEnd.SelectedDate != null && DatePickerStart.SelectedDate != null)
            {
                if (DatePickerEnd.SelectedDate > DatePickerStart.SelectedDate)
                {
                    var start = DatePickerStart.SelectedDate;
                    var end = DatePickerEnd.SelectedDate;
                    if (InvoiceFiltrationTypeComboBox.SelectedIndex == 0) // Проверка на тип фильтрации
                        filteredData = filteredData.Cast<Invoice>() // По дате Отбытия
                            .Where(x => x.DepartureDate >= start && x.DepartureDate <= end)
                            .ToList();
                    else
                        filteredData = filteredData.Cast<Invoice>() // По дате Прибытия
                            .Where(x => x.ArrivalDate >= start && x.ArrivalDate <= end)
                            .ToList();
                    InvoicesDataGrid.ItemsSource = filteredData;
                }
            }
        }

        private void FiltrationByCost()
        {
            if (InvoicesDataGrid is null) return;
            //var filteredData = InvoicesDataGrid.ItemsSource;
            var lowerBound = LowerBoundSlider.Value;
            var upperBound = UpperBoundSlider.Value;
            if (lowerBound < upperBound)
            {
                var data = Data.Invoices
                    .Where(x => x.Cost >= lowerBound && x.Cost <= upperBound)
                    .ToList();
                InvoicesDataGrid.ItemsSource = data;
            }
        }

        private void AppMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TabContoler.SelectedIndex = Properties.Settings.Default.LastTabIndex;
            bool lastTheme = Properties.Settings.Default.IsDarkTheme;
            SwithTheme(lastTheme);
            DarkThemeToggle.IsChecked = lastTheme;
        }

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            FiltrationByCost();
        }
    }


    public class Data
    {
        
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Forwarder> Forwarders { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}
