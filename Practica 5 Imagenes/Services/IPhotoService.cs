using Practica_5_Imagenes.Models;

namespace Practica_5_Imagenes.Services;

/// <summary>
/// Interfaz que define el contrato para el servicio de obtención de fotografías.
/// Mantener interfaces facilita testing (mocks) y permite cambiar implementaciones.
/// </summary>
public interface IPhotoService
{
    /// <summary>
    /// Obtiene una lista de fotografías desde una fuente de datos (API, base de datos local, etc.)
    /// </summary>
    /// <param name="count">Número de fotos a obtener</param>
    /// <param name="ct">Token de cancelación para interrumpir la operación</param>
    /// <returns>Lista de fotografías</returns>
    Task<IReadOnlyList<Photo>> GetPhotosAsync(int count = 20, CancellationToken ct = default);

    /// <summary>
    /// Obtiene una fotografía específica por su ID
    /// </summary>
    /// <param name="id">ID de la fotografía</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Fotografía encontrada o null si no existe</returns>
    Task<Photo?> GetPhotoByIdAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Busca fotografías por término
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda</param>
    /// <param name="count">Número máximo de resultados</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>Lista de fotografías que coinciden con el término</returns>
    Task<IReadOnlyList<Photo>> SearchPhotosAsync(string searchTerm, int count = 20, CancellationToken ct = default);

    /// <summary>
    /// Verifica si una URL de imagen es válida y accesible
    /// </summary>
    /// <param name="imageUrl">URL de la imagen a verificar</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>True si la imagen es accesible, false en caso contrario</returns>
    Task<bool> ValidateImageUrlAsync(string imageUrl, CancellationToken ct = default);
}