using CUADERNODELPROFESOR.database.models;
using CUADERNODELPROFESOR.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CUADERNODELPROFESOR
{
    /// <summary>
    /// Lógica de interacción para AlumnosAltaWindow.xaml
    /// </summary>
    public partial class AlumnosMatricularWindow : Window
    {
        private Asignatura asignatura;
        private List<Alumno> listaAlumnos;
        public AlumnosMatricularWindow(Asignatura asignatura)
        {
            InitializeComponent();
            this.asignatura = asignatura;
            nombreAsignatura();
            cargarCBAlumnos();
        }

        private void nombreAsignatura()
        {
            txtAsignaturaMatricular.Text = asignatura.nombre;
        }

        private void cargarCBAlumnos()
        {

            // HACEMOS LA CONSULTA DE LOS ALUMNOS DEL PROFESOR
            WebService webService = new WebService();
            AlumnoResponse response = webService.getAlumnos(MainWindow.user.id);

            if (response.estado == 1)
            {
                // IGUALAMOS LAS ASIGNATURAS RESPUESTA A NUESTRA LISTA DE ASIGNATURAS
                listaAlumnos = response.alumnos;

                // RELLENAMOS UNA LISTA CON LOS NOMBRES DE LOS ALUMNOS
                List<String> nombresAlumnos = new List<String>();

                // POR CADA ASIGNATURA EN LA LISTA DE RESPUESTA VAMOS AÑADIENDO EL NOMBRE
                foreach (Alumno alumno in response.alumnos)
                {
                    nombresAlumnos.Add(alumno.nombre + " " + alumno.apellidos);
                }
                // ASIGNAMOS LA LISTA DE NOMBRES AL ITEM SOURCE
                cbAlumnoMatricula.ItemsSource = nombresAlumnos;
            }
            else
            {
                Utils.msgBox("No tiene alumnos dados de alta", "ok", "warning");
            }
        }

        private void matricularAlumno(object sender, RoutedEventArgs e)
        {
            if (cbAlumnoMatricula.SelectedIndex != -1)
            {
                // RECOGEMOS EL ID ALUMNO MEDIANTE LA LISTA DE ALUMNOS y EL ÍNDICE SELECCIONADO
                string idAlumno = listaAlumnos.ElementAt(cbAlumnoMatricula.SelectedIndex).id;
                // RECOGEMOS EL ID ASIGNATURA DEL OBJETO ASIGNATURA
                string idAsignatura = asignatura.id;

                // HACEMOS EL INSERT A LA BD
                WebService webService = new WebService();
                EstadoMensajeResponse response = webService.matricular(idAlumno, idAsignatura);

                if (response.estado == 1)
                {
                    Utils.msgBox(response.mensaje, "ok", "info");
                    ((MainWindow)this.Owner).cargarAlumnosClases();
                    cbAlumnoMatricula.SelectedIndex = -1;
                }
                else
                {
                    Utils.msgBox(response.mensaje, "ok", "warning");
                }
            }
            else
            {
                Utils.msgBox("No ha seleccionado ningún alumno a matricular", "ok", "warning");
            }
        }



    }
}
