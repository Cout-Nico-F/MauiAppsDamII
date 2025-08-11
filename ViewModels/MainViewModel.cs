using System.ComponentModel;
using System.Runtime.CompilerServices;
using MauiApp2MVVM.Models;

namespace MauiApp2MVVM.ViewModels
{
    // El ViewModel conecta la vista (UI) con el modelo (datos).
    // Expone propiedades y comandos para que la vista los consuma mediante binding.
    public class MainViewModel : INotifyPropertyChanged
    {
        // Instancia del modelo de datos.
        private CounterModel _counter = new CounterModel();

        // Propiedad expuesta para la vista. Al cambiar, notifica a la UI.
        public int Count
        {
            get => _counter.Count;
            set
            {
                if (_counter.Count != value)
                {
                    _counter.Count = value;
                    // Notifica a la vista que la propiedad cambió.
                    OnPropertyChanged();
                }
            }
        }

        // Propiedad adicional para mostrar un mensaje en la vista.
        public string Message
        {
            get => _counter.Message;
            set
            {
                if (_counter.Message != value)
                {
                    _counter.Message = value;
                    // Notifica a la vista que la propiedad cambió.
                    OnPropertyChanged();
                }
            }
        }

        // Comando que la vista puede ejecutar (por ejemplo, al presionar un botón).
        public Command IncrementCommand { get; }
        // Ejemplo: comando para cambiar el mensaje.
        public Command ChangeMessageCommand { get; }

        // Constructor: inicializa los comandos y define las acciones a ejecutar.
        public MainViewModel()
        {
            // Al ejecutar el comando, incrementa el contador.
            IncrementCommand = new Command(() => Count++);
            // Al ejecutar este comando, cambia el mensaje.
            ChangeMessageCommand = new Command(() => Message = "¡Mensaje cambiado desde el ViewModel!");
        }

        // Evento que notifica a la vista cuando una propiedad cambia.
        public event PropertyChangedEventHandler? PropertyChanged;

        // Método que dispara el evento PropertyChanged.
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
