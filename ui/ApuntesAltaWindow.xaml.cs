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
    /// Lógica de interacción para ApuntesAltaWindow.xaml
    /// </summary>
    public partial class ApuntesAltaWindow : Window
    {
        private Asignatura asignatura;
        public ApuntesAltaWindow(Asignatura asignatura)
        {
            InitializeComponent();
            this.asignatura = asignatura;
            txtNombreAsignaturaApuntes.Text = this.asignatura.nombre;
        }

        private void subirApunte(object sender, RoutedEventArgs e)
        {
            if (validarFormulario())
            {
                string nombre = Utils.initCap(txtNombreApuntes.Text, "no");
                string tipo = cbTipoApuntes.Text.ToString();
                string enlace = txtEnlaceApuntes.Text;
                string descripcion= txtDescripcionAputnes.Text;

                WebService webService = new WebService();
                EstadoMensajeResponse response = webService.subirApuntes(this.asignatura.id, nombre, tipo, enlace, descripcion);

                if (response.estado == 1)
                {
                    Utils.msgBox(response.mensaje, "ok", "info");
                    ((MainWindow)this.Owner).cargarApuntesClases();
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
            if (txtNombreApuntes.Text.Length < 1)
            {
                Utils.msgBox("El campo nombre de apuntes está vacío", "ok", "warning");
                return false;

            }
            if (cbTipoApuntes.SelectedIndex == -1)
            {
                Utils.msgBox("No se ha seleccionado un tipo de apuntes, por favor, seleccione uno", "ok", "warning");
                return false;
            }
            if (txtEnlaceApuntes.Text.Length < 1)
            {
                Utils.msgBox("No ha añadido un enlace a los apuntes", "ok", "warning");
                return false;
            }
            if (!regNombre.IsMatch(txtNombreApuntes.Text))
            {
                Utils.msgBox("Formato del nombre de asignatura incorrecto", "ok", "warning");
                return false;

            }
            return true;
        }

        private void limpiarCampos()
        {
            txtNombreApuntes.Text = "";
            cbTipoApuntes.SelectedIndex = -1;
            txtEnlaceApuntes.Text = "";
            txtDescripcionAputnes.Text = "";
        }
    }
}
