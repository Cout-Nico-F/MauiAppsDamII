namespace Practica6Shell.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public bool EnStock { get; set; }
        public DateTime FechaCreacion { get; set; }
        
        public Producto() { }
        
        public Producto(int id, string nombre, string descripcion, decimal precio, string categoria, bool enStock = true)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            Precio = precio;
            Categoria = categoria;
            EnStock = enStock;
            FechaCreacion = DateTime.Now;
        }
    }

    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int CantidadProductos { get; set; }
        
        public Categoria() { }
        
        public Categoria(int id, string nombre, string descripcion, int cantidad)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            CantidadProductos = cantidad;
        }
    }

    public class OpcionNavegacion
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Ruta { get; set; } = string.Empty;
        
        public OpcionNavegacion() { }
        
        public OpcionNavegacion(string titulo, string descripcion, string ruta)
        {
            Titulo = titulo;
            Descripcion = descripcion;
            Ruta = ruta;
        }
    }
}