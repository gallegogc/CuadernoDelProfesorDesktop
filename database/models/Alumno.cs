using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUADERNODELPROFESOR.database.models
{
    class Alumno
    {
        public String id { get; set; }
        public String idprofesor { get; set; }
        public String nombre{ get; set; }
        public String apellidos { get; set; }
        public String direccion { get; set; }
        public String telefono { get; set; }
        public String email{ get; set; }

    }
}
