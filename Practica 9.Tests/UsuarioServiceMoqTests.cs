using Moq;
using Practica9.Models;
using Practica9.Services;
using Xunit;

namespace Practica9.Tests;

/// <summary>
/// Tests para UsuarioService utilizando MOQ (framework de mocks dinámicos).
/// 
/// Moq permite:
/// - Generar mocks automáticamente sin implementar interfaces manualmente
/// - Definir comportamiento con Setup
/// - Verificar interacciones con Verify
/// - Parametrizar mocks fácilmente
/// 
/// Ventajas sobre Fakes:
/// - Menos código: no necesitas implementar toda la interfaz
/// - Flexible: cambias comportamiento sin modificar la clase fake
/// - Preciso: verificas exactamente qué métodos fueron llamados y cuántas veces
/// 
/// Desventajas:
/// - Puede ser más "mágico" y difícil de debuggear para principiantes
/// - Overkill para lógica muy simple (donde fakes son suficientes)
/// 
/// Referencia: https://github.com/moq/moq4
/// </summary>
public class UsuarioServiceMoqTests
{
    /// <summary>
    /// Test 1: AutenticarAsync con Moq - credenciales válidas.
    /// 
    /// Patrón Moq:
    /// 1. Crear mock: new Mock<IInterfaz>()
    /// 2. Setup: definir comportamiento esperado
    /// 3. Act: ejecutar método
    /// 4. Assert: verificar resultado
    /// 5. Verify (opcional): asegurar que los mocks fueron usados correctamente
    /// </summary>
    [Fact]
    public async Task AutenticarAsync_CredencialesValidas_RetornaTrue_ConMoq()
    {
        // Arrange: crear mock del repositorio
        var mockRepo = new Mock<IUsuarioRepository>();
        
        // Setup: cuando se llame a ObtenerPorEmailAsync("test@mail.com"),
        //        retorna el usuario preparado
        var usuarioExistente = new Usuario 
        { 
            Id = 1, 
            Email = "test@mail.com", 
            Password = "1234", 
            Nombre = "Test", 
            Activo = true 
        };

        mockRepo
            .Setup(r => r.ObtenerPorEmailAsync("test@mail.com"))
            .ReturnsAsync(usuarioExistente);

        // Crear el servicio con el mock
        var servicio = new UsuarioService(mockRepo.Object);

        // Act
        bool resultado = await servicio.AutenticarAsync("test@mail.com", "1234");

        // Assert
        Assert.True(resultado);
        
        // Verify: verificar que el repositorio fue llamado exactamente una vez
        mockRepo.Verify(
            r => r.ObtenerPorEmailAsync("test@mail.com"), 
            Times.Once,
            "El repositorio debe ser consultado exactamente una vez"
        );
    }

    /// <summary>
    /// Test 2: AutenticarAsync con email no existente.
    /// 
    /// El mock retorna null cuando el email no existe.
    /// </summary>
    [Fact]
    public async Task AutenticarAsync_EmailNoExistente_RetornaFalse_ConMoq()
    {
        // Arrange
        var mockRepo = new Mock<IUsuarioRepository>();
        
        // Setup: para cualquier email, retorna null (no configuramos un caso específico)
        mockRepo
            .Setup(r => r.ObtenerPorEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((Usuario?)null);

        var servicio = new UsuarioService(mockRepo.Object);

        // Act
        bool resultado = await servicio.AutenticarAsync("noexiste@mail.com", "cualquier");

        // Assert
        Assert.False(resultado);
        
        // Verify
        mockRepo.Verify(r => r.ObtenerPorEmailAsync("noexiste@mail.com"), Times.Once);
    }

    /// <summary>
    /// Test 3: AutenticarAsync con usuario inactivo.
    /// 
    /// Demuestra: el servicio verifica el estado Activo del usuario.
    /// </summary>
    [Fact]
    public async Task AutenticarAsync_UsuarioInactivo_LanzaExcepcion_ConMoq()
    {
        // Arrange
        var mockRepo = new Mock<IUsuarioRepository>();
        var usuarioInactivo = new Usuario 
        { 
            Id = 1, 
            Email = "inactivo@mail.com", 
            Password = "1234", 
            Nombre = "Inactivo", 
            Activo = false // ? Clave: inactivo
        };

        mockRepo
            .Setup(r => r.ObtenerPorEmailAsync("inactivo@mail.com"))
            .ReturnsAsync(usuarioInactivo);

        var servicio = new UsuarioService(mockRepo.Object);

        // Act & Assert
        var excepcion = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            servicio.AutenticarAsync("inactivo@mail.com", "1234")
        );

        Assert.Contains("inactivo", excepcion.Message);
    }

    /// <summary>
    /// Test 4: RegistrarAsync con email duplicado.
    /// 
    /// Demuestra: Moq puede retornar diferentes valores según el parámetro.
    /// </summary>
    [Fact]
    public async Task RegistrarAsync_EmailDuplicado_LanzaExcepcion_ConMoq()
    {
        // Arrange
        var mockRepo = new Mock<IUsuarioRepository>();
        var usuarioExistente = new Usuario 
        { 
            Id = 1, 
            Email = "existente@mail.com", 
            Password = "1234", 
            Nombre = "Existente" 
        };

        // Setup: cuando consulta por "existente@mail.com", retorna el usuario
        mockRepo
            .Setup(r => r.ObtenerPorEmailAsync("existente@mail.com"))
            .ReturnsAsync(usuarioExistente);

        var servicio = new UsuarioService(mockRepo.Object);

        // Act & Assert
        var excepcion = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            servicio.RegistrarAsync("existente@mail.com", "password", "Nombre")
        );

        Assert.Contains("ya está registrado", excepcion.Message);
    }

    /// <summary>
    /// Test 5: RegistrarAsync exitoso con Verify completo.
    /// 
    /// Demuestra: verificamos que GuardarAsync fue llamado con datos específicos.
    /// </summary>
    [Fact]
    public async Task RegistrarAsync_DatosValidos_GuardaYRetornaTrue_ConMoq()
    {
        // Arrange
        var mockRepo = new Mock<IUsuarioRepository>();

        // Setup: ObtenerPorEmailAsync retorna null (email no existe)
        mockRepo
            .Setup(r => r.ObtenerPorEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((Usuario?)null);

        // Setup: GuardarAsync siempre retorna true
        mockRepo
            .Setup(r => r.GuardarAsync(It.IsAny<Usuario>()))
            .ReturnsAsync(true);

        var servicio = new UsuarioService(mockRepo.Object);

        // Act
        bool resultado = await servicio.RegistrarAsync("nuevo@mail.com", "password", "Nuevo Usuario");

        // Assert
        Assert.True(resultado);

        // Verify: el repositorio fue consultado para verificar que no existe el email
        mockRepo.Verify(r => r.ObtenerPorEmailAsync("nuevo@mail.com"), Times.Once);

        // Verify: GuardarAsync fue llamado exactamente una vez
        mockRepo.Verify(r => r.GuardarAsync(It.IsAny<Usuario>()), Times.Once);
    }

    /// <summary>
    /// Test 6: Verificar parámetros específicos en Verify.
    /// 
    /// Demuestra: It.Is<T>() para verificaciones más precisas.
    /// </summary>
    [Fact]
    public async Task RegistrarAsync_GuardaConNombreCorreo_ConMoq()
    {
        // Arrange
        var mockRepo = new Mock<IUsuarioRepository>();
        mockRepo
            .Setup(r => r.ObtenerPorEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((Usuario?)null);
        mockRepo
            .Setup(r => r.GuardarAsync(It.IsAny<Usuario>()))
            .ReturnsAsync(true);

        var servicio = new UsuarioService(mockRepo.Object);

        // Act
        await servicio.RegistrarAsync("nuevo@mail.com", "pass123", "Mi Nombre");

        // Assert & Verify: verificar que GuardarAsync fue llamado con un usuario
        // que tiene el nombre exacto "Mi Nombre"
        mockRepo.Verify(
            r => r.GuardarAsync(It.Is<Usuario>(u => u.Nombre == "Mi Nombre")),
            Times.Once
        );

        // Verificar además que el email sea correcto
        mockRepo.Verify(
            r => r.GuardarAsync(It.Is<Usuario>(u => u.Email == "nuevo@mail.com")),
            Times.Once
        );
    }

    /// <summary>
    /// Test 7: Usar It.IsAny para casos que no importa el parámetro.
    /// </summary>
    [Theory]
    [InlineData("email1@mail.com")]
    [InlineData("email2@mail.com")]
    [InlineData("email3@mail.com")]
    public async Task AutenticarAsync_ConEmailsVariados_VerificaVeces_ConMoq(string email)
    {
        // Arrange
        var mockRepo = new Mock<IUsuarioRepository>();
        mockRepo
            .Setup(r => r.ObtenerPorEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((Usuario?)null);

        var servicio = new UsuarioService(mockRepo.Object);

        // Act
        await servicio.AutenticarAsync(email, "cualquier");

        // Assert: verificar que se llamó a ObtenerPorEmailAsync exactamente una vez
        mockRepo.Verify(r => r.ObtenerPorEmailAsync(It.IsAny<string>()), Times.Once);
    }

    /// <summary>
    /// Test 8: Verificar que un método NUNCA fue llamado (Never).
    /// 
    /// Útil para asegurar que cierto código no se ejecutó.
    /// </summary>
    [Fact]
    public async Task RegistrarAsync_ContraseñaInvalida_NoGuarda_ConMoq()
    {
        // Arrange
        var mockRepo = new Mock<IUsuarioRepository>();
        mockRepo
            .Setup(r => r.ObtenerPorEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((Usuario?)null);

        var servicio = new UsuarioService(mockRepo.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            servicio.RegistrarAsync("nuevo@mail.com", "123", "Nombre") // Contraseña muy corta
        );

        // Verify: GuardarAsync NUNCA debe haber sido llamado (porque validación falló)
        mockRepo.Verify(r => r.GuardarAsync(It.IsAny<Usuario>()), Times.Never);
    }

    /// <summary>
    /// Test 9: Setup con diferentes comportamientos según parámetro.
    /// 
    /// Demuestra: SetupSequence para retornar diferentes valores en llamadas sucesivas.
    /// </summary>
    [Fact]
    public async Task ObtenerPerfilAsync_MultiplasConsultas_ConMoq()
    {
        // Arrange
        var mockRepo = new Mock<IUsuarioRepository>();
        
        // Setup: primera llamada con ID 1 retorna usuario, segunda retorna null
        mockRepo
            .SetupSequence(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Usuario { Id = 1, Email = "test@mail.com" })
            .ReturnsAsync((Usuario?)null);

        var servicio = new UsuarioService(mockRepo.Object);

        // Act: primera consulta
        var usuario1 = await servicio.ObtenerPerfilAsync(1);
        Assert.NotNull(usuario1);

        // Act: segunda consulta (misma o diferente ID)
        var usuario2 = await servicio.ObtenerPerfilAsync(2);
        Assert.Null(usuario2);
    }

    /// <summary>
    /// Test 10: Verificar con It.IsInRange (para valores numéricos).
    /// </summary>
    [Fact]
    public async Task ObtenerPerfilAsync_ConIdValido_ConMoq()
    {
        // Arrange
        var mockRepo = new Mock<IUsuarioRepository>();
        mockRepo
            .Setup(r => r.ObtenerPorIdAsync(It.IsInRange(1, 1000, Moq.Range.Inclusive)))
            .ReturnsAsync(new Usuario { Id = 1, Email = "test@mail.com" });

        var servicio = new UsuarioService(mockRepo.Object);

        // Act
        var usuario = await servicio.ObtenerPerfilAsync(500);

        // Assert
        Assert.NotNull(usuario);
    }
}