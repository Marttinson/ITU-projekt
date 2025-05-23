﻿/**EmptyTextConverter
 * V
 * Vojtěch Hrabovský (xhrabo18)
 * Konverter pro zbarvení prázdných polí při editaci uživatelských otázek
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ITU_projekt.Converters;

public class EmptyTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.IsNullOrEmpty(value?.ToString());
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}

