using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUADERNODELPROFESOR.database.models
{
    class Apunte
    {
        public String id { get; set; }
        public String idAsignatura { get; set; }
        public String nombre { get; set; }
        public String tipo { get; set; }
        public String enlace { get; set; }
        public String descripcion { get; set; }

    }
}
