using CUADERNODELPROFESOR.database.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUADERNODELPROFESOR.viewmodels
{
    class CursosVM
    {
        public CursosVM()
        {
            CursosAdapterList = new ObservableCollection<Curso>();
        }

        public ObservableCollection<Curso> CursosAdapterList { get; private set; }
    }
}
