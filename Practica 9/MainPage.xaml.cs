using Practica9.Pages;
using Practica9.ViewModels;

namespace Practica_9
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new object(); // Página estática de bienvenida
        }

        private async void OnCalculadoraClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CalculadoraPage());
        }

        private async void OnTiendaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TiendaPage());
        }
    }
}
