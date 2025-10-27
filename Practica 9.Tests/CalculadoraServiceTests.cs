using Practica9.Services;
using Xunit;

namespace Practica9.Tests;

/// <summary>
/// Tests unitarios para la clase CalculadoraService.
/// 
/// Estos tests son los MÁS SIMPLES y son ideales para:
/// - Entender la estructura AAA (Arrange, Act, Assert)
/// - Ver la ejecución básica de xUnit
/// - Aprender a nombrar tests
/// 
/// Patrones demostrados:
/// - [Fact]: test sin parámetros
/// - [Theory] + [InlineData]: tests parametrizados
/// - Assert.Equal, Assert.Throws
/// 
/// Ejecución: dotnet test
/// Salida esperada: todos los tests pasan (green check)
/// </summary>
public class CalculadoraServiceTests
{
    // En lugar de crear una nueva instancia en cada test,
    // podríamos usar un fixture (demostrado más abajo).
    // Aquí usamos el patrón simple para claridad educativa.

    private readonly CalculadoraService _calculadora = new();

    #region Tests básicos: [Fact]

    /// <summary>
    /// Test 1: Sumar_NumerosPosit ivos_RetornaResultadoCorreo.
    /// 
    /// Patrón AAA:
    /// - Arrange: preparar datos de entrada
    /// - Act: ejecutar el método a testear
    /// - Assert: verificar el resultado
    /// 
    /// Nombrado: Método_Escenario_ResultadoEsperado
    /// </summary>
    [Fact]
    public void Sumar_NumerosPositivos_RetornaResultadoCorrecto()
    {
        // Arrange (preparar)
        int a = 5;
        int b = 3;
        int resultadoEsperado = 8;

        // Act (ejecutar)
        int resultado = _calculadora.Sumar(a, b);

        // Assert (verificar)
        Assert.Equal(resultadoEsperado, resultado);
    }

    /// <summary>
    /// Test 2: Sumar con números negativos.
    /// Demuestra que el test es agnóstico a valores específicos.
    /// </summary>
    [Fact]
    public void Sumar_NumerosNegativos_RetornaResultadoNegativo()
    {
        // Arrange
        int a = -5;
        int b = -3;

        // Act
        int resultado = _calculadora.Sumar(a, b);

        // Assert
        Assert.Equal(-8, resultado);
    }

    /// <summary>
    /// Test 3: Multiplicar números.
    /// Introduce otro método y tipo de dato.
    /// </summary>
    [Fact]
    public void Multiplicar_TresY4_Retorna12()
    {
        int resultado = _calculadora.Multiplicar(3, 4);
        Assert.Equal(12, resultado);
    }

    #endregion

    #region Tests con excepciones

    /// <summary>
    /// Test 4: Dividir por cero lanza excepción.
    /// 
    /// Patrón: Assert.Throws<ExcepcionTipo>(() => método())
    /// Esto verifica que se lance la excepción esperada.
    /// 
    /// IMPORTANTE: en tests async, usaremos Assert.ThrowsAsync<T>
    /// </summary>
    [Fact]
    public void Dividir_DivisorCero_LanzaDivideByZeroException()
    {
        // Assert.Throws retorna la excepción lanzada
        // Podríamos capturarla y verificar el mensaje.
        var excepcion = Assert.Throws<DivideByZeroException>(() =>
        {
            _calculadora.Dividir(10, 0);
        });

        // Verificar el mensaje de la excepción
        Assert.Contains("El divisor no puede ser cero", excepcion.Message);
    }

    /// <summary>
    /// Test 5: Dividir correctamente.
    /// </summary>
    [Fact]
    public void Dividir_10Entre2_Retorna5()
    {
        decimal resultado = _calculadora.Dividir(10, 2);
        Assert.Equal(5, resultado);
    }

    /// <summary>
    /// Test 6: ObtenerPromedio con lista vacía lanza excepción.
    /// </summary>
    [Fact]
    public void ObtenerPromedio_ListaVacia_LanzaArgumentException()
    {
        var excepcion = Assert.Throws<ArgumentException>(() =>
        {
            _calculadora.ObtenerPromedio(new List<int>());
        });

        Assert.Contains("no puede estar vacía", excepcion.Message);
    }

    #endregion

    #region Tests parametrizados: [Theory] + [InlineData]

    /// <summary>
    /// Test 7: Sumar_ParametrizadoVarios_ResultadosCorrecto.
    /// 
    /// [Theory]: permite múltiples sets de datos.
    /// [InlineData(a, b, esperado)]: proporciona los datos.
    /// 
    /// Ventaja: un solo test, múltiples escenarios.
    /// Salida: 3 ejecuciones, cada una con sus datos.
    /// </summary>
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(10, 20, 30)]
    [InlineData(-5, 5, 0)]
    [InlineData(0, 0, 0)]
    public void Sumar_DadosVariosValores_RetornaCorrectamente(int a, int b, int esperado)
    {
        int resultado = _calculadora.Sumar(a, b);
        Assert.Equal(esperado, resultado);
    }

    /// <summary>
    /// Test 8: Multiplicar parametrizado.
    /// </summary>
    [Theory]
    [InlineData(2, 3, 6)]
    [InlineData(5, 0, 0)]
    [InlineData(-2, 3, -6)]
    public void Multiplicar_Parametrizado_RetornaCorrectamente(int a, int b, int esperado)
    {
        int resultado = _calculadora.Multiplicar(a, b);
        Assert.Equal(esperado, resultado);
    }

    /// <summary>
    /// Test 9: Dividir parametrizado (casos válidos).
    /// </summary>
    [Theory]
    [InlineData(10, 2, 5)]
    [InlineData(15, 3, 5)]
    [InlineData(100, 4, 25)]
    public void Dividir_Parametrizado_RetornaCorrectamente(decimal dividendo, decimal divisor, decimal esperado)
    {
        decimal resultado = _calculadora.Dividir(dividendo, divisor);
        Assert.Equal(esperado, resultado);
    }

    #endregion

    #region Tests con MemberData (para lógica más compleja)

    /// <summary>
    /// Test 10: ObtenerPromedio con datos válidos.
    /// Demuestra uso de [Theory] con datos de lista.
    /// </summary>
    [Theory]
    [InlineData(new int[] { 1, 2, 3 }, 2)]           // (1+2+3)/3 = 2
    [InlineData(new int[] { 10 }, 10)]                // un único valor
    [InlineData(new int[] { 0, 0, 0 }, 0)]            // todos ceros
    [InlineData(new int[] { -5, 5 }, 0)]              // negativos
    public void ObtenerPromedio_ConDatos_RetornaPromedioCorreo(int[] numeros, int esperado)
    {
        decimal resultado = _calculadora.ObtenerPromedio(numeros);
        Assert.Equal(esperado, resultado);
    }

    #endregion

    #region Tests con Display Name (para mejor legibilidad en salida)

    /// <summary>
    /// Test 11: Sumar con display name personalizado.
    /// Mejora legibilidad en la salida de dotnet test.
    /// </summary>
    [Fact(DisplayName = "Sumar 5 + 3 debe retornar 8 (simple)")]
    public void SumarSimple_DebeRetornar8()
    {
        Assert.Equal(8, _calculadora.Sumar(5, 3));
    }

    #endregion
}