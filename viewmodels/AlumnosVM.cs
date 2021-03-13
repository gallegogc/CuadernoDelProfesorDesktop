using CUADERNODELPROFESOR.database.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUADERNODELPROFESOR.viewmodels
{
    class AlumnosVM
    {
        public AlumnosVM()
        {
            AlumnosAdapterList = new ObservableCollection<Alumno>();
        }

        public ObservableCollection<Alumno> AlumnosAdapterList { get; private set; }
    }
}
