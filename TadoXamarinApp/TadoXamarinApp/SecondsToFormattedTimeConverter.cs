using System;
using System.Globalization;
using Xamarin.Forms;

namespace TadoXamarinApp
{
    /// <summary>
    /// Convert from: A time span in seconds (as an int)
    /// Convert to: A fomatted time period Hours, Minutes and Seconds (as a string) 
    /// </summary>
    public class SecondsToFormattedTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => TimeSpan.FromSeconds((int)value).ToString();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
