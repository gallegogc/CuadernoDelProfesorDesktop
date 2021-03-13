using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUADERNODELPROFESOR.database.models
{
    public class Profesor
    {
        public String id { get; set; }
        public String user { get; set; }
        public String pass{ get; set; }
        public String nombre{ get; set; }
        public String apellidos { get; set; }
        public String status { get; set; }
    }
}
