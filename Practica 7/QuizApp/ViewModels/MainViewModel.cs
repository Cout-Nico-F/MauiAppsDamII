using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizApp.Models;
using QuizApp.Services;
using System.Collections.ObjectModel;

namespace QuizApp.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly IQuizService _quizService;

        [ObservableProperty]
        private ObservableCollection<CategoriaQuiz> categorias = new();

        [ObservableProperty]
        private int totalPreguntasDisponibles;

        [ObservableProperty]
        private int quizzesCompletados;

        public MainViewModel(IQuizService quizService)
        {
            _quizService = quizService;
            Title = "Quiz MAUI";
        }

        public override async Task InicializarAsync()
        {
            await CargarCategorias();
            await CargarEstadisticas();
        }

        [RelayCommand]
        private async Task CargarCategorias()
        {
            await EjecutarOperacionAsync(async () =>
            {
                var categoriasData = await _quizService.ObtenerCategoriasAsync();
                Categorias.Clear();
                foreach (var categoria in categoriasData)
                {
                    Categorias.Add(categoria);
                }
            }, "Error al cargar categorias");
        }

        [RelayCommand]
        private async Task CargarEstadisticas()
        {
            await EjecutarOperacionAsync(async () =>
            {
                var historial = await _quizService.ObtenerHistorialAsync();
                QuizzesCompletados = historial.Count;
                
                var categorias = await _quizService.ObtenerCategoriasAsync();
                TotalPreguntasDisponibles = categorias.Sum(c => c.TotalPreguntas);
            });
        }

        [RelayCommand]
        private async Task IniciarQuizPorCategoria(CategoriaQuiz categoria)
        {
            if (categoria == null) return;

            var parametros = new Dictionary<string, object>
            {
                ["categoria"] = categoria.Nombre,
                ["modo"] = "categoria"
            };

            await NavegarA("quiz", parametros);
        }

        [RelayCommand]
        private async Task IniciarQuizAleatorio()
        {
            var parametros = new Dictionary<string, object>
            {
                ["categoria"] = "Aleatorio",
                ["modo"] = "aleatorio"
            };

            await NavegarA("quiz", parametros);
        }

        [RelayCommand]
        private async Task MostrarAyuda()
        {
            await MostrarAlerta(
                "Como usar la app",
                "1. Selecciona una categoria para hacer un quiz especifico\n" +
                "2. Usa 'Quiz Aleatorio' para preguntas de todas las categorias\n" +
                "3. Cada quiz te mostrara tu puntuacion al final"
            );
        }
    }
}