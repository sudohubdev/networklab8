using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using NetworkLabAPI.Models;

namespace NetworkLabAPI.Converters
{

    public class LanguagesConverter : IValueConverter  // Must be public
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                IEnumerable<Currency> currencies => string.Join(", ", currencies.Select(c => c.Name)),
                IEnumerable<string> languages => string.Join(", ", languages),
                _ => string.Empty
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}