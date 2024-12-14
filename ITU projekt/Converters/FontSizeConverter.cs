using System;
using System.Globalization;
using System.Windows.Data;

namespace ITU_projekt.Converters;

public class FontSizeConverter : IValueConverter
{
    // Define a scaling factor to adjust the font size proportionally
    public double ScalingFactor { get; set; } = 0.05; // For example, font size is 5% of the width

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Check if the value is a valid double (representing the width of the element)
        if (value is double width)
        {
            // Scale the font size based on the width of the parent container
            return width * ScalingFactor;
        }
        return 12.0; // Default font size if the value is not valid
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // No need for the ConvertBack method in this case
        throw new NotImplementedException();
    }
}
