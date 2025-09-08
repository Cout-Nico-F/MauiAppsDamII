using System.Net.Http.Json;
using System.Text.Json;
using Practica_5_Imagenes.Models;

namespace Practica_5_Imagenes.Services;

/// <summary>
/// Implementación del servicio de fotografías que consume la API pública Lorem Picsum.
/// 
/// Lorem Picsum (https://picsum.photos/) es una API gratuita que proporciona imágenes placeholder
/// de alta calidad, perfecta para demos y prototipado. 
/// 
/// Buenas prácticas implementadas:
/// - HttpClient inyectado con IHttpClientFactory para manejo eficiente de conexiones
/// - Manejo de errores específicos con try/catch detallados
/// - Soporte completo para cancelación cooperativa
/// - URLs optimizadas para diferentes tamaños (thumbnails vs full size)
/// - Validación de URLs de imágenes
/// - Timeout y headers configurados adecuadamente
/// </summary>
public class PhotoService : IPhotoService
{
    private readonly HttpClient _httpClient;

    public PhotoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        
        // Configurar timeout específico para imágenes (pueden ser archivos grandes)
        _httpClient.Timeout = TimeSpan.FromSeconds(45);
        
        // Headers recomendados para APIs de imágenes
        if (!_httpClient.DefaultRequestHeaders.UserAgent.Any())
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Practica5Imagenes-MAUI/1.0");
        }
        
        // Accept headers para imágenes y JSON
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        _httpClient.DefaultRequestHeaders.Accept.ParseAdd("image/*");
    }

    public async Task<IReadOnlyList<Photo>> GetPhotosAsync(int count = 20, CancellationToken ct = default)
    {
        try
        {
            // Lorem Picsum API: lista de fotos con metadatos
            // Limitamos el count para evitar respuestas muy grandes
            var limitedCount = Math.Min(Math.Max(count, 1), 100);
            var url = $"https://picsum.photos/v2/list?page=1&limit={limitedCount}";

            var response = await _httpClient.GetFromJsonAsync<PicsumPhoto[]>(url, ct);
            
            if (response == null)
                return new List<Photo>();

            // Mapear de PicsumPhoto a nuestro modelo Photo
            return response.Select(MapPicsumToPhoto).ToList();
        }
        catch (OperationCanceledException)
        {
            // Propagar cancelación sin envolver
            throw;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Error de conexión al obtener las fotografías. Verifique su conexión a internet.", ex);
        }
        catch (JsonException ex)
        {
            throw new Exception("Error al procesar la respuesta del servicio de fotografías.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error inesperado al obtener fotografías: {ex.Message}", ex);
        }
    }

    public async Task<Photo?> GetPhotoByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            // Lorem Picsum API: información específica de una foto
            var url = $"https://picsum.photos/id/{id}/info";
            
            var response = await _httpClient.GetFromJsonAsync<PicsumPhoto>(url, ct);
            
            return response != null ? MapPicsumToPhoto(response) : null;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("404"))
        {
            // La foto no existe
            return null;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error de red al obtener la fotografía {id}.", ex);
        }
        catch (JsonException ex)
        {
            throw new Exception("Error al procesar la información de la fotografía.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error inesperado al obtener la fotografía {id}: {ex.Message}", ex);
        }
    }

    public async Task<IReadOnlyList<Photo>> SearchPhotosAsync(string searchTerm, int count = 20, CancellationToken ct = default)
    {
        // Lorem Picsum no soporta búsqueda real, pero simulamos el comportamiento
        // filtrando por autor o generando fotos relacionadas con el término
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetPhotosAsync(count, ct);

            // Obtener fotos y filtrar por autor que contenga el término de búsqueda
            var allPhotos = await GetPhotosAsync(count * 2, ct); // Obtener más para filtrar
            
            var filtered = allPhotos
                .Where(p => p.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                           p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Take(count)
                .ToList();

            // Si no hay coincidencias, devolver fotos aleatorias pero marcarlas como búsqueda
            if (!filtered.Any())
            {
                var randomPhotos = await GetPhotosAsync(count, ct);
                foreach (var photo in randomPhotos)
                {
                    photo.Title = $"Búsqueda: {searchTerm} - {photo.Title}";
                }
                return randomPhotos;
            }

            return filtered;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al buscar fotografías con el término '{searchTerm}': {ex.Message}", ex);
        }
    }

    public async Task<bool> ValidateImageUrlAsync(string imageUrl, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return false;

        try
        {
            // Realizar una petición HEAD para verificar que la imagen existe
            // HEAD es más eficiente que GET porque no descarga el contenido
            using var response = await _httpClient.SendAsync(
                new HttpRequestMessage(HttpMethod.Head, imageUrl), ct);

            // Verificar que sea exitoso y que el Content-Type sea una imagen
            if (!response.IsSuccessStatusCode)
                return false;

            var contentType = response.Content.Headers.ContentType?.MediaType;
            return !string.IsNullOrEmpty(contentType) && contentType.StartsWith("image/");
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch
        {
            // En caso de cualquier error, considerar la URL como inválida
            return false;
        }
    }

    /// <summary>
    /// Mapea un objeto PicsumPhoto (de la API) a nuestro modelo Photo
    /// </summary>
    private static Photo MapPicsumToPhoto(PicsumPhoto picsum)
    {
        return new Photo
        {
            Id = int.Parse(picsum.Id ?? "0"),
            Title = $"Fotografía por {picsum.Author}",
            Author = picsum.Author ?? "Autor desconocido",
            Width = picsum.Width,
            Height = picsum.Height,
            // URLs optimizadas: thumbnail de 300px de ancho, imagen completa de 800px
            ThumbnailUrl = $"https://picsum.photos/id/{picsum.Id}/300/200",
            Url = $"https://picsum.photos/id/{picsum.Id}/800/600",
            // Estimar el tamaño del archivo (aproximación)
            FileSize = (long)(picsum.Width * picsum.Height * 0.3), // Estimación burda
            IsFavorite = false
        };
    }

    /// <summary>
    /// Modelo interno que mapea la respuesta JSON de Lorem Picsum API
    /// </summary>
    private class PicsumPhoto
    {
        public string? Id { get; set; }
        public string? Author { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string? Url { get; set; }
        public string? Download_url { get; set; }
    }
}