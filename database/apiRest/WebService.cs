using CUADERNODELPROFESOR.database.models;
using CUADERNODELPROFESOR.utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CUADERNODELPROFESOR
{
    class WebService
    {
        private string baseUrl = "http://4a7a63a664.gclientes.com/";

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////                PROFESORES             ////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        public LoginProfesorResponse loginProfesor(string user, string pass)
        {
            string endpoint = this.baseUrl + "/profesores/login";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                user = user,
                pass = pass
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<LoginProfesorResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public EstadoMensajeResponse registroProfesor(string user, string pass, string nombre, string apellidos)
        {
            string endpoint = this.baseUrl + "/profesores/logup";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                user = user,
                pass = pass,
                nombre = nombre,
                apellidos = apellidos
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }





        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////                CURSOS                 ////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CursoResponse getCursos(String idProfesor)
        {
            string endpoint = this.baseUrl + "/cursos/profesor/" + idProfesor;

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<CursoResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CursoResponse getCurso(String idCurso)
        {
            string endpoint = this.baseUrl + "/cursos/curso/" + idCurso;

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<CursoResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public EstadoMensajeResponse altaCurso(string idProfesor, string nombre, string tipo, string grado, string nivel, string letra)
        {
            string endpoint = this.baseUrl + "/cursos";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                idProfesor = idProfesor,
                nombre = nombre,
                tipo = tipo,
                grado = grado,
                nivel = nivel,
                letra = letra
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public EstadoMensajeResponse editarCurso(string id, string idProfesor, string nombre, string tipo, string grado, string nivel, string letra)
        {
            string endpoint = this.baseUrl + "/cursos/edit";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                id = id,
                idProfesor = idProfesor,
                nombre = nombre,
                tipo = tipo,
                grado = grado,
                nivel = nivel,
                letra = letra
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public EstadoMensajeResponse eliminarCurso(String idCurso)
        {
            string endpoint = this.baseUrl + "/cursos/eliminar/" + idCurso;
            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////                ASIGNATURAS            ////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        public AsignaturaResponse getAsignaturas(String idProfesor)
        {
            string endpoint = this.baseUrl + "/asignaturas/profesor/" + idProfesor;
            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<AsignaturaResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public AsignaturaResponse getAsignatura(String idAsignatura)
        {
            string endpoint = this.baseUrl + "/asignaturas/asignatura/" + idAsignatura;
            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<AsignaturaResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public AsignaturaResponse getAsignaturasCurso(String idCurso)
        {
            string endpoint = this.baseUrl + "/asignaturas/curso/" + idCurso;
            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<AsignaturaResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public AsignaturaResponse getAsignaturasMatriculadas(string idAlumno)
        {
            string endpoint = this.baseUrl + "/asignaturas/alumno/" + idAlumno;
            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<AsignaturaResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public EstadoMensajeResponse altaAsignatura(string idProfesor, string idCurso, string nombre, string hora1, string hora2, string hora3, string hora4, string hora5)
        {
            string endpoint = this.baseUrl + "/asignaturas";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                idProfesor = idProfesor,
                idCurso = idCurso,
                nombre = nombre,
                hora1 = hora1,
                hora2 = hora2,
                hora3 = hora3,
                hora4 = hora4,
                hora5 = hora5
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public EstadoMensajeResponse editarAsignatura(string id, string idProfesor, string idCurso, string nombre, string hora1, string hora2, string hora3, string hora4, string hora5)
        {
            string endpoint = this.baseUrl + "/asignaturas/edit";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                id = id,
                idProfesor = idProfesor,
                idCurso = idCurso,
                nombre = nombre,
                hora1 = hora1,
                hora2 = hora2,
                hora3 = hora3,
                hora4 = hora4,
                hora5 = hora5
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public EstadoMensajeResponse eliminarAsignatura(String idAsignatura)
        {
            string endpoint = this.baseUrl + "/asignaturas/eliminar/" + idAsignatura;
            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////                ALUMNOS                ////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        public AlumnoResponse getAlumnos(String idProfesor)
        {
            string endpoint = this.baseUrl + "/alumnos/profesor/" + idProfesor;

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<AlumnoResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public AlumnoResponse getAlumno(String idAlumno)
        {
            string endpoint = this.baseUrl + "/alumnos/alumno/" + idAlumno;

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<AlumnoResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public AlumnoResponse getAlumnosAsignatura(String idAsignaturaSeleccionada)
        {
            string endpoint = this.baseUrl + "/alumnos/asignatura/" + idAsignaturaSeleccionada;

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<AlumnoResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }



        public MatriculaResponse getMatriculas(String idAlumno)
        {
            string endpoint = this.baseUrl + "/alumnos/matriculas/" + idAlumno;

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<MatriculaResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public EstadoMensajeResponse matricular(String idAlumno, String idAsignatura)
        {
            string endpoint = this.baseUrl + "/alumnos/matricular/" + idAlumno + "/" + idAsignatura;

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public EstadoMensajeResponse desmatricular(String idAlumno, String idAsignatura)
        {
            string endpoint = this.baseUrl + "/alumnos/desmatricular/" + idAlumno + "/" + idAsignatura;

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }



        public EstadoMensajeResponse altaAlumno(string idProfesor, string nombre, string apellidos, string direccion, string telefono, string email)
        {
            string endpoint = this.baseUrl + "/alumnos";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                idProfesor = idProfesor,
                nombre = nombre,
                apellidos = apellidos,
                direccion = direccion,
                telefono = telefono,
                email = email
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public AlumnoResponse getAlumnoID(Alumno alumno)
        {
            string endpoint = this.baseUrl + "/alumnos/alumno/" + alumno.id;

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<AlumnoResponse>(response);

            }
            catch (Exception)
            {
                return null;
            }
        }



        public EstadoMensajeResponse editarAlumno(string id, string idProfesor, string nombre, string apellidos, string direccion, string telefono, string email)
        {
            string endpoint = this.baseUrl + "/alumnos/edit";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                id = id,
                idProfesor = idProfesor,
                nombre = nombre,
                apellidos = apellidos,
                direccion = direccion,
                telefono = telefono,
                email = email
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public EstadoMensajeResponse eliminarAlumno(String id)
        {
            string endpoint = this.baseUrl + "/alumnos/eliminar/" + id;
            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////                APUNTES                ////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////


        public ApunteResponse getApuntesAsignatura(string idAsignaturaSeleccionada)
        {
            string endpoint = this.baseUrl + "/apuntes/asignatura/" + idAsignaturaSeleccionada;

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<ApunteResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApunteResponse getApunte(string id)
        {
            string endpoint = this.baseUrl + "/apuntes/apunte/" + id;

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<ApunteResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public EstadoMensajeResponse subirApuntes(string idAsignatura, string nombre, string tipo, string enlace, string descripcion)
        {
            string endpoint = this.baseUrl + "/apuntes";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                idAsignatura = idAsignatura,
                nombre = nombre,
                tipo = tipo,
                enlace = enlace,
                descripcion = descripcion
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public EstadoMensajeResponse editarApuntes(string id, string idAsignatura, string nombre, string tipo, string enlace, string descripcion)
        {
            string endpoint = this.baseUrl + "/apuntes/edit";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                id = id,
                idAsignatura = idAsignatura,
                nombre = nombre,
                tipo = tipo,
                enlace = enlace,
                descripcion = descripcion
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public EstadoMensajeResponse eliminarApuntes(string id)
        {
            string endpoint = this.baseUrl + "/apuntes/eliminar/" + id;
            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////                  TAREAS               ////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        public TareaResponse getTareasAsignatura(string idAsignaturaSeleccionada)
        {
            string endpoint = this.baseUrl + "/tareas/asignatura/" + idAsignaturaSeleccionada;

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<TareaResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TareaResponse getTarea(string idTarea)
        {
            string endpoint = this.baseUrl + "/tareas/tarea/" + idTarea;

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<TareaResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public EstadoMensajeResponse altaTarea(string idAsignatura, string nombre, string fecha, string valoracion, string tipo)
        {
            string endpoint = this.baseUrl + "/tareas";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                idAsignatura = idAsignatura,
                nombre = nombre,
                fecha = fecha,
                valoracion = valoracion,
                tipo = tipo
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public EstadoMensajeResponse editarTarea(string id, string idAsignatura, string nombre, string fecha, string valoracion, string tipo)
        {
            string endpoint = this.baseUrl + "/tareas/edit";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                id = id,
                idAsignatura = idAsignatura,
                nombre = nombre,
                fecha = fecha,
                valoracion = valoracion,
                tipo = tipo
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public EstadoMensajeResponse eliminarTarea(String id)
        {
            string endpoint = this.baseUrl + "/tareas/eliminar/" + id;
            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////                  NOTAS                ////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        public NotaResponse getNotaTarea(string idAlumno, string idTarea)
        {
            string endpoint = this.baseUrl + "/notas/tareaAlumno/" + idAlumno + "/" + idTarea;

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<NotaResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public EstadoMensajeResponse altaNota(string idAlumno, string idTarea, string puntuacion)
        {
            string endpoint = this.baseUrl + "/notas";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                idAlumno = idAlumno,
                idTarea = idTarea,
                puntuacion = puntuacion
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////                  HORARIO                ////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////


        public EstadoMensajeResponse getHorario(string idProfesor, string horaABuscar,string dia)
        {
            string endpoint = this.baseUrl + "/asignaturas/horario/"+idProfesor+"/"+horaABuscar+"/"+dia;
            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.DownloadString(endpoint);
                return JsonConvert.DeserializeObject<EstadoMensajeResponse>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }


}


