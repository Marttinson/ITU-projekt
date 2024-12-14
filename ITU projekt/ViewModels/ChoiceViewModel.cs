using ITU_projekt.API;
using ITU_projekt.Models;
using ITU_projekt.Templates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media; // Nezapomeň přidat tento using pro práci s Brush
using System.IO;

namespace ITU_projekt.ViewModels;
public class ChoiceViewModel : INotifyPropertyChanged
{
    // Vlastnosti barvy pozadí tlačítek sloužící pro odpovědi
    private Brush _button1Background = (Brush)Application.Current.Resources["PrimaryButtonBackground"];
    public Brush Button1Background
    {
        get => _button1Background;
        set
        {
            _button1Background = value;
            OnPropertyChanged(nameof(Button1Background));
        }
    }

    private Brush _button2Background = (Brush)Application.Current.Resources["PrimaryButtonBackground"];
    public Brush Button2Background
    {
        get => _button2Background;
        set
        {
            _button2Background = value;
            OnPropertyChanged(nameof(Button2Background));
        }
    }

    private Brush _button3Background = (Brush)Application.Current.Resources["PrimaryButtonBackground"];
    public Brush Button3Background
    {
        get => _button3Background;
        set
        {
            _button3Background = value;
            OnPropertyChanged(nameof(Button3Background));
        }
    }

    // Spojená viditelnost tlačítek volby
    private Visibility _buttonGroupVisibility = Visibility.Visible;
    public Visibility ButtonGroupVisibility
    {
        get => _buttonGroupVisibility;
        set
        {
            _buttonGroupVisibility = value;
            OnPropertyChanged(nameof(ButtonGroupVisibility));
        }
    }

    // Znění otázky
    private string _textToTranslate;
    public string TextToTranslate
    {
        get => _textToTranslate;
        set
        {
            _textToTranslate = value;
            OnPropertyChanged(nameof(TextToTranslate));
        }
    }

    // Vlastnost viditelnosti tlačítka sloužící pro přechod na další otázku
    private Visibility _continueButtonVisibility = Visibility.Collapsed;
    public Visibility ContinueButtonVisibility
    {
        get => _continueButtonVisibility;
        set
        {
            _continueButtonVisibility = value;
            OnPropertyChanged(nameof(ContinueButtonVisibility));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ICommand EvaluateAnswerCommand { get; }

    private PickFromThreeQuestion question;

    // Obsahy tlačítek volby
    public string Option1 { get; set; }
    public string Option2 { get; set; }
    public string Option3 { get; set; }

    private string unit;
    private MainWindowViewModel VM;
    private int turn;

    public ChoiceViewModel(MainWindowViewModel _VM, string _unit, ref int _turn)
    {
        EvaluateAnswerCommand = new RelayCommand(EvaluateAnswer);

        // Načítání otázek ze souborů
        JsonHandler jsonHandler = new JsonHandler();

        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string jsonPath = Path.Combine(appDataPath, "ITU", "Choice.json");

        unit = _unit;
        List<PickFromThreeQuestion> questions = jsonHandler.LoadOptionsQuestions(jsonPath, unit);
        Console.WriteLine(questions);

        QuestionUtils qutils = new QuestionUtils();

        question = qutils.GetRandomOptionsQuestions(questions, 1)[0];
        TextToTranslate = question.QuestionText;

        // Náhodné seřazení odpovědí na tlačítka
        List<int> numbers = Enumerable.Range(0, 3).ToList();
        Random random = new Random();
        numbers = numbers.OrderBy(x => random.Next()).ToList();

        Option1 = question.Options[numbers[0]];
        Option2 = question.Options[numbers[1]];
        Option3 = question.Options[numbers[2]];

        NextQuestion = new RelayCommand<object>(ExecuteNextQuestion);
        VM = _VM;
        turn = _turn;
    }

    // Funkce sloužící pro kontrolu, zda bylo zmáčknuto tlačítko se správnou odpovědí
    private void EvaluateAnswer(object parameter)
    {
        string selectedOption = parameter as string;

        switch (selectedOption)
        {
            case "Možnost 1":
                // Logika pro Možnost 1
                if (string.Equals(Option1, question.Answer, StringComparison.OrdinalIgnoreCase))
                {
                    // DOBŘE STAT
                    VM.incrementRight();
                    ButtonGroupVisibility = Visibility.Collapsed;
                    ContinueButtonVisibility = Visibility.Visible;
                }
                else
                {
                    // CHYBA STAT
                    VM.incrementWrong();
                    Button1Background = "Red";
                }
                break;
            case "Možnost 2":
                // Logika pro Možnost 2
                if (string.Equals(Option2, question.Answer, StringComparison.OrdinalIgnoreCase))
                {
                    // DOBŘE STAT
                    VM.incrementRight();
                    ButtonGroupVisibility = Visibility.Collapsed;
                    ContinueButtonVisibility = Visibility.Visible;
                }
                else
                {
                    // CHYBA STAT
                    VM.incrementWrong();
                    Button2Background = "Red";
                }
                break;
            case "Možnost 3":
                // Logika pro Možnost 3
                if (string.Equals(Option3, question.Answer, StringComparison.OrdinalIgnoreCase))
                {
                    // DOBŘE STAT
                    VM.incrementRight();
                    ButtonGroupVisibility = Visibility.Collapsed;
                    ContinueButtonVisibility = Visibility.Visible;
                }
                else 
                { 
                    // CHYBA STAT
                    VM.incrementWrong();
                    Button3Background = "Red";
                }
                break;
        }
    }

    // Funkce sloužící pro přechod na další otázku
    public ICommand NextQuestion { get; }
    private void ExecuteNextQuestion(object parameter)
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
}
