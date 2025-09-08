using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Practica6Shell.Models;
using System.Collections.ObjectModel;

namespace Practica6Shell.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<OpcionNavegacion> opcionesNavegacion;

        public MainViewModel()
        {
            Title = "Navegacion MAUI";
            
            OpcionesNavegacion = new ObservableCollection<OpcionNavegacion>
            {
                new("Productos", "Ver catalogo de productos", "productos"),
                new("Categorias", "Ver categorias disponibles", "categorias"),
                new("Configuracion", "Ajustes de la aplicacion", "configuracion"),
                new("Acerca de", "Informacion de la aplicacion", "acerca")
            };
        }

        [RelayCommand]
        private async Task NavegarASeccion(string ruta)
        {
            if (string.IsNullOrEmpty(ruta)) return;

            await EjecutarOperacionAsync(async () =>
            {
                switch (ruta)
                {
                    case "productos":
                        await NavegarA("//productos");
                        break;
                    case "categorias":
                        await NavegarA("//categorias");
                        break;
                    case "configuracion":
                        await Shell.Current.GoToAsync("configuracion", true); // Modal
                        break;
                    case "acerca":
                        await NavegarA("//acerca");
                        break;
                }
            }, "Error al navegar a la seccion");
        }

        [RelayCommand]
        private async Task MostrarInfo()
        {
            try
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.DisplayAlert(
                        "Informacion", 
                        "Practica 6: Navegacion en .NET MAUI\n\nDemuestra diferentes tipos de navegacion", 
                        "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al mostrar informacion: {ex.Message}");
            }
        }
    }
}