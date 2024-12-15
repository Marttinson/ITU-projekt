/* ListeningExerciseViewModel
 * VM
 * Vojtěch Hrabovský (xhrabo18)
 * 
 * VM - Loads audio tape, checks answers, starts next task
 */

using System;
using System.ComponentModel;
using System.Windows;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Media;

using ITU_projekt.Templates;
using ITU_projekt.API;
using ITU_projekt.Models;

namespace ITU_projekt.ViewModels;


public class ListeningExerciseViewModel : INotifyPropertyChanged
{

    /* Proměnné pro zobrazení výstupu - informování uživatele */
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
    public ListeningExerciseViewModel(MainWindowViewModel _VM, string _unit, ref int _turn)
    {
        unit = _unit;
        VM = _VM;
        turn = _turn;

        // Set questions, correct answers from json
        string uid = unit.Substring(5);
        switch (unit)
        {
            case "Unit 1":
                Statements = new ObservableCollection<ExerciseStatement>(JsonHandler.LoadExerciseStatements(uid, "Listening_statements.json")); // Convert to ObservableCollection<T>
                break;
            case "Unit 2":
                Statements = new ObservableCollection<ExerciseStatement>(JsonHandler.LoadExerciseStatements(uid, "Listening_statements.json")); // Convert to ObservableCollection<T>
                break;
            case "Unit 3":
                Statements = new ObservableCollection<ExerciseStatement>(JsonHandler.LoadExerciseStatements(uid, "Listening_statements.json")); // Convert to ObservableCollection<T>
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
    public void ExecuteNextQuestion()
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
            {
                turn++;
                VM.SetBackToMenuVisible();
                // Next task is reading 
                VM.CurrentUserControl = new ReadingExercise(VM, unit, ref turn);
                return;
            }
        }

        Debug.WriteLine("TURN: " + turn);

        // Vygenerování náhodného čísla v intervalu <1; 4> a podle toho zvolení následující otázky,
        // všechny mají stejnou pravděpodobnost
        Random random = new Random();
        int randomNumber = random.Next(1, 5);

        if (randomNumber == 1)
            VM.CurrentUserControl = new TranslateWord(VM, unit, ref turn);
        else if (randomNumber == 2)
            VM.CurrentUserControl = new WordMatching(VM, unit, ref turn);
        else if (randomNumber == 3)
            VM.CurrentUserControl = new Choice(VM, unit, ref turn);
        else if (randomNumber == 4)
            VM.CurrentUserControl = new SentenceMaking(VM, unit, ref turn);
    }



    // Gets correct audio tape
    public string getAudio()
    {
        return JsonHandler.getAudio(unit);
    }


    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}