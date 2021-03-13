using CUADERNODELPROFESOR.database.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUADERNODELPROFESOR.viewmodels
{
    class ApuntesVM
    {
        public ApuntesVM()
        {
            ApuntesAdapterList = new ObservableCollection<Apunte>();
        }

        public ObservableCollection<Apunte> ApuntesAdapterList { get; private set; }
    }
}
