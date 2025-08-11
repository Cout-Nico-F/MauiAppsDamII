namespace MauiFirstApp
{
    // Clase parcial App: Representa la aplicación MAUI y su ciclo de vida
    // Relación con XAML:
    // - partial: Permite que la definición de la clase esté dividida entre el archivo App.xaml y App.xaml.cs
    // - x:Class en XAML enlaza este archivo con la configuración visual y recursos globales
    public partial class App : Application
    {
        // Constructor de la aplicación
        // Inicializa los componentes definidos en App.xaml
        public App()
        {
            InitializeComponent(); // Método generado automáticamente que enlaza los recursos XAML con el código C#
        }

        // Método que crea la ventana principal de la aplicación
        // Se puede personalizar para definir la página inicial o el flujo de navegación
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell()); // Crea una nueva ventana y establece AppShell como contenedor de navegación principal
        }
    }
}

/*
Explicación didáctica de los componentes principales:

- public partial class App : Application
  Define la clase principal de la aplicación y la hereda de Application, que gestiona el ciclo de vida y recursos globales.
  El modificador partial permite que la clase esté dividida entre el archivo XAML y el archivo C#.

- public App()
  Constructor de la aplicación. Llama a InitializeComponent para cargar y enlazar los recursos definidos en XAML.

- protected override Window CreateWindow(IActivationState? activationState)
  Método que permite personalizar la creación de la ventana principal. Por defecto, establece AppShell como la raíz de navegación.

En resumen, este archivo contiene la lógica de inicialización y el ciclo de vida de la aplicación MAUI, conectando la configuración visual y los recursos globales definidos en XAML con la funcionalidad en C#.
*/