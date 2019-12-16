using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TadoXamarinApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private LoginPageViewModel _viewModel;

        public LoginPage()
        {
            void ExitPage() => Navigation.PopAsync();
            _viewModel = new LoginPageViewModel(ExitPage);
            BindingContext = _viewModel;
            InitializeComponent();
        }
    }
}