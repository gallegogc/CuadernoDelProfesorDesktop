using CUADERNODELPROFESOR.database.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUADERNODELPROFESOR.viewmodels
{
    class HorarioVM
    {
        public HorarioVM()
        {
            HorarioAdapterList = new ObservableCollection<Horario>();
        }

        public ObservableCollection<Horario> HorarioAdapterList { get; private set; }
    }
}
