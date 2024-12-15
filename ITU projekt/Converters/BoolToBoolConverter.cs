using System.Globalization;
using System.Windows.Data;
using System;

namespace ITU_projekt.Converters;

public class BoolToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            // If the boolean value is true, return the value of the converter parameter (true/false)
            if (parameter != null && parameter.ToString() == boolValue.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false; // default if the value is not a boolean
    }

    // Converts back, not needed
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            // Return value based on button state
            return boolValue;
        }

        return false; // default
    }
}
