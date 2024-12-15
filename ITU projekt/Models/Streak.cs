/**
 * Streak
 * M
 * Vojtěch Hrabovský (xhrabo18)
 * Slouží k prezentaci uživatelského streaku (po sobě jdoucí dny, kdy byl uživatel aktivní)
 */

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ITU_projekt.Models
{
    /// <summary>
    /// Class for representing Streak Information
    /// </summary>
    public class Streak
    {
        // Last lesson completed date
        public DateTime last_date;
        private int _length;

        // Streak length
        public int length
        {
            get => _length;
            set
            {
                if (_length != value)
                {
                    _length = value;
                    OnPropertyChanged();
                }
            }
        }

        // Add INotifyPropertyChanged for proper binding
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
