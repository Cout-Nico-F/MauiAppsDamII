using Practica6Shell.Models;

namespace Practica6Shell.Services
{
    public interface IProductoService
    {
        Task<List<Producto>> ObtenerProductosAsync();
        Task<List<Categoria>> ObtenerCategoriasAsync();
        Task<Producto?> ObtenerProductoPorIdAsync(int id);
    }

    public class ProductoService : IProductoService
    {
        private static readonly List<Producto> _productos = new()
        {
            new(1, "Laptop", "Computadora portatil de alta gama", 1200.00m, "Tecnologia"),
            new(2, "Mouse", "Mouse inalambrico ergonomico", 45.99m, "Accesorios"),
            new(3, "Teclado", "Teclado mecanico RGB", 89.99m, "Accesorios"),
            new(4, "Monitor", "Monitor 4K de 27 pulgadas", 399.99m, "Tecnologia"),
            new(5, "Webcam", "Camara web HD con microfono", 79.99m, "Accesorios")
        };

        private static readonly List<Categoria> _categorias = new()
        {
            new(1, "Tecnologia", "Dispositivos tecnologicos", 2),
            new(2, "Accesorios", "Accesorios para computadora", 3)
        };

        public async Task<List<Producto>> ObtenerProductosAsync()
        {
            await Task.Delay(100);
            return _productos.ToList();
        }

        public async Task<List<Categoria>> ObtenerCategoriasAsync()
        {
            await Task.Delay(100);
            return _categorias.ToList();
        }

        public async Task<Producto?> ObtenerProductoPorIdAsync(int id)
        {
            await Task.Delay(100);
            return _productos.FirstOrDefault(p => p.Id == id);
        }
    }
}