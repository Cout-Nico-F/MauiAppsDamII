using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Practica6Shell.Models;
using Practica6Shell.Services;
using System.Collections.ObjectModel;

namespace Practica6Shell.ViewModels
{
    public partial class ProductosViewModel : BaseViewModel
    {
        private readonly IProductoService _productoService;

        [ObservableProperty]
        private ObservableCollection<Producto> productos = new();

        [ObservableProperty]
        private Producto? productoSeleccionado;

        public ProductosViewModel(IProductoService productoService)
        {
            _productoService = productoService;
            Title = "Productos";
        }

        public override async Task InicializarAsync()
        {
            await CargarProductos();
        }

        [RelayCommand]
        private async Task CargarProductos()
        {
            await EjecutarOperacionAsync(async () =>
            {
                var productosData = await _productoService.ObtenerProductosAsync();
                Productos.Clear();
                foreach (var producto in productosData)
                {
                    Productos.Add(producto);
                }
            }, "Error al cargar productos");
        }

        [RelayCommand]
        private async Task VerDetalle(Producto producto)
        {
            if (producto == null) return;

            var parametros = new Dictionary<string, object>
            {
                ["producto"] = producto
            };

            await NavegarA("detalleproducto", parametros);
        }

        partial void OnProductoSeleccionadoChanged(Producto? value)
        {
            if (value != null)
            {
                _ = Task.Run(async () => await VerDetalle(value));
            }
        }
    }
}