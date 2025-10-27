using System.Collections.ObjectModel;
using System.Windows.Input;
using Practica9.Models;
using Practica9.Services;

namespace Practica9.ViewModels;

/// <summary>
/// ViewModel para demostrar la calculadora
/// Expone métodos de cálculo con validación y manejo de errores
/// </summary>
public class CalculadoraViewModel : BaseViewModel
{
    private readonly CalculadoraService _calculadora = new();
    
    private string _numeroA = "0";
    private string _numeroB = "0";
    private string _resultado = "0";
    private string _mensajeError = string.Empty;

    public string NumeroA
    {
        get => _numeroA;
        set => SetProperty(ref _numeroA, value);
    }

    public string NumeroB
    {
        get => _numeroB;
        set => SetProperty(ref _numeroB, value);
    }

    public string Resultado
    {
        get => _resultado;
        set => SetProperty(ref _resultado, value);
    }

    public string MensajeError
    {
        get => _mensajeError;
        set => SetProperty(ref _mensajeError, value);
    }

    public CalculadoraViewModel()
    {
        Title = "Calculadora";
    }

    // Comandos
    public ICommand SumarCommand => new Command(Sumar);
    public ICommand RestarCommand => new Command(Restar);
    public ICommand MultiplicarCommand => new Command(Multiplicar);
    public ICommand DividirCommand => new Command(Dividir);
    public ICommand LimpiarCommand => new Command(Limpiar);

    private void Sumar()
    {
        try
        {
            MensajeError = string.Empty;
            if (int.TryParse(NumeroA, out int a) && int.TryParse(NumeroB, out int b))
            {
                Resultado = _calculadora.Sumar(a, b).ToString();
            }
            else
            {
                MensajeError = "? Ingresa números válidos";
            }
        }
        catch (Exception ex)
        {
            MensajeError = $"? Error: {ex.Message}";
        }
    }

    private void Restar()
    {
        try
        {
            MensajeError = string.Empty;
            if (int.TryParse(NumeroA, out int a) && int.TryParse(NumeroB, out int b))
            {
                Resultado = (a - b).ToString();
            }
            else
            {
                MensajeError = "? Ingresa números válidos";
            }
        }
        catch (Exception ex)
        {
            MensajeError = $"? Error: {ex.Message}";
        }
    }

    private void Multiplicar()
    {
        try
        {
            MensajeError = string.Empty;
            if (int.TryParse(NumeroA, out int a) && int.TryParse(NumeroB, out int b))
            {
                Resultado = _calculadora.Multiplicar(a, b).ToString();
            }
            else
            {
                MensajeError = "? Ingresa números válidos";
            }
        }
        catch (Exception ex)
        {
            MensajeError = $"? Error: {ex.Message}";
        }
    }

    private void Dividir()
    {
        try
        {
            MensajeError = string.Empty;
            if (decimal.TryParse(NumeroA, out decimal a) && decimal.TryParse(NumeroB, out decimal b))
            {
                Resultado = _calculadora.Dividir(a, b).ToString("F2");
            }
            else
            {
                MensajeError = "? Ingresa números válidos";
            }
        }
        catch (DivideByZeroException)
        {
            MensajeError = "? No se puede dividir por cero";
            Resultado = "0";
        }
        catch (Exception ex)
        {
            MensajeError = $"? Error: {ex.Message}";
        }
    }

    private void Limpiar()
    {
        NumeroA = "0";
        NumeroB = "0";
        Resultado = "0";
        MensajeError = string.Empty;
    }
}

/// <summary>
/// ViewModel para demostrar la tienda de productos
/// Muestra carrito de compras y cálculos con descuentos
/// </summary>
public class TiendaViewModel : BaseViewModel
{
    private ObservableCollection<Producto> _productos;
    private ObservableCollection<Producto> _carrito;
    private decimal _totalCarrito;
    private string _descuentoAplicar = "0";
    private decimal _precioFinal;

    public ObservableCollection<Producto> Productos
    {
        get => _productos;
        set => SetProperty(ref _productos, value);
    }

    public ObservableCollection<Producto> Carrito
    {
        get => _carrito;
        set => SetProperty(ref _carrito, value);
    }

    public decimal TotalCarrito
    {
        get => _totalCarrito;
        set => SetProperty(ref _totalCarrito, value);
    }

    public string DescuentoAplicar
    {
        get => _descuentoAplicar;
        set => SetProperty(ref _descuentoAplicar, value);
    }

    public decimal PrecioFinal
    {
        get => _precioFinal;
        set => SetProperty(ref _precioFinal, value);
    }

    public ICommand AgregarAlCarritoCommand => new Command<Producto>(AgregarAlCarrito);
    public ICommand LimpiarCarritoCommand => new Command(LimpiarCarrito);
    public ICommand AplicarDescuentoCommand => new Command(AplicarDescuento);

    public TiendaViewModel()
    {
        Title = "Tienda";
        Productos = new ObservableCollection<Producto>
        {
            new() { Id = 1, Nombre = "?? iPhone 15", Precio = 999m, Cantidad = 1, Disponible = true },
            new() { Id = 2, Nombre = "?? MacBook Pro", Precio = 1999m, Cantidad = 1, Disponible = true },
            new() { Id = 3, Nombre = "? Apple Watch", Precio = 399m, Cantidad = 1, Disponible = true },
            new() { Id = 4, Nombre = "?? AirPods Pro", Precio = 249m, Cantidad = 1, Disponible = true },
        };

        Carrito = new ObservableCollection<Producto>();
    }

    private void AgregarAlCarrito(Producto producto)
    {
        if (producto == null) return;

        var itemEnCarrito = Carrito.FirstOrDefault(p => p.Id == producto.Id);
        if (itemEnCarrito != null)
        {
            itemEnCarrito.Cantidad++;
        }
        else
        {
            Carrito.Add(new Producto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Cantidad = 1,
                Disponible = producto.Disponible
            });
        }

        ActualizarTotal();
    }

    private void LimpiarCarrito()
    {
        Carrito.Clear();
        TotalCarrito = 0;
        PrecioFinal = 0;
        DescuentoAplicar = "0";
    }

    private void AplicarDescuento()
    {
        if (TotalCarrito == 0)
        {
            MainThread.BeginInvokeOnMainThread(() =>
                Application.Current?.MainPage?.DisplayAlert("Carrito vacío", "Agrega productos primero", "OK")
            );
            return;
        }

        if (decimal.TryParse(DescuentoAplicar, out decimal descuento))
        {
            try
            {
                var producto = new Producto { Precio = TotalCarrito };
                PrecioFinal = producto.AplicarDescuento(descuento);
            }
            catch
            {
                MainThread.BeginInvokeOnMainThread(() =>
                    Application.Current?.MainPage?.DisplayAlert("Error", "Descuento debe estar entre 0 y 100", "OK")
                );
                PrecioFinal = TotalCarrito;
            }
        }
    }

    private void ActualizarTotal()
    {
        TotalCarrito = Carrito.Sum(p => p.CalcularTotal());
        PrecioFinal = TotalCarrito;
    }
}
