using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace TadoXamarinApp
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private Action _exitPage;
        private string _username;
        private string _password;

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginPageViewModel(Action exitPage)
        {
            _exitPage = exitPage;
            LoginCommand = new Command(LoginButtonClick);
        }

        public ICommand LoginCommand { get; }

        public string Username
        {
            get => _username;

            set
            {
                if (_username == value) return;
                _username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
            }
        }

        public string Password
        {
            get => _password;

            set
            {
                if (_password == value) return;
                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        private async void LoginButtonClick()
        {
            // TODO: Call model to log in.

            _exitPage?.Invoke();
        }
    }
}