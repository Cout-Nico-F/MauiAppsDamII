namespace Practica4sinDI.Models;

// Modelo de dominio simple que representa un "Post" proveniente de una API pública.
// En MVVM, los Model son clases de datos puras (sin lógica de UI).
public class Post
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
