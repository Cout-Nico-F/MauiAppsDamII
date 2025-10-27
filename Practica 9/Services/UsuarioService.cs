namespace Practica9.Services;

using Practica9.Models;

/// <summary>
/// Interfaz que define el contrato para acceso a usuarios.
/// Esto permite testear servicios que dependen de esta interfaz
/// utilizando fakes o mocks en lugar de una BD real.
/// 
/// Principio: Dependency Inversion —  depender de abstracciones, no de implementaciones concretas.
/// </summary>
public interface IUsuarioRepository
{
    /// <summary>
    /// Obtiene un usuario por su email.
    /// </summary>
    Task<Usuario?> ObtenerPorEmailAsync(string email);

    /// <summary>
    /// Obtiene un usuario por su ID.
    /// </summary>
    Task<Usuario?> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Obtiene todos los usuarios.
    /// </summary>
    Task<IReadOnlyList<Usuario>> ObtenerTodosAsync();

    /// <summary>
    /// Guarda un usuario (crear o actualizar).
    /// </summary>
    Task<bool> GuardarAsync(Usuario usuario);

    /// <summary>
    /// Elimina un usuario por ID.
    /// </summary>
    Task<bool> EliminarAsync(int id);
}

/// <summary>
/// Servicio de autenticación y gestión de usuarios.
/// 
/// Este servicio será testeado con:
/// - Fakes: implementación manual de IUsuarioRepository
/// - Mocks: stubs generados por Moq
/// 
/// Demostraremos:
/// - Tests sincronos y asincronos
/// - Manejo de excepciones
/// - Verificación de interacciones (Verify)
/// </summary>
public class UsuarioService
{
    private readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Autentica un usuario con email y contraseña.
    /// 
    /// Casos a testear:
    /// - Credenciales válidas ? true
    /// - Email no existe ? false
    /// - Contraseña incorrecta ? false
    /// - Email vacío o null ? exception
    /// </summary>
    public async Task<bool> AutenticarAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email no puede estar vacío", nameof(email));

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("La contraseña no puede estar vacía", nameof(password));

        var usuario = await _repository.ObtenerPorEmailAsync(email);
        
        if (usuario == null)
            return false;

        if (!usuario.Activo)
            throw new InvalidOperationException("El usuario está inactivo");

        return usuario.Password == password;
    }

    /// <summary>
    /// Registra un nuevo usuario.
    /// </summary>
    public async Task<bool> RegistrarAsync(string email, string password, string nombre)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email no puede estar vacío", nameof(email));

        if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
            throw new ArgumentException("La contraseña debe tener al menos 4 caracteres", nameof(password));

        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío", nameof(nombre));

        // Verificar que el email no exista ya
        var usuarioExistente = await _repository.ObtenerPorEmailAsync(email);
        if (usuarioExistente != null)
            throw new InvalidOperationException("El email ya está registrado");

        var nuevoUsuario = new Usuario
        {
            Email = email,
            Password = password,
            Nombre = nombre,
            Activo = true
        };

        return await _repository.GuardarAsync(nuevoUsuario);
    }

    /// <summary>
    /// Obtiene el perfil de un usuario por ID.
    /// </summary>
    public async Task<Usuario?> ObtenerPerfilAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("El ID de usuario no es válido", nameof(userId));

        return await _repository.ObtenerPorIdAsync(userId);
    }

    /// <summary>
    /// Obtiene la cantidad de usuarios activos.
    /// </summary>
    public async Task<int> ObtenerCountUsuariosActivosAsync()
    {
        var usuarios = await _repository.ObtenerTodosAsync();
        return usuarios.Count(u => u.Activo);
    }
}

/// <summary>
/// Servicio de cálculos para demonstrar tests unitarios simples.
/// No tiene dependencias externas, es ideal para primer test.
/// </summary>
public class CalculadoraService
{
    /// <summary>
    /// Suma dos números.
    /// Test básico: verifica resultado correcto.
    /// </summary>
    public int Sumar(int a, int b) => a + b;

    /// <summary>
    /// Divide dos números.
    /// Test con exception: verifica que lance excepción cuando divisor es 0.
    /// </summary>
    public decimal Dividir(decimal dividendo, decimal divisor)
    {
        if (divisor == 0)
            throw new DivideByZeroException("El divisor no puede ser cero");

        return dividendo / divisor;
    }

    /// <summary>
    /// Multiplica dos números.
    /// </summary>
    public int Multiplicar(int a, int b) => a * b;

    /// <summary>
    /// Calcula el promedio de una lista de números.
    /// Test con validación: verifica lista vacía.
    /// </summary>
    public decimal ObtenerPromedio(IEnumerable<int> numeros)
    {
        if (numeros == null || !numeros.Any())
            throw new ArgumentException("La lista de números no puede estar vacía", nameof(numeros));

        return (decimal)numeros.Sum() / numeros.Count();
    }
}