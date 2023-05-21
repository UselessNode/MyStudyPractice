using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDB.ViewModel
{
    internal class AppTheme
    {
        PaletteHelper palette;

        public AppTheme()
        {
            palette = new PaletteHelper();
        }

        public void ChangeTheme(IBaseTheme newTheme)
        {
            ITheme theme = palette.GetTheme();
            theme.SetBaseTheme(newTheme);
            palette.SetTheme(theme);
        }
        public bool IsDarkTheme()
        {
            ITheme theme = palette.GetTheme();
            bool result = theme.GetBaseTheme() == BaseTheme.Dark;
            return result;
        }
    }
}
