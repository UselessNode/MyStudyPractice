using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppDB.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        public ICommand DarkModeCommand { get; }

        public MainViewModel()
        {
            DarkModeCommand = new ViewModelCommand(ExecuteDarkModeCommand);
        }

        private void ExecuteDarkModeCommand(object obj)
        {
            PaletteHelper palette = new PaletteHelper();

            ITheme theme = palette.GetTheme();

            if (IsChecked)
            {
                theme.SetBaseTheme(Theme.Dark);
            }
            else
            {
                theme.SetBaseTheme(Theme.Light);
            }
            palette.SetTheme(theme);
        }
    }
}
