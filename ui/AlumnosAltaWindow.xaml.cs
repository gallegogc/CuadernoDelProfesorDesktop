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
    /// Lógica de interacción para AlumnosAltaWindow.xaml
    /// </summary>
    public partial class AlumnosAltaWindow : Window
    {
        public AlumnosAltaWindow()
        {
            InitializeComponent();
        }

        private void altaAlumno(object sender, RoutedEventArgs e)
        {
            // SI EL FORMULARIO SE VALIDA CORRECTAMENTE PROCEDEMOS A REALIZAR EL INSERT
            if (validarFormulario())
            {
                // RECOGEMOS LA INFORMACIÓN DE LOS CAMPOS Y LA PASAMOS A LOWER CASE SALVO LA PASS
                string nombre = Utils.initCap(txtNombreAltaAlumno.Text,"no");
                string apellidos = Utils.initCap(txtApellidosAltaAlumno.Text,"mayus");
                string direccion = Utils.initCap(txtDireccionAltaAlumno.Text,"no");
                string telefono = txtTelefonoAltaAlumno.Text;
                string email = txtEmailAltaAlumno.Text.ToLower();

                // INSTANCIAMOS EL WEBSERVICE LLAMANDO AL MÉTODO REGISTRO PROFESOR, ENVÍANDO LOS DATOS Y REALIZANDO EL INSERT EN LA BD
                WebService webService = new WebService();
                EstadoMensajeResponse response = webService.altaAlumno(MainWindow.user.id, nombre, apellidos, direccion, telefono, email);

                // VALORAMOS EL ESTADO DE LA RESPUESTA RECOGIDA PARA MOSTRAR LA INFORMACIÓN PERTINENTE
                if (response.estado == 1)
                {
                    Utils.msgBox(response.mensaje, "ok", "info");
                    // ACTUALIZAMOS LA TABLA DEL MAIN WINDOW
                    ((MainWindow)this.Owner).cargarAlumnos();
                    // LIMPIAMOS CAMPOS
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
            Regex regNombre = new Regex("^[A-z\\s]{2,19}$");
            Regex regApellidos = new Regex("^[A-z\\s]{4,39}$");
            if (txtNombreAltaAlumno.Text.Length < 1 || txtApellidosAltaAlumno.Text.Length < 1 || txtDireccionAltaAlumno.Text.Length < 1 || txtTelefonoAltaAlumno.Text.Length < 1 || txtEmailAltaAlumno.Text.Length < 1)
            {
                Utils.msgBox("Al menos un campo está vacío", "ok", "warning");
                return false;

            }
            if (!regNombre.IsMatch(txtNombreAltaAlumno.Text))
            {
                Utils.msgBox("Formato del nombre incorrecto", "ok", "warning");
                return false;

            }
            if (!regApellidos.IsMatch(txtApellidosAltaAlumno.Text))
            {
                Utils.msgBox("Formato de los apellidos incorrecto", "ok", "warning");
                return false;
            }
            return true;
        }

        private void limpiarCampos()
        {
            txtNombreAltaAlumno.Text = "";
            txtApellidosAltaAlumno.Text = "";
            txtDireccionAltaAlumno.Text = "";
            txtTelefonoAltaAlumno.Text = "";
            txtEmailAltaAlumno.Text = "";
        }

     
    }
}
