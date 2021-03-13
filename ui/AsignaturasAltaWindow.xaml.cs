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
    public partial class AsignaturasAltaWindow : Window
    {

        private List<Curso> listaCursos = new List<Curso>();
        public AsignaturasAltaWindow()
        {
            InitializeComponent();
            rellenarCBCursos();

        }

     
        private void rellenarCBCursos() {
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
                cbCursoAltaAsignatura.ItemsSource = nombresCursos;
            }
            else
            {
                Utils.msgBox("No tiene cursos dados de alta, por favor, añada un curso antes de intentar añadir asignaturas", "ok", "warning");
            }
        }


        private void altaAsignatura(object sender, RoutedEventArgs e)
        {
            // SI EL FORMULARIO SE VALIDA CORRECTAMENTE PROCEDEMOS A REALIZAR EL INSERT
            if (validarFormulario())
            {
                // COGEMOS EL ID DEL CURSO QUE ESTÁ EN LA POSICIÓN DEL INDEX SELECCIONADO DEL COMBOBOX
                string idCurso = listaCursos.ElementAt(cbCursoAltaAsignatura.SelectedIndex).id;
                string nombre = Utils.initCap(txtNombreAltaAsignatura.Text,"no");
                string hora1 = txtHora1AltaAsignatura.Text;
                string hora2 = txtHora2AltaAsignatura.Text;
                string hora3 = txtHora3AltaAsignatura.Text;
                string hora4 = txtHora4AltaAsignatura.Text;
                string hora5 = txtHora5AltaAsignatura.Text;

                WebService webService = new WebService();
                EstadoMensajeResponse response = webService.altaAsignatura(MainWindow.user.id, idCurso, nombre, hora1, hora2, hora3, hora4, hora5);

                if (response.estado == 1)
                {
                    Utils.msgBox(response.mensaje, "ok", "info");
                    ((MainWindow)this.Owner).cargarAsignaturas();
                    limpiarCampos();
                }
                else
                {
                    Utils.msgBox(response.mensaje, "ok", "warning");
                }
            }
        }

        private bool validarFormulario()
        {
            Regex regNombre = new Regex("^[A-za-z\\s]{1,30}$");
            if (txtNombreAltaAsignatura.Text.Length < 1)
            {
                Utils.msgBox("El campo nombre de asignatura está vacío", "ok", "warning");
                return false;

            }
            if (cbCursoAltaAsignatura.SelectedIndex == -1)
            {
                Utils.msgBox("No se le ha asignado un curso a la asignatura, por favor, seleccione uno", "ok", "warning");
                return false;
            }
            if (txtHora1AltaAsignatura.SelectedIndex < 1 && txtHora2AltaAsignatura.SelectedIndex < 1 && txtHora3AltaAsignatura.SelectedIndex < 1 && txtHora4AltaAsignatura.SelectedIndex < 1 && txtHora5AltaAsignatura.SelectedIndex < 1)
            {
                Utils.msgBox("No se ha asignado ninguna hora a la asignatura", "ok", "warning");
                return false;
            }
            if (!regNombre.IsMatch(txtNombreAltaAsignatura.Text))
            {
                Utils.msgBox("Formato del nombre de asignatura incorrecto", "ok", "warning");
                return false;

            }
            return true;
        }



        private void limpiarCampos()
        {
            txtNombreAltaAsignatura.Text = "";
            cbCursoAltaAsignatura.SelectedIndex = -1;
            txtHora1AltaAsignatura.Text = "";
            txtHora2AltaAsignatura.Text = "";
            txtHora3AltaAsignatura.Text = "";
            txtHora4AltaAsignatura.Text = "";
            txtHora5AltaAsignatura.Text = "";
        }


    }
}
