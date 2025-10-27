# ?? Cheat Sheet: xUnit + Moq

**Referencia rápida para copiar/pegar en la clase**

---

## ?? xUnit: Básicos

### Test Básico

```csharp
using Xunit;

[Fact]
public void Sumar_5Y3_Retorna8()
{
    // Arrange
    var calc = new CalculadoraService();
    
    // Act
    int resultado = calc.Sumar(5, 3);
    
    // Assert
    Assert.Equal(8, resultado);
}
```

### Test con Parámetros

```csharp
[Theory]
[InlineData(1, 2, 3)]
[InlineData(10, 20, 30)]
[InlineData(-5, 5, 0)]
public void Sumar_Parametrizado_RetornaCorrectamente(int a, int b, int esperado)
{
    var resultado = calculadora.Sumar(a, b);
    Assert.Equal(esperado, resultado);
}
```

### Testear Excepciones

```csharp
[Fact]
public void Dividir_DivisorCero_LanzaExcepcion()
{
    var excepcion = Assert.Throws<DivideByZeroException>(() =>
        calc.Dividir(10, 0)
    );
    
    Assert.Contains("divisor", excepcion.Message);
}
```

### Testear Excepciones en Async

```csharp
[Fact]
public async Task AutenticarAsync_EmailVacio_LanzaArgumentException()
{
    var excepcion = await Assert.ThrowsAsync<ArgumentException>(() =>
        service.AutenticarAsync("", "1234")
    );
    
    Assert.Contains("email", excepcion.Message);
}
```

---

## ?? Aserciones Comunes

```csharp
Assert.Equal(5, resultado);                    // Igualdad
Assert.NotEqual(5, resultado);                 // Desigualdad
Assert.True(booleano);                         // Verdadero
Assert.False(booleano);                        // Falso
Assert.Null(objeto);                           // Null
Assert.NotNull(objeto);                        // No null
Assert.Empty(coleccion);                       // Colección vacía
Assert.Single(coleccion);                      // Un elemento
Assert.Contains(item, coleccion);              // Item en colección
Assert.DoesNotContain(item, coleccion);        // Item no en colección
Assert.Throws<ExcepcionTipo>(() => método()); // Excepción
Assert.IsType<Tipo>(objeto);                   // Tipo específico
Assert.IsAssignableFrom<Base>(objeto);         // Herencia
```

---

## ?? Moq: Basics

### Crear Mock

```csharp
using Moq;

var mockRepo = new Mock<IUsuarioRepository>();
var mockService = mockRepo.Object;  // Usa mockService en lugar del repo real
```

### Setup Simple

```csharp
mockRepo
    .Setup(r => r.ObtenerPorIdAsync(1))
    .ReturnsAsync(new Usuario { Id = 1, Email = "test@mail.com" });

// Uso
var usuario = await mockRepo.Object.ObtenerPorIdAsync(1);
// usuario = Usuario { Id = 1, Email = "test@mail.com" }
```

### Setup con It.IsAny<T>

```csharp
// Para CUALQUIER parámetro
mockRepo
    .Setup(r => r.ObtenerPorEmailAsync(It.IsAny<string>()))
    .ReturnsAsync((Usuario?)null);

// Uso
var u1 = await mockRepo.Object.ObtenerPorEmailAsync("a@mail.com");
var u2 = await mockRepo.Object.ObtenerPorEmailAsync("b@mail.com");
// Ambos retornan null
```

### Setup con It.Is<T>

```csharp
// Para parámetros específicos (con predicado)
mockRepo
    .Setup(r => r.GuardarAsync(It.Is<Usuario>(u => u.Email == "test@mail.com")))
    .ReturnsAsync(true);

// Uso
var resultado = await mockRepo.Object.GuardarAsync(
    new Usuario { Email = "test@mail.com", ... }
);
// resultado = true

var resultado2 = await mockRepo.Object.GuardarAsync(
    new Usuario { Email = "otro@mail.com", ... }
);
// resultado2 = null (no configurado)
```

### Verify (Verificar Interacciones)

```csharp
// Llamado exactamente una vez
mockRepo.Verify(r => r.ObtenerPorIdAsync(1), Times.Once);

// Llamado exactamente 3 veces
mockRepo.Verify(r => r.GuardarAsync(It.IsAny<Usuario>()), Times.Exactly(3));

// Nunca llamado
mockRepo.Verify(r => r.EliminarAsync(It.IsAny<int>()), Times.Never);

// Al menos una vez
mockRepo.Verify(r => r.ObtenerTodosAsync(), Times.AtLeastOnce);

// Con parámetro específico
mockRepo.Verify(r => r.ObtenerPorEmailAsync("test@mail.com"), Times.Once);
```

### SetupSequence (Múltiples llamadas)

```csharp
mockRepo
    .SetupSequence(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
    .ReturnsAsync(new Usuario { Id = 1 })
    .ReturnsAsync((Usuario?)null)
    .ReturnsAsync(new Usuario { Id = 2 });

// Primer call retorna Usuario { Id = 1 }
// Segundo call retorna null
// Tercer call retorna Usuario { Id = 2 }
```

### Setup que Lanza Excepción

```csharp
mockRepo
    .Setup(r => r.ObtenerPorIdAsync(-1))
    .ThrowsAsync(new ArgumentException("ID inválido"));

// Uso
await Assert.ThrowsAsync<ArgumentException>(() =>
    mockRepo.Object.ObtenerPorIdAsync(-1)
);
```

---

## ?? Test Completo (Ejemplo Real)

```csharp
[Fact]
public async Task RegistrarUsuario_ConDatosValidos_GuardaYRetornaTrue()
{
    // ? Arrange: Setup del mock
    var mockRepo = new Mock<IUsuarioRepository>();
    
    // Setup 1: ObtenerPorEmailAsync retorna null (email no existe)
    mockRepo
        .Setup(r => r.ObtenerPorEmailAsync("nuevo@mail.com"))
        .ReturnsAsync((Usuario?)null);
    
    // Setup 2: GuardarAsync retorna true
    mockRepo
        .Setup(r => r.GuardarAsync(It.IsAny<Usuario>()))
        .ReturnsAsync(true);

    var servicio = new UsuarioService(mockRepo.Object);

    // ? Act
    var resultado = await servicio.RegistrarAsync(
        "nuevo@mail.com", 
        "password123", 
        "Nuevo Usuario"
    );

    // ? Assert
    Assert.True(resultado);

    // ? Verify
    // El repositorio fue consultado para verificar email duplicado
    mockRepo.Verify(r => r.ObtenerPorEmailAsync("nuevo@mail.com"), Times.Once);
    
    // GuardarAsync fue llamado con un usuario que tiene el email correcto
    mockRepo.Verify(
        r => r.GuardarAsync(It.Is<Usuario>(u => u.Email == "nuevo@mail.com")),
        Times.Once
    );
}
```

---

## ??? Estructura Archivo de Tests

```csharp
using Moq;
using Practica9.Models;
using Practica9.Services;
using Xunit;

namespace Practica9.Tests;

/// <summary>
/// Tests para UsuarioService usando Moq
/// </summary>
public class UsuarioServiceMoqTests
{
    #region Tests de Autenticación

    [Fact]
    public async Task AutenticarAsync_CredencialesValidas_RetornaTrue()
    {
        // Test aquí...
    }

    #endregion

    #region Tests de Registro

    [Fact]
    public async Task RegistrarAsync_DatosValidos_RetornaTrue()
    {
        // Test aquí...
    }

    #endregion
}
```

---

## ?? Comando de Ejecución

```bash
# Todos los tests
dotnet test

# Filter por clase
dotnet test --filter "UsuarioServiceMoqTests"

# Filter por nombre
dotnet test --filter "AutenticarAsync_CredencialesValidas"

# Con verbosidad
dotnet test --verbosity detailed

# Ejecutar y guardar resultado
dotnet test > test-results.txt 2>&1
```

---

## ?? Fake Rápido

```csharp
public class UsuarioRepositoryFake : IUsuarioRepository
{
    private readonly List<Usuario> _usuarios = new()
    {
        new Usuario { Id = 1, Email = "test@mail.com", Password = "1234" }
    };

    public Task<Usuario?> ObtenerPorEmailAsync(string email) =>
        Task.FromResult(_usuarios.FirstOrDefault(u => u.Email == email));

    public Task<bool> GuardarAsync(Usuario usuario)
    {
        _usuarios.Add(usuario);
        return Task.FromResult(true);
    }

    // ... resto de métodos ...
}
```

**Uso:**

```csharp
[Fact]
public async Task Test_ConFake()
{
    var fake = new UsuarioRepositoryFake();
    var servicio = new UsuarioService(fake);

    var resultado = await servicio.AutenticarAsync("test@mail.com", "1234");

    Assert.True(resultado);
}
```

---

## ?? Errores Comunes

### ? NO hagas esto:

```csharp
// Bloquea thread
var resultado = metodoAsync().Result;

// Espera innecesaria
await Task.Delay(1000);

// .Result + espera
var task = metodoAsync();
task.Wait();

// Múltiples asserts sin propósito
[Fact]
public void TodoEnUno() // ? MAL
{
    Assert.Equal(5, calc.Sumar(2, 3));
    Assert.Equal(6, calc.Multiplicar(2, 3));
    Assert.Throws<Exception>(() => calc.Dividir(1, 0));
}

// Nombre genérico
[Fact]
public void Test1() { } // ? MAL
```

### ? HAZ esto:

```csharp
// Usa await
var resultado = await metodoAsync();

// Sin delays
// (fixture para setup costoso si es necesario)

// Un assert por propósito
[Fact]
public void Sumar_2Y3_Retorna5() { Assert.Equal(5, calc.Sumar(2, 3)); }

[Fact]
public void Multiplicar_2Y3_Retorna6() { Assert.Equal(6, calc.Multiplicar(2, 3)); }

[Fact]
public void Dividir_1Y0_LanzaExcepcion() { Assert.Throws<Exception>(...); }

// Nombre descriptivo
[Fact]
public void AutenticarAsync_CredencialesInvalidas_RetornaFalse() { }
```

---

## ?? Referencias

- **xUnit Doc:** https://xunit.net/docs/getting-started
- **Moq Doc:** https://github.com/moq/moq4/wiki
- **Assert methods:** https://xunit.net/docs/api/inlinedata

---

**Última actualización:** 2024  
**Versión:** 1.0  
**Uso:** Copiar/pegar en clase