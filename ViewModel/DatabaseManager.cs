using AppDB.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppDB.ViewModel
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

        public DatabaseEntities Entities { get; set; }
        public List<Invoice> Invoices { get; set; }
        public List<Product> Products { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<ProductType> ProductTypes  { get; set; }
        public List<Supplier> Suppliers  { get; set; }
        public List<Forwarder> Forwarders  { get; set; }




        public void ReadData()
        {
            Entities = new DatabaseEntities();
        }
    }
}
