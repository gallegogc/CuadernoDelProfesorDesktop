using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CUADERNODELPROFESOR.utils
{
    class Utils
    {
        // LA HACEMOS ESTÁTICA PARA NO NECESITAR OBJETO Y LA HACEMOS DE TIPO MessageBoxResult
        // PARA QUE NOS VALGA TANTO PARA LAS CONFIRMACIONES LA USUARIO COMO PARA LOS MENSAJES
        public static MessageBoxResult msgBox(String mensaje, String boton, String icono)
        {

            // DECLARAMOS DOS VARIABLES TIPO BOTÓN Y TIPO ICONO, LAS INICIALIZAMOS A CUALQUIER VALOR
            System.Windows.MessageBoxButton tipoBoton = System.Windows.MessageBoxButton.OK;
            MessageBoxImage tipoIcono = MessageBoxImage.Error;

            // EN FUNCIÓN DE LOS PARÁMETROS CAMBIAMOS El TIPO DE BOTÓN E ICONO
            switch (boton)
            {
                case "ok":
                    tipoBoton = System.Windows.MessageBoxButton.OK;
                    break;
                case "yesno":
                    tipoBoton = System.Windows.MessageBoxButton.YesNo;
                    break;
            }

            switch (icono)
            {
                case "info":
                    tipoIcono = MessageBoxImage.Information;
                    break;
                case "warning":
                    tipoIcono = MessageBoxImage.Warning;
                    break;
                case "error":
                    tipoIcono = MessageBoxImage.Error;
                    break;
                case "question":
                    tipoIcono = MessageBoxImage.Question;
                    break;
            }

            // MOSTRAMOS EL MENSAJE
            return System.Windows.MessageBox.Show(mensaje, "Cuaderno del Profesor", tipoBoton, tipoIcono);
        }

        public static String initCap(String cadena, String comando)
        {
            // SI QUEREMOS QUE TODAS SEAN MAYÚSCULAS SALVO CIERTOS PRONOMBRES DE APELLIDOS O ASIGNATURAS            if (comando == "mayus")
            if (comando == "mayus")
            {
                String[] elementos = cadena.Split(' ');
                cadena = "";
                for (int i = 0; i < elementos.Length; i++)
                {
                    // SI EL ELEMENTO ES DISTINTO DE LOS PRONOMBRES INDICADOS, HACEMOS INITCAP
                    if (elementos[i] != "de" && elementos[i] != "del" && elementos[i] != "a")
                    {
                        elementos[i] = elementos[i].ToLower();
                        elementos[i] = elementos[i].Substring(0, 1).ToUpper() + elementos[i].Substring(1);
                    }
                    // AÑADIMOS LA PARTE DE LA CADENA A LA CADENA, SI ES LA ÚLTIMA NO AÑADIMOS UN ESPACIO AL FINAL
                    if (i < elementos.Length - 1)
                    {
                        cadena += elementos[i] + " ";
                    }
                    else
                    {
                        cadena += elementos[i];
                    }
                }
            }
            // SINO,SÓLO HACEMOS MAYÚSCULA LA PRIMERA
            else
            {
                cadena = cadena.ToLower();
                cadena = cadena.Substring(0, 1).ToUpper() + cadena.Substring(1);
            }
            return cadena;
        }

    }
}

