using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Practica_5_Imagenes.ViewModels;

/// <summary>
/// Clase base para ViewModels que implementa INotifyPropertyChanged.
/// 
/// INotifyPropertyChanged es esencial en MVVM para que la UI se actualice automáticamente
/// cuando las propiedades del ViewModel cambian. Esta implementación incluye:
/// 
/// - SetProperty: helper genérico que compara valores y dispara PropertyChanged solo si hay cambio
/// - OnPropertyChanged: método para disparar el evento manualmente
/// - CallerMemberName: atributo que automáticamente pasa el nombre de la propiedad
/// </summary>
public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Dispara el evento PropertyChanged para notificar cambios en una propiedad.
    /// El atributo CallerMemberName hace que el compilador pase automáticamente el nombre
    /// de la propiedad que llama a este método.
    /// </summary>
    /// <param name="propertyName">Nombre de la propiedad que cambió</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    /// <summary>
    /// Helper genérico para actualizar propiedades de manera eficiente.
    /// Compara el valor actual con el nuevo, y solo actualiza y notifica si hay cambio.
    /// 
    /// Uso típico:
    /// private string _name = string.Empty;
    /// public string Name 
    /// { 
    ///     get => _name; 
    ///     set => SetProperty(ref _name, value); 
    /// }
    /// </summary>
    /// <typeparam name="T">Tipo de la propiedad</typeparam>
    /// <param name="backingField">Campo privado que almacena el valor</param>
    /// <param name="value">Nuevo valor a asignar</param>
    /// <param name="propertyName">Nombre de la propiedad (automático)</param>
    /// <returns>True si el valor cambió, false si era el mismo</returns>
    protected virtual bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(backingField, value))
            return false;

        backingField = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}