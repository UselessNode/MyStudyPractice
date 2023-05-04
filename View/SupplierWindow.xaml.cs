using AppDB.Model;
using System;
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
    public partial class SupplierWindow : Window
    {
        MainWindow _mainWindow;
        DatabaseEntities database;
        Supplier _supplier;
        enum Type
        {
            Editing = 0,
            Adding = 1
        }
        Type operationType;

        public SupplierWindow(Supplier supplier, MainWindow mainWindow)
        {
            InitializeComponent();
            operationType = Type.Editing;
            _supplier = supplier;
            _mainWindow = mainWindow;
            database = _mainWindow.Entities;
            SaveButton.Content = "Сохранить";
            DataContext = _supplier;
        }
        public SupplierWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            operationType = Type.Adding;
            _supplier = new Supplier();
            _mainWindow = mainWindow;
            database = _mainWindow.Entities;
            SaveButton.Content = "Добавить";
            DataContext = _supplier;
        }

        private void ValidateInput()
        {
            // Чтение ввода и запись в новую накладную
            _supplier.Name = TextBoxName.Text;
            _supplier.Address = TextBoxAddress.Text;
            _supplier.PhoneNumber = long.Parse(TextBoxPhone.Text);
            _supplier.Email = TextBoxEmail.Text;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateInput();
            if (operationType == Type.Adding) 
                database.Supplier.Add(_supplier);
            database.SaveChanges();
            _mainWindow.ReadData();
            Hide();
        }
    }
}
