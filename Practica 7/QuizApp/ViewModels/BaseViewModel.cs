using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace QuizApp.ViewModels
{
    public abstract partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private string mensaje = string.Empty;

        [RelayCommand]
        protected virtual async Task GoBack()
        {
            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error navegando hacia atrás: {ex.Message}");
            }
        }

        protected async Task EjecutarOperacionAsync(Func<Task> operacion, string? mensajeError = null)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                await operacion();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en operación: {ex}");
                Mensaje = mensajeError ?? ex.Message;
                await MostrarAlerta("Error", Mensaje);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected async Task<T?> EjecutarOperacionAsync<T>(Func<Task<T>> operacion, string? mensajeError = null)
        {
            if (IsBusy) return default;

            try
            {
                IsBusy = true;
                return await operacion();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en operación: {ex}");
                Mensaje = mensajeError ?? ex.Message;
                await MostrarAlerta("Error", Mensaje);
                return default;
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected async Task NavegarA(string ruta, IDictionary<string, object>? parametros = null)
        {
            try
            {
                if (parametros != null)
                {
                    await Shell.Current.GoToAsync(ruta, parametros);
                }
                else
                {
                    await Shell.Current.GoToAsync(ruta);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error de navegación: {ex}");
                await MostrarAlerta("Error de navegación", $"No se pudo navegar a {ruta}: {ex.Message}");
            }
        }

        protected async Task MostrarAlerta(string titulo, string mensaje)
        {
            try
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert(titulo, mensaje, "OK");
                }
                else if (Shell.Current != null)
                {
                    await Shell.Current.DisplayAlert(titulo, mensaje, "OK");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"No se pudo mostrar alerta: {titulo} - {mensaje}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al mostrar alerta: {ex.Message}");
            }
        }

        public virtual async Task InicializarAsync()
        {
            try
            {
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en inicialización: {ex}");
                await MostrarAlerta("Error", $"Error al inicializar: {ex.Message}");
            }
        }
    }
}