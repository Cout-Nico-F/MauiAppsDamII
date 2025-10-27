?using Practica_8_persistencia1.Models;
using Practica_8_persistencia1.Services;

namespace Practica_8_persistencia1
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public MainPage(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            LoadPeople();
        }

        private async void OnAddPersonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameEntry.Text))
            {
                await _databaseService.SavePersonAsync(new Person { Name = NameEntry.Text });
                NameEntry.Text = string.Empty;
                LoadPeople();
            }
        }

        private async void LoadPeople()
        {
            PeopleListView.ItemsSource = await _databaseService.GetPeopleAsync();
        }
    }
}