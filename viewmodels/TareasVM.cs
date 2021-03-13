using CUADERNODELPROFESOR.database.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUADERNODELPROFESOR.viewmodels
{
    class TareasVM
    {
        public TareasVM()
        {
            TareasAdapterList = new ObservableCollection<Tarea>();
        }

        public ObservableCollection<Tarea> TareasAdapterList { get; private set; }
    }
}
