using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizApp.Models;
using QuizApp.Services;
using System.Collections.ObjectModel;

namespace QuizApp.ViewModels
{
    [QueryProperty(nameof(Categoria), "categoria")]
    [QueryProperty(nameof(Modo), "modo")]
    public partial class QuizViewModel : BaseViewModel
    {
        private readonly IQuizService _quizService;
        private DateTime _inicioQuiz;
        private DateTime _inicioPregunta;

        [ObservableProperty]
        private string categoria = string.Empty;

        [ObservableProperty]
        private string modo = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Pregunta> preguntas = new();

        [ObservableProperty]
        private Pregunta? preguntaActual;

        [ObservableProperty]
        private int indicePreguntaActual;

        [ObservableProperty]
        private int totalPreguntas;

        [ObservableProperty]
        private int? respuestaSeleccionada;

        [ObservableProperty]
        private bool mostrarExplicacion;

        [ObservableProperty]
        private bool quizCompletado;

        [ObservableProperty]
        private ResultadoQuiz? resultado;

        [ObservableProperty]
        private ObservableCollection<RespuestaUsuario> respuestas = new();

        [ObservableProperty]
        private string textoBotonSiguiente = "Siguiente";

        [ObservableProperty]
        private bool puedeResponder = true;

        // Propiedades seguras para binding
        public string Opcion0 => GetOpcionSegura(0);
        public string Opcion1 => GetOpcionSegura(1);
        public string Opcion2 => GetOpcionSegura(2);
        public string Opcion3 => GetOpcionSegura(3);

        public string ProgresoPorcentaje => TotalPreguntas > 0 ? $"{(double)(IndicePreguntaActual + 1) / TotalPreguntas * 100:F0}%" : "0%";
        public string ProgresoTexto => $"Pregunta {IndicePreguntaActual + 1} de {TotalPreguntas}";
        public double ProgresoValor => TotalPreguntas > 0 ? (double)(IndicePreguntaActual + 1) / TotalPreguntas : 0.0;

        public QuizViewModel(IQuizService quizService)
        {
            _quizService = quizService;
            Title = "Quiz MAUI";
        }

        private string GetOpcionSegura(int index)
        {
            try
            {
                if (PreguntaActual?.Opciones != null && index >= 0 && index < PreguntaActual.Opciones.Count)
                    return PreguntaActual.Opciones[index];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Quiz] Error accediendo opcion {index}: {ex.Message}");
            }
            return string.Empty;
        }

        public override async Task InicializarAsync()
        {
            try
            {
                await CargarPreguntas();
                IniciarQuiz();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Quiz] Error inicializando: {ex}");
                await MostrarAlerta("Error", $"No se pudieron cargar las preguntas: {ex.Message}");
            }
        }

        partial void OnCategoriaChanged(string value) => Title = $"Quiz: {value}";

        partial void OnPreguntaActualChanged(Pregunta? value)
        {
            OnPropertyChanged(nameof(Opcion0));
            OnPropertyChanged(nameof(Opcion1));
            OnPropertyChanged(nameof(Opcion2));
            OnPropertyChanged(nameof(Opcion3));
        }

        partial void OnIndicePreguntaActualChanged(int value)
        {
            OnPropertyChanged(nameof(ProgresoPorcentaje));
            OnPropertyChanged(nameof(ProgresoTexto));
            OnPropertyChanged(nameof(ProgresoValor));
        }

        [RelayCommand]
        private async Task CargarPreguntas()
        {
            try
            {
                List<Pregunta> preguntasData = Modo == "aleatorio"
                    ? await _quizService.ObtenerPreguntasAleatoriasAsync(10)
                    : await _quizService.ObtenerPreguntasPorCategoriaAsync(Categoria);

                // Validación defensiva de datos
                foreach (var p in preguntasData)
                {
                    if (p.Opciones == null || p.Opciones.Count < 2)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Quiz][WARN] Pregunta {p.Id} con opciones insuficientes");
                        p.Opciones = p.Opciones?.Count > 0 ? p.Opciones : new List<string> { "N/D", "N/D" };
                    }
                    if (p.RespuestaCorrecta < 0 || p.RespuestaCorrecta >= p.Opciones.Count)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Quiz][WARN] Pregunta {p.Id} con índice de respuesta inválido. Ajustando a 0");
                        p.RespuestaCorrecta = 0;
                    }
                }

                Preguntas.Clear();
                foreach (var pregunta in preguntasData)
                    Preguntas.Add(pregunta);

                TotalPreguntas = Preguntas.Count;

                if (TotalPreguntas == 0)
                    await MostrarAlerta("Sin preguntas", "No se encontraron preguntas para esta categoría.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Quiz] Error cargando preguntas: {ex}");
                await MostrarAlerta("Error", $"Error cargando preguntas: {ex.Message}");
                throw;
            }
        }

        private void IniciarQuiz()
        {
            if (Preguntas.Count == 0) return;
            _inicioQuiz = DateTime.Now;
            IndicePreguntaActual = 0;
            PreguntaActual = Preguntas[0];
            _inicioPregunta = DateTime.Now;
            QuizCompletado = false;
            MostrarExplicacion = false;
            PuedeResponder = true;
            RespuestaSeleccionada = null;
            TextoBotonSiguiente = "Responder";
        }

        [RelayCommand]
        private void SeleccionarRespuesta(int indice)
        {
            if (!PuedeResponder) return;
            RespuestaSeleccionada = indice;
            TextoBotonSiguiente = "Responder";
        }

        [RelayCommand]
        private async Task SiguientePregunta()
        {
            try
            {
                if (RespuestaSeleccionada == null)
                {
                    await MostrarAlerta("Atención", "Por favor selecciona una respuesta");
                    return;
                }
                if (PuedeResponder)
                {
                    await ProcesarRespuesta();
                    return;
                }
                await AvanzarPregunta();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Quiz] Error en SiguientePregunta: {ex}");
                await MostrarAlerta("Error", $"Error procesando respuesta: {ex.Message}");
            }
        }

        private async Task ProcesarRespuesta()
        {
            try
            {
                if (RespuestaSeleccionada == null || PreguntaActual == null) return;
                PuedeResponder = false;

                bool esCorrecta = false;
                string respuestaCorrectaTexto = "N/D";
                try
                {
                    esCorrecta = PreguntaActual.EsRespuestaCorrecta(RespuestaSeleccionada.Value);
                    if (PreguntaActual.Opciones != null &&
                        PreguntaActual.RespuestaCorrecta >= 0 &&
                        PreguntaActual.RespuestaCorrecta < PreguntaActual.Opciones.Count)
                    {
                        respuestaCorrectaTexto = PreguntaActual.Opciones[PreguntaActual.RespuestaCorrecta];
                    }
                }
                catch (Exception inner)
                {
                    System.Diagnostics.Debug.WriteLine($"[Quiz][ERROR] Validando respuesta: {inner.Message}");
                }

                Respuestas.Add(new RespuestaUsuario
                {
                    PreguntaId = PreguntaActual.Id,
                    RespuestaSeleccionada = RespuestaSeleccionada.Value,
                    EsCorrecta = esCorrecta,
                    TiempoRespuesta = DateTime.Now
                });

                if (!string.IsNullOrEmpty(PreguntaActual.Explicacion))
                    MostrarExplicacion = true;

                var titulo = esCorrecta ? "Correcto" : "Incorrecto";
                var mensaje = esCorrecta ?
                    $"Respuesta correcta: {respuestaCorrectaTexto}" :
                    $"Respuesta correcta: {respuestaCorrectaTexto}";

                if (!string.IsNullOrEmpty(PreguntaActual.Explicacion))
                    mensaje += $"\n\n{PreguntaActual.Explicacion}";

                await MostrarAlerta(titulo, mensaje);
                TextoBotonSiguiente = IndicePreguntaActual >= TotalPreguntas - 1 ? "Ver Resultados" : "Siguiente";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Quiz] Error en ProcesarRespuesta: {ex}");
                await MostrarAlerta("Error", $"No se pudo procesar la respuesta: {ex.Message}");
                PuedeResponder = true; // permitir reintentar
            }
        }

        private async Task AvanzarPregunta()
        {
            try
            {
                if (IndicePreguntaActual < TotalPreguntas - 1)
                {
                    IndicePreguntaActual++;
                    PreguntaActual = Preguntas[IndicePreguntaActual];
                    RespuestaSeleccionada = null;
                    MostrarExplicacion = false;
                    PuedeResponder = true;
                    _inicioPregunta = DateTime.Now;
                    TextoBotonSiguiente = "Responder";
                }
                else
                {
                    await CompletarQuiz();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Quiz] Error avanzando de pregunta: {ex}");
                await MostrarAlerta("Error", $"No se pudo avanzar: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task CompletarQuiz()
        {
            try
            {
                var tiempoTotal = DateTime.Now - _inicioQuiz;
                var respuestasCorrectas = Respuestas.Count(r => r.EsCorrecta);

                Resultado = new ResultadoQuiz
                {
                    TotalPreguntas = TotalPreguntas,
                    RespuestasCorrectas = respuestasCorrectas,
                    RespuestasIncorrectas = TotalPreguntas - respuestasCorrectas,
                    TiempoTotal = tiempoTotal,
                    Respuestas = Respuestas.ToList(),
                    Categoria = Categoria
                };

                await _quizService.GuardarResultadoAsync(Resultado);
                QuizCompletado = true;
                await NavegarA("resultado", new Dictionary<string, object> { ["resultado"] = Resultado });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Quiz] Error completando quiz: {ex}");
                await MostrarAlerta("Error", $"Error completando quiz: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task ReiniciarQuiz()
        {
            try
            {
                Respuestas.Clear();
                await CargarPreguntas();
                IniciarQuiz();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Quiz] Error reiniciando: {ex}");
                await MostrarAlerta("Error", $"Error reiniciando quiz: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task VolverAlMenu()
        {
            try
            {
                await NavegarA("//main");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Quiz] Error navegando al menú: {ex}");
                await MostrarAlerta("Error", $"Error navegando al menú: {ex.Message}");
            }
        }
    }
}