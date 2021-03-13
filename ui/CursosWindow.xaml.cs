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

    public partial class CursosWindow : Window
    {
        private String idCurso;

        public CursosWindow(String id)
        {
            InitializeComponent();
            this.idCurso = id;
            rellenarCampos();
            txtGradoCurso.IsEnabled = false;
        }


        private void rellenarCampos()
        {
            WebService webService = new WebService();
            CursoResponse response = webService.getCurso(this.idCurso);

            if (response.estado == 1)
            {
                Curso curso = response.cursos.ElementAt(0);
                cbTipoCurso.Text = curso.tipo;
                txtGradoCurso.Text = curso.grado;
                cbNivelFichaCurso.Text = curso.nivel;
                txtLetraFichaCurso.Text = curso.letra;
                txtNombreCurso.Text = curso.nombre;
            }
            else if (response.estado == 2)
            {
                Utils.msgBox(response.mensaje, "ok", "error");
            }
        }


        // MÉTODO QUE ACTUALIZA EL CAMPO NOMBRE AL CAMBIAR EL TEXTO EN CUALQUIERA DEL RESTO DE TEXTBOX
        private void actualizarNombre(object sender, EventArgs e)
        {
            txtNombreCurso.Text = cbNivelFichaCurso.Text + " " + cbTipoCurso.Text + " " + txtGradoCurso.Text + " " + txtLetraFichaCurso.Text;
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
                    txtGradoCurso.Text = "";
                    txtGradoCurso.IsEnabled = false;
                    txtGradoCurso.IsReadOnly = true;
                }
                else
                {
                    txtGradoCurso.IsEnabled = true;
                    txtGradoCurso.IsReadOnly = false;
                }
            }
            else
            {
                tipo = "";
            }
            // ACTUALIZAMOS EL NOMBRE
            txtNombreCurso.Text = cbNivelFichaCurso.Text + " " + tipo + " " + txtGradoCurso.Text + " " + txtLetraFichaCurso.Text;
        }

        // MÉTODO QUE ACTUALIZA EL NOMBRE AL CAMBIAR EL NIVEL SELECCIONADO
        private void actualizarNombreNivel(object sender, SelectionChangedEventArgs args)
        {
            String nivel = (sender as ComboBox).SelectedItem as String;
            txtNombreCurso.Text = nivel + " " + cbTipoCurso.Text + " " + txtGradoCurso.Text + " " + txtLetraFichaCurso.Text;
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
            cbNivelFichaCurso.ItemsSource = listaNiveles;
        }





        private void editarCurso(object sender, RoutedEventArgs e)
        {
            Button boton = sender as Button;
            if (boton.Content.Equals("MODIFICAR"))
            {
                boton.Content = "GUARDAR";
                boton.Background = (Brush)new BrushConverter().ConvertFrom("#FF7B9763");
                cbTipoCurso.IsEnabled = true;
                cbTipoCurso.IsReadOnly = false;
                txtLetraFichaCurso.IsEnabled = true;
                txtLetraFichaCurso.IsReadOnly = false;
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
                        string nombre = txtNombreCurso.Text.ToString().ToUpper();
                        string tipo = cbTipoCurso.Text.ToString();
                        string grado = txtGradoCurso.Text.ToString();
                        String nivel = cbNivelFichaCurso.SelectedItem as String;
                        string letra = txtLetraFichaCurso.Text.ToString().ToUpper();

                        WebService webService = new WebService();
                        EstadoMensajeResponse response = webService.editarCurso(this.idCurso, MainWindow.user.id, nombre, tipo, grado, nivel, letra);


                        if (response.estado == 1)
                        {
                            Utils.msgBox(response.mensaje, "ok", "info");
                            ((MainWindow)this.Owner).cargarCursos();

                            boton.Content = "MODIFICAR";
                            boton.Background = (Brush)new BrushConverter().ConvertFrom("#FF979563");
                            txtNombreCurso.IsEnabled = false;
                            txtNombreCurso.IsReadOnly = true;
                            cbTipoCurso.IsEnabled = false;
                            cbTipoCurso.IsReadOnly = true;
                            cbNivelFichaCurso.IsEnabled = false;
                            cbNivelFichaCurso.IsReadOnly = true;
                            txtGradoCurso.IsEnabled = false;
                            txtGradoCurso.IsReadOnly = true;
                            txtLetraFichaCurso.IsEnabled = false;
                            txtLetraFichaCurso.IsReadOnly = true;
                        }
                        else
                        {
                            Utils.msgBox(response.mensaje, "ok", "warning");
                        }
                    }
                }
            }
        }

        private void eliminarCurso(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = Utils.msgBox("¿Desea eliminar el curso? Se eliminarán también las asignaturas asociadas al mismo, así como sus matrículas, tareas y apuntes", "yesno", "question");
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                WebService webService = new WebService();
                EstadoMensajeResponse response = webService.eliminarCurso(this.idCurso);

                if (response.estado == 1)
                {
                    Utils.msgBox(response.mensaje, "ok", "info");
                    // ACTUALIZAMOS LAS ASIGNATURAS DEL MAIN
                    ((MainWindow)this.Owner).cargarCursos();
                    this.Close();
                }
                else
                {
                    Utils.msgBox(response.mensaje, "ok", "error");
                }
            }
        }

        private bool validarFormulario()
        {
            if (txtNombreCurso.Text.Length < 1)
            {
                Utils.msgBox("El campo nombre está vacío ", "ok", "warning");
                return false;
            }
            if (cbTipoCurso.Text.Equals("FP") && txtGradoCurso.Text.Equals(""))
            {
                Utils.msgBox("Los cursos de FP no pueden tener el campo Grado vacío", "ok", "warning");
                return false;
            }
            if (txtLetraFichaCurso.Text.Length > 1)
            {
                Utils.msgBox("El campo letra sólo puede tener un carácter cómo máximo", "ok", "warning");
                return false;
            }
            return true;
        }

    }
}
