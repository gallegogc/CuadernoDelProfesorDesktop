using CUADERNODELPROFESOR.database.models;
using CUADERNODELPROFESOR.utils;
using CUADERNODELPROFESOR.viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class AlumnosPerfilWindow : Window
    {

        private String idAlumno;
        private List<Asignatura> listaAsignaturasMatriculadas;
        private List<Tarea> listaTareasAsignatura;

        public AlumnosPerfilWindow(String id)
        {
            InitializeComponent();
            this.idAlumno = id;
            rellenarCampos();
            cargarMatriculas();
            cargarCBAsignaturasMatriculadas();
        }

        private void rellenarCampos()
        {
            WebService webService = new WebService();
            AlumnoResponse response = webService.getAlumno(this.idAlumno);

            if (response.estado == 1)
            {
                Alumno alumno = response.alumnos.ElementAt(0);
                txtNombrePerfilAlumno.Text = alumno.nombre;
                txtApellidosPerfilAlumno.Text = alumno.apellidos;
                txtDireccionPerfilAlumno.Text = alumno.direccion;
                txtTelefonoPerfilAlumno.Text = alumno.telefono;
                txtEmailPerfilAlumno.Text = alumno.email;
            }
            else
            {
                Utils.msgBox(response.mensaje, "ok", "error");
            }
        }


        public void cargarMatriculas()
        {
            // INSTANCIAMOS FUERA DEL CONDICIONAL PARA RECARGAR LAS MATRICULAS SI SE HA DESMATRICULADO DE ALGUNA EL ALUMNO
            var viewModel = new MatriculasVM();
            viewModel.MatriculasAdapterList.Clear();
            dgMatriculasPerfilAlumno.ItemsSource = null;
            WebService webService = new WebService();
            MatriculaResponse response = webService.getMatriculas(this.idAlumno);

            if (response.estado == 1)
            {
                viewModel = new MatriculasVM();
                foreach (Matricula matricula in response.matriculas)
                {
                    viewModel.MatriculasAdapterList.Add(matricula);
                }
                dgMatriculasPerfilAlumno.ItemsSource = viewModel.MatriculasAdapterList;
            }

        }

        private void cargarCBAsignaturasMatriculadas()
        {

            WebService webService = new WebService();
            AsignaturaResponse response = webService.getAsignaturasMatriculadas(this.idAlumno);

            if (response.estado == 1)
            {
                listaAsignaturasMatriculadas = response.asignaturas;


                List<String> nombresAsignaturasMatriculadas = new List<String>();
                foreach (Asignatura asignatura in response.asignaturas)
                {
                    nombresAsignaturasMatriculadas.Add(asignatura.nombre);
                }
                // ASIGNAMOS LA LISTA DE NOMBRES AL ITEM SOURCE
                cbAsignaturasMatriculadas.ItemsSource = nombresAsignaturasMatriculadas;
            }
        }

        private void actualizarCBTareas(object sender, SelectionChangedEventArgs args)
        {
            int numAsignatura = (sender as ComboBox).SelectedIndex;
            string idAsignaturaSeleccionada = listaAsignaturasMatriculadas.ElementAt(numAsignatura).id;

            WebService webService = new WebService();
            TareaResponse response = webService.getTareasAsignatura(idAsignaturaSeleccionada);


            if (response.estado == 1)
            {
                listaTareasAsignatura = response.tareas;

                List<String> nombresTareas = new List<String>();
                foreach (Tarea tarea in response.tareas)
                {
                    nombresTareas.Add(tarea.nombre);
                }
                // ASIGNAMOS LA LISTA DE NOMBRES AL ITEM SOURCE
                cbTareasAsignaturasMatriculadas.ItemsSource = nombresTareas;
            }
            else
            {
                Utils.msgBox(response.mensaje, "ok", "warning");
                cbTareasAsignaturasMatriculadas.ItemsSource = null;
            }
        }

        private void actualizarNota(object sender, SelectionChangedEventArgs args)
        {
            int numTarea = (sender as ComboBox).SelectedIndex;
            if (numTarea != -1)
            {
                string idTareaSeleccionada = listaTareasAsignatura.ElementAt(numTarea).id;

                WebService webService = new WebService();
                NotaResponse response = webService.getNotaTarea(this.idAlumno, idTareaSeleccionada);

                if (response.estado == 1)
                {
                    txtNotaPerfilAlumno.Text = response.notas.ElementAt(0).puntuacion;
                }
                else
                {
                    txtNotaPerfilAlumno.Text = "-";
                }
            }
            else {
                txtNotaPerfilAlumno.Text = "";
            }
        }


        private void camposGridMatriculas(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "id":
                    e.Column.Header = "ID Matricula";
                    break;
                case "idAsignatura":
                    e.Column.Header = "ID Asignatura";
                    break;
                case "nombreAsignatura":
                    e.Column.Header = "Asignatura matriculada";
                    break;
                default:
                    break;
            }
        }

        private void editarAlumno(object sender, RoutedEventArgs e)
        {
            Button boton = sender as Button;
            if (boton.Content.Equals("MODIFICAR"))
            {
                boton.Content = "GUARDAR";
                boton.Background = (Brush)new BrushConverter().ConvertFrom("#FF7B9763");
                txtNombrePerfilAlumno.IsEnabled = true;
                txtNombrePerfilAlumno.IsReadOnly = false;
                txtApellidosPerfilAlumno.IsEnabled = true;
                txtApellidosPerfilAlumno.IsReadOnly = false;
                txtDireccionPerfilAlumno.IsEnabled = true;
                txtDireccionPerfilAlumno.IsReadOnly = false;
                txtTelefonoPerfilAlumno.IsEnabled = true;
                txtTelefonoPerfilAlumno.IsReadOnly = false;
                txtEmailPerfilAlumno.IsEnabled = true;
                txtEmailPerfilAlumno.IsReadOnly = false;
            }
            else
            {
                // PEDIMOS CONFIRMACIÓN
                MessageBoxResult messageBoxResult = Utils.msgBox("¿Desea guardar los cambios?", "yesno", "question");
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    // SI EL FORMULARIO SE VALIDA CORRECTAMENTE PROCEDEMOS A REALIZAR EL INSERT
                    if (validarFormulario())
                    {
                        string nombre = Utils.initCap(txtNombrePerfilAlumno.Text, "no");
                        string apellidos = Utils.initCap(txtApellidosPerfilAlumno.Text, "mayus");
                        string direccion = Utils.initCap(txtDireccionPerfilAlumno.Text, "no");
                        string telefono = txtTelefonoPerfilAlumno.Text;
                        string email = txtEmailPerfilAlumno.Text.ToLower();

                        WebService webService = new WebService();
                        EstadoMensajeResponse response = webService.editarAlumno(this.idAlumno, MainWindow.user.id, nombre, apellidos, direccion, telefono, email);


                        if (response.estado == 1)
                        {
                            Utils.msgBox(response.mensaje, "ok", "info");
                            ((MainWindow)this.Owner).cargarAlumnos();

                            boton.Content = "MODIFICAR";
                            boton.Background = (Brush)new BrushConverter().ConvertFrom("#FF979563");
                            txtNombrePerfilAlumno.IsEnabled = false;
                            txtNombrePerfilAlumno.IsReadOnly = true;
                            txtApellidosPerfilAlumno.IsEnabled = false;
                            txtApellidosPerfilAlumno.IsReadOnly = true;
                            txtDireccionPerfilAlumno.IsEnabled = false;
                            txtDireccionPerfilAlumno.IsReadOnly = true;
                            txtTelefonoPerfilAlumno.IsEnabled = false;
                            txtTelefonoPerfilAlumno.IsReadOnly = true;
                            txtEmailPerfilAlumno.IsEnabled = false;
                            txtEmailPerfilAlumno.IsReadOnly = true;
                        }
                        else
                        {
                            Utils.msgBox(response.mensaje, "ok", "warning");
                        }
                    }
                }
            }
        }

        private void eliminarAlumno(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = Utils.msgBox("¿Desea eliminar el alumno? Se eliminarán también sus matrículas, notas y faltas", "yesno", "question");
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                WebService webService = new WebService();
                EstadoMensajeResponse response = webService.eliminarAlumno(this.idAlumno);

                if (response.estado == 1)
                {
                    Utils.msgBox(response.mensaje, "ok", "info");
                    ((MainWindow)this.Owner).cargarAlumnos();
                    this.Close();
                }
                else
                {
                    Utils.msgBox(response.mensaje, "ok", "error");
                }
            }
        }

        private void desmatricular(object sender, RoutedEventArgs e)
        {
            Matricula matriculaSeleccionada = dgMatriculasPerfilAlumno.SelectedItem as Matricula;
            if (matriculaSeleccionada != null)
            {
                MessageBoxResult messageBoxResult = Utils.msgBox("¿Desea desmatricular al alumno de la asignatura seleccionada? Se eliminarán también las notas y faltas de la asignatura", "yesno", "question");
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    WebService webService = new WebService();
                    EstadoMensajeResponse response = webService.desmatricular(this.idAlumno, matriculaSeleccionada.idAsignatura);

                    if (response.estado == 1)
                    {
                        Utils.msgBox(response.mensaje, "ok", "info");
                        // RECARGAMOS LAS MATRÍCULAS DEL ALUMNO
                        cargarMatriculas();
                        cargarCBAsignaturasMatriculadas();
                    }
                    else
                    {
                        Utils.msgBox(response.mensaje, "ok", "error");
                    }
                }
            }
            else
            {
                Utils.msgBox("Seleccione una asignatura en la tabla superior", "ok", "warning");
            }
        }

        private void ponerNota(object sender, RoutedEventArgs e)
        {
            if (cbAsignaturasMatriculadas.SelectedIndex != -1 && cbTareasAsignaturasMatriculadas.SelectedIndex != -1)
            {
                double datoSalida;
                // SI SE PUEDE PASEAR A DOUBLE Y ESTÁ ENTRE 0 y 10 LA SUBIMOS
                if (double.TryParse(txtNotaPerfilAlumno.Text, out datoSalida))
                {
                    if (double.Parse(txtNotaPerfilAlumno.Text) >= 0 && double.Parse(txtNotaPerfilAlumno.Text) <= 10)
                    {
                        string idTarea = listaTareasAsignatura.ElementAt(cbTareasAsignaturasMatriculadas.SelectedIndex).id;
                        string puntuacion = txtNotaPerfilAlumno.Text.ToString();
                        WebService webService = new WebService();

                        EstadoMensajeResponse response = webService.altaNota(this.idAlumno, idTarea, puntuacion);

                        if (response.estado == 1)
                        {
                            Utils.msgBox(response.mensaje, "ok", "info");
                            // RECARGAMOS LAS MATRÍCULAS DEL ALUMNO
                            cargarMatriculas();
                            cargarCBAsignaturasMatriculadas();
                        }
                        else if (response.estado == 2)
                        {
                            Utils.msgBox(response.mensaje, "ok", "info");
                        }
                        else {
                            Utils.msgBox(response.mensaje, "ok", "error");
                        }
                    }
                    else
                    {
                        Utils.msgBox("La nota debe estar comprendida entre 0 y 10", "ok", "warning");
                    }
                }
                else
                {
                    Utils.msgBox("La puntuación no tiene un formato adecuado, tiene que ser un número, admite decimales", "ok", "warning");
                }
            }
            else
            {
                Utils.msgBox("Debe seleccionar una asignatura en la que esté matriculado el alumno y una tarea para editar la nota", "ok", "warning");
            }
        }

        private bool validarFormulario()
        {
            Regex regNombre = new Regex("^[A-z\\s]{2,19}$");
            Regex regApellidos = new Regex("^[A-z\\s]{4,39}$");
            Regex regDireccion = new Regex("^[A-z0-9\\s]{4,59}$");
            if (txtNombrePerfilAlumno.Text.Length < 1 || txtApellidosPerfilAlumno.Text.Length < 1 || txtDireccionPerfilAlumno.Text.Length < 1 || txtTelefonoPerfilAlumno.Text.Length < 1 || txtEmailPerfilAlumno.Text.Length < 1)
            {
                Utils.msgBox("Al menos un campo está vacío", "ok", "warning");
                return false;

            }
            if (!regNombre.IsMatch(txtNombrePerfilAlumno.Text))
            {
                Utils.msgBox("Formato del nombre incorrecto", "ok", "warning");
                return false;

            }
            if (!regApellidos.IsMatch(txtApellidosPerfilAlumno.Text))
            {
                Utils.msgBox("Formato de los apellidos incorrecto", "ok", "warning");
                return false;
            }
            if (!regDireccion.IsMatch(txtDireccionPerfilAlumno.Text))
            {
                Utils.msgBox("Formato de la dirección es incorrecto", "ok", "warning");
                return false;
            }
            return true;
        }
    }
}
