using CUADERNODELPROFESOR.database.models;
using CUADERNODELPROFESOR.utils;
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
    /// Lógica de interacción para CursosAltaWindow.xaml
    /// </summary>
    public partial class AsignaturasWindow : Window
    {
        private String idAsignatura;
        private List<Curso> listaCursos = new List<Curso>();

        public AsignaturasWindow(String id)
        {
            InitializeComponent();
            this.idAsignatura = id;
            // RELLENAMOS EL CB DE CURSOS ANTES DE RELLENAR LOS CAMPOS CON LOS DATOS DE LA ASIGNATURA
            rellenarCBCursos();
            // HACEMOS LA CONSULTA Y RELLENAMOS LOS CAMPOS CON LOS DATOS
            rellenarCampos();
        }

        private void rellenarCBCursos()
        {
            WebService webService = new WebService();
            CursoResponse response = webService.getCursos(MainWindow.user.id);

            if (response.estado == 1)
            {
                // RECOGEMOS LA LISTA DE LOS CURSOS DE LA CONSULTA EN NUESTRA LISTA ATRIBUTO LOCAL DE CURSOS
                listaCursos = response.cursos;
                // INSTANCIAMOS UNA LISTA DE NOMBRES DE CURSOS DONDE VAMOS A ALMACENAR LOS NOMBRES PARA
                // MOSTRARLOS EN EL COMBOBOX
                List<String> nombresCursos = new List<String>();
                // POR CADA CURSO EN LA LISTA DE CURSOS RESPUESTA VAMOS AÑADIENDO EL NOMBRE
                foreach (Curso curso in response.cursos)
                {
                    nombresCursos.Add(curso.nombre);
                }
                // ASIGNAMOS LA LISTA DE NOMBRES AL ITEM SOURCE
                cbCursoAsignatura.ItemsSource = nombresCursos;
            }
            else
            {
                Utils.msgBox("No tiene cursos dados de alta, por favor, añada un curso antes de intentar añadir asignaturas", "ok", "warning");
            }
        }

        private void rellenarCampos()
        {
            WebService webService = new WebService();
            AsignaturaResponse response = webService.getAsignatura(this.idAsignatura);

            if (response.estado == 1)
            {
                Asignatura asignatura = response.asignaturas.ElementAt(0);
                txtNombreFichaAsignatura.Text = asignatura.nombre;
                cbCursoAsignatura.Text = asignatura.nombreCurso;

                txtHora1FichaAsignatura.Text = asignatura.hora1;
                txtHora2FichaAsignatura.Text = asignatura.hora2;
                txtHora3FichaAsignatura.Text = asignatura.hora3;
                txtHora4FichaAsignatura.Text = asignatura.hora4;
                txtHora5FichaAsignatura.Text = asignatura.hora5;
            }
            else if (response.estado == 2)
            {
                Utils.msgBox(response.mensaje, "ok", "error");
            }
        }

        private void eliminarAsignatura(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = Utils.msgBox("¿Desea eliminar la asignatura? Se eliminarán también las matrículas, tareas y apuntes de la misma", "yesno", "question");
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                WebService webService = new WebService();
                EstadoMensajeResponse response = webService.eliminarAsignatura(this.idAsignatura);

                if (response.estado == 1)
                {
                    Utils.msgBox(response.mensaje, "ok", "info");
                    // ACTUALIZAMOS LAS ASIGNATURAS DEL MAIN
                    ((MainWindow)this.Owner).cargarAsignaturas();
                    this.Close();
                }
                else if (response.estado == 2)
                {
                    Utils.msgBox(response.mensaje, "ok", "error");
                }
            }
        }

        private void editarAsignatura(object sender, RoutedEventArgs e)
        {
            Button boton = sender as Button;
            if (boton.Content.Equals("MODIFICAR"))
            {


                boton.Content = "GUARDAR";
                boton.Background = (Brush)new BrushConverter().ConvertFrom("#FF7B9763");
                txtNombreFichaAsignatura.IsEnabled = true;
                txtNombreFichaAsignatura.IsReadOnly = false;
                cbCursoAsignatura.IsEnabled = true;
                cbCursoAsignatura.IsReadOnly = false;
                txtHora1FichaAsignatura.IsEnabled = true;
                txtHora1FichaAsignatura.IsReadOnly = false;
                txtHora2FichaAsignatura.IsEnabled = true;
                txtHora2FichaAsignatura.IsReadOnly = false;
                txtHora3FichaAsignatura.IsEnabled = true;
                txtHora3FichaAsignatura.IsReadOnly = false;
                txtHora4FichaAsignatura.IsEnabled = true;
                txtHora4FichaAsignatura.IsReadOnly = false;
                txtHora5FichaAsignatura.IsEnabled = true;
                txtHora5FichaAsignatura.IsReadOnly = false;
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
                        // COGEMOS EL ID DEL CURSO QUE ESTÁ EN LA POSICIÓN DEL INDEX SELECCIONADO DEL COMBOBOX
                        string idCurso = listaCursos.ElementAt(cbCursoAsignatura.SelectedIndex).id;
                        string nombre = Utils.initCap(txtNombreFichaAsignatura.Text, "no");
                        string hora1 = txtHora1FichaAsignatura.Text;
                        string hora2 = txtHora2FichaAsignatura.Text;
                        string hora3 = txtHora3FichaAsignatura.Text;
                        string hora4 = txtHora4FichaAsignatura.Text;
                        string hora5 = txtHora5FichaAsignatura.Text;

                        WebService webService = new WebService();
                        EstadoMensajeResponse response = webService.editarAsignatura(this.idAsignatura, MainWindow.user.id, idCurso, nombre, hora1, hora2, hora3, hora4, hora5);


                        if (response.estado == 1)
                        {
                            Utils.msgBox(response.mensaje, "ok", "info");
                            ((MainWindow)this.Owner).cargarAsignaturas();

                            boton.Content = "MODIFICAR";
                            boton.Background = (Brush)new BrushConverter().ConvertFrom("#FF979563");
                            txtNombreFichaAsignatura.IsEnabled = false;
                            txtNombreFichaAsignatura.IsReadOnly = true;
                            cbCursoAsignatura.IsEnabled = false;
                            cbCursoAsignatura.IsReadOnly = true;
                            txtHora1FichaAsignatura.IsEnabled = false;
                            txtHora1FichaAsignatura.IsReadOnly = true;
                            txtHora2FichaAsignatura.IsEnabled = false;
                            txtHora2FichaAsignatura.IsReadOnly = true;
                            txtHora3FichaAsignatura.IsEnabled = false;
                            txtHora3FichaAsignatura.IsReadOnly = true;
                            txtHora4FichaAsignatura.IsEnabled = false;
                            txtHora4FichaAsignatura.IsReadOnly = true;
                            txtHora5FichaAsignatura.IsEnabled = false;
                            txtHora5FichaAsignatura.IsReadOnly = true;
                        }
                        else
                        {
                            Utils.msgBox(response.mensaje, "ok", "warning");
                        }
                    }
                }
            }
        }

        private bool validarFormulario()
        {
            Regex regNombre = new Regex("^[A-za-z\\s]{1,30}$");
            if (txtNombreFichaAsignatura.Text.Length < 1)
            {
                Utils.msgBox("El campo nombre de asignatura está vacío", "ok", "warning");
                return false;

            }
            if (cbCursoAsignatura.SelectedIndex == -1)
            {
                Utils.msgBox("No se le ha asignado un curso a la asignatura, por favor, seleccione uno", "ok", "warning");
                return false;
            }
            if (txtHora1FichaAsignatura.SelectedIndex == -1 && txtHora2FichaAsignatura.SelectedIndex == -1 && txtHora3FichaAsignatura.SelectedIndex == -1 && txtHora4FichaAsignatura.SelectedIndex == -1 && txtHora5FichaAsignatura.SelectedIndex == -1)
            {
                Utils.msgBox("No se ha asignado ninguna hora a la asignatura", "ok", "warning");
                return false;
            }
            if (!regNombre.IsMatch(txtNombreFichaAsignatura.Text))
            {
                Utils.msgBox("Formato del nombre de asignatura incorrecto", "ok", "warning");
                return false;

            }
            return true;
        }
    }



}
