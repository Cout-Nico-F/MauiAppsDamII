using System.Net.Http.Json;
using System.Text.Json; // Para JsonException y opciones de serialización
using HTTPmaui.Models;

namespace HTTPmaui.Services;

// Implementación concreta del servicio que consume una API REST usando HttpClient.
// Buenas prácticas usadas aquí:
// - HttpClient es provisto por DI (Dependency Injection) usando IHttpClientFactory.
// - Se serializan/deserializan JSON con System.Net.Http.Json (métodos de extensión).
// - Manejo de excepciones con try/catch y mensajes claros para la capa superior (VM).
public class ApiService : IApiService
{
    private readonly HttpClient _http;

    // La instancia de HttpClient es inyectada.
    public ApiService(HttpClient http)
    {
        _http = http;
        // BaseAddress define la URL base para todas las solicitudes.
        // Para demo usamos la API pública jsonplaceholder.
        _http.BaseAddress ??= new Uri("https://jsonplaceholder.typicode.com/");
        // También podríamos configurar encabezados comunes aquí (User-Agent, etc.).
    }

    public async Task<IReadOnlyList<Post>> GetPostsAsync(CancellationToken ct = default)
    {
        try
        {
            // GET /posts -> retorna JSON. ReadFromJsonAsync deserializa automáticamente.
            var posts = await _http.GetFromJsonAsync<List<Post>>("posts", ct);
            return posts ?? new List<Post>();
        }
        catch (OperationCanceledException)
        {
            // Importante propagar cancelaciones para que la UI lo maneje adecuadamente.
            throw;
        }
        catch (HttpRequestException ex)
        {
            // Excepción típica por problemas de red o status code no exitoso.
            // La re-lanzamos con un mensaje más pedagógico.
            throw new Exception("Error de red al consultar la lista de posts. Verifica tu conexión o intenta nuevamente.", ex);
        }
        catch (NotSupportedException ex)
        {
            // Ocurre si el contenido no es JSON o el media type no es soportado.
            throw new Exception("La API respondió con un formato no soportado.", ex);
        }
        catch (JsonException ex)
        {
            // Ocurre si el JSON tiene un formato inesperado.
            throw new Exception("No se pudo interpretar la respuesta de la API.", ex);
        }
    }

    public async Task<Post?> GetPostByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            // GET /posts/{id}
            return await _http.GetFromJsonAsync<Post>($"posts/{id}", ct);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error de red al consultar el post {id}.", ex);
        }
        catch (NotSupportedException ex)
        {
            throw new Exception("La API respondió con un formato no soportado.", ex);
        }
        catch (JsonException ex)
        {
            throw new Exception("No se pudo interpretar la respuesta de la API.", ex);
        }
    }
}
