namespace HTTPmaui.Models;

// Modelo de dominio simple que representa un "Post" proveniente de una API pública.
// Este tipo de clases viven en la carpeta Models y NO contienen lógica de UI,
// solo datos. En MVVM, la UI se enlaza (binding) a propiedades expuestas por el ViewModel,
// el cual a su vez manipula modelos como este.
public class Post
{
    // Identificador del recurso en el servidor
    public int Id { get; set; }

    // Id del usuario dueño del post (detalle propio de la API de ejemplo)
    public int UserId { get; set; }

    // Título del post
    public string Title { get; set; } = string.Empty;

    // Contenido del post
    public string Body { get; set; } = string.Empty;
}
