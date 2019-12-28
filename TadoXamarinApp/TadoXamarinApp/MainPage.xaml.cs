using System.ComponentModel;
using Xamarin.Forms;

namespace TadoXamarinApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;

        public MainPage()
        {
            void ShowLoginPage() => Navigation.PushAsync(new LoginPage(_viewModel.Model));
            _viewModel = new MainPageViewModel(ShowLoginPage);
            BindingContext = _viewModel;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (IsLoggedIn()) return;
            await _viewModel.Model.Initialise();
        }

        private bool IsLoggedIn()
        {
            if (string.IsNullOrEmpty(_viewModel.Model.Username)) return false;
            if (string.IsNullOrEmpty(_viewModel.Model.Password)) return false;
            return true;
        }
    }
}
