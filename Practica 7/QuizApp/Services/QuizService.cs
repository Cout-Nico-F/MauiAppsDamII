using QuizApp.Models;

namespace QuizApp.Services
{
    public interface IQuizService
    {
        Task<List<CategoriaQuiz>> ObtenerCategoriasAsync();
        Task<List<Pregunta>> ObtenerPreguntasPorCategoriaAsync(string categoria);
        Task<List<Pregunta>> ObtenerPreguntasAleatoriasAsync(int cantidad = 10);
        Task GuardarResultadoAsync(ResultadoQuiz resultado);
        Task<List<ResultadoQuiz>> ObtenerHistorialAsync();
    }

    public class QuizService : IQuizService
    {
        private static readonly List<CategoriaQuiz> _categorias = new()
        {
            new() { Nombre = "Navegacion", Descripcion = "Shell Navigation, rutas y parametros", Icono = "NAV", Color = "#007ACC", TotalPreguntas = 8 },
            new() { Nombre = "MVVM", Descripcion = "Patron MVVM, ViewModels y Commands", Icono = "MVVM", Color = "#28A745", TotalPreguntas = 8 },
            new() { Nombre = "Bindings", Descripcion = "Data Binding, Converters y Triggers", Icono = "BIND", Color = "#DC3545", TotalPreguntas = 7 },
            new() { Nombre = "Controles", Descripcion = "Controles MAUI, Layouts y Propiedades", Icono = "CTRL", Color = "#17A2B8", TotalPreguntas = 7 },
            new() { Nombre = "Datos", Descripcion = "Servicios, APIs y Persistencia", Icono = "DATA", Color = "#FFC107", TotalPreguntas = 6 }
        };

        private static readonly List<Pregunta> _preguntas = new()
        {
            // Navegación MAUI (8 preguntas)
            new() { Id = 1, Texto = "¿Qué es Shell en .NET MAUI?", Opciones = new() { "Un control de contenedor", "Un sistema de navegación", "Un tipo de layout", "Una librería externa" }, RespuestaCorrecta = 1, Categoria = "Navegacion", Dificultad = "Facil", Explicacion = "Shell proporciona navegación, routing y estructura visual de la aplicación" },
            new() { Id = 2, Texto = "¿Qué método se usa para navegar programáticamente en Shell?", Opciones = new() { "Navigate()", "GoToAsync()", "PushAsync()", "MoveToPage()" }, RespuestaCorrecta = 1, Categoria = "Navegacion", Dificultad = "Medio" },
            new() { Id = 3, Texto = "¿Cómo se registra una ruta personalizada en MAUI?", Opciones = new() { "Routing.Register()", "Routing.RegisterRoute()", "Shell.AddRoute()", "Navigation.Register()" }, RespuestaCorrecta = 1, Categoria = "Navegacion", Dificultad = "Medio" },
            new() { Id = 4, Texto = "¿Qué propiedad se usa para definir la ruta de una página en Shell?", Opciones = new() { "Route", "Path", "Navigation", "Uri" }, RespuestaCorrecta = 0, Categoria = "Navegacion", Dificultad = "Facil" },
            new() { Id = 5, Texto = "¿Cuál es la sintaxis para navegar hacia atrás en Shell?", Opciones = new() { "Shell.Current.GoBack()", "Shell.Current.GoToAsync('..')", "Shell.Current.PopAsync()", "Shell.Current.NavigateBack()" }, RespuestaCorrecta = 1, Categoria = "Navegacion", Dificultad = "Medio" },
            new() { Id = 6, Texto = "¿Qué representa el '//' en una ruta de navegación Shell?", Opciones = new() { "Navegación relativa", "Navegación absoluta", "Navegación modal", "Navegación anidada" }, RespuestaCorrecta = 1, Categoria = "Navegacion", Dificultad = "Dificil", Explicacion = "'//' indica navegación absoluta, limpiando toda la pila de navegación" },
            new() { Id = 7, Texto = "¿Cómo se pasan parámetros en la navegación Shell?", Opciones = new() { "Como query strings", "En un Dictionary", "Ambas opciones son válidas", "Solo como propiedades" }, RespuestaCorrecta = 2, Categoria = "Navegacion", Dificultad = "Medio" },
            new() { Id = 8, Texto = "¿Qué atributo se usa para recibir parámetros de navegación?", Opciones = new() { "[Parameter]", "[QueryProperty]", "[NavigationParameter]", "[RouteData]" }, RespuestaCorrecta = 1, Categoria = "Navegacion", Dificultad = "Medio" },

            // MVVM (8 preguntas)
            new() { Id = 9, Texto = "¿Qué significa MVVM?", Opciones = new() { "Model View ViewModel", "Model Visual View Manager", "Master View Virtual Model", "Multiple View Version Manager" }, RespuestaCorrecta = 0, Categoria = "MVVM", Dificultad = "Facil", Explicacion = "MVVM separa la lógica de negocio (Model), la presentación (View) y la lógica de presentación (ViewModel)" },
            new() { Id = 10, Texto = "¿Cuál es la clase base recomendada para ViewModels en CommunityToolkit.Mvvm?", Opciones = new() { "BaseViewModel", "ObservableObject", "ViewModelBase", "INotifyPropertyChanged" }, RespuestaCorrecta = 1, Categoria = "MVVM", Dificultad = "Medio" },
            new() { Id = 11, Texto = "¿Qué atributo genera automáticamente propiedades observables?", Opciones = new() { "[Observable]", "[ObservableProperty]", "[Notify]", "[Property]" }, RespuestaCorrecta = 1, Categoria = "MVVM", Dificultad = "Medio" },
            new() { Id = 12, Texto = "¿Qué atributo genera automáticamente comandos ICommand?", Opciones = new() { "[Command]", "[RelayCommand]", "[ICommand]", "[AsyncCommand]" }, RespuestaCorrecta = 1, Categoria = "MVVM", Dificultad = "Medio" },
            new() { Id = 13, Texto = "¿Cuál es la ventaja principal del patrón MVVM?", Opciones = new() { "Mejor rendimiento", "Separación de responsabilidades", "Menos código", "Navegación automática" }, RespuestaCorrecta = 1, Categoria = "MVVM", Dificultad = "Facil" },
            new() { Id = 14, Texto = "¿Qué interfaz implementa ObservableObject?", Opciones = new() { "IObservable", "INotifyPropertyChanged", "ICommand", "IViewModel" }, RespuestaCorrecta = 1, Categoria = "MVVM", Dificultad = "Medio" },
            new() { Id = 15, Texto = "¿Cómo se puede pasar parámetros a un RelayCommand?", Opciones = new() { "Solo como CommandParameter", "Solo en el constructor", "CommandParameter o genérico T", "No se puede" }, RespuestaCorrecta = 2, Categoria = "MVVM", Dificultad = "Dificil" },
            new() { Id = 16, Texto = "¿Qué método se usa para notificar cambios de propiedades manualmente?", Opciones = new() { "NotifyChanged()", "OnPropertyChanged()", "RaisePropertyChanged()", "UpdateProperty()" }, RespuestaCorrecta = 1, Categoria = "MVVM", Dificultad = "Medio" },

            // Bindings (7 preguntas)
            new() { Id = 17, Texto = "¿Qué propiedad habilita los compiled bindings en XAML?", Opciones = new() { "x:Compile", "x:DataType", "x:Bind", "x:CompileBindings" }, RespuestaCorrecta = 1, Categoria = "Bindings", Dificultad = "Medio", Explicacion = "x:DataType especifica el tipo de datos y habilita compiled bindings para mejor rendimiento" },
            new() { Id = 18, Texto = "¿Cuál es el modo de binding por defecto?", Opciones = new() { "OneTime", "OneWay", "TwoWay", "OneWayToSource" }, RespuestaCorrecta = 1, Categoria = "Bindings", Dificultad = "Facil" },
            new() { Id = 19, Texto = "¿Qué interfaz implementan los Value Converters?", Opciones = new() { "IConverter", "IValueConverter", "IBindingConverter", "ITypeConverter" }, RespuestaCorrecta = 1, Categoria = "Bindings", Dificultad = "Medio" },
            new() { Id = 20, Texto = "¿Cuándo se usa TwoWay binding típicamente?", Opciones = new() { "Para Labels", "Para controles de entrada", "Para imágenes", "Para botones" }, RespuestaCorrecta = 1, Categoria = "Bindings", Dificultad = "Facil" },
            new() { Id = 21, Texto = "¿Qué permite hacer StringFormat en un binding?", Opciones = new() { "Validar datos", "Formatear la salida", "Convertir tipos", "Cambiar colores" }, RespuestaCorrecta = 1, Categoria = "Bindings", Dificultad = "Medio" },
            new() { Id = 22, Texto = "¿Cómo se especifica un binding relativo al ancestro?", Opciones = new() { "RelativeSource=Parent", "RelativeSource={RelativeSource AncestorType=...}", "Source=Parent", "Binding=Ancestor" }, RespuestaCorrecta = 1, Categoria = "Bindings", Dificultad = "Dificil" },
            new() { Id = 23, Texto = "¿Qué trigger se activa cuando una propiedad cambia de valor?", Opciones = new() { "EventTrigger", "DataTrigger", "PropertyTrigger", "Trigger" }, RespuestaCorrecta = 3, Categoria = "Bindings", Dificultad = "Medio" },

            // Controles MAUI (7 preguntas)
            new() { Id = 24, Texto = "¿Cuál es el layout más eficiente para posicionamiento absoluto?", Opciones = new() { "StackLayout", "Grid", "AbsoluteLayout", "FlexLayout" }, RespuestaCorrecta = 2, Categoria = "Controles", Dificultad = "Medio" },
            new() { Id = 25, Texto = "¿Qué layout es mejor para elementos que se ajustan automáticamente?", Opciones = new() { "Grid", "StackLayout", "FlexLayout", "AbsoluteLayout" }, RespuestaCorrecta = 2, Categoria = "Controles", Dificultad = "Medio", Explicacion = "FlexLayout proporciona flexibilidad similar a CSS Flexbox" },
            new() { Id = 26, Texto = "¿Qué control se usa para mostrar listas virtualizadas?", Opciones = new() { "ListView", "CollectionView", "StackLayout", "Repeater" }, RespuestaCorrecta = 1, Categoria = "Controles", Dificultad = "Facil" },
            new() { Id = 27, Texto = "¿Cuál es la diferencia principal entre Frame y Border?", Opciones = new() { "No hay diferencia", "Frame está obsoleto en .NET 9", "Border es más ligero", "Ambas B y C" }, RespuestaCorrecta = 3, Categoria = "Controles", Dificultad = "Medio" },
            new() { Id = 28, Texto = "¿Qué control permite entrada de texto multilínea?", Opciones = new() { "Entry", "Editor", "Label", "TextBox" }, RespuestaCorrecta = 1, Categoria = "Controles", Dificultad = "Facil" },
            new() { Id = 29, Texto = "¿Cómo se define el espaciado entre elementos en Grid?", Opciones = new() { "Spacing", "ColumnSpacing y RowSpacing", "Margin", "Padding" }, RespuestaCorrecta = 1, Categoria = "Controles", Dificultad = "Medio" },
            new() { Id = 30, Texto = "¿Qué propiedad controla si un control puede recibir foco?", Opciones = new() { "Focusable", "CanFocus", "IsTabStop", "IsFocusable" }, RespuestaCorrecta = 2, Categoria = "Controles", Dificultad = "Dificil" },

            // Datos (6 preguntas)
            new() { Id = 31, Texto = "¿Cuál es el patrón recomendado para servicios de datos en MAUI?", Opciones = new() { "Singleton", "Dependency Injection", "Static classes", "Repository pattern" }, RespuestaCorrecta = 1, Categoria = "Datos", Dificultad = "Medio", Explicacion = "DI permite desacoplamiento, testing y mejor mantenibilidad" },
            new() { Id = 32, Texto = "¿Qué cliente HTTP se recomienda usar con DI?", Opciones = new() { "HttpClient directo", "IHttpClientFactory", "WebClient", "RestSharp" }, RespuestaCorrecta = 1, Categoria = "Datos", Dificultad = "Medio" },
            new() { Id = 33, Texto = "¿Dónde se configuran los servicios en MAUI?", Opciones = new() { "App.xaml.cs", "MauiProgram.cs", "AppShell.xaml.cs", "MainPage.xaml.cs" }, RespuestaCorrecta = 1, Categoria = "Datos", Dificultad = "Facil" },
            new() { Id = 34, Texto = "¿Qué lifetime es apropiado para servicios de datos sin estado?", Opciones = new() { "Singleton", "Transient", "Scoped", "Temporal" }, RespuestaCorrecta = 0, Categoria = "Datos", Dificultad = "Medio" },
            new() { Id = 35, Texto = "¿Cuál es la ubicación recomendada para datos de la app?", Opciones = new() { "Carpeta de instalación", "FileSystem.AppDataDirectory", "Escritorio", "Documentos" }, RespuestaCorrecta = 1, Categoria = "Datos", Dificultad = "Medio" },
            new() { Id = 36, Texto = "¿Qué librería es popular para manejo de JSON en .NET?", Opciones = new() { "Newtonsoft.Json", "System.Text.Json", "Ambas son válidas", "JsonConvert" }, RespuestaCorrecta = 2, Categoria = "Datos", Dificultad = "Facil" }
        };

        private static readonly List<ResultadoQuiz> _historial = new();

        public Task<List<CategoriaQuiz>> ObtenerCategoriasAsync()
        {
            return Task.FromResult(_categorias.ToList());
        }

        public Task<List<Pregunta>> ObtenerPreguntasPorCategoriaAsync(string categoria)
        {
            var preguntas = _preguntas.Where(p => p.Categoria == categoria).ToList();
            return Task.FromResult(preguntas);
        }

        public Task<List<Pregunta>> ObtenerPreguntasAleatoriasAsync(int cantidad = 10)
        {
            var random = new Random();
            var preguntasAleatorias = _preguntas.OrderBy(x => random.Next()).Take(cantidad).ToList();
            return Task.FromResult(preguntasAleatorias);
        }

        public Task GuardarResultadoAsync(ResultadoQuiz resultado)
        {
            _historial.Add(resultado);
            return Task.CompletedTask;
        }

        public Task<List<ResultadoQuiz>> ObtenerHistorialAsync()
        {
            return Task.FromResult(_historial.OrderByDescending(r => r.Respuestas.FirstOrDefault()?.TiempoRespuesta ?? DateTime.MinValue).ToList());
        }
    }
}