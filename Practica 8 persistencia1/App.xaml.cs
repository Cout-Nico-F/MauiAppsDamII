?using Practica_8_persistencia1.Services;

namespace Practica_8_persistencia1
{
    public partial class App : Application
    {
        public App(DatabaseService databaseService)
        {
            InitializeComponent();

            MainPage = new MainPage(databaseService);
        }
    }
}