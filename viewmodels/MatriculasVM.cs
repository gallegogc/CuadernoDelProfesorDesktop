using CUADERNODELPROFESOR.database.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUADERNODELPROFESOR.viewmodels
{
    class MatriculasVM
    {
        public MatriculasVM()
        {
            MatriculasAdapterList = new ObservableCollection<Matricula>();
        }

        public ObservableCollection<Matricula> MatriculasAdapterList { get; private set; }
    }
}
