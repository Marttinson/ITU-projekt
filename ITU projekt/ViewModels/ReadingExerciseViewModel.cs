/* ReadingExerciseViewModel
 * VM
 * Vojtěch Hrabovský (xhrabo18)
 * 
 * VM - Sets text, checks answers, starts next task
 */

using ITU_projekt.API;
using ITU_projekt.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media;
using ITU_projekt.Templates;

namespace ITU_projekt.ViewModels;


public class ReadingExerciseViewModel : INotifyPropertyChanged
{

    // Proměnné pro zobrazení výstupu - informování uživatele 
    private Brush _answerBarBackground = Brushes.Transparent;
    public Brush AnswerBarBackground
    {
        get => _answerBarBackground;
        set
        {
            if (_answerBarBackground != value)
            {
                _answerBarBackground = value;
                OnPropertyChanged(nameof(AnswerBarBackground));
            }
        }
    }

    private Visibility _answerVisibility = Visibility.Hidden;
    public Visibility AnswerVisibility
    {
        get => _answerVisibility;
        set
        {
            if (_answerVisibility != value)
            {
                _answerVisibility = value;
                OnPropertyChanged(nameof(AnswerVisibility));
            }
        }
    }

    private string _answerText = "";
    public string AnswerText
    {
        get => _answerText;
        set
        {
            if(_answerText != value) 
            { 
                _answerText = value;
                OnPropertyChanged(nameof(AnswerText));
            }
        }
    }

    // Task text (zadání)
    private string _Text = "";
    public string Text
    {
        get => _Text;
        set
        {
            if (_Text != value)
            {
                _Text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
    }

    private string unit;
    private MainWindowViewModel VM;
    private int turn;

    // Kolekce otázek a uživatelských odpovědí
    public ObservableCollection<ExerciseStatement> Statements { get; set; }

    /// <summary>
    /// Initializes instance
    /// </summary>
    /// <param name="_VM"> MainWindowViewModel </param>
    /// <param name="_unit"> Unit ID as a string "Unit {id}"</param>
    /// <param name="_turn"> Task count </param>
    public ReadingExerciseViewModel(MainWindowViewModel _VM, string _unit, ref int _turn)
    {
        // Pro načtení otázek ze souborů, pro konkrétní lekci
        JsonHandler jsonHandler = new JsonHandler();

        unit = _unit;
        VM = _VM;
        turn = _turn;

        // Set text, questions, correct answers from json
        string uid = unit.Substring(5);
        switch (unit)
        {
            case "Unit 1":
                Text = jsonHandler.LoadReadingExercise(uid);
                Statements = new ObservableCollection<ExerciseStatement>(JsonHandler.LoadExerciseStatements(uid, "Reading_statements.json")); // Convert to ObservableCollection<T>
                break;
            case "Unit 2":
                Text = jsonHandler.LoadReadingExercise(uid);
                Statements = new ObservableCollection<ExerciseStatement>(JsonHandler.LoadExerciseStatements(uid, "Reading_statements.json")); // Convert to ObservableCollection<T>
                break;
            case "Unit 3":
                Text = jsonHandler.LoadReadingExercise(uid);
                Statements = new ObservableCollection<ExerciseStatement>(JsonHandler.LoadExerciseStatements(uid, "Reading_statements.json")); // Convert to ObservableCollection<T>
                break;
        }
    }

    // Checks correctness on the answer
    public void CheckAnswer()
    {
        int correctAnswers = 0;


        foreach (var statement in Statements)
        {
            // Check if the user's answer matches the correct answer
            if (statement.UserAnswer == statement.answer)
            {
                correctAnswers++;
            }
        }


       if(correctAnswers == Statements.Count)
        {
            // All correct
            AnswerBarBackground = Brushes.Green;
            AnswerText = "Correct answer!";
            AnswerVisibility = Visibility.Visible;
            // Delay // Go to next
            ExecuteNextQuestion();
        }
        else
        {
            // Wrong answers
            AnswerBarBackground = Brushes.Red;
            AnswerText = "Wrong answer!";
            AnswerVisibility = Visibility.Visible;
        }

    }

    // When user changes their input
    public void resetAnswer()
    {
        AnswerBarBackground = Brushes.Transparent;
        AnswerText = "";
        AnswerVisibility = Visibility.Hidden;
    }

    // Next Task
    private void ExecuteNextQuestion()
    {
        // Kontrola, zda již neproběhlo 10 otázek
        if (turn > 0)
        {
            if (turn == 10)
            {
                VM.LessonFinished();
                VM.CurrentUserControl = new UnitSelection(VM);
                return;
            }
            else
                turn++;
        }

        // Vygenerování náhodného čísla v intervalu <1; 3> a podle toho zvolení následující otázky,
        // všechny mají stejnou pravděpodobnost
        Random random = new Random();
        int randomNumber = random.Next(1, 4);

        if (randomNumber == 1)
            VM.CurrentUserControl = new TranslateWord(VM, unit, ref turn);
        else if (randomNumber == 2)
            VM.CurrentUserControl = new WordMatching(VM, unit, ref turn);
        else if (randomNumber == 3)
            VM.CurrentUserControl = new Choice(VM, unit, ref turn);
    }

    // INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}