using Practica6Shell.ViewModels;

namespace Practica6Shell.Views
{
    public partial class DetalleProductoPage : ContentPage
    {
        private readonly DetalleProductoViewModel _viewModel;

        public DetalleProductoPage(DetalleProductoViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }
    }
}