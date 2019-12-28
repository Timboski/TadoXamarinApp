using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace TadoXamarinApp
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private int _numClicks = 0;
        private int _selectedTemperatureInCelcius = 5;
        private int _selectedOverrideTimeInSeconds = 3600;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel(Action showLoginPage)
        {
            TestClickCommand = new Command(TestButtonClick);
            Zone1TestCommand = new Command(SetOverride);
            Zone1CancelCommand = new Command(CancelOverride);
            AllOffCommand = new Command(AllOff);
            CancelAllOffCommand = new Command(CancelAllOverides);
            ShowLoginPageCommand = new Command(showLoginPage); 

            Model = new TadoController();
        }

        public ICommand TestClickCommand { get; }
        public ICommand Zone1TestCommand { get; }
        public ICommand Zone1CancelCommand { get; }
        public ICommand AllOffCommand { get; }
        public ICommand CancelAllOffCommand { get; }
        public ICommand ShowLoginPageCommand { get; }

        public TadoController Model { get; }

        private void TestButtonClick()
        {
            ++NumClicks;
        }

        private async void SetOverride()
        {
            var info = await Model.ReadInfo();
            await Model.SetOverlayTemperature(info, _selectedTemperatureInCelcius, _selectedOverrideTimeInSeconds, 1);
        }

        private async void CancelOverride()
        {
            var info = await Model.ReadInfo();
            await Model.CancelOverlayTemperature(info, 1);
        }

        private async void AllOff()
        {
            var info = await Model.ReadInfo();
            for (int z = 1; z <= 13; ++z) await Model.SetOverlayTemperature(info, _selectedTemperatureInCelcius, _selectedOverrideTimeInSeconds, z);
        }

        private async void CancelAllOverides()
        {
            var info = await Model.ReadInfo();
            for (int z = 1; z <= 13; ++z) await Model.CancelOverlayTemperature(info, z);
        }

        public int NumClicks
        {
            get => _numClicks;

            set
            {
                if (_numClicks == value) return;
                _numClicks = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumClicks)));
            }
        }

        public int SelectedTemperature
        {
            get => _selectedTemperatureInCelcius;

            set
            {
                if (_selectedTemperatureInCelcius == value) return;
                _selectedTemperatureInCelcius = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTemperature)));
            }
        }

        public int SelectedOverridePeriod
        {
            get => _selectedOverrideTimeInSeconds;

            set
            {
                if (_selectedOverrideTimeInSeconds == value) return;
                _selectedOverrideTimeInSeconds = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedOverridePeriod)));
            }
        }
    }
}
