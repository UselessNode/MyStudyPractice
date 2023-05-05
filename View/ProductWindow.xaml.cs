using AppDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    public partial class ProductWindow : Window
    {
        Product _product;
        MainWindow _mainWindow; // Главная страница
        DatabaseEntities database;
        enum Type { Editing = 0, Adding = 1 }
        Type operationType;
        public ProductWindow(Product product, MainWindow mainWindow)
        {
            InitializeComponent();
            operationType = Type.Editing;
            SaveButton.Content = "Сохранить";
            _product = product;
            _mainWindow = mainWindow;
            ReadData();
        }

        public ProductWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            operationType = Type.Adding;
            SaveButton.Content = "Добавить";
            _product = new Product();
            _mainWindow = mainWindow;
            ReadData();
        }

        private void ReadData()
        {
            database = _mainWindow.Entities;
            DataContext = _product;

            // Инициализация данных ComboBox'ов
            ComboBoxProductCathegory.ItemsSource = database.ProductCategory.ToList();
            ComboBoxProductType.ItemsSource = database.ProductType.ToList();
            ComboBoxProductCathegory.SelectedIndex = _product.CategoryId is null ? -1 : (int)_product.CategoryId;
            ComboBoxProductType.SelectedIndex = _product.TypeId is null ? -1 : (int)_product.TypeId;
        }

        // Сбор данных с полей ввода
        private void ValidateInput()
        {
            // Чтение ввода и запись в новую накладную
            _product.Name = TextBoxProductName.Text;
            _product.CategoryId = ComboBoxProductCathegory.SelectedIndex == -1 ? null : ComboBoxProductCathegory.SelectedIndex + 1;
            _product.TypeId = ComboBoxProductType.SelectedIndex == -1 ? null : ComboBoxProductType.SelectedIndex + 1;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateInput();
            if (operationType == Type.Adding) 
                database.Product.Add(_product);
            database.SaveChanges();
            _mainWindow.ReadData();
            Hide();
        }

        private void CreateNewProductCathegoryButton_Click(object sender, RoutedEventArgs e)
        {
            var productCathegoryWindow = new ProductCathegoryWindow(_mainWindow);
            productCathegoryWindow.ShowDialog();
            ReadData();
        }

        private void CreateNewProductTypeButton_Click(object sender, RoutedEventArgs e)
        {
            var productTypeWindow = new ProductTypeWindow(_mainWindow);
            productTypeWindow.ShowDialog();
            ReadData();
        }
    }
}
