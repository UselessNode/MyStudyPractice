using AppDB.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppDB.DataView
{

    class DatabaseManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if(PropertyChanged != null) 
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); 
        }

        private Invoice invoice;
        public Invoice Invoice { get { return invoice; } set { invoice = value; OnPropertyChanged("Invoice"); } } 

    }
}
