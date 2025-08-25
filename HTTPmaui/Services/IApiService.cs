using HTTPmaui.Models;

namespace HTTPmaui.Services;

// Interfaz que define el contrato de un servicio de acceso a datos vía HTTP.
// Mantener una interfaz facilita:
// - Mockear en tests.
// - Cambiar la implementación sin tocar el resto del código.
public interface IApiService
{
    // Obtiene una lista de posts desde una API pública.
    // Se utiliza CancellationToken para permitir cancelaciones desde la UI.
    Task<IReadOnlyList<Post>> GetPostsAsync(CancellationToken ct = default);

    // Obtiene un post por Id, demostrando el uso de rutas con parámetros.
    Task<Post?> GetPostByIdAsync(int id, CancellationToken ct = default);
}
