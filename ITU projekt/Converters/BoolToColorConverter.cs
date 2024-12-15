/** BoolToColorConverter
 * V
 * Vojtěch Hrabovský (xhrabo18)
 * Konverter pro zbarvení ikony streaku
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ITU_projekt.Converters
{

    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCompleted)
            {
                return isCompleted ? Brushes.Green : Brushes.Gray;
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false; // Not needed for this scenario
        }
    }


}