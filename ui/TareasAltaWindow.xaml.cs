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
    public partial class TareasAltaWindow : Window
    {
        private Asignatura asignatura;
        public TareasAltaWindow(Asignatura asignatura)
        {
            InitializeComponent();
            this.asignatura = asignatura;
            txtNombreAsignaturaTarea.Text = this.asignatura.nombre;


        }

        private void programarTarea(object sender, RoutedEventArgs e)
        {
            if (validarFormulario())
            {
                // COGEMOS EL ID DEL CURSO QUE ESTÁ EN LA POSICIÓN DEL INDEX SELECCIONADO DEL COMBOBOX
                string nombre = Utils.initCap(txtNombreTarea.Text, "no");
                string fecha = dateTimePickerFecha.Text;
                string valoracion = txtValoracionTarea.Text;
                string tipo = cbTipoTarea.Text;

                WebService webService = new WebService();
                EstadoMensajeResponse response = webService.altaTarea(this.asignatura.id, nombre, fecha, valoracion, tipo);

                if (response.estado == 1)
                {
                    Utils.msgBox(response.mensaje, "ok", "info");
                    ((MainWindow)this.Owner).cargarTareasClases();
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
            Regex regNombre = new Regex("^[A-za-z0-9\\s]{1,30}$");
            Regex regValoracion = new Regex("^[0-9]{1,3}$");

            if (txtNombreTarea.Text.Length < 1)
            {
                Utils.msgBox("El campo nombre de tarea está vacío", "ok", "warning");
                return false;

            }
            if (cbTipoTarea.SelectedIndex == -1)
            {
                Utils.msgBox("No se le ha asignado un tipo a la tarea, por favor, seleccione uno", "ok", "warning");
                return false;
            }
            if (dateTimePickerFecha.SelectedDate == null)
            {
                Utils.msgBox("No ha elegido una fecha", "ok", "warning");
                return false;
            }
            if (!regValoracion.IsMatch(txtValoracionTarea.Text))
            {
                Utils.msgBox("No ha introducido una valoración ponderada o el valor no es adecuado (Sólo números naturales positivos)", "ok", "warning");
                return false;
            }
            if (!regNombre.IsMatch(txtNombreTarea.Text))
            {
                Utils.msgBox("Formato del nombre de tarea incorrecto", "ok", "warning");
                return false;
            }
            return true;
        }

        private void limpiarCampos()
        {
            txtNombreTarea.Text = "";
            cbTipoTarea.SelectedIndex = -1;
            txtValoracionTarea.Text = "";
        }
    }


}
