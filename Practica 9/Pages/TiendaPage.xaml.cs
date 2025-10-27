using Practica9.ViewModels;

namespace Practica9.Pages;

public partial class TiendaPage : ContentPage
{
    public TiendaPage()
    {
        InitializeComponent();
        BindingContext = new TiendaViewModel();
    }
}
