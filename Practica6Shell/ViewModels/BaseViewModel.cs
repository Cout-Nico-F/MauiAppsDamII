using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Practica6Shell.ViewModels
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
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        protected virtual async Task Refresh()
        {
            await Task.CompletedTask;
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
                Mensaje = mensajeError ?? ex.Message;
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
                Mensaje = $"Error al navegar: {ex.Message}";
            }
        }

        public virtual async Task InicializarAsync()
        {
            await Task.CompletedTask;
        }
    }
}