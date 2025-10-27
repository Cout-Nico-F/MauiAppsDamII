using Practica9.Models;
using Practica9.Services;
using Xunit;

namespace Practica9.Tests;

/// <summary>
/// Tests para los modelos Producto y CarritoCompras.
/// 
/// Estos tests demuestran:
/// - Testear lógica de negocio en modelos
/// - Validaciones en métodos
/// - Excepciones en casos límite
/// - Tests parametrizados para múltiples escenarios
/// 
/// Patrón: estos modelos NO dependen de interfaces externas,
/// por lo que son los más fáciles de testear (unit tests puros).
/// </summary>
public class ProductoTests
{
    /// <summary>
    /// Test 1: Calcular total con valores válidos.
    /// </summary>
    [Fact]
    public void CalcularTotal_ConPrecioYCantidadValidos_RetornaResultadoCorrecto()
    {
        // Arrange
        var producto = new Producto { Precio = 10m, Cantidad = 5 };

        // Act
        decimal total = producto.CalcularTotal();

        // Assert
        Assert.Equal(50m, total);
    }

    /// <summary>
    /// Test 2: Calcular total con cantidad cero.
    /// </summary>
    [Fact]
    public void CalcularTotal_ConCantidadCero_RetornaCero()
    {
        var producto = new Producto { Precio = 10m, Cantidad = 0 };
        Assert.Equal(0m, producto.CalcularTotal());
    }

    /// <summary>
    /// Test 3: Calcular total con precio cero.
    /// </summary>
    [Fact]
    public void CalcularTotal_ConPrecioCero_RetornaCero()
    {
        var producto = new Producto { Precio = 0m, Cantidad = 5 };
        Assert.Equal(0m, producto.CalcularTotal());
    }

    /// <summary>
    /// Test 4: CalcularTotal parametrizado con múltiples casos.
    /// </summary>
    [Theory]
    [InlineData(10, 5, 50)]      // Normal
    [InlineData(1, 100, 100)]    // Múltiples
    [InlineData(100, 1, 100)]    // Una unidad
    [InlineData(10, 0, 0)]       // Cero cantidad
    [InlineData(0, 1000, 0)]     // Cero precio
    public void CalcularTotal_Parametrizado_RetornaCorrectamente(int precio, int cantidad, int esperado)
    {
        var producto = new Producto { Precio = precio, Cantidad = cantidad };
        Assert.Equal(esperado, producto.CalcularTotal());
    }

    /// <summary>
    /// Test 5: AplicarDescuento con porcentaje válido.
    /// </summary>
    [Fact]
    public void AplicarDescuento_ConPorcentajeValido_RetornaPrecioConDescuento()
    {
        // Arrange
        var producto = new Producto { Precio = 100m };
        decimal descuento = 10m; // 10%

        // Act
        decimal precioDescuento = producto.AplicarDescuento(descuento);

        // Assert
        Assert.Equal(90m, precioDescuento);
    }

    /// <summary>
    /// Test 6: AplicarDescuento con 50%.
    /// </summary>
    [Fact]
    public void AplicarDescuento_50Porciento_RetornaMitadDelPrecio()
    {
        var producto = new Producto { Precio = 100m };
        decimal resultado = producto.AplicarDescuento(50m);
        Assert.Equal(50m, resultado);
    }

    /// <summary>
    /// Test 7: AplicarDescuento con 0% (sin descuento).
    /// </summary>
    [Fact]
    public void AplicarDescuento_CeroPorciento_RetornaElMismoPrecio()
    {
        var producto = new Producto { Precio = 100m };
        decimal resultado = producto.AplicarDescuento(0m);
        Assert.Equal(100m, resultado);
    }

    /// <summary>
    /// Test 8: AplicarDescuento con 100% (descuento total).
    /// </summary>
    [Fact]
    public void AplicarDescuento_100Porciento_RetornaCero()
    {
        var producto = new Producto { Precio = 100m };
        decimal resultado = producto.AplicarDescuento(100m);
        Assert.Equal(0m, resultado);
    }

    /// <summary>
    /// Test 9: AplicarDescuento con porcentaje > 100 ? lanza excepción.
    /// </summary>
    [Fact]
    public void AplicarDescuento_PorcentajeMayorA100_LanzaArgumentException()
    {
        var producto = new Producto { Precio = 100m };
        
        var excepcion = Assert.Throws<ArgumentException>(() =>
            producto.AplicarDescuento(150m)
        );

        Assert.Contains("entre 0 y 100", excepcion.Message);
    }

    /// <summary>
    /// Test 10: AplicarDescuento con porcentaje negativo ? lanza excepción.
    /// </summary>
    [Fact]
    public void AplicarDescuento_PorcentajeNegativo_LanzaArgumentException()
    {
        var producto = new Producto { Precio = 100m };
        
        var excepcion = Assert.Throws<ArgumentException>(() =>
            producto.AplicarDescuento(-10m)
        );

        Assert.Contains("entre 0 y 100", excepcion.Message);
    }

    /// <summary>
    /// Test 11: AplicarDescuento con precio negativo ? lanza excepción.
    /// </summary>
    [Fact]
    public void AplicarDescuento_PrecioNegativo_LanzaArgumentException()
    {
        var producto = new Producto { Precio = -100m };
        
        var excepcion = Assert.Throws<ArgumentException>(() =>
            producto.AplicarDescuento(10m)
        );

        Assert.Contains("no puede ser negativo", excepcion.Message);
    }

    /// <summary>
    /// Test 12: AplicarDescuento parametrizado con valores válidos.
    /// </summary>
    [Theory]
    [InlineData(100, 10, 90)]     // 10% de 100 = 90
    [InlineData(200, 50, 100)]    // 50% de 200 = 100
    [InlineData(50, 20, 40)]      // 20% de 50 = 40
    [InlineData(1000, 1, 990)]    // 1% de 1000 = 990
    public void AplicarDescuento_ConValoresValidos_RetornaCorrectamente(
        int precio, int porcentaje, int esperado)
    {
        var producto = new Producto { Precio = precio };
        decimal resultado = producto.AplicarDescuento(porcentaje);
        Assert.Equal(esperado, (int)resultado);
    }
}

/// <summary>
/// Tests para la clase CarritoCompras.
/// 
/// Demuestra: testear lógica de colecciones y estado.
/// </summary>
public class CarritoComprasTests
{
    /// <summary>
    /// Test 1: Carrito nuevo está vacío.
    /// </summary>
    [Fact]
    public void CarritoNuevo_EstaVacio()
    {
        var carrito = new CarritoCompras();
        Assert.Empty(carrito.Items);
        Assert.Equal(0, carrito.ObtenerCantidadItems());
    }

    /// <summary>
    /// Test 2: Agregar un producto al carrito.
    /// </summary>
    [Fact]
    public void AgregarProducto_UnProducto_CarritoContieneUnItem()
    {
        // Arrange
        var carrito = new CarritoCompras();
        var producto = new Producto { Id = 1, Nombre = "Producto 1", Precio = 10m, Cantidad = 1 };

        // Act
        carrito.AgregarProducto(producto);

        // Assert
        Assert.Single(carrito.Items);
        Assert.Contains(producto, carrito.Items);
    }

    /// <summary>
    /// Test 3: Agregar múltiples productos.
    /// </summary>
    [Fact]
    public void AgregarProducto_TresProductos_CarritoContieneTresItems()
    {
        var carrito = new CarritoCompras();
        var p1 = new Producto { Id = 1, Nombre = "P1", Precio = 10m, Cantidad = 1 };
        var p2 = new Producto { Id = 2, Nombre = "P2", Precio = 20m, Cantidad = 1 };
        var p3 = new Producto { Id = 3, Nombre = "P3", Precio = 30m, Cantidad = 1 };

        carrito.AgregarProducto(p1);
        carrito.AgregarProducto(p2);
        carrito.AgregarProducto(p3);

        Assert.Equal(3, carrito.ObtenerCantidadItems());
    }

    /// <summary>
    /// Test 4: Calcular total del carrito.
    /// </summary>
    [Fact]
    public void CalcularTotal_ConProductos_RetornaSumaDeItems()
    {
        var carrito = new CarritoCompras();
        carrito.AgregarProducto(new Producto { Precio = 10m, Cantidad = 2 });  // Total: 20
        carrito.AgregarProducto(new Producto { Precio = 15m, Cantidad = 3 });  // Total: 45

        decimal total = carrito.CalcularTotal();

        Assert.Equal(65m, total);
    }

    /// <summary>
    /// Test 5: Calcular total de carrito vacío.
    /// </summary>
    [Fact]
    public void CalcularTotal_CarritoVacio_RetornaCero()
    {
        var carrito = new CarritoCompras();
        Assert.Equal(0m, carrito.CalcularTotal());
    }

    /// <summary>
    /// Test 6: Remover producto del carrito.
    /// </summary>
    [Fact]
    public void RemoverProducto_ProductoExistente_SeRemueveDelCarrito()
    {
        var carrito = new CarritoCompras();
        var producto = new Producto { Id = 1, Precio = 10m, Cantidad = 1 };
        carrito.AgregarProducto(producto);

        carrito.RemoverProducto(1);

        Assert.Empty(carrito.Items);
    }

    /// <summary>
    /// Test 7: Remover producto que no existe (no lanza error).
    /// </summary>
    [Fact]
    public void RemoverProducto_ProductoNoExistente_NoLanzaError()
    {
        var carrito = new CarritoCompras();
        carrito.AgregarProducto(new Producto { Id = 1, Precio = 10m, Cantidad = 1 });

        carrito.RemoverProducto(999); // No existe

        Assert.Single(carrito.Items); // Aún hay 1 item
    }

    /// <summary>
    /// Test 8: Limpiar carrito.
    /// </summary>
    [Fact]
    public void Limpiar_CarritoConItems_CarritoSeQuedaVacio()
    {
        var carrito = new CarritoCompras();
        carrito.AgregarProducto(new Producto { Precio = 10m, Cantidad = 1 });
        carrito.AgregarProducto(new Producto { Precio = 20m, Cantidad = 1 });

        carrito.Limpiar();

        Assert.Empty(carrito.Items);
    }

    /// <summary>
    /// Test 9: Agregar producto null lanza excepción.
    /// </summary>
    [Fact]
    public void AgregarProducto_ProductoNull_LanzaArgumentNullException()
    {
        var carrito = new CarritoCompras();

        var excepcion = Assert.Throws<ArgumentNullException>(() =>
            carrito.AgregarProducto(null!)
        );

        Assert.Equal("producto", excepcion.ParamName);
    }

    /// <summary>
    /// Test 10: Agregar producto no disponible lanza excepción.
    /// </summary>
    [Fact]
    public void AgregarProducto_ProductoNoDisponible_LanzaInvalidOperationException()
    {
        var carrito = new CarritoCompras();
        var producto = new Producto { Id = 1, Precio = 10m, Cantidad = 1, Disponible = false };

        var excepcion = Assert.Throws<InvalidOperationException>(() =>
            carrito.AgregarProducto(producto)
        );

        Assert.Contains("no está disponible", excepcion.Message);
    }

    /// <summary>
    /// Test 11: Agregar producto con precio negativo lanza excepción.
    /// </summary>
    [Fact]
    public void AgregarProducto_PrecioNegativo_LanzaArgumentException()
    {
        var carrito = new CarritoCompras();
        var producto = new Producto { Id = 1, Precio = -10m, Cantidad = 1 };

        var excepcion = Assert.Throws<ArgumentException>(() =>
            carrito.AgregarProducto(producto)
        );

        Assert.Contains("no puede ser negativo", excepcion.Message);
    }
}