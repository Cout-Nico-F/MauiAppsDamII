using QuizApp.ViewModels;

namespace QuizApp.Views
{
    public partial class ResultadoPage : ContentPage
    {
        private readonly ResultadoViewModel _viewModel;

        public ResultadoPage(ResultadoViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }
    }
}