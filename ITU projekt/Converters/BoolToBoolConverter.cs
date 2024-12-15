using System.Globalization;
using System.Windows.Data;
using System;

namespace ITU_projekt.Converters;

public class BoolToBoolConverter : IValueConverter
{
    // Converts the boolean value to a boolean nullable value (true/false/null)
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

    // Converts back, not needed in this case as we are only interested in binding the radio buttons
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            // Return the boolean value based on the radio button state (true/false)
            return boolValue;
        }

        return false; // default
    }
}
