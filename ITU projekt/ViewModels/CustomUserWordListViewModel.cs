using ITU_projekt.API;
using ITU_projekt.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ITU_projekt.ViewModels;

internal class CustomUserWordListViewModel : INotifyPropertyChanged
{
    private UnitModel model;

    // Use ObservableCollection to automatically update UI
    private ObservableCollection<Question> _userQuestions;
    public ObservableCollection<Question> UserQuestions
    {
        get => _userQuestions;
        set
        {
            if (_userQuestions != value)
            {
                _userQuestions = value;
                OnPropertyChanged(nameof(UserQuestions));  // Notify that the property has changed
            }
        }
    }

    public CustomUserWordListViewModel(UnitModel model)
    {
        this.model = model;
        UserQuestions = new ObservableCollection<Question>(model.UserQuestions);
    }

    // Implement INotifyPropertyChanged for automatic UI updates when property changes
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
