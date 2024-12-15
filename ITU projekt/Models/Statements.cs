/** Statements
 * M
 * Vojtěch Hrabovský (xhrabo18)
 * 
 * Model pro ukládání statementů k poslechu/čtění
 */

using System.ComponentModel;

namespace ITU_projekt.Models;

public class ExerciseStatement : INotifyPropertyChanged
{
    public string Text { get; set; }
    public int id { get; set; }
    public bool answer { get; set; }

    private bool? _userAnswer; // nullable bool to handle no answer selected
    public bool? UserAnswer
    {
        get => _userAnswer;
        set
        {
            if (_userAnswer != value)
            {
                _userAnswer = value;
                OnPropertyChanged(nameof(UserAnswer)); // Notify the UI of the change
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}