using Practica6Shell.Views;

namespace Practica6Shell
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegistrarRutas();
        }

        private void RegistrarRutas()
        {
            Routing.RegisterRoute("detalleproducto", typeof(DetalleProductoPage));
            Routing.RegisterRoute("editarproducto", typeof(EditarProductoPage));
            Routing.RegisterRoute("configuracion", typeof(ConfiguracionPage));
        }

        private async void OnHelpClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Ayuda", "Practica 6: Navegacion en .NET MAUI", "OK");
        }
    }
}