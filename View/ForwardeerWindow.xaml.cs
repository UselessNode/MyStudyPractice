using AppDB.Model;
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

namespace AppDB.View
{
    /// <summary>
    /// Логика взаимодействия для AddNewForwardeerWindow.xaml
    /// </summary>
    public partial class ForwardeerWindow : Window
    {
        Forwarder _forwarder;
        MainWindow _mainWindow; // Главная страница
        DatabaseEntities database;
        enum Type { Editing = 0, Adding = 1 }
        Type operationType;
        public ForwardeerWindow(Forwarder forwarder, MainWindow mainWindow)
        {
            InitializeComponent();
            operationType = Type.Editing;
            SaveButton.Content = "Сохранить";
            _forwarder = forwarder;
            _mainWindow = mainWindow;
            ReadData();
        }

        private void ReadData()
        {
            database = _mainWindow.Entities;
            DataContext = _forwarder;

            // Инициализация данных ComboBox'ов
            ComboBoxSupplier.ItemsSource = database.Supplier.ToList();
            ComboBoxSupplier.SelectedIndex = _forwarder.SupplierId;
        }

        public ForwardeerWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            operationType = Type.Adding;
            SaveButton.Content = "Добавить";
            _forwarder = new Forwarder();
            _mainWindow = mainWindow;
            ReadData();
        }

        // Сбор данных с полей ввода
        private void ValidateInput()
        {
            // Чтение ввода и запись в новую накладную
            _forwarder.Name = TextBoxName.Text;
            _forwarder.SupplierId = ComboBoxSupplier.SelectedIndex;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateInput();
            if (operationType == Type.Adding) 
                database.Forwarder.Add(_forwarder);
            database.SaveChanges();
            _mainWindow.ReadData();
            Hide();
        }

        private void CreateNewSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            var supplierWindow = new SupplierWindow(_mainWindow);
            supplierWindow.ShowDialog();
            ReadData();
        }
    }
}
