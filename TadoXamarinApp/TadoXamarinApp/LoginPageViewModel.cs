using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace TadoXamarinApp
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private readonly Action _exitPage;
        private readonly Command _loginCommand;
        private string _username;
        private string _password;

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginPageViewModel(Action exitPage)
        {
            _exitPage = exitPage;
            _loginCommand = new Command(LoginButtonClick, IsLoginSupplied);
        }

        public ICommand LoginCommand => _loginCommand;

        public string Username
        {
            get => _username;

            set
            {
                if (_username == value) return;
                _username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
                _loginCommand.ChangeCanExecute();
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
                _loginCommand.ChangeCanExecute();
            }
        }

        private async void LoginButtonClick()
        {
            // TODO: Call model to log in.

            _exitPage?.Invoke();
        }

        private bool IsLoginSupplied()
        {
            if (string.IsNullOrEmpty(Username)) return false;
            if (string.IsNullOrEmpty(Password)) return false;
            return true;
        }
    }
}