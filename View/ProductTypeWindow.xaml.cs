using AppDB.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для AddNewProductTypeWindow.xaml
    /// </summary>
    public partial class ProductTypeWindow : Window
    {
        DatabaseEntities database;
        ProductType _productType;
        MainWindow _mainWindow;
        enum Type { Editing = 0, Adding = 1 }
        Type operationType;

        public ProductTypeWindow(MainWindow mainWindow, ProductType productType)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _productType = productType;
            SaveButton.Content = "Сохранить";
            operationType = Type.Editing;
            ReadData();
        }

        public ProductTypeWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _productType = new ProductType();
            SaveButton.Content = "Добавить";
            operationType = Type.Adding;
            ReadData();
        }

        void ReadData()
        {
            database = _mainWindow.Entities;
            DataContext = _productType;
        }

        void ValidateInput()
        {
            _productType.Name = TextBoxTypeName.Text;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateInput();
            if (operationType == Type.Adding)
                database.ProductType.Add(_productType);
            database.SaveChanges();
            _mainWindow.ReadData();
            Hide();
        }
    }
}
