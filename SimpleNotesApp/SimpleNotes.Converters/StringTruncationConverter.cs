using System;
using System.Globalization;
using Xamarin.Forms;

namespace Converters
{
    // TODO: Add a converter test
    public class StringTruncationConverter : IValueConverter
    {
        private string? originalValue;

        public int Length { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string)value;
            this.originalValue = str;
            return str.Length >= this.Length ? $"{str.Substring(0, this.Length)}…" : str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.originalValue ?? (string)value;
        }
    }
}