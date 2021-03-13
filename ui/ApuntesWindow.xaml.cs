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
    public partial class ApuntesWindow : Window
    {
        private string idApunte;
        Asignatura asignatura;
        public ApuntesWindow(Asignatura asignaturaSeleccionada,string idApunte)
        {
            InitializeComponent();
            this.asignatura = asignaturaSeleccionada;
            this.idApunte = idApunte;
            rellenarCampos();
        }

        private void rellenarCampos() {
            WebService webService = new WebService();
            ApunteResponse response = webService.getApunte(this.idApunte);

            if (response.estado == 1)
            {
                Apunte apunte = response.apuntes.ElementAt(0);
                txtNombreApuntes.Text = apunte.nombre;
                cbTipoApuntes.Text = apunte.tipo;
                txtNombreAsignatura.Text = this.asignatura.nombre;
                txtEnlaceApuntes.Text = apunte.enlace;
                txtDescripcionAputnes.Text = apunte.descripcion;

            }
        }

        private void editarApunte(object sender, RoutedEventArgs e)
        {
            Button boton = sender as Button;
            if (boton.Content.Equals("MODIFICAR"))
            {
                boton.Content = "GUARDAR";
                boton.Background = (Brush)new BrushConverter().ConvertFrom("#FF7B9763");
                cbTipoApuntes.IsEnabled = true;
                cbTipoApuntes.IsReadOnly = false;
                txtNombreApuntes.IsEnabled = true;
                txtNombreApuntes.IsReadOnly = false;
                txtEnlaceApuntes.IsEnabled = true;
                txtEnlaceApuntes.IsReadOnly = false;
                txtDescripcionAputnes.IsEnabled = true;
                txtDescripcionAputnes.IsReadOnly = false;
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
                        string nombre = Utils.initCap(txtNombreApuntes.Text, "no");
                        string tipo = cbTipoApuntes.Text.ToString();
                        string enlace = txtEnlaceApuntes.Text;
                        string descripcion = txtDescripcionAputnes.Text;

                        WebService webService = new WebService();
                        EstadoMensajeResponse response = webService.editarApuntes(this.idApunte, this.asignatura.id, nombre, tipo, enlace, descripcion);

                        if (response.estado == 1)
                        {
                            Utils.msgBox(response.mensaje, "ok", "info");
                            ((MainWindow)this.Owner).cargarApuntesClases();

                            boton.Content = "MODIFICAR";
                            boton.Background = (Brush)new BrushConverter().ConvertFrom("#FF979563");
                            cbTipoApuntes.IsEnabled = false;
                            cbTipoApuntes.IsReadOnly = true;
                            txtNombreApuntes.IsEnabled = false;
                            txtNombreApuntes.IsReadOnly = true;
                            txtEnlaceApuntes.IsEnabled = false;
                            txtEnlaceApuntes.IsReadOnly = true;
                            txtDescripcionAputnes.IsEnabled = false;
                            txtDescripcionAputnes.IsReadOnly = true;
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

        private void eliminarApunte(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = Utils.msgBox("¿Desea eliminar los apuntes?", "yesno", "question");
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                WebService webService = new WebService();
                EstadoMensajeResponse response = webService.eliminarApuntes(this.idApunte);

                if (response.estado == 1)
                {
                    Utils.msgBox(response.mensaje, "ok", "info");
                    ((MainWindow)this.Owner).cargarApuntesClases();
                    this.Close();
                }
                else
                {
                    Utils.msgBox(response.mensaje, "ok", "error");
                }
            }
        }
    }
}
