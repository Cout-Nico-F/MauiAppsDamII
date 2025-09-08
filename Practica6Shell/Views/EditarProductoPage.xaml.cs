namespace Practica6Shell.Views
{
    public partial class EditarProductoPage : ContentPage, IQueryAttributable
    {
        public EditarProductoPage()
        {
            InitializeComponent();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("producto") && query["producto"] is Models.Producto producto)
            {
                NombreEntry.Text = producto.Nombre;
                DescripcionEditor.Text = producto.Descripcion;
                PrecioEntry.Text = producto.Precio.ToString("F2");
                CategoriaPicker.SelectedItem = producto.Categoria;
                EnStockSwitch.IsToggled = producto.EnStock;
            }
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Guardar", "Producto guardado correctamente", "OK");
            await Shell.Current.GoToAsync("..");
        }

        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}