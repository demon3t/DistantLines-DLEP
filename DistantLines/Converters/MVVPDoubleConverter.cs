using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace WpfApp.Converters
{
    internal class MVVPDoubleConverter
    {
        [ValueConversion(typeof(double), typeof(string))]
        public class DoubleConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                double dValue = (double)value;
                if (double.IsNaN(dValue))
                    return "";
                return dValue.ToString().Replace('.', ',');
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is string str)
                {
                    str = str.Replace('.', ',');
                    if (double.TryParse(str, out double result))
                    {
                        return result;
                    }
                }
                return double.NaN;
            }
        }
    }
}
