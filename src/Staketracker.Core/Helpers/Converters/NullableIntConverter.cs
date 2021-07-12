using System;
using System.Globalization;
using Xamarin.Forms;

namespace Staketracker.Core.Helpers.Converters
{
    public class NullableIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nullable = value as int?;
            var result = string.Empty;

            if (nullable.HasValue)
            {
                if (nullable == 0)
                    result = string.Empty;
                else
                    result = nullable.Value.ToString();
            }

            return result;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;
            int intValue;
            int? result = null;
            if (stringValue == string.Empty)
                stringValue = "0";

            if (int.TryParse(stringValue, out intValue))
            {
                result = new Nullable<int>(intValue);
            }

            return result;
        }

    }
}
