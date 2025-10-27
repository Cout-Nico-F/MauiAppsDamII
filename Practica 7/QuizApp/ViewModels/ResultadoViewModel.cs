using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizApp.Models;

namespace QuizApp.ViewModels
{
    [QueryProperty(nameof(Resultado), "resultado")]
    public partial class ResultadoViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ResultadoQuiz? resultado;

        [ObservableProperty]
        private string mensajeFelicitacion = string.Empty;

        [ObservableProperty]
        private string tiempoFormateado = string.Empty;

        public ResultadoViewModel()
        {
            Title = "Resultado del Quiz";
        }

        partial void OnResultadoChanged(ResultadoQuiz? value)
        {
            if (value != null)
            {
                ActualizarDatos();
            }
        }

        private void ActualizarDatos()
        {
            if (Resultado == null) return;

            // Formatear tiempo
            var tiempo = Resultado.TiempoTotal;
            if (tiempo.TotalMinutes >= 1)
            {
                TiempoFormateado = $"{tiempo.Minutes:D2}:{tiempo.Seconds:D2}";
            }
            else
            {
                TiempoFormateado = $"{tiempo.Seconds} segundos";
            }

            // Mensaje de felicitacion personalizado
            MensajeFelicitacion = Resultado.Porcentaje switch
            {
                >= 90 => "Excelente trabajo!",
                >= 70 => "Muy bien hecho!",
                >= 50 => "Buen esfuerzo!",
                >= 30 => "Puedes mejorar",
                _ => "Sigue practicando"
            };
        }

        [RelayCommand]
        private async Task VolverAlMenu()
        {
            await NavegarA("//main");
        }

        [RelayCommand]
        private async Task RepetirQuiz()
        {
            if (Resultado == null) return;

            var parametros = new Dictionary<string, object>
            {
                ["categoria"] = Resultado.Categoria,
                ["modo"] = Resultado.Categoria == "Aleatorio" ? "aleatorio" : "categoria"
            };

            await NavegarA("quiz", parametros);
        }

        [RelayCommand]
        private async Task VerHistorial()
        {
            await NavegarA("historial");
        }

        [RelayCommand]
        private async Task CompartirResultado()
        {
            if (Resultado == null) return;

            var mensaje = $"Termine un quiz en Quiz MAUI!\n" +
                         $"Categoria: {Resultado.Categoria}\n" +
                         $"Puntuacion: {Resultado.RespuestasCorrectas}/{Resultado.TotalPreguntas} ({Resultado.Porcentaje:F0}%)\n" +
                         $"Calificacion: {Resultado.Calificacion}\n" +
                         $"Tiempo: {TiempoFormateado}";

            await MostrarAlerta("Compartir Resultado", mensaje);
        }
    }
}