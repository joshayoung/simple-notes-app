using System;
using System.Globalization;
using Xamarin.Forms;

namespace SimpleNotes.Converters
{
    public class StringTruncationConverter : IValueConverter
    {
        public int Length { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string)value;
            return str.Length >= this.Length ? $"{str.Substring(0, this.Length)}…" : str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}