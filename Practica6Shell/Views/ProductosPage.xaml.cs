using Practica6Shell.ViewModels;

namespace Practica6Shell.Views
{
    public partial class ProductosPage : ContentPage
    {
        private readonly ProductosViewModel _viewModel;

        public ProductosPage(ProductosViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.InicializarAsync();
        }
    }
}