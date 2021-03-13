using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUADERNODELPROFESOR.database.models
{
    class MatriculaResponse
    {
        public int estado { get; set; }
        public String mensaje { get; set; }
        public List<Matricula> matriculas { get; set; }

    }
}
