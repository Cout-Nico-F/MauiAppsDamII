namespace MauiFirstApp
{
    // Clase parcial MainPage: Representa la lógica de la página principal definida en MainPage.xaml
    // Relación con XAML:
    // - partial: Permite que la definición de la clase esté dividida entre el archivo XAML y el archivo C#.
    // - x:Class en XAML enlaza este archivo con la interfaz visual.
    public partial class MainPage : ContentPage
    {
        int count = 0; // Variable para contar los clics en el botón

        // Constructor de la página principal
        // Inicializa los componentes definidos en XAML
        public MainPage()
        {
            InitializeComponent(); // Método generado automáticamente que enlaza los controles XAML con el código C#
        }

        // Evento que se ejecuta al hacer clic en el botón
        // Actualiza el texto del botón y anuncia el cambio para accesibilidad
        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text); // Anuncia el texto actualizado para usuarios con necesidades de accesibilidad
        }
    }
}

/*
Explicación didáctica de los componentes principales:

- public partial class MainPage : ContentPage
  Define la clase de la página principal y la hereda de ContentPage, que es el contenedor base para páginas en MAUI.
  El modificador partial permite que la clase esté dividida entre el archivo XAML y el archivo C#.

- int count = 0;
  Variable que almacena el número de veces que se ha hecho clic en el botón.

- public MainPage()
  Constructor de la página. Llama a InitializeComponent para cargar y enlazar los controles definidos en XAML.

- private void OnCounterClicked(object? sender, EventArgs e)
  Método que maneja el evento Clicked del botón. Actualiza el texto del botón y utiliza SemanticScreenReader para accesibilidad.

- CounterBtn
  Referencia al botón definida en XAML mediante x:Name. Permite modificar sus propiedades desde el código.

En resumen, este archivo contiene la lógica y el comportamiento de la página principal, conectando la interfaz visual definida en XAML con la funcionalidad en C#.
*/
