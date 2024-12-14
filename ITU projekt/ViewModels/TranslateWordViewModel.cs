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
using System.IO;

namespace ITU_projekt.ViewModels;
public class TranslateWordViewModel : INotifyPropertyChanged
{
    // Text, který se vypíše uživateli, a obsahuje co má uživatel překládat
    private string _wordToTranslate;
    public string WordToTranslate
    {
        get => _wordToTranslate;
        set
        {
            _wordToTranslate = value;
            OnPropertyChanged(nameof(WordToTranslate));
        }
    }

    // Vlastnost pro viditelnost tlačítka na validaci odpovědi
    private Visibility buttonOdpovedetVisibility = Visibility.Visible;
    public Visibility ButtonOdpovedetVisibility
    {
        get => buttonOdpovedetVisibility;
        set
        {
            buttonOdpovedetVisibility = value;
            OnPropertyChanged(nameof(ButtonOdpovedetVisibility));
        }
    }

    // Vlastnost pro viditelnost tlačítka sloužící pro pokračování na další otázku
    private Visibility buttonDalsiVisibility = Visibility.Collapsed;
    public Visibility ButtonDalsiVisibility
    {
        get => buttonDalsiVisibility;
        set
        {
            buttonDalsiVisibility = value;
            OnPropertyChanged(nameof(ButtonDalsiVisibility));
        }
    }

    // Hodnota, kterou uživatel zadal do textového pole po kliknutí na tlačítko pro vyhodnocení odpovědi
    private string _userAnswer;
    public string UserAnswer
    {
        get => _userAnswer;
        set
        {
            _userAnswer = value;
            OnPropertyChanged(nameof(UserAnswer));
        }
    }

    private TranslateWordQuestion question;
    private string unit;
    MainWindowViewModel VM;
    private int turn;

    public TranslateWordViewModel(MainWindowViewModel _VM, string _unit, ref int _turn)
    {
        // Načtení otázek ze souborů, pro konkrétní lekci
        JsonHandler jsonHandler = new JsonHandler();

        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string jsonPath = Path.Combine(appDataPath, "ITU");

        unit = _unit;
        List<TranslateWordQuestion> questions = jsonHandler.LoadTranslateWordQuestions(jsonPath + "/TranslateWord.json", unit);
        List<TranslateWordQuestion> userQuestions = jsonHandler.LoadTranslateWordUserQuestions(jsonPath + "/lekce.json", unit);
        questions.AddRange(userQuestions);

        QuestionUtils qutils = new QuestionUtils();

        // Vybrání náhodné otázky
        question = qutils.GetRandomTranslateWordQuestions(questions, 1)[0];
        WordToTranslate = question.QuestionText;

        EvaluateAnswerCommand = new RelayCommand(_ => EvaluateAnswer());
        NextQuestion = new RelayCommand<object>(ExecuteNextQuestion);
        VM = _VM;
        turn = _turn;
    }

    // Funkce kontrolující, zda je zadaný překlad správně
    public ICommand EvaluateAnswerCommand { get; }
    public void EvaluateAnswer()
    {
        if (string.Equals(UserAnswer, question.Answer, StringComparison.OrdinalIgnoreCase)) // Nahraďte skutečnou logikou
        {
            // DOBŘE STAT
            VM.incrementRight();
            ButtonOdpovedetVisibility = Visibility.Collapsed;
            ButtonDalsiVisibility = Visibility.Visible;
        }
        else
        {
            // CHYBA STAT
            VM.incrementWrong();
            MessageBox.Show("Špatná odpověď, zkuste to znovu.");
        }
    }

    // Funkce sloužící pro pokračování na další otázku
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

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

