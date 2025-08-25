using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HTTPmaui.ViewModels;

// Clase base para ViewModels.
// Implementa INotifyPropertyChanged para que la UI se actualice cuando cambian propiedades.
public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    // Helper para notificar cambios de propiedad.
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    // SetProperty reduce repetición al asignar campos y notificar cambios.
    protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(backingField, value)) return false;
        backingField = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
