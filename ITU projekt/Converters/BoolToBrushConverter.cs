using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ITU_projekt.Converters;

public class BoolToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isEdited && isEdited)
        {
            return Brushes.LightYellow; // Highlight color
        }
        return Brushes.White; // Default background
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
