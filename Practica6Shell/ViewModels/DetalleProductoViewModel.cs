using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Practica6Shell.Models;

namespace Practica6Shell.ViewModels
{
    [QueryProperty(nameof(Producto), "producto")]
    public partial class DetalleProductoViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Producto? producto;

        public DetalleProductoViewModel()
        {
            Title = "Detalle del Producto";
        }

        partial void OnProductoChanged(Producto? value)
        {
            if (value != null)
            {
                Title = $"Detalle: {value.Nombre}";
            }
        }

        [RelayCommand]
        private async Task EditarProducto()
        {
            if (Producto == null) return;

            var parametros = new Dictionary<string, object>
            {
                ["producto"] = Producto
            };

            await NavegarA("editarproducto", parametros);
        }

        [RelayCommand]
        private async Task CompartirProducto()
        {
            if (Producto == null) return;

            try
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.DisplayAlert(
                        "Compartir",
                        $"Compartir {Producto.Nombre} - ${Producto.Precio}",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al compartir producto: {ex.Message}");
            }
        }
    }
}