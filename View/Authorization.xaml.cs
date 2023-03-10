using MaterialDesignThemes.Wpf;
using System;
using System.Windows;

namespace AppDB.View
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
            OpenMainWindow();
        }

        private void EnterenceButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "Admin" &&
                Password.Password == "admin")
                OpenMainWindow();
            else
                LoginTextBox.Focus();

        }

        void OpenMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }
    }
}
