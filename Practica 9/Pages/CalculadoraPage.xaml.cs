using Practica9.ViewModels;

namespace Practica9.Pages;

public partial class CalculadoraPage : ContentPage
{
    public CalculadoraPage()
    {
        InitializeComponent();
        BindingContext = new CalculadoraViewModel();
    }
}
