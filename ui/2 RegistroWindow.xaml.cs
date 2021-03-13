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
  
    public partial class RegistroWindow : Window
    {
        private bool registroFinalizado = false;

        public RegistroWindow()
        {
            InitializeComponent();
        }



        private void registroProfesor(object sender, RoutedEventArgs e)
        {
            // SI EL FORMULARIO SE VALIDA CORRECTAMENTE PROCEDEMOS A REALIZAR EL INSERT
            if (validarFormulario())
            {
                // RECOGEMOS LA INFORMACIÓN DE LOS CAMPOS Y LA PASAMOS A LOWER CASE SALVO LA PASS
                string user = txtUserRegistro.Text.ToLower();
                string pass = txtPassRegistro.Password;
                string nombre = txtNombreRegistro.Text.ToLower();
                string apellidos = txtApellidosRegistro.Text.ToLower();

                // INSTANCIAMOS EL WEBSERVICE LLAMANDO AL MÉTODO REGISTRO PROFESOR, ENVÍANDO LOS DATOS Y REALIZANDO EL INSERT EN LA BD
                WebService webService = new WebService();
                EstadoMensajeResponse response = webService.registroProfesor(user, pass, nombre, apellidos);

                // VALORAMOS EL ESTADO DE LA RESPUESTA RECOGIDA PARA MOSTRAR LA INFORMACIÓN PERTINENTE
                if (response.estado == 1)
                {
                    Utils.msgBox("Usuario registrado con éxito","ok", "info");
                    limpiarCampos();
                }
                else
                {
                    Utils.msgBox("El nombre de usuario ya está registrado","ok", "warning");
                }
            }
        }

        private bool validarFormulario()
        {
            Regex regUser = new Regex("^[A-z0-9_][a-z0-9_]{4,19}$");
            Regex regNombre = new Regex("^[A-z\\s]{2,19}$");
            Regex regApellidos = new Regex("^[A-z\\s]{4,39}$");
            if (txtUserRegistro.Text.Length < 1 || txtPassRegistro.Password.Length < 1 || txtPass2Registro.Password.Length < 1 || txtNombreRegistro.Text.Length < 1 || txtApellidosRegistro.Text.Length < 1)
            {
                Utils.msgBox("Al menos un campo está vacío","ok", "warning");
                return false;

            }
            if (!regUser.IsMatch(txtUserRegistro.Text.ToString()))
            {
                Utils.msgBox("Formato del nombre de usuario incorrecto","ok", "warning");
                return false;

            }
            if (txtPassRegistro.Password.Length < 2 || txtPassRegistro.Password.Length > 19)
            {
                Utils.msgBox("Longitud de la contraseña no apropiada (3 - 18 caracteres)", "ok", "warning");
                return false;

            }
            if (txtPassRegistro.Password != txtPass2Registro.Password)
            {
                Utils.msgBox("Las contraseñas no coinciden", "ok", "warning");
                return false;

            }
            if (!regNombre.IsMatch(txtNombreRegistro.Text))
            {
                Utils.msgBox("Formato del nombre incorrecto","ok", "warning");
                return false;

            }
            if (!regApellidos.IsMatch(txtApellidosRegistro.Text))
            {
                Utils.msgBox("Formato de los apellidos incorrecto", "ok", "warning");
                return false;
            }
            return true;
        }

        private void limpiarCampos()
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            registroFinalizado = true;
            this.Close();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!registroFinalizado)
            {

                MessageBoxResult messageBoxResult = Utils.msgBox("¿Desea volver a la ventana de inicio de sesión?","yesno", "question");

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    LoginWindow login = new LoginWindow();
                    login.Show();
                }
                else
                {
                    e.Cancel = true;
                }

            }
        }


    }
}
