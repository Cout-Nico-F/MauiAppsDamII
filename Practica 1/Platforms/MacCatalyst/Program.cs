using ObjCRuntime;
using UIKit;

namespace MauiFirstApp
{
    // Clase Program: Punto de entrada de la aplicación en MacCatalyst
    // Relación con el ciclo de vida de la app:
    // - static void Main: Método principal que inicia la aplicación en la plataforma MacCatalyst
    // - UIApplication.Main: Llama al ciclo de vida de la app y utiliza AppDelegate para la inicialización
    public class Program
    {
        // Este es el punto de entrada principal de la aplicación.
        static void Main(string[] args)
        {
            // Si quieres usar una clase Application Delegate diferente a "AppDelegate"
            // puedes especificarla aquí.
            UIApplication.Main(args, null, typeof(AppDelegate)); // Inicia la app usando AppDelegate como delegado principal
        }
    }
}

/*
Explicación didáctica de los componentes principales:

- static void Main(string[] args)
  Método principal que inicia la ejecución de la aplicación en MacCatalyst.

- UIApplication.Main(args, null, typeof(AppDelegate))
  Llama al ciclo de vida de la aplicación y utiliza la clase AppDelegate para la inicialización y configuración.

En resumen, este archivo define el punto de entrada y el arranque de la app MAUI en la plataforma MacCatalyst.
*/
