# ?? Práctica 9: Testing Unitario — Guía de Ejecución

## ¿Qué hay en este proyecto?

```
Practica9/
??? Models/
?   ??? Usuario.cs              # Modelos testables (Producto, CarritoCompras, Usuario)
??? Services/
?   ??? UsuarioService.cs       # Servicios con interfaz IUsuarioRepository
??? MainPage.xaml(.cs)          # UI (no modificar para testing)

Practica9.Tests/
??? CalculadoraServiceTests.cs  # Tests de lógica simple (AAA, [Fact], [Theory])
??? ProductoTests.cs             # Tests de modelos (validaciones, excepciones)
??? UsuarioServiceFakeTests.cs   # Tests con Fakes (implementación manual)
??? UsuarioServiceMoqTests.cs    # Tests con Moq (mocks dinámicos)
??? GUIA_TESTING.md              # Documentación completa
```

---

## ?? Inicio Rápido

### Paso 1: Verificar estructura

```bash
# Desde la raíz de tu solución
dotnet sln list

# Deberías ver:
# Practica 9\Practica 9.csproj
# Practica 9.Tests\Practica 9.Tests.csproj
```

### Paso 2: Restaurar dependencias

```bash
cd Practica9.Tests
dotnet restore
```

Se instalarán:
- xunit
- xunit.runner.visualstudio
- Moq
- FluentAssertions

### Paso 3: Ejecutar todos los tests

```bash
dotnet test
```

**Salida esperada:**

```
Test Results: Passed: 50 Failed: 0 Skipped: 0
Duration: 234 ms
```

---

## ?? Ejecuciones Específicas

### Ejecutar solo tests de Calculadora

```bash
dotnet test --filter "CalculadoraServiceTests"
```

### Ejecutar un test específico

```bash
dotnet test --filter "Sumar_5Y3_Retorna8"
```

### Ver detalle de ejecución

```bash
dotnet test --verbosity detailed
```

### Ejecutar y generar reporte

```bash
dotnet test > test-results.txt 2>&1
cat test-results.txt  # En Windows: type test-results.txt
```

---

## ?? Archivos de Tests Explicados

### 1. `CalculadoraServiceTests.cs` (Nivel 1: Lo más simple)

**Demuestra:**
- Patrón AAA (Arrange-Act-Assert)
- [Fact] para tests sin parámetros
- [Theory] + [InlineData] para parametrizados
- Assert.Equal, Assert.Throws

**Tests incluidos:** 11

```bash
dotnet test --filter "CalculadoraServiceTests"
```

**Salida:**
```
? Sumar_NumerosPositivos_RetornaResultadoCorrecto
? Sumar_NumerosNegativos_RetornaResultadoNegativo
? Dividir_DivisorCero_LanzaDivideByZeroException
...
```

---

### 2. `ProductoTests.cs` (Nivel 2: Modelos con lógica)

**Demuestra:**
- Testear métodos de modelos (no servicios)
- Validaciones
- Excepciones en casos límite
- Parametrizados para múltiples escenarios

**Tests incluidos:** 12

```bash
dotnet test --filter "ProductoTests"
```

---

### 3. `UsuarioServiceFakeTests.cs` (Nivel 3: Fakes)

**Demuestra:**
- Implementación manual de interfaz (IUsuarioRepository)
- Tests async con Task/await
- Verificar lógica de servicios sin BD real
- Assert.ThrowsAsync para excepciones async

**Tests incluidos:** 13

```bash
dotnet test --filter "UsuarioServiceFakeTests"
```

---

### 4. `UsuarioServiceMoqTests.cs` (Nivel 4: Moq)

**Demuestra:**
- Crear mocks con `new Mock<T>()`
- Setup para definir comportamiento
- Verify para asegurar interacciones
- It.IsAny<T>() para parámetros
- Times.Once, Times.Never para contar llamadas

**Tests incluidos:** 10

```bash
dotnet test --filter "UsuarioServiceMoqTests"
```

---

## ? Checklist de Aprendizaje

Marca lo que ya entiendes:

- [ ] ¿Qué es un test unitario y para qué sirve?
- [ ] Patrón AAA (Arrange-Act-Assert)
- [ ] Diferencia entre [Fact] y [Theory]
- [ ] Cómo testear excepciones
- [ ] Qué es un Fake y cómo implementarlo
- [ ] Qué es Moq y cuándo usarlo
- [ ] Setup para definir comportamiento de mocks
- [ ] Verify para asegurar interacciones
- [ ] Tests async (await, Assert.ThrowsAsync)
- [ ] Buenas prácticas de nombrado

---

## ?? Debugging

### Test falla pero no ves el error

```bash
dotnet test --verbosity detailed
```

### Ejecutar en Visual Studio

1. Abre `Test Explorer` (Ctrl+E, T)
2. Haz clic en un test
3. Click derecho ? Debug Selected Tests

### Agregar prints (xUnit los captura)

```csharp
[Fact]
public void MiTest()
{
    System.Console.WriteLine("Debug info aquí");
    Assert.True(true);
}

// Ejecuta:
// dotnet test --logger "console;verbosity=detailed"
```

---

## ?? Próximos Pasos

1. **Lee** `GUIA_TESTING.md` completo (conceptos + código)
2. **Haz** los ejercicios de la consigna
3. **Crea** tests para tu propio código
4. **Estudia** TDD (Test-Driven Development)
5. **Practica** CI/CD en GitHub Actions

---

## ?? Preguntas Frecuentes

### P: ¿Debo testear TODO?

R: No. Enfócate en:
- Lógica de negocio
- Validaciones
- Casos límite
- Métodos críticos

Evita:
- Getters/setters simples
- Métodos UI
- Métodos que solo delegan

### P: ¿Qué cobertura debería alcanzar?

R: Regla general:
- 60%+: bueno
- 80%+: muy bueno
- 100%: overkill (a veces)

**Calidad > Cantidad de líneas cubiertas**

### P: ¿Fakes o Moq?

R: Respuesta corta:
- Interfaz simple ? Fake
- Interfaz compleja ? Moq
- Regla general ? Moq

### P: ¿Puedo usar tests para TDD?

R: ¡Sí! TDD workflow:
1. Escribe test (fallará)
2. Implementa método mínimo
3. Test pasa
4. Refactoriza
5. Test sigue pasando

---

## ?? Soporte

- **Foro:** Publica preguntas con tag `#testing`
- **GitHub:** Issues en el repositorio
- **Documentación:** Ver `GUIA_TESTING.md`

---

## ?? Objetivos Logrados

Al completar esta práctica podrás:

? Escribir tests unitarios con xUnit  
? Usar Fakes para aislar dependencias  
? Usar Moq para mocks dinámicos  
? Testear código async  
? Verificar excepciones  
? Nombrar tests correctamente  
? Aplicar patrón AAA  
? Debuggear tests fallidos  

**¡Listo para la clase! ??**