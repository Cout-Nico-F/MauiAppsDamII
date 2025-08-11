using Foundation;

namespace MauiFirstApp
{
    // Clase AppDelegate: Delegado de la aplicación para MacCatalyst
    // Relación con el ciclo de vida de la app:
    // - [Register]: Atributo que registra la clase como delegado de la aplicación
    // - MauiUIApplicationDelegate: Clase base que integra MAUI con el ciclo de vida de la app en MacCatalyst
    // - CreateMauiApp: Método que inicializa y retorna la instancia principal de la app MAUI
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        // Método que crea y retorna la instancia principal de la app MAUI
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}

/*
Explicación didáctica de los componentes principales:

- [Register("AppDelegate")]
  Atributo que indica que esta clase será el delegado principal de la aplicación en MacCatalyst.

- public class AppDelegate : MauiUIApplicationDelegate
  Hereda de MauiUIApplicationDelegate para integrar el ciclo de vida de MAUI con la plataforma MacCatalyst.

- protected override MauiApp CreateMauiApp()
  Método que inicializa y retorna la instancia principal de la app MAUI, llamando a la configuración definida en MauiProgram.cs.

En resumen, este archivo conecta el ciclo de vida de la app en MacCatalyst con la inicialización y configuración de MAUI.
*/
