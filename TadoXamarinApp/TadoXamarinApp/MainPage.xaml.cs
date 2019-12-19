using System.ComponentModel;
using Xamarin.Forms;

namespace TadoXamarinApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _viewModel = new MainPageViewModel();

        public MainPage()
        {
            BindingContext = _viewModel;
            InitializeComponent();

            Navigation.PushAsync(new LoginPage(_viewModel.Model));
        }
    }
}
