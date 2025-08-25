using System.Net.Http.Json;
using System.Text.Json;
using Practica4sinDI.Models;

namespace Practica4sinDI.Services;

// Servicio HTTP sin usar DI del contenedor. Aquí administramos explícitamente el HttpClient.
// Buenas prácticas destacadas en este archivo:
// - Reutilizar HttpClient (evita agotar sockets y mejora performance).
public class ApiService
{
    // HttpClient es costoso de crear; se recomienda reutilizarlo durante la vida de la app.
    // Nota: en proyectos más avanzados se prefiere IHttpClientFactory (DI). Aquí, al evitar DI,
    // usamos un campo estático que se configura una sola vez.
    private static readonly HttpClient _http = new()
    {
        // BaseAddress facilita construir rutas relativas: Get("posts") termina en .../posts
        BaseAddress = new Uri("https://jsonplaceholder.typicode.com/")
    };

    // Constructor estático: se ejecuta una sola vez, ideal para headers/timeout comunes.
    static ApiService()
    {
        // Establecemos un User-Agent simple para fines educativos; algunas APIs lo requieren.
        if (!_http.DefaultRequestHeaders.UserAgent.Any())
            _http.DefaultRequestHeaders.UserAgent.ParseAdd("Practica4sinDI-HttpClient/1.0");

        // Timeout de red razonable. Aclaración: preferimos cancelación cooperativa con CancellationToken,
        // pero fijar Timeout ayuda a evitar esperas indefinidas si la red se queda colgada.
        _http.Timeout = TimeSpan.FromSeconds(30);
    }

    // ------------------------
    // Métodos GET
    // ------------------------
    public async Task<IReadOnlyList<Post>> GetPostsAsync(CancellationToken ct = default)
    {
        // Patrón try/catch recomendado en la capa de acceso HTTP: capturar errores previsibles
        // y traducirlos a mensajes entendibles por la capa superior (ViewModel/UI).
        try
        {
            // GET /posts: ReadFromJsonAsync deserializa el JSON a List<Post>.
            // El CancellationToken permite cancelar la solicitud desde la UI.
            var posts = await _http.GetFromJsonAsync<List<Post>>("posts", ct);
            return posts ?? new List<Post>();
        }
        catch (OperationCanceledException)
        {
            // Propagamos la cancelación sin envolver; la UI debe diferenciarla de un error real.
            throw;
        }
        catch (HttpRequestException ex)
        {
            // Fallos de red o códigos de estado no exitosos cuando se usa EnsureSuccessStatusCode.
            throw new Exception("Error de red al consultar la lista de posts.", ex);
        }
        catch (NotSupportedException ex)
        {
            // Content-Type inesperado o no soportado por los helpers de System.Net.Http.Json.
            throw new Exception("La API respondió con un formato no soportado.", ex);
        }
        catch (JsonException ex)
        {
            // JSON mal formado o que no mapea al modelo Post.
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

    // ------------------------
    // Métodos POST, PUT, DELETE
    // ------------------------

    // Crea un nuevo recurso Post en el servidor.
    // En jsonplaceholder, la API simula la creación y devuelve un Id ficticio.
    public async Task<Post> CreatePostAsync(Post newPost, CancellationToken ct = default)
    {
        try
        {
            // PostAsJsonAsync serializa el objeto a JSON y envía la solicitud.
            var response = await _http.PostAsJsonAsync("posts", newPost, ct);

            // Siempre validar el código de estado: si no es exitoso (2xx), lanza HttpRequestException.
            response.EnsureSuccessStatusCode();

            // Leer y deserializar el cuerpo a Post.
            var created = await response.Content.ReadFromJsonAsync<Post>(cancellationToken: ct);
            if (created == null)
                throw new Exception("La API no devolvió el recurso creado.");

            return created;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Error de red al crear el post.", ex);
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

    // Actualiza un recurso existente identificándolo por id.
    public async Task<Post> UpdatePostAsync(int id, Post update, CancellationToken ct = default)
    {
        try
        {
            var response = await _http.PutAsJsonAsync($"posts/{id}", update, ct);
            response.EnsureSuccessStatusCode();

            var updated = await response.Content.ReadFromJsonAsync<Post>(cancellationToken: ct);
            if (updated == null)
                throw new Exception("La API no devolvió el recurso actualizado.");

            return updated;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error de red al actualizar el post {id}.", ex);
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

    // Elimina un recurso por id. Retorna true si el servidor confirma éxito.
    public async Task<bool> DeletePostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var response = await _http.DeleteAsync($"posts/{id}", ct);
            if (response.IsSuccessStatusCode)
                return true;

            // Si deseamos detalles de error, podemos leer el contenido de la respuesta.
            var body = await response.Content.ReadAsStringAsync(ct);
            throw new Exception($"El servidor respondió {((int)response.StatusCode)} {response.StatusCode}. Detalle: {body}");
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error de red al eliminar el post {id}.", ex);
        }
    }
}
