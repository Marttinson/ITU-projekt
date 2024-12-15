/**
 * Question
 * M
 * Vojtěch Hrabovský (xhrabo18)
 * Slouží k prezentaci uživatelem vytvořených otázek
 */


using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ITU_projekt.Models
{
    /// <summary>
    /// Class for representing user's custom questions
    /// </summary>
    public class Question : INotifyPropertyChanged
    {
        // ID
        public int ID { get; set; }

        // Question text
        private string _questionText;
        public string QuestionText
        {
            get => _questionText;
            set
            {
                if (_questionText != value)
                {
                    _questionText = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsModified));  // Notify 
                }
            }
        }

        // Answer
        private string _answer;
        public string Answer
        {
            get => _answer;
            set
            {
                if (_answer != value)
                {
                    _answer = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsModified));  // Notify
                }
            }
        }

        // Are there multiple questions with the same text
        private bool _hasDuplicate;
        public bool HasDuplicate
        {
            get => _hasDuplicate;
            set
            {
                if (_hasDuplicate != value)
                {
                    _hasDuplicate = value;
                    OnPropertyChanged();
                }
            }
        }

        // Has the user modified this question since last save
        private bool _isModified;
        public bool IsModified
        {
            get => _isModified;
            set
            {
                if (_isModified != value)
                {
                    _isModified = value;
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
