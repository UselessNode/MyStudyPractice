using AppDB.Model;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppDB.View
{
    /// <summary>
    /// Логика взаимодействия для AddNewInvoiceWindow.xaml
    /// </summary>
    public partial class InvoiceWindow : Window
    {
        Invoice _invoice; // Выбранная для изменения накладная
        MainWindow _mainWindow; // Главная страница
        DatabaseEntities database;
        enum Type { Editing = 0, Adding = 1 }
        Type operationType;

        public InvoiceWindow(Invoice invoice, MainWindow mainWindow)
        {
            InitializeComponent();
            operationType = Type.Editing;
            SaveButton.Content = "Сохранить";
            _invoice = invoice;
            _mainWindow = mainWindow;
            ReadData();
        }

        public InvoiceWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            operationType = Type.Adding;
            SaveButton.Content = "Добавить";
            _invoice = new Invoice();
            _mainWindow = mainWindow;
            ReadData();
        }

        private void ReadData()
        {
            database = _mainWindow.Entities;
            DataContext = _invoice;

            // Инициализация данных ComboBox'ов
            ComboBoxProduct.ItemsSource = database.Product.ToList();
            ComboBoxPurveyor.ItemsSource = database.Supplier.ToList();
            ComboBoxForwarder.ItemsSource = database.Forwarder.ToList();

            // Индексы при редактировании
            ComboBoxProduct.SelectedIndex = _invoice.ProductId is null ? -1 : (int)_invoice.ProductId;
            ComboBoxPurveyor.SelectedIndex = _invoice.SupplierId is null ? -1 : (int)_invoice.SupplierId;
            ComboBoxForwarder.SelectedIndex = _invoice.ForwarderId is null ? -1 : (int)_invoice.ForwarderId;
        }

        // Сбор данных с полей ввода
        private void ValidateInput()
        {
            // Чтение ввода и запись в новую накладную
            _invoice.DepartureDate = String.IsNullOrEmpty(DepartureDateInput.Text) ? DateTime.Now : DateTime.Parse(DepartureDateInput.Text);
            _invoice.ArrivalDate = String.IsNullOrEmpty(ArrivalDateInput.Text) ? DateTime.Now : DateTime.Parse(ArrivalDateInput.Text);
            _invoice.ProductId = ComboBoxProduct.SelectedIndex == -1 ? null : ComboBoxProduct.SelectedIndex + 1;
            _invoice.SupplierId = ComboBoxPurveyor.SelectedIndex == -1 ? null : ComboBoxPurveyor.SelectedIndex + 1;
            _invoice.ForwarderId = ComboBoxForwarder.SelectedIndex == -1 ? null : ComboBoxForwarder.SelectedIndex + 1;
            _invoice.Cost = String.IsNullOrEmpty(TextBoxCost.Text) ? 0 : int.Parse(Regex.Match(TextBoxCost.Text, @"\d+").Value);
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateInput();
            if (operationType == Type.Adding) 
                database.Invoice.Add(_invoice);
            database.SaveChanges();
            _mainWindow.ReadData();
            Hide();
        }

        private void CreateNewProductButton_Click(object sender, RoutedEventArgs e)
        {
            var productWindow = new ProductWindow(_mainWindow);
            productWindow.ShowDialog();
            ReadData();
        }

        private void CreateNewSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            var supplierWindow = new SupplierWindow(_mainWindow);
            supplierWindow.ShowDialog();
            ReadData();
        }

        private void CreateNewForwarderButton_Click(object sender, RoutedEventArgs e)
        {
            var forwarderWindow = new ForwardeerWindow(_mainWindow);
            forwarderWindow.ShowDialog();
            ReadData();
        }
    }
}
