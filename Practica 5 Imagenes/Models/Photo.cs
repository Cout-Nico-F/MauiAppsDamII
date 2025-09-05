namespace Practica_5_Imagenes.Models;

/// <summary>
/// Modelo que representa una fotografía con metadatos.
/// Simula datos que podrían venir de una API de fotos como Unsplash, Picsum, etc.
/// En MVVM, los Models son POCOs (Plain Old CLR Objects) sin lógica de negocio.
/// </summary>
public class Photo
{
    /// <summary>
    /// Identificador único de la foto
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Título o descripción de la foto
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// URL de la imagen en tamaño completo (alta resolución)
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// URL de la miniatura (baja resolución, para listas)
    /// Usar thumbnails mejora el rendimiento en CollectionView con muchas imágenes
    /// </summary>
    public string ThumbnailUrl { get; set; } = string.Empty;

    /// <summary>
    /// Autor o fotógrafo (opcional)
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Ancho original de la imagen en píxeles
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Alto original de la imagen en píxeles
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Tamaño del archivo en bytes (si está disponible)
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Indica si la imagen está marcada como favorita por el usuario
    /// </summary>
    public bool IsFavorite { get; set; }
}