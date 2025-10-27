# ?? PRÁCTICA 9: Testing Unitario y Mocks — Resumen Ejecutivo

**Estado:** ? Proyecto completo y listo para clase  
**Fecha:** 27/10 (Unidades 3–4)  
**Tipo:** Asincrónica (grabada/guía educativa)  
**Duración:** 60 min clase + ejercicios

---

## ?? ¿Qué Se Entrega?

### Estructura de Proyectos

```
Practica 9/                          ? Proyecto MAUI principal
??? Models/
?   ??? Usuario.cs                   # Models testables
??? Services/
?   ??? UsuarioService.cs            # Servicios con interfaces
??? MainPage.xaml(.cs)               # UI (opcional para testing)

Practica 9.Tests/                    ? Proyecto de TESTS NUEVO
??? CalculadoraServiceTests.cs       # Tests básicos (11 tests)
??? ProductoTests.cs                 # Tests de modelos (12 tests)
??? UsuarioServiceFakeTests.cs       # Tests con Fakes (13 tests)
??? UsuarioServiceMoqTests.cs        # Tests con Moq (10 tests)
??? Practica 9.Tests.csproj          # Configuración (xUnit, Moq)
??? README.md                        # Guía de ejecución
??? GUIA_TESTING.md                  # Documentación pedagógica completa
??? GUION_DOCENTE.md                 # Guion de clase (minuto a minuto)
```

**Total:** 46 tests + documentación + guion docente

---

## ?? Objetivos Logrados

Al completar esta práctica, el estudiante podrá:

? Crear un proyecto de tests con xUnit  
? Escribir tests unitarios (patrón AAA)  
? Testear métodos síncronos y asincronos  
? Usar [Fact] y [Theory] + [InlineData]  
? Testear excepciones  
? Aislar dependencias con Fakes  
? Usar Moq para mocks dinámicos  
? Setup y Verify con Moq  
? Nombrar tests correctamente  
? Ejecutar tests y leer resultados  

---

## ?? Archivos Clave

### Para el Estudiante

| Archivo | Propósito | Tiempo |
|---------|-----------|--------|
| `README.md` | Guía de inicio rápido | 5 min |
| `GUIA_TESTING.md` | Documentación completa + ejemplos | 30 min |
| Tests `.cs` | Código para copiar y aprender | 45 min |

### Para el Docente

| Archivo | Propósito | Tiempo |
|---------|-----------|--------|
| `GUION_DOCENTE.md` | Script de grabación (0:00–1:00) | 60 min |
| `.csproj` | Configuración de dependencias | Setup único |

---

## ?? Cómo Ejecutar

### Verificar compilación

```bash
dotnet build
```

**Esperado:** Sin errores

### Ejecutar todos los tests

```bash
dotnet test
```

**Esperado:**

```
Test Results: Passed: 46  Failed: 0  Skipped: 0
Duration: 500 ms
```

### Ejecutar tests específicos

```bash
# Solo Calculadora
dotnet test --filter "CalculadoraServiceTests"

# Solo Moq
dotnet test --filter "MoqTests"

# Un test específico
dotnet test --filter "Sumar_5Y3_Retorna8"
```

### Ver detalle

```bash
dotnet test --verbosity detailed
```

---

## ?? Contenido Educativo

### Nivel 1: Básico (CalculadoraServiceTests)

- [x] Patrón AAA
- [x] [Fact] sin parámetros
- [x] [Theory] con [InlineData]
- [x] Assert.Equal, Assert.Throws
- [x] Display names

**Tests:** 11

---

### Nivel 2: Modelos (ProductoTests)

- [x] Testear métodos de clases
- [x] Validaciones
- [x] Excepciones
- [x] Parametrizados
- [x] Carrito (colecciones)

**Tests:** 12

---

### Nivel 3: Fakes (UsuarioServiceFakeTests)

- [x] Implementar interfaz manualmente
- [x] Tests async (Task<T>, await)
- [x] Assert.ThrowsAsync
- [x] Lógica de servicios
- [x] Datos en memoria

**Tests:** 13

---

### Nivel 4: Moq (UsuarioServiceMoqTests)

- [x] new Mock<T>()
- [x] Setup para comportamiento
- [x] Verify para interacciones
- [x] It.IsAny<T>()
- [x] It.Is<T>() con predicados
- [x] Times (Once, Never, Exactly)
- [x] SetupSequence

**Tests:** 10

---

## ?? Estructura de Clase (60 min)

```
0:00–0:02   Intro (2 min)
0:02–0:08   ¿Por qué testear? + ejemplos reales (6 min)
0:08–0:18   xUnit setup + AAA (10 min)
0:18–0:25   Buenas prácticas + nombrado (7 min)
0:25–0:35   Fakes: implementación manual (10 min)
0:35–0:45   Moq: Setup + Verify (10 min)
0:45–0:50   Tests async (5 min)
0:50–0:55   Fixtures (5 min)
0:55–1:00   Cierre + Ejercicios (5 min)
```

Ver `GUION_DOCENTE.md` para narración completa minuto a minuto.

---

## ? Ejercicios para Estudiantes

### Consigna 1: Tests básicos (20 min)

Implementar 4 tests en `CalculadoraTests.cs`:
1. Sumar 5 + 3 = 8
2. Restar 10 - 3 = 7
3. Dividir 10 / 0 ? exception
4. Promedio [1, 2, 3] = 2

### Consigna 2: Usar Fake (15 min)

Crear test `AutenticarAsync_ConFake_RetornaTrue()` usando `UsuarioRepositoryFake`

### Consigna 3: Usar Moq (15 min)

Crear el mismo test con `Moq` + `Setup` + `Verify`

### Consigna 4: Modelos (10 min)

3 tests para `Producto.CalcularTotal()` y `AplicarDescuento()`

### Consigna Final

Captura de `dotnet test` (screenshot) + comentarios en foro

---

## ?? Rubrica de Evaluación

### Para Estudiante (4 tests mínimo)

| Criterio | 3 pts | 2 pts | 1 pto | 0 pts |
|----------|-------|-------|-------|-------|
| **Ejecutables** | 4+ pasan | 3 pasan | 2 pasan | < 2 |
| **Nombrado** | Perfecto AAA | Bueno | OK | Genérico |
| **AAA** | Claro | Parcial | Confuso | Ausente |
| **Moq/Fake** | Setup+Verify | Setup solo | Incompleto | No usado |
| **Async** | await correcto | Task presente | .Result | N/A |
| **Excepciones** | Assert.Throws[Async] | Parcial | Try/catch | No |
| **Screenshot** | Presente, claro | Presente | Borroso | Ausente |

---

## ?? Dependencias Instaladas

```xml
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.2" />
<PackageReference Include="xunit" Version="2.6.6" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.5.6" />
<PackageReference Include="Moq" Version="4.20.70" />
<PackageReference Include="FluentAssertions" Version="6.12.0" />
<PackageReference Include="coverlet.collector" Version="6.0.0" />
```

---

## ?? Enlaces Útiles

- **xUnit Docs:** https://xunit.net/
- **Moq GitHub:** https://github.com/moq/moq4
- **Microsoft Testing Docs:** https://learn.microsoft.com/dotnet/core/testing/
- **TDD Intro:** https://en.wikipedia.org/wiki/Test-driven_development

---

## ?? Próximos Pasos

**Después de esta clase:**

1. Estudiante: Completa ejercicios + comenta en foro
2. Docente: Revisa entregas con rúbrica
3. Próxima clase: CI/CD (GitHub Actions + tests automatizados)

---

## ?? Checklist Pre-Clase

- [ ] Solución compila: `dotnet build`
- [ ] Tests ejecutan: `dotnet test` ? Passed: 46
- [ ] Guion memorizado o al lado
- [ ] IDE limpio (sin errores visuales)
- [ ] Terminal con fuente grande (zoom 150%)
- [ ] Micrófono/cámara testeados
- [ ] Grabador de pantalla listo (OBS)
- [ ] Slides preparadas
- [ ] Café ?

---

## ?? Preguntas para Foro

### P1: Nombrado de Tests

**Publica el nombre correcto para un test que:**
- Prueba `CalcularTotalCarrito()`
- Con 3 productos
- Debe retornar suma correcta

---

### P2: Fakes vs Moq

**Discute:** ¿Cuándo usarías cada uno? ¿Ventajas?  
Comenta 2 posts de compañeros.

---

### P3: Casos Críticos

**¿Qué métodos de tu código son críticos?** (validación, cálculo, integración)  
¿Por qué los priorizarías para testing?

---

## ?? Materiales de Grabación

**Para descargar/entregar con la clase:**

1. ? Código fuente (este repo)
2. ? `GUION_DOCENTE.md` (script completo)
3. ? Video grabado (60 min)
4. ? Ejemplos compilados (código)
5. ? `GUIA_TESTING.md` (referencia)
6. ? Plantilla de ejercicios

---

## ?? Soporte Técnico

**Si hay problemas:**

1. **Compilación:** `dotnet restore && dotnet clean && dotnet build`
2. **Tests no corren:** Verificar `TargetFramework` en `.csproj`
3. **Moq no carga:** `dotnet add package Moq`
4. **Errores de namespaces:** `using Moq;` y `using Xunit;`

---

## ?? Estado Final

? **Proyecto Completo**
- 46 tests listos
- 4 archivos de tests
- Documentación pedagógica
- Guion de clase
- Ejemplos ejecutables

**¡Listo para clase! ????**

---

**Última actualización:** 2024  
**Versión:** 1.0 (Completa)  
**Autor:** Docente MAUI/.NET  
**Licencia:** Educativa — Libre uso en aula