using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUADERNODELPROFESOR.database.models
{
    class Horario
    {

        public Horario(string hora,string asignaturaLunes,string asignaturaMartes,string asignaturaMiercoles,string asignaturaJueves,string asignaturaViernes) {
            this.hora = hora;
            this.asignaturaLunes = asignaturaLunes;
            this.asignaturaMartes = asignaturaMartes;
            this.asignaturaMiercoles = asignaturaMiercoles;
            this.asignaturaJueves = asignaturaJueves;
            this.asignaturaViernes = asignaturaViernes;
        }

        public String hora { get; set; }
        public String asignaturaLunes { get; set; }
        public String asignaturaMartes { get; set; }
        public String asignaturaMiercoles { get; set; }
        public String asignaturaJueves { get; set; }
        public String asignaturaViernes { get; set; }
    }
}
