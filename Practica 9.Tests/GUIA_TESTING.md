# Práctica 9: Testing Unitario y Mocks con xUnit

**Unidad:** 3–4  
**Fecha:** 27/10  
**Modalidad:** Asincrónica (Guía + Ejercicios)  
**Objetivo:** Aprender a escribir pruebas unitarias con xUnit, usar fakes y Moq, y aplicar buenas prácticas de testing.

---

## ?? Tabla de Contenidos

1. [Conceptos Clave](#conceptos-clave)
2. [¿Por qué testear?](#por-qué-testear)
3. [Paso a paso: Crear proyecto de tests](#paso-a-paso-crear-proyecto-de-tests)
4. [Estructura AAA](#estructura-aaa)
5. [Buenas Prácticas](#buenas-prácticas)
6. [Fakes vs Moq](#fakes-vs-moq)
7. [Ejecutar y Ver Resultados](#ejecutar-y-ver-resultados)
8. [Ejercicios y Consignas](#ejercicios-y-consignas)
9. [Preguntas para el Foro](#preguntas-para-el-foro)

---

## Conceptos Clave

### ¿Qué es un test unitario?

Un **test unitario** es una pieza de código que verifica el comportamiento de un método o clase de forma **aislada y automatizada**.

- **Aislado**: no accede a BD, red, filesystem — usa substitutos ("mocks", "fakes")
- **Automatizado**: se ejecuta con `dotnet test`; no requiere intervención manual
- **Rápido**: idealmente < 1 segundo cada test
- **Determinístico**: siempre da el mismo resultado con los mismos datos

### Frameworks

- **xUnit**: framework moderno para writing and running tests (.NET)
- **Moq**: librería para generar **mocks dinámicos** (substitutos inteligentes)
- **FluentAssertions** (opcional): aserciones más legibles

---

## ¿Por qué testear?

### Ejemplo 1: Cambios accidentales

```csharp
// Cambias una fórmula de cálculo sin tests
public decimal CalcularImpuesto(decimal monto)
{
    return monto * 0.21; // Fue *, ahora pones /
}
```

Sin tests, el bug llega a producción. Con tests, ? Test falla ? detectas el bug antes.

### Ejemplo 2: Refactorización segura

```csharp
// Refactoras código por rendimiento.
// Tests verifican que el comportamiento NO cambió.
public IEnumerable<Usuario> ObtenerActivos()
{
    // Antes: LINQ query
    // Después: Algoritmo O(1) en cache
    // Pero los tests pasan ? funciona igual ?
}
```

### Ejemplo 3: Documentación ejecutable

Los tests son documentación viva:

```csharp
[Fact]
public void AutenticarAsync_ConCredencialesValidas_RetornaTrue()
{
    // Lee como: "Autenticar con credenciales válidas retorna true"
}
```

---

## Paso a paso: Crear Proyecto de Tests

### 1. Crear proyecto xUnit

```bash
# Desde la carpeta raíz de tu solución
dotnet new xunit -n Practica9.Tests
```

Esto crea:
- `Practica9.Tests.csproj` con referencias a xUnit
- carpeta `bin/` y `obj/`
- archivo `UnitTest1.cs` (plantilla)

### 2. Agregar referencia al proyecto principal

```bash
cd Practica9.Tests
dotnet add reference ../Practica9/Practica9.csproj
```

Esto permite que los tests usen código de `Practica9`.

### 3. Instalar Moq (opcional, pero recomendado)

```bash
dotnet add package Moq
```

### 4. Agregar a la solución

```bash
cd ..
dotnet sln add Practica9.Tests/Practica9.Tests.csproj
```

Verificar:

```bash
dotnet sln list
```

Deberías ver:
```
Practica 9\Practica 9.csproj
Practica 9.Tests\Practica 9.Tests.csproj
```

---

## Estructura AAA

Todo test sigue el patrón **Arrange-Act-Assert**:

```csharp
[Fact]
public void Sumar_5Y3_Retorna8()
{
    // ? ARRANGE: preparar datos
    var calc = new CalculadoraService();
    int a = 5;
    int b = 3;

    // ? ACT: ejecutar el método
    int resultado = calc.Sumar(a, b);

    // ? ASSERT: verificar
    Assert.Equal(8, resultado);
}
```

### ¿Por qué AAA?

- **Claridad**: lector entiende qué se testa en 3 pasos
- **Reutilización**: puedes copiar Arrange para otro test
- **Debugging**: si falla, sabes en qué etapa ocurrió

---

## Buenas Prácticas

### 1. Nombrado de Tests

**Patrón recomendado:**

```
Método_Escenario_ResultadoEsperado
```

**Ejemplos:**

```csharp
// ? Bueno
public void Sumar_NumerosPositivos_RetornaResultadoCorrecto() { }
public void Dividir_DivisorCero_LanzaDivideByZeroException() { }
public void AutenticarAsync_CredencialesInvalidas_RetornaFalse() { }

// ? Malo
public void Test1() { }
public void SumarTest() { }
public void Verificar() { }
```

**Por qué:** el nombre es documentación. Dice qué se testa sin leer el código.

---

### 2. Un Test = Un Propósito

**? Bien:**

```csharp
[Fact]
public void Dividir_DivisorCero_LanzaExcepcion()
{
    Assert.Throws<DivideByZeroException>(() => calc.Dividir(10, 0));
}
```

**? Mal:**

```csharp
[Fact]
public void Dividir_MultiplesCasos() // ? Demasiadas cosas
{
    Assert.Equal(5, calc.Dividir(10, 2));
    Assert.Equal(3.33m, calc.Dividir(10, 3));
    Assert.Throws<DivideByZeroException>(() => calc.Dividir(10, 0));
}
```

Si uno falla, ¿cuál? No se sabe.

---

### 3. Evitar I/O en Unit Tests

**? NO HAGAS:**

```csharp
[Fact]
public async Task ObtenerUsuario_LeeDelFichero() // ? I/O
{
    var usuario = await service.ObtenerUsuarioAsync(); // Accede a BD real
}
```

**? HAZ:**

```csharp
[Fact]
public async Task ObtenerUsuario_ConRepositoryMock_RetornaUsuario()
{
    var mockRepo = new Mock<IUsuarioRepository>();
    mockRepo.Setup(r => r.ObtenerAsync(1))
            .ReturnsAsync(new Usuario { Id = 1, Email = "test@mail.com" });
    
    var service = new UsuarioService(mockRepo.Object);
    var usuario = await service.ObtenerAsync(1);
    
    Assert.NotNull(usuario);
}
```

**Por qué:**
- Tests sin I/O son rápidos (< 1ms vs segundos con BD)
- Determinísticos: no dependen de estado externo
- Paralelizable: 1000 tests en < 1 segundo

---

### 4. Tests Asincronos

**Sintaxis:**

```csharp
[Fact]
public async Task AutenticarAsync_CredencialesValidas_RetornaTrue()
{
    // Arrange
    var service = new UsuarioService(mockRepo.Object);

    // Act
    bool resultado = await service.AutenticarAsync("test@mail.com", "1234");

    // Assert
    Assert.True(resultado);
}
```

**?? No hagas esto:**

```csharp
// ? MALO: bloquea el thread
bool resultado = service.AutenticarAsync("test@mail.com", "1234").Result;

// ? MALO: espera sin beneficio
Task.Delay(1000).Wait();
```

**? BIEN:**

```csharp
// Usa await
var resultado = await service.AutenticarAsync(...);
```

---

### 5. Assert.ThrowsAsync para excepciones en async

```csharp
[Fact]
public async Task AutenticarAsync_ConEmailVacio_LanzaArgumentException()
{
    // Act & Assert
    var excepcion = await Assert.ThrowsAsync<ArgumentException>(() =>
        service.AutenticarAsync("", "1234")
    );

    // Verificar el mensaje
    Assert.Contains("email", excepcion.Message, StringComparison.OrdinalIgnoreCase);
}
```

---

## Fakes vs Moq

### Fakes (Implementación Manual)

```csharp
public class UsuarioRepositoryFake : IUsuarioRepository
{
    private readonly List<Usuario> _usuarios = new()
    {
        new Usuario { Id = 1, Email = "test@mail.com", Password = "1234" }
    };

    public Task<Usuario?> ObtenerPorEmailAsync(string email) =>
        Task.FromResult(_usuarios.FirstOrDefault(u => u.Email == email));

    // ... resto de métodos ...
}
```

**Uso:**

```csharp
[Fact]
public async Task AutenticarAsync_ConFake_RetornaTrue()
{
    var repo = new UsuarioRepositoryFake();
    var service = new UsuarioService(repo);

    var resultado = await service.AutenticarAsync("test@mail.com", "1234");

    Assert.True(resultado);
}
```

**Ventajas:**
- Completamente bajo control
- Fácil de debuggear
- Determinístico

**Desventajas:**
- Código repetitivo
- Mantenimiento si interfaz cambia
- Puede duplicar lógica

---

### Moq (Mocks Dinámicos)

```csharp
[Fact]
public async Task AutenticarAsync_ConMoq_RetornaTrue()
{
    // Arrange: crear mock
    var mockRepo = new Mock<IUsuarioRepository>();
    
    // Setup: definir comportamiento
    mockRepo
        .Setup(r => r.ObtenerPorEmailAsync("test@mail.com"))
        .ReturnsAsync(new Usuario { Email = "test@mail.com", Password = "1234" });

    var service = new UsuarioService(mockRepo.Object);

    // Act
    bool resultado = await service.AutenticarAsync("test@mail.com", "1234");

    // Assert
    Assert.True(resultado);
    
    // Verify: asegurar interacción
    mockRepo.Verify(r => r.ObtenerPorEmailAsync("test@mail.com"), Times.Once);
}
```

**Ventajas:**
- Menos código
- Flexible: cambias Setup sin modificar clase fake
- Verify: aseguras que métodos fueron llamados correctamente

**Desventajas:**
- Puede ser "mágico" para principiantes
- Más lento que fakes (aunque imperceptible)

---

### ¿Cuándo usar cada uno?

| Caso | Recomienda |
|------|-----------|
| Interfaz simple (1–2 métodos) | Fake |
| Interfaz compleja | Moq |
| Quieres verificar interacciones | Moq |
| Necesitas lógica custom en el mock | Fake |
| Tests rápidos y simples | Fake |

**Regla general:** Moq para la mayoría de casos reales.

---

## Ejecutar y Ver Resultados

### Ejecutar todos los tests

```bash
dotnet test
```

Salida esperada:

```
  Determining projects to restore...
  Restored /.../Practica9.Tests/Practica9.Tests.csproj (in 123 ms)
  Building [========================================] 100% /.../Practica9.Tests/Practica9.Tests.csproj

Test run for /.../Practica9.Tests/bin/Debug/net9.0/Practica9.Tests.dll (.NETCoreApp,Version=v9.0)
Microsoft.VisualStudio.TestPlatform.TestExecutor.Core
  CalculadoraServiceTests.Sumar_5Y3_Retorna8 [OK (1 ms)]
  CalculadoraServiceTests.Sumar_NumerosNegativos_RetornaResultadoNegativo [OK (1 ms)]
  UsuarioServiceFakeTests.AutenticarAsync_CredencialesValidas_RetornaTrue [OK (15 ms)]
  UsuarioServiceMoqTests.AutenticarAsync_CredencialesValidas_RetornaTrue_ConMoq [OK (12 ms)]
  ...

Test Results: Passed: 50 Failed: 0 Skipped: 0
Duration: 234 ms
```

### Ejecutar un test específico

```bash
dotnet test --filter "CalculadoraServiceTests"
```

### Ejecutar con verbosidad

```bash
dotnet test --verbosity detailed
```

### Generar reporte de cobertura (opcional)

```bash
dotnet test /p:CollectCoverageMetrics=true
```

---

## Ejercicios y Consignas

### Consigna 1: Implementar 4 tests básicos (20 min)

Crea un archivo `CalculadoraTests.cs` con:

1. **Test 1:** `Sumar_5Y3_Retorna8()`
   - Arrange: calc = new, a = 5, b = 3
   - Act: resultado = calc.Sumar(5, 3)
   - Assert: Assert.Equal(8, resultado)

2. **Test 2:** `Restar_10Y3_Retorna7()`
   - Similar, pero para método Restar

3. **Test 3:** `Dividir_10Y0_LanzaExcepcion()`
   - Usa `Assert.Throws<DivideByZeroException>()`

4. **Test 4:** `ObtenerPromedio_1y2y3_Retorna2()`
   - Input: [1, 2, 3]
   - Output: 2

**Ejecuta:**

```bash
dotnet test
```

Captura la salida y envía screenshot.

---

### Consigna 2: Usar Fake (15 min)

Crea `UsuarioServiceFakeTests.cs`:

1. Copia la clase `UsuarioRepositoryFake` (ver arriba)
2. Escribe un test: `AutenticarAsync_ConFake_CredencialesValidas_RetornaTrue()`
3. Verifica que el usuario fue guardado en el fake

**Expectativa:**

```csharp
[Fact]
public async Task AutenticarAsync_ConFake_CredencialesValidas_RetornaTrue()
{
    var fake = new UsuarioRepositoryFake();
    var service = new UsuarioService(fake);

    var resultado = await service.AutenticarAsync("test@mail.com", "1234");

    Assert.True(resultado);
}
```

---

### Consigna 3: Usar Moq (15 min)

Crea `UsuarioServiceMoqTests.cs`:

1. Escribe el mismo test, pero con `Moq`:

```csharp
[Fact]
public async Task AutenticarAsync_ConMoq_CredencialesValidas_RetornaTrue()
{
    var mockRepo = new Mock<IUsuarioRepository>();
    mockRepo
        .Setup(r => r.ObtenerPorEmailAsync("test@mail.com"))
        .ReturnsAsync(new Usuario { Email = "test@mail.com", Password = "1234", Activo = true });

    var service = new UsuarioService(mockRepo.Object);
    var resultado = await service.AutenticarAsync("test@mail.com", "1234");

    Assert.True(resultado);
    mockRepo.Verify(r => r.ObtenerPorEmailAsync("test@mail.com"), Times.Once);
}
```

2. Ejecuta y verifica que pasa.

---

### Consigna 4: Tests con modelo (10 min)

Crea `ProductoTests.cs`:

```csharp
[Fact]
public void CalcularTotal_10x5_Retorna50()
{
    var producto = new Producto { Precio = 10m, Cantidad = 5 };
    Assert.Equal(50m, producto.CalcularTotal());
}

[Fact]
public void AplicarDescuento_100con10Porciento_Retorna90()
{
    var producto = new Producto { Precio = 100m };
    var resultado = producto.AplicarDescuento(10m);
    Assert.Equal(90m, resultado);
}

[Fact]
public void AplicarDescuento_MayorA100_LanzaExcepcion()
{
    var producto = new Producto { Precio = 100m };
    Assert.Throws<ArgumentException>(() => producto.AplicarDescuento(150m));
}
```

---

### Consigna Final: Captura de dotnet test

```bash
dotnet test > test-results.txt 2>&1
```

Envía el archivo `test-results.txt` como evidencia.

---

## Preguntas para el Foro

### P1: Nombrado de Tests

**Publicar en el foro:**

Escribe el nombre correcto para un test que:
- Prueba el método `CalcularTotalCarrito()`
- Escenario: carrito con 3 productos
- Resultado esperado: suma correcta

Formato:

```
Mi test sería: ___________________

Explicación: ...
```

---

### P2: Fakes vs Moq

**Discusión:**

1. ¿Cuándo usarías Fake vs Moq en tu proyecto?
2. ¿Qué ventajas ves en cada uno?

Comenta al menos 2 posts de compañeros.

---

### P3: Cobertura

**Pregunta:**

En tu código de producción, ¿cuáles son los métodos más críticos para testear? ¿Por qué?

Ejemplo:
- Métodos de validación (seguridad)
- Métodos de cálculo (precisión)
- Métodos de integración (confiabilidad)

---

## Recursos Adicionales

- [xUnit Documentation](https://xunit.net/)
- [Moq GitHub](https://github.com/moq/moq4)
- [Microsoft: Unit Testing Best Practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
- [Test-Driven Development (TDD)](https://en.wikipedia.org/wiki/Test-driven_development)

---

## Checklist de Revisión (para los ayudantes)

- [ ] 4 tests ejecutables y pasando
- [ ] Tests siguen patrón AAA
- [ ] Nombres descriptivos
- [ ] Uso correcto de Assert
- [ ] Al menos 1 test con Moq
- [ ] Al menos 1 test async
- [ ] Manejo de excepciones correcto
- [ ] Captura de `dotnet test` adjunta

---

**¡Éxito en tus tests! ??**