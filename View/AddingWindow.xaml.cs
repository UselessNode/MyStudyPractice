using AppDB.Model;
using System;
using System.Linq;
using System.Windows;


namespace AppDB.View
{
    public partial class AddingWindow : Window
    {
        private Invoice _invoice; // Выбранная для изменения накладная
        private MainWindow _mainWindow; // Главная страница
        private DatabaseEntities database;

        public AddingWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _invoice = new Invoice();
            _mainWindow = mainWindow;
            database = _mainWindow.Entities;
            DataContext = _invoice;
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
            _invoice.Cost = String.IsNullOrEmpty(TextBoxCost.Text) ? 0 : int.Parse(TextBoxCost.Text);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateInput();
            database.Invoice.Add(_invoice);
            database.SaveChanges();
            _mainWindow.ReadData();
            Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Инициализация данных ComboBox'ов
            ComboBoxProduct.ItemsSource = database.Product.ToList();
            ComboBoxPurveyor.ItemsSource = database.Supplier.ToList();
            ComboBoxForwarder.ItemsSource = database.Forwarder.ToList();
            
        }
    }
}
