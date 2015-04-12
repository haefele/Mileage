using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using Mileage.Shared.Common;

namespace Mileage.Client.Windows.Converter
{
    public class StringWithHighlightsToTextBlockConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string == false)
                return DependencyProperty.UnsetValue;

            var input = (string)value;

            var result = new TextBlock();
            result.TextWrapping = TextWrapping.Wrap;

            while (input.Contains(Highlightings.StartTag))
            {
                int indexOfStartTag = input.IndexOf(Highlightings.StartTag, StringComparison.Ordinal);
                int endOfStartTag = indexOfStartTag + Highlightings.StartTag.Length;

                int indexOfEndTag = input.IndexOf(Highlightings.EndTag, StringComparison.Ordinal);
                int endOfEndTag = indexOfEndTag + Highlightings.EndTag.Length;

                var normalTextUpToHighlightedText = new Run(input.Substring(0, indexOfStartTag));
                result.Inlines.Add(normalTextUpToHighlightedText);

                var highlightedText = new Run(input.Substring(endOfStartTag, indexOfEndTag - endOfStartTag))
                {
                    FontWeight = FontWeights.SemiBold
                };
                result.Inlines.Add(highlightedText);

                input = input.Substring(endOfEndTag);
            }

            if (input.Length > 0)
            {
                var normalText = new Run(input);
                result.Inlines.Add(normalText);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}