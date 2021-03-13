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

    public partial class CursosAltaWindow : Window
    {
        public CursosAltaWindow()
        {
            InitializeComponent();
        }

        // MÉTODO QUE ACTUALIZA EL CAMPO NOMBRE AL CAMBIAR EL TEXTO EN CUALQUIERA DEL RESTO DE TEXTBOX
        private void actualizarNombre(object sender, EventArgs e)
        {
            txtNombreAltaCurso.Text = cbNivelAltaCurso.Text + " " + cbTipoAltaCurso.Text + " " + txtGradoAltaCurso.Text + " " + txtLetraAltaCurso.Text;
        }

        // MÉTODO QUE ACTUALIZA EL NOMBRE Y LOS NIVELES DEL CB NIVELES AL CAMBIAR DE TIPO DE CURSO
        private void actualizarNombreTipo(object sender, SelectionChangedEventArgs args)
        {
            // TENEMOS QUE RECOGER LA PROPIEDAD SELECTEDITEM PARA OBTENER EL VALOR REAL ACTUAL SELECCIONADO
            // SINO TENDRÍAMOS SELECCIONADO EL ANTERIOR
            String tipo = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content as String;
            // ACTUALIZAMOS LOS NIVELES A MOSTRAR
            actualizarNiveles(tipo);
            // VALORANDO EL TIPO DE CURSO BLOQUEAMOS O NO EL CAMPO GRADO
            if (!tipo.Equals(""))
            {
                if (!tipo.Equals("FP"))
                {
                    txtGradoAltaCurso.Text = "";
                    txtGradoAltaCurso.IsEnabled = false;
                    txtGradoAltaCurso.IsReadOnly = true;
                }
                else
                {
                    txtGradoAltaCurso.IsEnabled = true;
                    txtGradoAltaCurso.IsReadOnly = false;
                }
            }
            else
            {
                tipo = "";
            }
            // ACTUALIZAMOS EL NOMBRE
            txtNombreAltaCurso.Text = cbNivelAltaCurso.Text + " " + tipo + " " + txtGradoAltaCurso.Text + " " + txtLetraAltaCurso.Text;
        }

        // MÉTODO QUE ACTUALIZA EL NOMBRE AL CAMBIAR EL NIVEL SELECCIONADO
        private void actualizarNombreNivel(object sender, SelectionChangedEventArgs args)
        {
            String nivel = (sender as ComboBox).SelectedItem as String;
            txtNombreAltaCurso.Text = nivel + " " + cbTipoAltaCurso.Text + " " + txtGradoAltaCurso.Text + " " + txtLetraAltaCurso.Text;
        }

        private void actualizarNiveles(String tipo)
        {
            List<String> listaNiveles = new List<String>();
            switch (tipo)
            {
                case "FP":
                    listaNiveles.Add("1");
                    listaNiveles.Add("2");
                    break;
                case "ESO":
                    listaNiveles.Add("1");
                    listaNiveles.Add("2");
                    listaNiveles.Add("3");
                    listaNiveles.Add("4");
                    break;
                case "BACH":
                    listaNiveles.Add("1");
                    listaNiveles.Add("2");
                    listaNiveles.Add("3");
                    listaNiveles.Add("4");
                    break;
                default:
                    break;
            }
            cbNivelAltaCurso.ItemsSource = listaNiveles;
        }

        private void altaCurso(object sender, RoutedEventArgs e)
        {
            // SI EL FORMULARIO SE VALIDA CORRECTAMENTE PROCEDEMOS A REALIZAR EL INSERT
            if (validarFormulario())
            {
                string nombre = txtNombreAltaCurso.Text.ToString().ToUpper();
                string tipo = cbTipoAltaCurso.Text.ToString();
                string grado = txtGradoAltaCurso.Text.ToString();
                String nivel = cbNivelAltaCurso.SelectedItem as String;
                string letra = txtLetraAltaCurso.Text.ToString().ToUpper();

                WebService webService = new WebService();
                EstadoMensajeResponse response = webService.altaCurso(MainWindow.user.id, nombre, tipo, grado, nivel, letra);
         
                if (response.estado == 1)
                {
                    Utils.msgBox(response.mensaje, "ok", "info");
                    ((MainWindow)this.Owner).cargarCursos();
                    limpiarCampos();
                }
                else
                {
                    Utils.msgBox(response.mensaje, "ok", "warning");
                }
            }
        }

        private void limpiarCampos()
        {
            txtNombreAltaCurso.Text = "";
            cbTipoAltaCurso.SelectedIndex = 0;
            cbNivelAltaCurso.SelectedIndex = -1;
            txtLetraAltaCurso.Text = "";
        }

        private bool validarFormulario()
        {
            if(txtNombreAltaCurso.Text.Length<1)
            {
                Utils.msgBox("El campo nombre está vacío ", "ok", "warning");
                return false;
            }
            if (cbTipoAltaCurso.Text.Equals("FP") && txtGradoAltaCurso.Text.Equals(""))
            {
                Utils.msgBox("Los cursos de FP no pueden tener el campo Grado vacío", "ok", "warning");
                return false;
            }
            if (txtLetraAltaCurso.Text.Length > 1)
            {
                Utils.msgBox("El campo letra sólo puede tener un carácter cómo máximo", "ok", "warning");
                return false;
            }
            return true;
        }
    }
}
