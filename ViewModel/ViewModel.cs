using AppDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDB.ViewModel
{
    internal class ViewModel
    {
        private DatabaseEntities db;
        public ViewModel()
        {
            db = new DatabaseEntities();
        }
    }
}
