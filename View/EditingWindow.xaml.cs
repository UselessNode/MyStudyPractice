﻿using AppDB.Data;
using AppDB.View;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    public partial class EditingWindow : Window
    {

        Invoice _invoice; // Выбранная для изменения накладная
        MainWindow _mainWindow; // Главная страница

        DatabaseEntities database;

        public EditingWindow(Invoice invoice, MainWindow mainWindow)
        {
            InitializeComponent();
            _invoice = invoice;
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
            _invoice.Cost = String.IsNullOrEmpty(TextBoxCost.Text) ? 0 : int.Parse(Regex.Match(TextBoxCost.Text, @"\d+").Value);
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateInput();
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
            
            // Индексы при редактировании
            ComboBoxProduct.SelectedIndex = _invoice.ProductId is null ? -1 : (int)_invoice.ProductId;
            ComboBoxPurveyor.SelectedIndex = _invoice.SupplierId is null? -1 : (int)_invoice.SupplierId;
            ComboBoxForwarder.SelectedIndex = _invoice.ForwarderId is null ? -1 : (int)_invoice.ForwarderId;
        }
    }
}
