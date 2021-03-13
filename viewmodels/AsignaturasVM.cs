using CUADERNODELPROFESOR.database.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUADERNODELPROFESOR.viewmodels
{
    class AsignaturasVM
    {
        public AsignaturasVM()
        {
            AsignaturasAdapterList = new ObservableCollection<Asignatura>();
        }

        public ObservableCollection<Asignatura> AsignaturasAdapterList { get; private set; }
    }
}
