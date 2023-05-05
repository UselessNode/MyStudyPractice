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
    /// Логика взаимодействия для AddNewProductCathegoryWindow.xaml
    /// </summary>
    public partial class ProductCathegoryWindow : Window
    {
        MainWindow _mainWindow;
        ProductCategory _category;
        DatabaseEntities database;
        enum Type { Editing = 0, Adding = 1 }
        Type operationType;

        public ProductCathegoryWindow(MainWindow mainWindow, ProductCategory category)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _category = category;
            operationType = Type.Editing;
            SaveButton.Content = "Сохранить";
            ReadData();
        }
        public ProductCathegoryWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _category = new ProductCategory();
            operationType = Type.Adding;
            SaveButton.Content = "Добавить";
            ReadData();
        }

        void ReadData()
        {
            database = _mainWindow.Entities;
            DataContext = _category;
        }

        void ValidateInput()
        {
            _category.Name = TextBoxCathegoryName.Text;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateInput();
            if (operationType == Type.Adding)
                database.ProductCategory.Add(_category);
            database.SaveChanges();
            _mainWindow.ReadData();
            Hide();
        }
    }
}
