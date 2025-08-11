using Microsoft.Extensions.Logging;

namespace MauiFirstApp
{
    // Archivo MauiProgram.cs: Configura y construye la aplicación MAUI
    // Componentes principales:
    // - using: Permite importar espacios de nombres necesarios para funcionalidades específicas.
    // - namespace: Agrupa las clases relacionadas bajo un mismo nombre lógico.
    // - static class: Clase que no puede ser instanciada, útil para métodos de configuración global.
    // - public static MauiApp CreateMauiApp(): Método principal que configura y retorna la app MAUI.
    // - MauiApp.CreateBuilder(): Inicializa el builder para configurar servicios, fuentes y la app.
    // - UseMauiApp<App>(): Indica la clase principal de la app definida en App.xaml/App.xaml.cs.
    // - ConfigureFonts: Permite agregar fuentes personalizadas para toda la app.
    // - builder.Logging.AddDebug(): Habilita el logging en modo depuración para facilitar el desarrollo.
    // - builder.Build(): Construye y retorna la aplicación lista para ejecutarse.
    public static class MauiProgram
    {
        // Método principal que crea y configura la app MAUI
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder(); // Crea el builder para configurar la app
            builder
                .UseMauiApp<App>() // Indica que la clase principal de la app es App.xaml/App.xaml.cs
                .ConfigureFonts(fonts => // Configura las fuentes personalizadas
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); // Fuente regular
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold"); // Fuente semibold
                });

#if DEBUG
            builder.Logging.AddDebug(); // Habilita el logging en modo depuración
#endif

            return builder.Build(); // Construye y retorna la app configurada
        }
    }
}

/*
Explicación detallada de cada elemento:

1. using Microsoft.Extensions.Logging;
   Importa el espacio de nombres para el sistema de logging (registro de eventos y errores).

2. public static class MauiProgram
   Clase estática que contiene la configuración principal de la app MAUI.

3. public static MauiApp CreateMauiApp()
   Método que crea y configura la instancia principal de la aplicación.

4. var builder = MauiApp.CreateBuilder();
   Inicializa el builder, que se usa para configurar servicios, fuentes, y la app en general.

5. builder.UseMauiApp<App>()
   Especifica que la clase principal de la app es App (definida en App.xaml/App.xaml.cs).

6. builder.ConfigureFonts(...)
   Permite agregar fuentes personalizadas que se usarán en la app.

7. #if DEBUG ... builder.Logging.AddDebug();
   En modo depuración, habilita el registro de eventos para facilitar el desarrollo y la solución de problemas.

8. return builder.Build();
   Construye y retorna la aplicación lista para ejecutarse.

En resumen, MauiProgram.cs es el punto de entrada donde se configura todo lo necesario para que la app MAUI funcione correctamente.

-------------------------------------------------------------

Explicación de los posibles errores de compilación:

1.error MAUIG1001: "Name cannot begin with the '<' character, hexadecimal value 0x3C. Line 5, position 59."
   Este error ocurre porque en el archivo AppShell.xaml se han colocado comentarios XML dentro de las propiedades de los elementos, por ejemplo:
   xmlns = "http://schemas.microsoft.com/dotnet/2021/maui" < !--Espacio de nombres principal de MAUI -->
   En XAML, los comentarios no pueden estar dentro de las etiquetas o propiedades, solo pueden estar entre elementos. Esto hace que el parser de XAML falle.

   Solución: Mueve los comentarios fuera de las líneas de propiedades y colócalos entre los elementos, nunca dentro de una etiqueta o propiedad.

2. error CS0103: "The name 'InitializeComponent' does not exist in the current context"
   Este error ocurre porque el archivo AppShell.xaml no se está compilando correctamente debido al error anterior, por lo que el archivo generado para InitializeComponent no existe. Esto hace que el constructor de AppShell.xaml.cs no pueda encontrar el método.

   Solución: Corrige el error de XAML para que el archivo se compile correctamente y se genere el método InitializeComponent.

En resumen, los errores se deben a la ubicación incorrecta de los comentarios en el XAML. Corrigiendo esto, la compilación debería funcionar correctamente.
*/
