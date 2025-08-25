using HTTPmaui.ViewModels;

namespace HTTPmaui
{
    public partial class MainPage : ContentPage
    {
        // En MVVM el code-behind queda mínimo. Aquí solo resolvemos el ViewModel
        // desde el contenedor de dependencias y lo asignamos como BindingContext.
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
