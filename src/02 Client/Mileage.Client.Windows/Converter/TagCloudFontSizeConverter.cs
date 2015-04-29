using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Mileage.Client.Windows.Converter
{
    public class TagCloudFontSizeConverter : IMultiValueConverter
    {
        private const int MinFontSize = 12;
        private const int MaxFontSize = 40;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int == false ||
                values[1] is int == false ||
                values[2] is int == false)
                return DependencyProperty.UnsetValue;

            var current = (int)values[0];
            var min = (int)values[1];
            var max = (int)values[2];

            float percentageFromMinToMax = (current - min) / (float)(max - min);
            return (double)(MinFontSize + (percentageFromMinToMax * (MaxFontSize - MinFontSize)));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}