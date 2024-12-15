/* CustomUserWordListViewModel
 * VM
 * Vojtěch Hrabovský (xhrabo18)
 * 
 * VM - Handles user question edit, add, remove, display
 */

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System;
using System.Windows;

using ITU_projekt.API;
using ITU_projekt.Models;

public class CustomUserWordListViewModel : INotifyPropertyChanged
{
    // Questions that will be shown, edited
    public ObservableCollection<Question> UserQuestions { get; set; }
    // List with reversed IDs (newly added question has biggest id but must be shown first)
    public ObservableCollection<Question> ReversedQuestions { get; set; }

    // Control commands
    public ICommand AddNewQuestionCommand { get; }
    public ICommand DeleteQuestionCommand { get; }
    public ICommand SaveQuestionsCommand { get; }

    // Unit with questions
    private UnitModel unitMod;

    /// <summary>
    /// initialize instance
    /// </summary>
    /// <param name="model">UnitModel with questions</param>
    public CustomUserWordListViewModel(UnitModel model)
    {
        unitMod = model;
        UserQuestions = new ObservableCollection<Question>(model.UserQuestions); // list of user questions
        ReversedQuestions = new ObservableCollection<Question>(UserQuestions.Reverse()); // Initialize reversed list (new questions should be on top thus reverse)

        AddNewQuestionCommand = new RelayCommand((obj) => AddNewQuestion());
        SaveQuestionsCommand = new RelayCommand((obj) => SaveQuestions());
        DeleteQuestionCommand = new RelayCommand<int>(DeleteQuestion);
    }

    // Add question to unit model
    public void AddNewQuestion()
    {
        // Create a new question, add to collection
        var newQuestion = new Question { ID = UserQuestions.Count + 10000 };
        UserQuestions.Add(newQuestion);

        // Add the new question at the beginning of the reversed list
        ReversedQuestions.Insert(0, newQuestion);

        // Check duplicates
        CheckForDuplicates();
    }

    // Check duplicate values (in question text)
    public void CheckForDuplicates()
    {
        foreach (var question in UserQuestions)
        {
            // If duplicate values in QuestionText, set HasDuplicate to true
            // This informs View and box is colored blue (BoolToColor converter)
            question.HasDuplicate = UserQuestions.Count(q => q.QuestionText == question.QuestionText) > 1;
        }
    }

    // Save questions to JSON
    private void SaveQuestions()
    {
        // Validate
        if (!ValidateQuestions())
        {
            // No duplicate questions, no empty fields
            MessageBox.Show("Please fill all fields and make sure there are no duplicate questions.");
            return;
        }

        // Save logic
        JsonHandler.SaveUserQuestions(unitMod.ID, UserQuestions);
    }

    // Validate
    public bool ValidateQuestions()
    {
        CheckForDuplicates();

        foreach (var question in UserQuestions)
        {
            // Both values must be filled
            if (string.IsNullOrEmpty(question.QuestionText) || string.IsNullOrEmpty(question.Answer))
            {
                return false;
            }

            // No duplicit QuestionText values
            if (UserQuestions.Count(q => q.QuestionText == question.QuestionText) > 1)
            {
                return false;
            }
        }
        return true;
    }

    // Delete question
    public void DeleteQuestion(int id)
    {
        // Find question
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

    // Update Reversed Questions List
    private void UpdateReversedQuestions()
    {
        // Reverse the UserQuestions list
        var reversedList = UserQuestions.Reverse().ToList(); 
        // Clear list
        ReversedQuestions.Clear();  

        foreach (var question in reversedList)
        {
            // Add each element from UserQuestions to reversed list
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
