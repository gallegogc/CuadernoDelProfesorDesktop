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
using CUADERNODELPROFESOR.database.models;
using CUADERNODELPROFESOR.utils;

namespace CUADERNODELPROFESOR
{

    public partial class LoginWindow : Window
    {

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void loginClick(object sender, RoutedEventArgs e)
        {
            if (txtUserLogin.Text.Length == 0 || txtPassLogin.Password.Length == 0)
            {
                Utils.msgBox("Al menos un campo está vacío", "ok", "warning");
            }
            else
            {
                string user = txtUserLogin.Text;
                string pass = txtPassLogin.Password;

                WebService webService = new WebService();
                LoginProfesorResponse response = webService.loginProfesor(user, pass);

                if (response.estado == 1)
                {
                    MainWindow main = new MainWindow(response.user);
                    main.Show();
                    this.Hide();
                    Utils.msgBox("Bienvenida/o "+response.user.nombre, "ok", "info");
                }
                else if(response.estado == 3)
                {
                    Utils.msgBox("El usuario introducido se dio de baja con anterioridad", "ok", "error");
                } 
                else if (response.estado == 2)
                {
                    Utils.msgBox("Usuario o contraseña incorrectos","ok", "warning");
                }
            }
        }
        

        private void registroClick(object sender, RoutedEventArgs e)
        {
            RegistroWindow registro = new RegistroWindow();
            registro.Show();
            this.Hide();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.Visibility == System.Windows.Visibility.Visible)
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
            }

    }
}
