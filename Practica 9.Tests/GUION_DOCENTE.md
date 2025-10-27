# ?? Guion de Clase: Testing Unitario con xUnit y Moq

**Unidad:** 3–4  
**Fecha:** 27/10  
**Duración:** ~60 minutos (grabada/asincrónica)  
**Nivel:** Principiante a Intermedio

---

## ?? Estructura de la Clase

```
0:00–0:02   Intro (2 min)
0:02–0:08   ¿Por qué testear? (6 min)
0:08–0:18   xUnit: Setup + AAA (10 min)
0:18–0:25   Buenas prácticas (7 min)
0:25–0:35   Aislamiento con Fakes (10 min)
0:35–0:45   Moq y Verify (10 min)
0:45–0:50   Tests asincronos (5 min)
0:50–0:55   Fixtures (5 min)
0:55–1:00   Cierre + Actividades (5 min)
```

---

## ?? Narración Minuto a Minuto

### 0:00–0:02 — Intro (2 min)

**Narración (tono didáctico, pausado):**

> "Hola. En este módulo vamos a ver cómo escribir **pruebas unitarias con xUnit** y cómo **aislar dependencias con fakes y mocks**. Esto no es solo teoría: al terminar, verás cómo crear un proyecto de tests, ejecutar pruebas y garantizar que tu lógica de negocio sea verificable y segura ante refactorizaciones. La clase es de apoyo: la Unidad 4 tiene el detalle técnico y ejercicios adicionales. Aquí vamos a practicar paso a paso."

**Acción en pantalla:**
- Título: "Testing Unitario: xUnit + Moq"
- Bullets:
  - ? Conceptos clave de unit testing
  - ? Crear tests con xUnit
  - ? Aislar dependencias (Fakes y Moq)
  - ? Buenas prácticas y ejemplos reales
- Mostrar el IDE abierto con la solución

**Transición:** "Empecemos preguntándonos: ¿por qué molestarse en testear?"

---

### 0:02–0:08 — ¿Por qué Testear? (6 min)

**Narración:**

> "Imagina dos historias. 
>
> **Historia 1: Cambias una fórmula de cálculo.** 
> Sin tests, el bug llega a producción — cliente realiza 500 ventas con descuento erróneo. ? 
> Con tests, la fórmula cambió, el test falla, lo ves en 5 segundos. ?
>
> **Historia 2: Refactorizas un algoritmo por rendimiento.**
> Sin tests, no sabes si mantuviste el comportamiento.
> Con tests, ejecutas `dotnet test` — todos pasan ? ? es seguro refactorizar.
>
> **Conclusión:** Los tests son una red de seguridad. Documentación viva. Alarma temprana."

**Acción:**
1. Mostrar código defectuoso (2–3 líneas):
```csharp
public decimal CalcularDescuento(decimal monto)
{
    return monto / 0.21;  // ? Bug: división en lugar de multiplicación
}
```

2. Preguntar: "¿Cuándo te enteras del bug? Respuesta: cuando te reclaman. ??"

3. Mostrar test que lo detecta:
```csharp
[Fact]
public void CalcularDescuento_100_Retorna21()
{
    var resultado = calculadora.CalcularDescuento(100m);
    Assert.Equal(21m, resultado);  // ? Falla inmediatamente ?
}
```

**Mini-tarea (pausa de 30 segundos):**

> "Pausa el video. Piensa en un método en tu código — p. ej., `CalcularTotal()` —. ¿Qué entradas serían críticas? (p. ej., precio negativo, cantidad cero). Anótalas; las usaremos en los tests. Continúa en 30 segundos..."

**Transición:** "Ahora veamos cómo empezar."

---

### 0:08–0:18 — xUnit: Setup + AAA (10 min)

**Narración paso a paso:**

> "Paso 1: Crear proyecto de tests."

**Acción:**

```bash
dotnet new xunit -n Practica9.Tests
dotnet sln add Practica9.Tests/Practica9.Tests.csproj
cd Practica9.Tests
dotnet add reference ../Practica9/Practica9.csproj
```

**Narración:**

> "¿Por qué proyecto separado? Tres razones:
> 1. **Aislamiento:** tests no contaminan código de producción.
> 2. **Dependencias:** tests necesitan xUnit, Moq; la app no.
> 3. **CI/CD:** en pipelines, tests se ejecutan por separado."

**Acción:** Mostrar estructura de carpetas:

```
Practica9/
??? Services/
??? Models/
??? ...

Practica9.Tests/  ? Nuevo proyecto
??? bin/
??? obj/
??? Practica9.Tests.csproj
??? UnitTest1.cs
```

**Narración: Patrón AAA**

> "Todo test sigue tres pasos: **Arrange-Act-Assert**. Veamos un ejemplo:"

**Acción:** Mostrar código comentado:

```csharp
[Fact]  // ? Fact: test sin parámetros
public void Sumar_5Y3_Retorna8()
{
    // ? ARRANGE: Preparar
    var calc = new CalculadoraService();
    int a = 5;
    int b = 3;

    // ? ACT: Ejecutar
    int resultado = calc.Sumar(a, b);

    // ? ASSERT: Verificar
    Assert.Equal(8, resultado);
}
```

**Narración:**

> "Arrange: prepara inputs. Act: ejecuta el método. Assert: verifica. Así de simple. Todos los tests siguen esto — léelos como una historia: 'Dado X, cuando hago Y, entonces espero Z'."

**Acción:** Ejecutar:

```bash
dotnet test
```

**Mostrar salida:**

```
CalculadoraServiceTests.Sumar_5Y3_Retorna8 [OK (1 ms)]

Test Results: Passed: 1 Failed: 0
```

**Narración:**

> "Verde. Pasó. ? Eso significa que el test verificó lo que esperaba."

**Acción:** Mostrar [Theory]:

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

**Narración:**

> "[Theory] es para datos parametrizados. Un test, múltiples escenarios. Ejecuta 3 veces — cada una con sus datos."

**Transición:** "Ahora que sabes la estructura, hablemos de nombres."

---

### 0:18–0:25 — Buenas Prácticas (7 min)

**Narración:**

> "Regla 1: **Nombrado claro**. Tu nombre de test es documentación. Cuando falla, alguien leerá `Sumar_NumerosNegativos_RetornaResultadoNegativo()` y dirá: 'Ah, testea negativos'. Así:
>
> ? Bien: `Autenticar_CredencialesInvalidas_LanzaExcepcion()`  
> ? Mal: `Test1()` o `VerificarAuth()`"

**Acción:** Mostrar tabla comparativa (visual):

```
? NOMBRES BUENOS              ? NOMBRES MALOS
Sumar_5Y3_Retorna8           Sumar()
Dividir_DivisorCero_Lanza    DividirTest()
RegistrarAsync_EmailDuplicado_Falla  Test_Register
```

**Narración:**

> "Regla 2: **Un test = un propósito**. Si 3 asserts fallan, ¿cuál fue el problema? No se sabe. Separa."

**Acción:** Mostrar código:

```csharp
// ? MAL
[Fact]
public void Dividir_MultiplesCasos()
{
    Assert.Equal(5, calc.Dividir(10, 2));
    Assert.Equal(3.33m, calc.Dividir(10, 3));
    Assert.Throws<DivideByZeroException>(() => calc.Dividir(10, 0));
}

// ? BIEN
[Fact]
public void Dividir_10Entre2_Retorna5() => Assert.Equal(5, calc.Dividir(10, 2));

[Fact]
public void Dividir_DivisorCero_LanzaExcepcion() => 
    Assert.Throws<DivideByZeroException>(() => calc.Dividir(10, 0));
```

**Narración:**

> "Regla 3: **Evita I/O en unit tests**. No accedas a BD, archivos, red. ¿Por qué? Son lentos (ms) y no determinísticos (fallan a veces). En su lugar: **fakes y mocks**."

**Acción:** Mostrar comparación:

```csharp
// ? NO: accede a BD real
[Fact]
public async Task GetUser_ReadsFromDatabase() { ... }  // Lento, frágil

// ? SÍ: usa mock, rápido y controlado
[Fact]
public async Task GetUser_WithMock_RetornsUser() { ... }  // < 1ms, determinístico
```

**Transición:** "Hablemos de cómo aislar dependencias."

---

### 0:25–0:35 — Aislamiento con Fakes (10 min)

**Narración:**

> "Aquí viene lo bueno: **aislar dependencias**. Tenemos dos métodos: Fakes y Moq. Empecemos con Fakes — es más simple."

**Acción:** Mostrar interfaz:

```csharp
public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorEmailAsync(string email);
}
```

**Narración:**

> "¿Qué es esto? Es un contrato. Dice 'quien implemente esto debe tener este método'. Ahora, en producción, usas una implementación con BD real. En tests, usas un fake — una implementación fake."

**Acción:** Mostrar fake:

```csharp
public class UsuarioRepositoryFake : IUsuarioRepository
{
    private readonly List<Usuario> _usuarios = new()
    {
        new Usuario { Email = "test@mail.com", Password = "1234" }
    };

    public Task<Usuario?> ObtenerPorEmailAsync(string email) =>
        Task.FromResult(_usuarios.FirstOrDefault(u => u.Email == email));
}
```

**Narración:**

> "Es BD en memoria. Nada de red, nada de discos. Solo datos. Rápido, determinístico. Ahora usa este fake en un test:"

**Acción:** Mostrar test:

```csharp
[Fact]
public async Task AutenticarAsync_CredencialesValidas_RetornaTrue()
{
    var repoFake = new UsuarioRepositoryFake();
    var servicio = new UsuarioService(repoFake);

    var resultado = await servicio.AutenticarAsync("test@mail.com", "1234");

    Assert.True(resultado);
}
```

**Narración:**

> "Fíjate: inyecta el fake en el constructor. El servicio no sabe si es fake o real — solo sabe que es `IUsuarioRepository`. Eso es inyección de dependencias. Y eso es testeable."

**Acción:** Ejecutar:

```bash
dotnet test --filter "UsuarioServiceFakeTests"
```

**Mini-tarea (1 min):**

> "Pausa. Implementa el fake completo. Luego ejecuta `dotnet test`. Continúa cuando veas 'Passed'."

**Transición:** "Los Fakes funcionan, pero hay forma de hacerlo más fácil: **Moq**."

---

### 0:35–0:45 — Moq: Mocks Dinámicos (10 min)

**Narración:**

> "Moq es un framework que genera mocks automáticamente. No escribes 10 líneas de fake — Moq lo hace por ti. Veamos:"

**Acción:** Instalar Moq:

```bash
dotnet add package Moq
```

**Narración:**

> "Ahora el mismo test, pero con Moq:"

**Acción:** Mostrar código:

```csharp
using Moq;

[Fact]
public async Task AutenticarAsync_CredencialesValidas_RetornaTrue_ConMoq()
{
    // ? Crear mock
    var mockRepo = new Mock<IUsuarioRepository>();
    
    // ? Setup: qué debe retornar
    mockRepo
        .Setup(r => r.ObtenerPorEmailAsync("test@mail.com"))
        .ReturnsAsync(new Usuario { Email = "test@mail.com", Password = "1234", Activo = true });

    // ? Usar el mock
    var servicio = new UsuarioService(mockRepo.Object);
    var resultado = await servicio.AutenticarAsync("test@mail.com", "1234");

    // ? Assert
    Assert.True(resultado);
    
    // ? Verify: verificar que fue llamado
    mockRepo.Verify(r => r.ObtenerPorEmailAsync("test@mail.com"), Times.Once);
}
```

**Narración:**

> "Cinco pasos:
> 1. Crea mock con `new Mock<IInterfaz>()`
> 2. Setup: define qué retorna
> 3. Crea servicio con `mockRepo.Object`
> 4. Act y Assert
> 5. Verify: asegura que el método fue llamado (opcional pero útil)
>
> Advantage sobre Fake: menos código. Setup es flexible."

**Acción:** Mostrar Verify más complicado:

```csharp
// Verify con It.IsAny<T>: para cualquier parámetro
mockRepo.Verify(r => r.ObtenerPorEmailAsync(It.IsAny<string>()), Times.Once);

// Verify con It.Is<T>: para parámetros específicos
mockRepo.Verify(r => r.GuardarAsync(It.Is<Usuario>(u => u.Email == "nuevo@mail.com")), Times.Once);

// Verify con Times
Times.Once         // exactamente 1 vez
Times.Never        // nunca fue llamado
Times.Exactly(3)   // exactamente 3 veces
Times.AtLeastOnce  // al menos 1 vez
```

**Narración:**

> "Con Verify, aseguras que tu lógica interactuó con las dependencias como esperabas. ¿El servicio llamó al repositorio? ¿Cuántas veces? Verify te lo dice."

**Transición:** "Ahora que sabes Setup y Verify, hablemos de async."

---

### 0:45–0:50 — Tests Asincronos (5 min)

**Narración:**

> "Muchos métodos en .NET Core son async. ¿Cómo testearlos?"

**Acción:** Mostrar:

```csharp
[Fact]
public async Task AutenticarAsync_CredencialesValidas_RetornaTrue()
{
    // Arrange
    var service = new UsuarioService(mockRepo.Object);

    // Act: NOTA: await
    bool resultado = await service.AutenticarAsync("test@mail.com", "1234");

    // Assert
    Assert.True(resultado);
}
```

**Narración:**

> "El test es `async Task`. Usas `await`. Simple. Pero ?? no hagas esto:"

**Acción:** Mostrar anti-patrones:

```csharp
// ? MALO: bloquea el thread
var resultado = service.AutenticarAsync("test@mail.com", "1234").Result;

// ? MALO: espera innecesaria
await Task.Delay(1000);
Assert.True(resultado);
```

**Narración:**

> ".Result bloquea. Task.Delay ralentiza tests. Usa await."

**Acción:** Mostrar excepciones async:

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

**Narración:**

> "`Assert.ThrowsAsync` para excepciones en async. Es idéntico a `Assert.Throws` pero con await."

---

### 0:50–0:55 — Fixtures (5 min)

**Narración:**

> "Cuando escribes muchos tests, repites código. Fixtures te ayudan a reutilizar setup."

**Acción:** Mostrar fixture simple:

```csharp
public class DbFixture : IDisposable
{
    private readonly List<Usuario> _usuarios;

    public DbFixture()
    {
        _usuarios = new() { /* cargar datos iniciales */ };
    }

    public void Dispose()
    {
        _usuarios.Clear(); // Cleanup
    }
}

public class UsuarioTests : IClassFixture<DbFixture>
{
    private readonly DbFixture _fixture;

    public UsuarioTests(DbFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Test1()
    {
        // _fixture ya está inicializado
        var usuario = _fixture._usuarios.First();
        Assert.NotNull(usuario);
    }
}
```

**Narración:**

> "Fixture se crea una vez por clase de tests. Si tienes 5 tests, Fixture.__ctor se ejecuta 1 vez. Útil para setup costoso. Cuidado: evita estado compartido."

---

### 0:55–1:00 — Cierre + Actividades (5 min)

**Narración:**

> "Resumen:
> - Unit tests son red de seguridad
> - xUnit + AAA para estructura
> - Fakes para simular dependencias simples
> - Moq para simples complejas
> - Tests async con await
> - Fixtures para setup reutilizable
>
> En la próxima clase: CI/CD — incorporar tests en GitHub Actions."

**Acción:** Mostrar consignas en pantalla:

```
ACTIVIDADES (entrega en 1 semana):

1??  4 tests en Practica9.Tests:
   - Sumar (unitario)
   - Dividir (caso cero ? exception)
   - AutenticarAsync (con fake)
   - AutenticarAsync (con Moq + Verify)

2??  Ejecuta: dotnet test
   Captura resultado (Screenshot)

3??  En el foro:
   - Publica nombre de un test + explicación
   - Comenta 2 posts de compañeros
```

**Narración final:**

> "El código está en el repositorio. La Unidad 4 tiene referencia de xUnit y Moq. Si tienen dudas: foro. ¡Adelante! ??"

---

## ?? Consejos de Grabación

### Producción

- **Clips cortos:** Divide la clase en 5–6 clips de 8–10 min (fácil editar, subtítulos, caché)
- **Subtítulos:** Agrega comandos y código clave
- **Pausa visual:** Cuando pidas micro-actividad, congela pantalla 30 segundos
- **Terminal:** Aumenta tamaño de fuente (zoom 150%)
- **Micrófono:** Habla pausado, claro, tono didáctico

### Edición

- Color verde para ? tests pasados
- Color rojo para ? tests fallidos
- Zoom en código relevante
- Mute de ruido de teclado

### Descripción Video

```
?? Práctica 9: Testing Unitario con xUnit y Moq

En este video aprenderás:
? Conceptos de unit testing
? Crear proyecto de tests
? Patrón AAA (Arrange-Act-Assert)
? Tests con xUnit ([Fact], [Theory])
? Aislar dependencias con Fakes
? Mocks dinámicos con Moq
? Tests asincronos
? Buenas prácticas

Duración: 60 min
Nivel: Principiante

?? Recursos:
- Código: github.com/...
- Unidad 4: [enlace]
- xUnit Docs: https://xunit.net/

?? Objetivos:
Al terminar, podrás escribir tests unitarios profesionales.

#dotnet #testing #csharp #educación
```

---

## ? Checklist Pre-Grabación

- [ ] Practica9.Tests proyecto creado y compila
- [ ] Todos los tests pasan: `dotnet test`
- [ ] IDE: VS Code o Visual Studio
- [ ] Terminal: PowerShell / Bash, fuente grande
- [ ] Cámara/Micrófono: testeo de audio
- [ ] Slides preparadas (título, bullets, imágenes)
- [ ] Guion memorizado o al lado (papel)
- [ ] Grabador de pantalla (OBS, ScreenFlow)
- [ ] Café ?

---

## ?? Rúbrica de Evaluación (para ayudantes)

### Para el estudiante

| Criterio | Excelente | Bueno | Regular | Deficiente |
|----------|-----------|-------|---------|-----------|
| **Tests ejecutables** | 4+ tests, todos pasan | 4 tests, 1 falla | 2–3 tests | 0–1 tests |
| **Nombrado** | Patrón Método_Escenario_Resultado | Descriptivo pero imperfecto | Confuso | Genérico (Test1) |
| **AAA** | 3 bloques claros | 2 bloques presentes | Parcial | No identificable |
| **Moq/Fake** | Setup y Verify correcto | Setup correcto | Setup incompleto | No usado |
| **Async** | await correcto | Task pero sin await | .Result usado | No async |
| **Excepciones** | Assert.Throws[Async] | Assert.Throws sin msg | Try/catch | No testeado |
| **Evidencia** | Screenshot `dotnet test` | Parcial | Falta datos | No presente |

### Puntuación

- Excelente: 90–100
- Bueno: 75–89
- Regular: 60–74
- Deficiente: < 60

---

**¡Listo para grabar! Éxito. ??**