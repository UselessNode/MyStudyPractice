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
            if(Properties.Settings.Default.isDarkTheme)
            {
                Console.Write("Theme = Dark");
                SwithTheme(Theme.Dark);
            }
            else
            {
                Console.Write("Theme = Light");
                SwithTheme(Theme.Light);
            }
            
        }

        public static void SwithTheme(IBaseTheme baseTheme)
        {
            PaletteHelper palette = new PaletteHelper();
            ITheme theme = palette.GetTheme();
            theme.SetBaseTheme(baseTheme);
            palette.SetTheme(theme);
        }

        private void EnterenceButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "Admin" &&
                Password.Password == "admin")
            {
                OpenMainWindow();
                
            }
            else
            {
                LoginTextBox.Focus();
            }

        }

        void OpenMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }
    }
}
