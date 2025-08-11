namespace MauiFirstApp
{
    // Clase parcial AppShell: Define la estructura de navegación principal de la app MAUI
    // Relación con XAML:
    // - partial: Permite que la definición de la clase esté dividida entre el archivo AppShell.xaml y AppShell.xaml.cs
    // - x:Class en XAML enlaza este archivo con la estructura visual de navegación
    public partial class AppShell : Shell
    {
        // Constructor de AppShell
        // Inicializa los componentes definidos en AppShell.xaml
        public AppShell()
        {
            InitializeComponent(); // Método generado automáticamente que enlaza los controles XAML con el código C#
        }
    }
}

/*
Explicación didáctica de los componentes principales:

- public partial class AppShell : Shell
  Define la clase de navegación principal y la hereda de Shell, que permite estructurar la navegación y las rutas de la app.
  El modificador partial permite que la clase esté dividida entre el archivo XAML y el archivo C#.

- public AppShell()
  Constructor de la clase. Llama a InitializeComponent para cargar y enlazar la estructura de navegación definida en XAML.

En resumen, este archivo contiene la lógica de inicialización de la estructura de navegación principal de la app MAUI.
*/
