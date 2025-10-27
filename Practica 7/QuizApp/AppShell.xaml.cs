using QuizApp.Views;

namespace QuizApp
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
            Routing.RegisterRoute("quiz", typeof(QuizPage));
            Routing.RegisterRoute("resultado", typeof(ResultadoPage));
            Routing.RegisterRoute("historial", typeof(MainPage)); // Por simplicidad redirige al main
            Routing.RegisterRoute("estadisticas", typeof(MainPage)); // Por simplicidad redirige al main
        }
    }
}
