namespace Practica9.Models;

/// <summary>
/// Modelo de Usuario para demostración de testing.
/// Este modelo es simple pero suficiente para mostrar conceptos de testing.
/// </summary>
public record Usuario
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Modelo de Producto para testear lógica de cálculo.
/// </summary>
public record Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Cantidad { get; set; }
    public bool Disponible { get; set; } = true;

    /// <summary>
    /// Calcula el total del producto (Precio * Cantidad).
    /// Este método será testeado en múltiples escenarios.
    /// </summary>
    public decimal CalcularTotal() => Precio * Cantidad;

    /// <summary>
    /// Aplica un descuento al precio.
    /// Testearemos casos: descuento válido, descuento inválido (>100%), precio negativo, etc.
    /// </summary>
    public decimal AplicarDescuento(decimal porcentaje)
    {
        if (porcentaje < 0 || porcentaje > 100)
            throw new ArgumentException("El descuento debe estar entre 0 y 100");
        
        if (Precio < 0)
            throw new ArgumentException("El precio no puede ser negativo");

        return Precio * (1 - porcentaje / 100);
    }
}

/// <summary>
/// Modelo de Carrito de compras.
/// Útil para testear lógica más compleja con múltiples items.
/// </summary>
public class CarritoCompras
{
    private readonly List<Producto> _items = new();

    public IReadOnlyList<Producto> Items => _items.AsReadOnly();

    public void AgregarProducto(Producto producto)
    {
        if (producto == null)
            throw new ArgumentNullException(nameof(producto));

        if (!producto.Disponible)
            throw new InvalidOperationException("El producto no está disponible");

        if (producto.Precio < 0)
            throw new ArgumentException("El precio no puede ser negativo");

        _items.Add(producto);
    }

    public void RemoverProducto(int productoId)
    {
        var item = _items.FirstOrDefault(p => p.Id == productoId);
        if (item != null)
            _items.Remove(item);
    }

    public decimal CalcularTotal() => _items.Sum(p => p.CalcularTotal());

    public void Limpiar() => _items.Clear();

    public int ObtenerCantidadItems() => _items.Count;
}