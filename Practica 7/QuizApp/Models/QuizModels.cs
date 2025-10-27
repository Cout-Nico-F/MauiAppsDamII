namespace QuizApp.Models
{
    public class Pregunta
    {
        public int Id { get; set; }
        public string Texto { get; set; } = string.Empty;
        public List<string> Opciones { get; set; } = new();
        public int RespuestaCorrecta { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public string Dificultad { get; set; } = string.Empty;
        public string? Explicacion { get; set; }

        public bool EsRespuestaCorrecta(int indiceRespuesta)
        {
            return indiceRespuesta == RespuestaCorrecta;
        }
    }

    public class RespuestaUsuario
    {
        public int PreguntaId { get; set; }
        public int RespuestaSeleccionada { get; set; }
        public bool EsCorrecta { get; set; }
        public DateTime TiempoRespuesta { get; set; }
    }

    public class ResultadoQuiz
    {
        public int TotalPreguntas { get; set; }
        public int RespuestasCorrectas { get; set; }
        public int RespuestasIncorrectas { get; set; }
        public TimeSpan TiempoTotal { get; set; }
        public List<RespuestaUsuario> Respuestas { get; set; } = new();
        public string Categoria { get; set; } = string.Empty;

        public double Porcentaje => TotalPreguntas > 0 ? (double)RespuestasCorrectas / TotalPreguntas * 100 : 0;

        public string Calificacion
        {
            get
            {
                return Porcentaje switch
                {
                    >= 90 => "Excelente",
                    >= 70 => "Muy Bien",
                    >= 50 => "Bien",
                    >= 30 => "Regular",
                    _ => "Necesita Mejorar"
                };
            }
        }

        public string ColorCalificacion
        {
            get
            {
                return Porcentaje switch
                {
                    >= 90 => "Green",
                    >= 70 => "LimeGreen",
                    >= 50 => "Orange",
                    >= 30 => "DarkOrange",
                    _ => "Red"
                };
            }
        }
    }

    public class CategoriaQuiz
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Icono { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int TotalPreguntas { get; set; }
    }
}