using Moq;
using Practica9.Models;
using Practica9.Services;
using Xunit;

namespace Practica9.Tests;

/// <summary>
/// FAKE: Implementación manual de IUsuarioRepository para testing.
/// 
/// Un "fake" es una implementación de una interfaz que:
/// - Simula comportamiento sin acceder a recursos reales (BD, red, etc.)
/// - Es simple de implementar manualmente
/// - Está bajo control total del desarrollador del test
/// 
/// Ventajas:
/// - Completamente determinista
/// - Muy rápido (sin I/O)
/// - Fácil de entender
/// 
/// Desventajas:
/// - Requiere mantenimiento manual
/// - Puede crecer si la interfaz tiene muchos métodos
/// - Potencial para duplicar lógica de la app
/// 
/// Alternativa: Moq (demostrado en otro archivo)
/// </summary>
public class UsuarioRepositoryFake : IUsuarioRepository
{
    // Base de datos "fake" en memoria
    private readonly List<Usuario> _usuarios = new()
    {
        new Usuario { Id = 1, Email = "test@mail.com", Password = "1234", Nombre = "Test User", Activo = true },
        new Usuario { Id = 2, Email = "admin@mail.com", Password = "admin", Nombre = "Admin", Activo = true },
        new Usuario { Id = 3, Email = "inactivo@mail.com", Password = "pass", Nombre = "Inactivo", Activo = false }
    };

    public Task<Usuario?> ObtenerPorEmailAsync(string email)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Email == email);
        return Task.FromResult(usuario);
    }

    public Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(usuario);
    }

    public Task<IReadOnlyList<Usuario>> ObtenerTodosAsync()
    {
        return Task.FromResult<IReadOnlyList<Usuario>>(_usuarios.AsReadOnly());
    }

    public Task<bool> GuardarAsync(Usuario usuario)
    {
        if (usuario == null)
            return Task.FromResult(false);

        var existente = _usuarios.FirstOrDefault(u => u.Id == usuario.Id);
        if (existente != null)
        {
            _usuarios.Remove(existente);
        }

        _usuarios.Add(usuario);
        return Task.FromResult(true);
    }

    public Task<bool> EliminarAsync(int id)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
        if (usuario != null)
        {
            _usuarios.Remove(usuario);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}

/// <summary>
/// Tests para UsuarioService utilizando FAKES.
/// 
/// Estos tests demuestran:
/// - Tests async con Task
/// - Patrón AAA con servicios
/// - Manejo de excepciones en async
/// - Tests de lógica de negocio compleja
/// 
/// Estructura: cada test se enfoca en UNA responsabilidad/escenario.
/// </summary>
public class UsuarioServiceFakeTests
{
    private readonly IUsuarioRepository _repositoryFake;
    private readonly UsuarioService _usuarioService;

    /// <summary>
    /// Constructor: setup del fixture.
    /// Se ejecuta una vez por test.
    /// </summary>
    public UsuarioServiceFakeTests()
    {
        _repositoryFake = new UsuarioRepositoryFake();
        _usuarioService = new UsuarioService(_repositoryFake);
    }

    #region Pruebas de Autenticación

    /// <summary>
    /// Test 1: AutenticarAsync con credenciales válidas ? retorna true.
    /// 
    /// Patrón AAA async:
    /// - Arrange: preparar datos
    /// - Act: await método async
    /// - Assert: verificar resultado
    /// </summary>
    [Fact]
    public async Task AutenticarAsync_CredencialesValidas_RetornaTrue()
    {
        // Arrange
        string email = "test@mail.com";
        string password = "1234";

        // Act
        bool resultado = await _usuarioService.AutenticarAsync(email, password);

        // Assert
        Assert.True(resultado);
    }

    /// <summary>
    /// Test 2: AutenticarAsync con email inexistente ? retorna false.
    /// </summary>
    [Fact]
    public async Task AutenticarAsync_EmailNoExiste_RetornaFalse()
    {
        // Arrange
        string email = "noexiste@mail.com";
        string password = "cualquier";

        // Act
        bool resultado = await _usuarioService.AutenticarAsync(email, password);

        // Assert
        Assert.False(resultado);
    }

    /// <summary>
    /// Test 3: AutenticarAsync con contraseña incorrecta ? retorna false.
    /// </summary>
    [Fact]
    public async Task AutenticarAsync_ContraseñaIncorrecta_RetornaFalse()
    {
        // Arrange
        string email = "test@mail.com";
        string password = "incorrecta";

        // Act
        bool resultado = await _usuarioService.AutenticarAsync(email, password);

        // Assert
        Assert.False(resultado);
    }

    /// <summary>
    /// Test 4: AutenticarAsync con usuario inactivo ? lanza InvalidOperationException.
    /// 
    /// Patrón: Assert.ThrowsAsync<T> para excepciones async.
    /// </summary>
    [Fact]
    public async Task AutenticarAsync_UsuarioInactivo_LanzaInvalidOperationException()
    {
        // Arrange
        string email = "inactivo@mail.com";
        string password = "pass";

        // Act & Assert
        var excepcion = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _usuarioService.AutenticarAsync(email, password)
        );

        Assert.Contains("inactivo", excepcion.Message);
    }

    /// <summary>
    /// Test 5: AutenticarAsync con email vacío ? lanza ArgumentException.
    /// </summary>
    [Fact]
    public async Task AutenticarAsync_EmailVacio_LanzaArgumentException()
    {
        // Arrange
        string email = "";
        string password = "1234";

        // Act & Assert
        var excepcion = await Assert.ThrowsAsync<ArgumentException>(() =>
            _usuarioService.AutenticarAsync(email, password)
        );

        Assert.Contains("email", excepcion.Message, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Test 6: AutenticarAsync con contraseña vacía ? lanza ArgumentException.
    /// </summary>
    [Fact]
    public async Task AutenticarAsync_ContraseñaVacia_LanzaArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _usuarioService.AutenticarAsync("test@mail.com", "")
        );
    }

    #endregion

    #region Pruebas de Registro

    /// <summary>
    /// Test 7: RegistrarAsync con datos válidos ? retorna true.
    /// </summary>
    [Fact]
    public async Task RegistrarAsync_DatosValidos_RetornaTrue()
    {
        // Arrange
        string email = "nuevo@mail.com";
        string password = "password123";
        string nombre = "Nuevo Usuario";

        // Act
        bool resultado = await _usuarioService.RegistrarAsync(email, password, nombre);

        // Assert
        Assert.True(resultado);
        
        // Verificación adicional: el usuario fue guardado
        var usuarioGuardado = await _repositoryFake.ObtenerPorEmailAsync(email);
        Assert.NotNull(usuarioGuardado);
        Assert.Equal(nombre, usuarioGuardado.Nombre);
    }

    /// <summary>
    /// Test 8: RegistrarAsync con email duplicado ? lanza InvalidOperationException.
    /// </summary>
    [Fact]
    public async Task RegistrarAsync_EmailDuplicado_LanzaInvalidOperationException()
    {
        // Arrange
        string email = "test@mail.com"; // Ya existe en el fake
        string password = "password123";
        string nombre = "Otro Usuario";

        // Act & Assert
        var excepcion = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _usuarioService.RegistrarAsync(email, password, nombre)
        );

        Assert.Contains("ya está registrado", excepcion.Message);
    }

    /// <summary>
    /// Test 9: RegistrarAsync con contraseña muy corta ? lanza ArgumentException.
    /// </summary>
    [Fact]
    public async Task RegistrarAsync_ContraseñaCorta_LanzaArgumentException()
    {
        var excepcion = await Assert.ThrowsAsync<ArgumentException>(() =>
            _usuarioService.RegistrarAsync("new@mail.com", "123", "Nombre")
        );

        Assert.Contains("al menos 4 caracteres", excepcion.Message);
    }

    #endregion

    #region Pruebas de consulta de perfil

    /// <summary>
    /// Test 10: ObtenerPerfilAsync con ID válido ? retorna usuario.
    /// </summary>
    [Fact]
    public async Task ObtenerPerfilAsync_IdValido_RetornaUsuario()
    {
        // Arrange
        int userId = 1;

        // Act
        var usuario = await _usuarioService.ObtenerPerfilAsync(userId);

        // Assert
        Assert.NotNull(usuario);
        Assert.Equal("test@mail.com", usuario.Email);
    }

    /// <summary>
    /// Test 11: ObtenerPerfilAsync con ID inválido (?0) ? lanza ArgumentException.
    /// </summary>
    [Fact]
    public async Task ObtenerPerfilAsync_IdInvalido_LanzaArgumentException()
    {
        var excepcion = await Assert.ThrowsAsync<ArgumentException>(() =>
            _usuarioService.ObtenerPerfilAsync(-1)
        );

        Assert.Contains("no es válido", excepcion.Message);
    }

    /// <summary>
    /// Test 12: ObtenerPerfilAsync con ID no existente ? retorna null.
    /// </summary>
    [Fact]
    public async Task ObtenerPerfilAsync_IdNoExistente_RetornaNull()
    {
        var usuario = await _usuarioService.ObtenerPerfilAsync(999);
        Assert.Null(usuario);
    }

    #endregion

    #region Pruebas de contadores

    /// <summary>
    /// Test 13: ObtenerCountUsuariosActivosAsync ? retorna cantidad correcta.
    /// </summary>
    [Fact]
    public async Task ObtenerCountUsuariosActivosAsync_RetornaCantidadCorrecta()
    {
        // El fake tiene 2 usuarios activos (id 1 y 2, id 3 es inactivo)
        int cantidad = await _usuarioService.ObtenerCountUsuariosActivosAsync();
        Assert.Equal(2, cantidad);
    }

    #endregion
}