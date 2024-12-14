using ITU_projekt.API;
using ITU_projekt.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Diagnostics;
using System;
using System.Windows;

public class CustomUserWordListViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Question> UserQuestions { get; set; }
    public ObservableCollection<Question> ReversedQuestions { get; set; } // Changed to ObservableCollection

    public ICommand AddNewQuestionCommand { get; }
    public ICommand DeleteQuestionCommand { get; }
    public ICommand SaveQuestionsCommand { get; }

    private UnitModel unitMod;

    public CustomUserWordListViewModel(UnitModel model)
    {
        unitMod = model;
        UserQuestions = new ObservableCollection<Question>(model.UserQuestions);
        ReversedQuestions = new ObservableCollection<Question>(UserQuestions.Reverse()); // Initialize reversed list

        AddNewQuestionCommand = new RelayCommand((obj) => AddNewQuestion());
        SaveQuestionsCommand = new RelayCommand((obj) => SaveQuestions());
        DeleteQuestionCommand = new RelayCommand<int>(DeleteQuestion);
    }

    public void AddNewQuestion()
    {
        var newQuestion = new Question { ID = UserQuestions.Count + 10000 };
    UserQuestions.Add(newQuestion);

    // Add the new question at the beginning of the reversed list
    ReversedQuestions.Insert(0, newQuestion);

    CheckForDuplicates();
    }

    public void CheckForDuplicates()
    {
        foreach (var question in UserQuestions)
        {
            question.HasDuplicate = UserQuestions.Count(q => q.QuestionText == question.QuestionText) > 1;
        }
    }

    private void SaveQuestions()
    {
        if (!ValidateQuestions())
        {
            MessageBox.Show("Please fill all fields and make sure there are no duplicate questions.");
            return;
        }

        // Save logic
        if (JsonHandler.SaveUserQuestions(unitMod.ID, UserQuestions))
        {
            
        }
    }

    public bool ValidateQuestions()
    {
        CheckForDuplicates();
        foreach (var question in UserQuestions)
        {
            if (string.IsNullOrEmpty(question.QuestionText) || string.IsNullOrEmpty(question.Answer))
            {
                return false;
            }

            if (UserQuestions.Count(q => q.QuestionText == question.QuestionText) > 1)
            {
                return false;
            }
        }
        return true;
    }

    public void DeleteQuestion(int id)
    {
        var questionToDelete = UserQuestions.FirstOrDefault(q => q.ID == id);
        if (questionToDelete != null)
        {
            // Remove the question from UserQuestions
            UserQuestions.Remove(questionToDelete);

            // Shift IDs for all subsequent questions in UserQuestions
            foreach (var question in UserQuestions.Where(q => q.ID > id))
            {
                question.ID -= 1;
            }

            // Rebuild the reversed list after deletion
            UpdateReversedQuestions();

            // Recheck for duplicates and update visibility of the save button
            CheckForDuplicates();
        }
    }

    private void UpdateReversedQuestions()
    {
        var reversedList = UserQuestions.Reverse().ToList();  // Reverse the UserQuestions list
        ReversedQuestions.Clear();
        foreach (var question in reversedList)
        {
            ReversedQuestions.Add(question);
        }
        OnPropertyChanged(nameof(ReversedQuestions));  // Notify the UI of the update
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
