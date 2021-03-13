using CUADERNODELPROFESOR.database.models;
using CUADERNODELPROFESOR.utils;
using CUADERNODELPROFESOR.viewmodels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CUADERNODELPROFESOR
{

    public partial class MainWindow : Window
    {

        public static Profesor user;

        public MainWindow(Profesor user)
        {
            InitializeComponent();
            MainWindow.user = user;
            nombreApellidosMayus();
        }

        private void nombreApellidosMayus()
        {
            String nombre = Utils.initCap(MainWindow.user.nombre, "no");
            String apellidos = Utils.initCap(MainWindow.user.apellidos, "mayus");
            lblNombreMainHome.Content = nombre + " " + apellidos;
        }


        ////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////     MÉTODOS MENÚ    ////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////

        private void bienvenidaMenuClick(object sender, RoutedEventArgs e)
        {
            panelBienvenida.Visibility = System.Windows.Visibility.Visible;
            panelCursos.Visibility = System.Windows.Visibility.Hidden;
            panelAsignaturas.Visibility = System.Windows.Visibility.Hidden;
            panelAlumnos.Visibility = System.Windows.Visibility.Hidden;
            panelClases.Visibility = System.Windows.Visibility.Hidden;
            panelHorario.Visibility = System.Windows.Visibility.Hidden;
        }

        private void cursosMenuClick(object sender, RoutedEventArgs e)
        {

            panelBienvenida.Visibility = System.Windows.Visibility.Hidden;
            panelCursos.Visibility = System.Windows.Visibility.Visible;
            panelAsignaturas.Visibility = System.Windows.Visibility.Hidden;
            panelAlumnos.Visibility = System.Windows.Visibility.Hidden;
            panelClases.Visibility = System.Windows.Visibility.Hidden;
            panelHorario.Visibility = System.Windows.Visibility.Hidden;
            cargarCursos();
        }

        private void asignaturasMenuClick(object sender, RoutedEventArgs e)
        {
            panelBienvenida.Visibility = System.Windows.Visibility.Hidden;
            panelCursos.Visibility = System.Windows.Visibility.Hidden;
            panelAsignaturas.Visibility = System.Windows.Visibility.Visible;
            panelAlumnos.Visibility = System.Windows.Visibility.Hidden;
            panelClases.Visibility = System.Windows.Visibility.Hidden;
            panelHorario.Visibility = System.Windows.Visibility.Hidden;
            cargarAsignaturas();
        }

        private void alumnosMenuClick(object sender, RoutedEventArgs e)
        {
            panelBienvenida.Visibility = System.Windows.Visibility.Hidden;
            panelCursos.Visibility = System.Windows.Visibility.Hidden;
            panelAsignaturas.Visibility = System.Windows.Visibility.Hidden;
            panelAlumnos.Visibility = System.Windows.Visibility.Visible;
            panelClases.Visibility = System.Windows.Visibility.Hidden;
            panelHorario.Visibility = System.Windows.Visibility.Hidden;
            cargarAlumnos();
        }

        private void clasesMenuClick(object sender, RoutedEventArgs e)
        {
            panelBienvenida.Visibility = System.Windows.Visibility.Hidden;
            panelCursos.Visibility = System.Windows.Visibility.Hidden;
            panelAsignaturas.Visibility = System.Windows.Visibility.Hidden;
            panelAlumnos.Visibility = System.Windows.Visibility.Hidden;
            panelClases.Visibility = System.Windows.Visibility.Visible;
            panelHorario.Visibility = System.Windows.Visibility.Hidden;

            prepararDataGrids();
            cargarCBCursos();
        }

        private void horarioMenuClick(object sender, RoutedEventArgs e)
        {
            panelBienvenida.Visibility = System.Windows.Visibility.Hidden;
            panelCursos.Visibility = System.Windows.Visibility.Hidden;
            panelAsignaturas.Visibility = System.Windows.Visibility.Hidden;
            panelAlumnos.Visibility = System.Windows.Visibility.Hidden;
            panelClases.Visibility = System.Windows.Visibility.Hidden;
            panelHorario.Visibility = System.Windows.Visibility.Visible;
            cargarHorario();
        }

        private void cerrarSesionMenuClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("¿Desea cerrar sesión y volver a la ventana de inicio de sesión?", "Confirmación", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                LoginWindow login = new LoginWindow();
                login.Show();
                this.Hide();
            }

        }


        ////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////     MÉTODOS CURSOS    //////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////

        public void cargarCursos()
        {
            WebService webService = new WebService();
            CursoResponse response = webService.getCursos(MainWindow.user.id + "");

            if (response.estado == 1)
            {
                var viewModel = new CursosVM();
                // RELLENAMOS EL APAPTER LIST CON LOS CURSOS 
                foreach (Curso curso in response.cursos)
                {
                    viewModel.CursosAdapterList.Add(curso);
                }
                DataContext = viewModel;
                // ESCONDEMOS LAS DOS PRIMERAS COLUMNAS, LAS DE ID E ID PROFESOR
                dgCursosMainCursos.Columns[0].Visibility = Visibility.Collapsed;
                dgCursosMainCursos.Columns[1].Visibility = Visibility.Collapsed;
            }
            else if (response.estado == -1)
            {
                Utils.msgBox(response.mensaje, "ok", "warning");
            }

        }
        private void cursosDGClick(object sender, MouseButtonEventArgs e)
        {
            if (!ventanaAbierta())
            {
                if (dgCursosMainCursos.SelectedItem != null)
                {
                    Curso curso = dgCursosMainCursos.SelectedItem as Curso;
                    CursosWindow cursosFicha = new CursosWindow(curso.id);
                    cursosFicha.Show();
                    cursosFicha.Owner = this;
                }
            }
        }
        private void camposGridCursos(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "nombre":
                    e.Column.Header = "Curso";
                    break;
                case "tipo":
                    e.Column.Header = "Tipo";
                    break;
                case "grado":
                    e.Column.Header = "Grado";
                    break;
                case "nivel":
                    e.Column.Header = "Nivel";
                    break;
                case "letra":
                    e.Column.Header = "Letra";
                    break;
                default:
                    break;
            }
        }



        private void añadirCurso(object sender, RoutedEventArgs e)
        {

            // SI NO TENEMOS UNA VENTANA YA LA MOSTRAMOS
            if (!ventanaAbierta())
            {
                CursosAltaWindow altaCursos = new CursosAltaWindow();
                altaCursos.Show();
                altaCursos.Owner = this;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////     MÉTODOS ASIGNATURAS    /////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////

        public void cargarAsignaturas()
        {
            WebService webService = new WebService();
            AsignaturaResponse response = webService.getAsignaturas(MainWindow.user.id + "");

            if (response.estado == 1)
            {
                var viewModel = new AsignaturasVM();
                foreach (Asignatura asignatura in response.asignaturas)
                {
                    viewModel.AsignaturasAdapterList.Add(asignatura);
                }
                DataContext = viewModel;
                // ESCONDEMOS LA PRIMERA COLUMNA, LA DE ID
                dgAsignaturasMainAsignaturas.Columns[0].Visibility = Visibility.Collapsed;
                dgAsignaturasMainAsignaturas.Columns[1].Width = 80;
                dgAsignaturasMainAsignaturas.Columns[3].Width = 65;
                dgAsignaturasMainAsignaturas.Columns[4].Width = 65;
                dgAsignaturasMainAsignaturas.Columns[5].Width = 65;
                dgAsignaturasMainAsignaturas.Columns[6].Width = 65;
                dgAsignaturasMainAsignaturas.Columns[7].Width = 65;
            }
            else if (response.estado == -1)
            {
                Utils.msgBox(response.mensaje, "ok", "warning");
            }

        }

        private void asignaturasDGClick(object sender, MouseButtonEventArgs e)
        {
            // RECOGEMOS LA ASIGNATURA SELECCIONADA Y LE PASAMOS AL OBJETO DE LA NUEVA VENTANA SU ID PARA 
            // MOSTRAR ALLÍ LOS DATOS
            if (!ventanaAbierta())
            {
                if (dgAsignaturasMainAsignaturas.SelectedItem != null)
                {
                    Asignatura asignatura = dgAsignaturasMainAsignaturas.SelectedItem as Asignatura;
                    AsignaturasWindow asignaturasFicha = new AsignaturasWindow(asignatura.id);
                    asignaturasFicha.Show();
                    asignaturasFicha.Owner = this;
                }
            }
        }

        private void camposGridAsignaturas(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "id":
                    e.Column.Header = "ID Asignatura";
                    break;
                case "nombreCurso":
                    e.Column.Header = "Curso";
                    break;
                case "nombre":
                    e.Column.Header = "Asignatura";
                    break;
                case "hora1":
                    e.Column.Header = "Lunes";
                    break;
                case "hora2":
                    e.Column.Header = "Martes";
                    break;
                case "hora3":
                    e.Column.Header = "Miércoles";
                    break;
                case "hora4":
                    e.Column.Header = "Jueves";
                    break;
                case "hora5":
                    e.Column.Header = "Viernes";
                    break;
                default:
                    break;
            }
        }

        private void añadirAsignatura(object sender, RoutedEventArgs e)
        {
            if (!ventanaAbierta())
            {
                AsignaturasAltaWindow asignaturasAlta = new AsignaturasAltaWindow();
                asignaturasAlta.Owner = this;
                asignaturasAlta.Show();
            }
        }



        ////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////     MÉTODOS ALUMNOS        /////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////

        public void cargarAlumnos()
        {
            WebService webService = new WebService();
            AlumnoResponse response = webService.getAlumnos(MainWindow.user.id + "");

            if (response.estado == 1)
            {
                var viewModel = new AlumnosVM();
                foreach (Alumno alumno in response.alumnos)
                {
                    viewModel.AlumnosAdapterList.Add(alumno);
                }
                DataContext = viewModel;
                // ESCONDEMOS LAS DOS PRIMERAS COLUMNAS, LAS DE ID E ID PROFESOR
                dgAlumnosMainAlumnos.Columns[0].Visibility = Visibility.Collapsed;
                dgAlumnosMainAlumnos.Columns[1].Visibility = Visibility.Collapsed;
            }
            else if (response.estado == -1)
            {
                Utils.msgBox(response.mensaje, "ok", "warning");
            }

        }

        private void alumnosDGClick(object sender, MouseButtonEventArgs e)
        {
            if (!ventanaAbierta())
            {
                if (dgAlumnosMainAlumnos.SelectedItem != null)
                {
                    // COMPROBAMOS QUE PANEL ESTÁ VISIBLE SI ALUMNOS O CLASES
                    // PARA SABER DE QUÉ DATAGRID DEBEMOS RECOGER EL ALUMNO CLICADO
                    Alumno alumno;
                    if (panelAlumnos.Visibility == Visibility.Visible)
                    {
                        alumno = dgAlumnosMainAlumnos.SelectedItem as Alumno;
                    }
                    else
                    {
                        alumno = dgAlumnosMainClases.SelectedItem as Alumno;

                    }
                    AlumnosPerfilWindow alumnoFicha = new AlumnosPerfilWindow(alumno.id);
                    alumnoFicha.Show();
                    alumnoFicha.Owner = this;
                }
            }
        }

        private void camposGridAlumnos(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "id":
                    e.Column.Header = "ID Asignatura";
                    break;
                case "nombreCurso":
                    e.Column.Header = "Curso";
                    break;
                case "nombre":
                    e.Column.Header = "Nombre";
                    break;
                case "apellidos":
                    e.Column.Header = "Apellidos";
                    break;
                case "direccion":
                    e.Column.Header = "Dirección";
                    break;
                case "telefono":
                    e.Column.Header = "Teléfono";
                    break;
                case "email":
                    e.Column.Header = "Correo Electrónico";
                    break;
                case "hora5":
                    e.Column.Header = "Viernes";
                    break;
                default:
                    break;
            }
        }

        private void añadirAlumnos(object sender, RoutedEventArgs e)
        {
            if (!ventanaAbierta())
            {
                AlumnosAltaWindow alumnosAlta = new AlumnosAltaWindow();
                // HACEMOS OWNER DE LA NUEVA VENTANA A LA MAIN, PARA PODER RECARGAR LOS ALUMNOS AL AÑADIRLOS
                alumnosAlta.Owner = this;
                alumnosAlta.Show();
            }
        }

        private void perfilAlumnos(object sender, RoutedEventArgs e)
        {

            // SI NO TENEMOS UNA VENTANA YA LA MOSTRAMOS
            if (!ventanaAbierta())
            {
                if (dgAlumnosMainAlumnos.SelectedItem != null)
                {
                    Alumno alumno = dgAlumnosMainAlumnos.SelectedItem as Alumno;
                    AlumnosPerfilWindow alumnoFicha = new AlumnosPerfilWindow(alumno.id);
                    alumnoFicha.Show();
                    alumnoFicha.Owner = this;
                }
            }
        }





        ////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////     MÉTODOS CLASES         /////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////
        private List<Curso> listaCursos = new List<Curso>();
        private List<Asignatura> listaAsignaturas = new List<Asignatura>();


        // MÉTODO QUE RELLENA LOS DATAGRID SIN DATOS PARA QUE APAREZCAN LOS CAMPOS ANTES DE SELECCIONAR UNA CLASE
        private void prepararDataGrids()
        {
            var viewModel = new AlumnosVM();
            dgAlumnosMainClases.ItemsSource = viewModel.AlumnosAdapterList;
            dgAlumnosMainClases.Columns[0].Visibility = Visibility.Collapsed;
            dgAlumnosMainClases.Columns[1].Visibility = Visibility.Collapsed;

            var viewModel2 = new ApuntesVM();
            dgApuntesMainClases.ItemsSource = viewModel2.ApuntesAdapterList;
            dgApuntesMainClases.Columns[0].Visibility = Visibility.Collapsed;
            dgApuntesMainClases.Columns[1].Visibility = Visibility.Collapsed;

            var viewModel3 = new TareasVM();
            dgTareasMainClases.ItemsSource = viewModel3.TareasAdapterList;
            dgTareasMainClases.Columns[0].Visibility = Visibility.Collapsed;
            dgTareasMainClases.Columns[1].Visibility = Visibility.Collapsed;
        }



        // MÉTODO QUE CARGA LOS CURSOS EN EL COMBOBOX CURSOS
        public void cargarCBCursos()
        {
            cbCursosClases.SelectedIndex = -1;
            cbAsignaturasClases.SelectedIndex = -1;

            // LISTA CON LOS CURSOS, CUYOS NOMBRES SE HAN CARGADO EN EL COMBOBOX 
            WebService webService = new WebService();
            CursoResponse response = webService.getCursos(MainWindow.user.id);

            if (response.estado == 1)
            {
                // IGUALAMOS LOS CURSOS A UNA LISTA DE CURSOS, CUANDO SE HAGA CLIC EN EL CURSO
                // SABREMOS EL ÍNDICE Y ACCEDEREMOS AL ÍNDICE DE ESTA LISTA
                // NO ACCEDEMOS DIRECTAMENTE AL SELECTED ITEM DEL COMBOBOX, PORQUE AL ESTAR LLENADO
                // MEDIANTE CONSULTA EN C# Y NO DESDE EL XAML DA PROBLEMAS
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
                cbCursosClases.ItemsSource = nombresCursos;
            }
            else
            {
                Utils.msgBox("No tiene cursos dados de alta, por favor, añada un curso antes de intentar añadir asignaturas", "ok", "warning");
            }
        }

        // MÉTODO QUE ACTUALIZA LOS DATOS DEL COMBOBOX ASIGNATURAS AL HACER CLIC EN UN CURSO DEL COMBOBOX CURSOS
        private void actualizarCBAsignaturasClases(object sender, SelectionChangedEventArgs args)
        {
            if (cbCursosClases.SelectedIndex != -1)
            {
                // RECOGEMOS EL INDEX SELECCIONADO DEL COMBOBOX
                int numCursoSeleccionado = (sender as ComboBox).SelectedIndex;
                // CON EL INDEX ACCEDEMOS A LA LISTA DE OBJETOS CURSO Y COGEMOS SU NOMBRE
                String idCurso = listaCursos.ElementAt(numCursoSeleccionado).id;

                // HACEMOS LA CONSULTA DE LAS ASIGNATURAS DEL CURSO
                WebService webService = new WebService();
                AsignaturaResponse response = webService.getAsignaturasCurso(idCurso);

                if (response.estado == 1)
                {
                    // IGUALAMOS LAS ASIGNATURAS RESPUESTA A NUESTRA LISTA DE ASIGNATURAS
                    listaAsignaturas = response.asignaturas;

                    // RELLENAMOS UNA LISTA CON LOS NOMBRES DE LAS ASIGNATURAS Y EL COMBOBOX DE LAS MISMAS
                    List<String> nombresAsignaturas = new List<String>();

                    // POR CADA ASIGNATURA EN LA LISTA DE RESPUESTA VAMOS AÑADIENDO EL NOMBRE
                    foreach (Asignatura asignatura in response.asignaturas)
                    {
                        nombresAsignaturas.Add(asignatura.nombre);
                    }
                    // ASIGNAMOS LA LISTA DE NOMBRES AL ITEM SOURCE
                    cbAsignaturasClases.ItemsSource = nombresAsignaturas;
                }
                else
                {
                    Utils.msgBox("No tiene asignaturas dadas de alta para ese curso", "ok", "warning");
                }
            }
        }

        // MÉTODO QUE MUESTRA LOS DATOS EN LOS DISTINTOS DATAGRID UNA VEZ SE HA SELECCIONADO UNA ASIGNATURA EN EL COMBOBOX ASIGNATURAS
        private void actualizarDatosClases(object sender, SelectionChangedEventArgs args)
        {
            if (cbAsignaturasClases.SelectedIndex != -1)
            {
                // CARGAMOS LOS DATOS EN LOS DISTINTOS DATA GRID
                cargarAlumnosClases();
                cargarApuntesClases();
                cargarTareasClases();

            }
        }




        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////      ALUMNOS CLASES
        public void cargarAlumnosClases()
        {
            // RECOGEMOS EL INDEX SELECCIONADO DEL COMBOBOX
            int numAsignaturaSeleccionada = cbAsignaturasClases.SelectedIndex;
            // CON EL INDEX ACCEDEMOS A LA LISTA DE OBJETOS CURSO Y COGEMOS SU NOMBRE
            string idAsignaturaSeleccionada = listaAsignaturas.ElementAt(numAsignaturaSeleccionada).id;

            // HACEMOS LA CONSULTA
            WebService webService = new WebService();
            AlumnoResponse response = webService.getAlumnosAsignatura(idAsignaturaSeleccionada);

            if (response.estado == 1)
            {
                // RELLENAMOS EL ADAPTADOR CON LOS ALUMNOS, BINDEAMOS EN LA VISTA EL DATAGRID ALUMNOS CON ESTE ADAPTERLIST
                var viewModel = new AlumnosVM();
                foreach (Alumno alumno in response.alumnos)
                {
                    viewModel.AlumnosAdapterList.Add(alumno);
                }
                dgAlumnosMainClases.ItemsSource = viewModel.AlumnosAdapterList;
                // ESCONDEMOS LAS DOS PRIMERAS COLUMNAS, LAS DE ID E ID PROFESOR
                dgAlumnosMainClases.Columns[0].Visibility = Visibility.Collapsed;
                dgAlumnosMainClases.Columns[1].Visibility = Visibility.Collapsed;

            }
            else if (response.estado == -1)
            {
                // SI NO HAY ALUMNOS EN LA ASIGNATURA REINSTANCIAMOS EL viewModel PARA VACIAR LA LISTA
                var viewModel = new AlumnosVM();
                dgAlumnosMainClases.ItemsSource = viewModel.AlumnosAdapterList;
            }
        }

        private void matricularAlumnos(object sender, RoutedEventArgs e)
        {
            if (!ventanaAbierta())
            {
                // RECOGEMOS LA ASIGNATURA SELECCIONADA DEL COMBOBOX
                int numAsignaturaSeleccionada = cbAsignaturasClases.SelectedIndex;
                if (numAsignaturaSeleccionada != -1)
                {
                    // CON EL INDEX ACCEDEMOS A LA LISTA DE OBJETOS ASIGNATURA Y COGEMOS LA ASIGNATURA
                    Asignatura asignaturaSeleccionada = listaAsignaturas.ElementAt(numAsignaturaSeleccionada);
                    // ABRIMOS LA VENTANA MATRICULAR ALUMNO PASÁNDOLE EL OBJETO DE LA ASIGNATURA TANTO PARA
                    // MOSTRAR EL NOMBRE DE LA MISMA COMO PARA TENER EL ID PARA MATRICULAR AL ALUMNO
                    AlumnosMatricularWindow alumnosMatricular = new AlumnosMatricularWindow(asignaturaSeleccionada);
                    alumnosMatricular.Owner = this;
                    alumnosMatricular.Show();
                }
                else
                {
                    Utils.msgBox("No ha seleccionado ninguna asignatura, use las listas superiores para ello", "ok", "warning");
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////      APUNTES CLASES

        private void camposGridApuntes(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "nombre":
                    e.Column.Header = "Nombre";
                    break;
                case "tipo":
                    e.Column.Header = "Tipo";
                    break;
                case "enlace":
                    e.Column.Header = "Enlace";
                    break;
                case "descripcion":
                    e.Column.Header = "Descripción";
                    break;
                default:
                    break;
            }
        }


        public void cargarApuntesClases()
        {
            // RECOGEMOS EL INDEX SELECCIONADO DEL COMBOBOX
            int numAsignaturaSeleccionada = cbAsignaturasClases.SelectedIndex;
            // CON EL INDEX ACCEDEMOS A LA LISTA DE OBJETOS CURSO Y COGEMOS SU NOMBRE
            string idAsignaturaSeleccionada = listaAsignaturas.ElementAt(numAsignaturaSeleccionada).id;

            // HACEMOS LA CONSULTA
            WebService webService = new WebService();
            ApunteResponse response = webService.getApuntesAsignatura(idAsignaturaSeleccionada);

            if (response.estado == 1)
            {
                var viewModel = new ApuntesVM();
                foreach (Apunte apunte in response.apuntes)
                {
                    viewModel.ApuntesAdapterList.Add(apunte);
                }
                dgApuntesMainClases.ItemsSource = viewModel.ApuntesAdapterList;
                dgApuntesMainClases.Columns[0].Visibility = Visibility.Collapsed;
                dgApuntesMainClases.Columns[1].Visibility = Visibility.Collapsed;
            }
            else if (response.estado == -1)
            {
                var viewModel = new ApuntesVM();
                dgApuntesMainClases.ItemsSource = viewModel.ApuntesAdapterList;
                dgApuntesMainClases.Columns[0].Visibility = Visibility.Collapsed;
                dgApuntesMainClases.Columns[1].Visibility = Visibility.Collapsed;
            }
        }

        private void apuntesDGClick(object sender, MouseButtonEventArgs e)
        {
            if (!ventanaAbierta())
            {
                if (dgApuntesMainClases.SelectedItem != null)
                {
                    Apunte apunte = dgApuntesMainClases.SelectedItem as Apunte;
                    int numAsignaturaSeleccionada = cbAsignaturasClases.SelectedIndex;
                    Asignatura asignaturaSeleccionada = listaAsignaturas.ElementAt(numAsignaturaSeleccionada);
                    ApuntesWindow apuntesFicha = new ApuntesWindow(asignaturaSeleccionada, apunte.id);
                    apuntesFicha.Show();
                    apuntesFicha.Owner = this;
                }
            }
        }

        private void añadirApuntes(object sender, RoutedEventArgs e)
        {
            if (!ventanaAbierta())
            {
                // RECOGEMOS LA ASIGNATURA SELECCIONADA DEL COMBOBOX
                int numAsignaturaSeleccionada = cbAsignaturasClases.SelectedIndex;
                if (numAsignaturaSeleccionada != -1)
                {
                    Asignatura asignaturaSeleccionada = listaAsignaturas.ElementAt(numAsignaturaSeleccionada);
                    ApuntesAltaWindow subirApuntes = new ApuntesAltaWindow(asignaturaSeleccionada);
                    subirApuntes.Owner = this;
                    subirApuntes.Show();
                }
                else
                {
                    Utils.msgBox("No ha seleccionado ninguna asignatura, use las listas superiores para ello", "ok", "warning");
                }
            }


        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////      TAREAS CLASES
        private void camposGridTareas(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "nombre":
                    e.Column.Header = "Nombre";
                    break;
                case "fecha":
                    e.Column.Header = "Fecha";
                    break;
                case "valoracion":
                    e.Column.Header = "Valoración";
                    break;
                case "tipo":
                    e.Column.Header = "Tipo";
                    break;
                default:
                    break;
            }
        }

        private void tareasDGClick(object sender, MouseButtonEventArgs e)
        {
            if (!ventanaAbierta())
            {
                if (dgTareasMainClases.SelectedItem != null)
                {
                    int numAsignaturaSeleccionada = cbAsignaturasClases.SelectedIndex;
                    Asignatura asignaturaSeleccionada = listaAsignaturas.ElementAt(numAsignaturaSeleccionada);
                    Tarea tarea = dgTareasMainClases.SelectedItem as Tarea;
                    TareasWindow tareaFicha = new TareasWindow(asignaturaSeleccionada, tarea.id);
                    tareaFicha.Show();
                    tareaFicha.Owner = this;
                }
            }
        }

        public void cargarTareasClases()
        {
            // RECOGEMOS EL INDEX SELECCIONADO DEL COMBOBOX
            int numAsignaturaSeleccionada = cbAsignaturasClases.SelectedIndex;
            // CON EL INDEX ACCEDEMOS A LA LISTA DE OBJETOS CURSO Y COGEMOS SU NOMBRE
            string idAsignaturaSeleccionada = listaAsignaturas.ElementAt(numAsignaturaSeleccionada).id;

            // HACEMOS LA CONSULTA
            WebService webService = new WebService();
            TareaResponse response = webService.getTareasAsignatura(idAsignaturaSeleccionada);

            if (response.estado == 1)
            {
                var viewModelTareas = new TareasVM();
                foreach (Tarea tarea in response.tareas)
                {
                    viewModelTareas.TareasAdapterList.Add(tarea);
                }
                dgTareasMainClases.ItemsSource = viewModelTareas.TareasAdapterList;
                dgTareasMainClases.Columns[0].Visibility = Visibility.Collapsed;
                dgTareasMainClases.Columns[1].Visibility = Visibility.Collapsed;
            }
            else if (response.estado == -1)
            {
                var viewModelTareas = new TareasVM();
                dgTareasMainClases.ItemsSource = viewModelTareas.TareasAdapterList;
                dgTareasMainClases.Columns[0].Visibility = Visibility.Collapsed;
                dgTareasMainClases.Columns[1].Visibility = Visibility.Collapsed;
            }
        }

        private void añadirTareas(object sender, RoutedEventArgs e)
        {
            if (!ventanaAbierta())
            {
                // RECOGEMOS LA ASIGNATURA SELECCIONADA DEL COMBOBOX
                int numAsignaturaSeleccionada = cbAsignaturasClases.SelectedIndex;
                if (numAsignaturaSeleccionada != -1)
                {
                    Asignatura asignaturaSeleccionada = listaAsignaturas.ElementAt(numAsignaturaSeleccionada);
                    TareasAltaWindow programarTareas = new TareasAltaWindow(asignaturaSeleccionada);
                    programarTareas.Owner = this;
                    programarTareas.Show();
                }
                else
                {
                    Utils.msgBox("No ha seleccionado ninguna asignatura, use las listas superiores para ello", "ok", "warning");
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////       MÉTODOS HORARIO      /////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////

        private void camposGridHorario(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "asignaturaLunes":
                    e.Column.Header = "Lunes";
                    break;
                case "asignaturaMartes":
                    e.Column.Header = "Martes";
                    break;
                case "asignaturaMiercoles":
                    e.Column.Header = "Miércoles";
                    break;
                case "asignaturaJueves":
                    e.Column.Header = "Jueves";
                    break;
                case "asignaturaViernes":
                    e.Column.Header = "Viernes";
                    break;
                default:
                    break;
            }
        }

        private void cargarHorario()
        {
            // DECLARAMOS EL VIEWMODEL QUE LE VAMOS A ASIGNAR AL DATAGRID
            HorarioVM viewModel = new HorarioVM();

            // BUCLE QUE RECORRE TODAS LAS HORAS DE CLASE
            for (int i = 9; i < 22; i++)
            {
                // PARA SALTAR LAS 15 HORAS, QUE NO SE IMPARTE CLASE
                if (i != 15)
                {
                    // EN CADA HORA VAMOS A RECOGER SI HAY CLASE O NO EN CADA DÍA DE LA SEMANA
                    // OBTENIENDO UN OBJETO DE NOMBRE DE ASIGNATURA EN CADA DIA DE LA SEMANA EN TAL HORA

                    // DEFINIMOS UN STRING HORA A BUSCAR
                    string horaABuscar = "";
                    // PARA EL CASO DE LAS 9 HAY QUE VALORARLO INDIVIDUALMENTE YA QUE ENTRARÍA EN CONFLICTO CON LAS 19:00
                    if (i == 9)
                    {
                        horaABuscar = "09:00";
                    }
                    else
                    {
                        horaABuscar = i + ":00";
                    }

                    // DECLARAMOS LAS 5 STRING ASIGNATURA Y EL VIEWMODEL
                    string asignaturaLunes = "", asignaturaMartes = "", asignaturaMiercoles = "", asignaturaJueves = "", asignaturaViernes = "";

                    // A CONTINUACIÓN HACEMOS LAS 5 CONSULTA
                    // LE PASAMOS EL ID DE PROFESOR PARA BUSCAR SUS ASIGNATURAS, LA HORA A BUSCAR Y EL DÍA QUE ESTAMOS BUSCANDO EN FORMA DE INTEGER
                    for (int j = 1; j <= 5; j++)
                    {
                        WebService webService = new WebService();
                        EstadoMensajeResponse response = webService.getHorario(MainWindow.user.id, horaABuscar, j + "");
                        if (response.estado == 1)
                        {
                            switch (j)
                            {
                                case 1:
                                    asignaturaLunes = response.mensaje;
                                    break;
                                case 2:
                                    asignaturaMartes = response.mensaje;
                                    break;
                                case 3:
                                    asignaturaMiercoles = response.mensaje;
                                    break;
                                case 4:
                                    asignaturaJueves = response.mensaje;
                                    break;
                                case 5:
                                    asignaturaViernes = response.mensaje;
                                    break;
                            }
                        }
                    }
                        // CON EL RESULTADO DE CADA UNA GENERAMOS EL OBJETO HORARIO Y LO AÑADIMOS A LA LISTA DEL HORARIOVM
                        Horario horario = new Horario(horaABuscar, asignaturaLunes, asignaturaMartes, asignaturaMiercoles, asignaturaJueves, asignaturaViernes);
                        viewModel.HorarioAdapterList.Add(horario);
                }
            }
            // FINALMENTE ASIGNAMOS EL VIEWMODEL AL DATASOURCE Y TENEMOS EL HORARIO
            dgHorario.ItemsSource = viewModel.HorarioAdapterList;
        }


        ////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////     OTROS MÉTODOS          /////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult messageBoxResult = Utils.msgBox("¿Desea salir del programa?", "yesno", "question");
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
            else
            {
                e.Cancel = true;
            }
        }


        // MÉTODO QUE VALORA SI UN OBJETO WINDOW ESTÁ ABIERTO
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

        // MÉTODO QUE COMPRUEBA QUE NO HAY OTRA VENTANA AUXILIAR ABIERTA Y PERMITE LA APERTURA DE LA SELECCIONADA POR EL USUARIO
        private bool ventanaAbierta()
        {
            List<String> listaVentanas = new List<string>(new string[] { "AlumnosAltaWindow1", "AlumnosMatricularWindow1", "AlumnosPerfilWindow1", "TareasAltaWindow1", "TareasWindow1", "CursosAltaWindow1", "CursosWindow1", "AsignaturasAltaWindow1", "AsignaturasWindow1", "ApuntesAltaWindow1", "ApuntesWindow1" });
            bool ventanaAbierta = false;
            listaVentanas.ForEach(delegate (String ventana)
            {
                if (IsWindowOpen<Window>(ventana))
                {
                    ventanaAbierta = true;
                }
            });
            if (ventanaAbierta)
            {
                Utils.msgBox("Ya tiene abierta otra ventana auxiliar, ciérrela antes de abrir otra.", "ok", "warning");
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
