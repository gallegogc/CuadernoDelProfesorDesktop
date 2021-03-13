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
    public partial class TareasWindow : Window
    {
        private string idTarea;
        private Asignatura asignatura;
        public TareasWindow(Asignatura asignaturaSeleccionada,string idTarea)
        {
            InitializeComponent();
            this.asignatura = asignaturaSeleccionada;
            this.idTarea = idTarea;
            rellenarCampos();
        }

        private void rellenarCampos()
        {

            WebService webService = new WebService();
            TareaResponse response = webService.getTarea(this.idTarea);

            if (response.estado == 1)
            {
                Tarea tarea = response.tareas.ElementAt(0);
                txtNombreTarea1.Text = tarea.nombre;
                cbTipoTarea.Text = tarea.tipo;
                txtNombreAsignaturaTarea.Text = this.asignatura.nombre;
                txtValoracionTarea.Text = tarea.valoracion;
            }
        }

        private void editarTarea(object sender, RoutedEventArgs e)
        {
            Button boton = sender as Button;
            if (boton.Content.Equals("MODIFICAR"))
            {
                boton.Content = "GUARDAR";
                boton.Background = (Brush)new BrushConverter().ConvertFrom("#FF7B9763");
                cbTipoTarea.IsEnabled = true;
                cbTipoTarea.IsReadOnly = false;
                txtNombreTarea1.IsEnabled = true;
                txtNombreTarea1.IsReadOnly = false;
                txtValoracionTarea.IsEnabled = true;
                txtValoracionTarea.IsReadOnly = false;
                dateTimePickerFecha.IsEnabled = true;
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
                        string nombre = Utils.initCap(txtNombreTarea1.Text, "no");
                        string fecha = dateTimePickerFecha.Text;
                        string valoracion = txtValoracionTarea.Text;
                        string tipo = cbTipoTarea.Text;

                        WebService webService = new WebService();
                        EstadoMensajeResponse response = webService.editarTarea(this.idTarea, this.asignatura.id, nombre, fecha, valoracion, tipo);

                        if (response.estado == 1)
                        {
                            Utils.msgBox(response.mensaje, "ok", "info");
                            ((MainWindow)this.Owner).cargarTareasClases();

                            boton.Content = "MODIFICAR";
                            boton.Background = (Brush)new BrushConverter().ConvertFrom("#FF979563");
                            cbTipoTarea.IsEnabled = false;
                            cbTipoTarea.IsReadOnly = true;
                            txtNombreTarea1.IsEnabled = false;
                            txtNombreTarea1.IsReadOnly = true;
                            txtValoracionTarea.IsEnabled = false;
                            txtValoracionTarea.IsReadOnly = true;
                            dateTimePickerFecha.IsEnabled = false;
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
            Regex regValoracion = new Regex("^[0-9]{1,3}$");

            if (txtNombreTarea1.Text.Length < 1)
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
            if (!regNombre.IsMatch(txtNombreTarea1.Text))
            {
                Utils.msgBox("Formato del nombre de tarea incorrecto", "ok", "warning");
                return false;
            }
            return true;
        }

        private void eliminarTarea(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = Utils.msgBox("¿Desea eliminar la tarea? Se eliminarán también todas las notas asociadas a ella", "yesno", "question");
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                WebService webService = new WebService();
                EstadoMensajeResponse response = webService.eliminarTarea(this.idTarea);

                if (response.estado == 1)
                {
                    Utils.msgBox(response.mensaje, "ok", "info");
                    // ACTUALIZAMOS LAS ASIGNATURAS DEL MAIN
                    ((MainWindow)this.Owner).cargarTareasClases();
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
