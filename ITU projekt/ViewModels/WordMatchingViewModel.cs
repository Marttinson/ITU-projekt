using ITU_projekt.API;
using ITU_projekt.Models;
using ITU_projekt.Templates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;

namespace ITU_projekt.ViewModels;

public class WordMatchingViewModel : INotifyPropertyChanged
{
    //  Nastavení, aby bylo možné měnit barvu tlačítek, podle toho zda jsou stisknuty nebo spojeny
    private Brush _word1Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];
    private Brush _word2Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];
    private Brush _word3Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];
    private Brush _word4Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];
    private Brush _word5Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];

    private Brush _slovo1Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];
    private Brush _slovo2Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];
    private Brush _slovo3Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];
    private Brush _slovo4Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];
    private Brush _slovo5Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];

    public Brush Word1Background
    {
        get { return _word1Background; }
        set
        {
            if (_word1Background != value)
            {
                _word1Background = value;
                OnPropertyChanged(nameof(Word1Background));
            }
        }
    }

    public Brush Word2Background
    {
        get { return _word2Background; }
        set
        {
            if (_word2Background != value)
            {
                _word2Background = value;
                OnPropertyChanged(nameof(Word2Background));
            }
        }
    }

    public Brush Word3Background
    {
        get { return _word3Background; }
        set
        {
            if (_word3Background != value)
            {
                _word3Background = value;
                OnPropertyChanged(nameof(Word3Background));
            }
        }
    }

    public Brush Word4Background
    {
        get { return _word4Background; }
        set
        {
            if (_word4Background != value)
            {
                _word4Background = value;
                OnPropertyChanged(nameof(Word4Background));
            }
        }
    }

    public Brush Word5Background
    {
        get { return _word5Background; }
        set
        {
            if (_word5Background != value)
            {
                _word5Background = value;
                OnPropertyChanged(nameof(Word5Background));
            }
        }
    }

    public Brush Slovo1Background
    {
        get { return _slovo1Background; }
        set
        {
            if (_slovo1Background != value)
            {
                _slovo1Background = value;
                OnPropertyChanged(nameof(Slovo1Background));
            }
        }
    }

    public Brush Slovo2Background
    {
        get { return _slovo2Background; }
        set
        {
            if (_slovo2Background != value)
            {
                _slovo2Background = value;
                OnPropertyChanged(nameof(Slovo2Background));
            }
        }
    }

    public Brush Slovo3Background
    {
        get { return _slovo3Background; }
        set
        {
            if (_slovo3Background != value)
            {
                _slovo3Background = value;
                OnPropertyChanged(nameof(Slovo3Background));
            }
        }
    }

    public Brush Slovo4Background
    {
        get { return _slovo4Background; }
        set
        {
            if (_slovo4Background != value)
            {
                _slovo4Background = value;
                OnPropertyChanged(nameof(Slovo4Background));
            }
        }
    }

    public Brush Slovo5Background
    {
        get { return _slovo5Background; }
        set
        {
            if (_slovo5Background != value)
            {
                _slovo5Background = value;
                OnPropertyChanged(nameof(Slovo5Background));
            }
        }
    }

    // Vlastnost viditelnosti tlačítka sloužící na pokračování na další lekci
    private Visibility completionButtonVisibility = Visibility.Collapsed;
    public Visibility CompletionButtonVisibility
    {
        get => completionButtonVisibility;
        set
        {
            completionButtonVisibility = value;
            OnPropertyChanged(nameof(CompletionButtonVisibility));
        }
    }

    public ICommand SelectWordButtonCommand { get; }
    public ICommand SelectSlovoButtonCommand { get; }

    private string selectedWordButton;
    private string selectedSlovoButton;

    private List<TranslateWordQuestion> randomQuestions;

    // Obsah jednotlivých tlačítek
    public string Word1 { get; set; }
    public string Word2 { get; set; }
    public string Word3 { get; set; }
    public string Word4 { get; set; }
    public string Word5 { get; set; }

    public string Slovo1 { get; set; }
    public string Slovo2 { get; set; }
    public string Slovo3 { get; set; }
    public string Slovo4 { get; set; }
    public string Slovo5 { get; set; }

    public List<int> wordNumbers;
    public List<int> slovoNumbers;

    // Listy obsahující již spojené slova
    public List<string> usedWord = new List<string>();
    public List<string> usedSlovo = new List<string>();

    private string unit;
    private MainWindowViewModel VM;
    private int turn;

    public WordMatchingViewModel(MainWindowViewModel _VM, string _unit, ref int _turn)
    {
        SelectWordButtonCommand = new RelayCommand<string>(OnWordButtonSelected);
        SelectSlovoButtonCommand = new RelayCommand<string>(OnSlovoButtonSelected);

        // Načítání otázek ze souborů
        JsonHandler jsonHandler = new JsonHandler();

        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string jsonPath = Path.Combine(appDataPath, "ITU");

        unit = _unit;
        List<TranslateWordQuestion> questions = jsonHandler.LoadTranslateWordQuestions(jsonPath + "/TranslateWord.json", unit);
        List<TranslateWordQuestion> userQuestions = jsonHandler.LoadTranslateWordUserQuestions(jsonPath + "/lekce.json", unit);
        questions.AddRange(userQuestions);

        QuestionUtils qutils = new QuestionUtils();

        randomQuestions = qutils.GetRandomTranslateWordQuestions(questions, 5);

        // Vytvoření náhodného pořadí
        List<int> numbers = Enumerable.Range(0, 5).ToList();
        Random random = new Random();

        wordNumbers = numbers.OrderBy(x => random.Next()).ToList();
        Word1 = randomQuestions[wordNumbers[0]].Answer;
        Word2 = randomQuestions[wordNumbers[1]].Answer;
        Word3 = randomQuestions[wordNumbers[2]].Answer;
        Word4 = randomQuestions[wordNumbers[3]].Answer;
        Word5 = randomQuestions[wordNumbers[4]].Answer;

        slovoNumbers = numbers.OrderBy(x => random.Next()).ToList();
        Slovo1 = randomQuestions[slovoNumbers[0]].QuestionText;
        Slovo2 = randomQuestions[slovoNumbers[1]].QuestionText;
        Slovo3 = randomQuestions[slovoNumbers[2]].QuestionText;
        Slovo4 = randomQuestions[slovoNumbers[3]].QuestionText;
        Slovo5 = randomQuestions[slovoNumbers[4]].QuestionText;

        VM = _VM;
        turn = _turn;
        NextQuestion = new RelayCommand<object>(ExecuteNextQuestion);
    }

    // Funkce vykonávající se při stisku tlačítka na jedné straně
    private void OnWordButtonSelected(string wordButtonValue)
    {
        // Kontrola, zda uživatel nekliknul na již spojené tlačítko
        if (!usedWord.Contains(wordButtonValue))
        {
            selectedWordButton = wordButtonValue;
            UpdateWordButtonColors();
            UpdateSlovoButtonColors();
        }

        // Kontrola, zda není stisknuto tlačítko na obou stranách
        if (!string.IsNullOrEmpty(selectedSlovoButton) && !string.IsNullOrEmpty(selectedWordButton))
        {
            EvaluateButtonPair(selectedWordButton, selectedSlovoButton);
        }
    }

    // Funkce vykonávající se při stisku tlačítka na druhé straně
    private void OnSlovoButtonSelected(string slovoButtonValue)
    {
        // Kontrola, zda uživatel nekliknul na již spojené tlačítko
        if (!usedSlovo.Contains(slovoButtonValue))
        {
            selectedSlovoButton = slovoButtonValue;
            UpdateSlovoButtonColors();
            UpdateWordButtonColors();
        }

        // Kontrola, zda není stisknuto tlačítko na obou stranách
        if (!string.IsNullOrEmpty(selectedWordButton) && !string.IsNullOrEmpty(selectedSlovoButton))
        {
            EvaluateButtonPair(selectedWordButton, selectedSlovoButton);
        }
    }

    // Kontrola, zda jsou stisknutá tlačítka se stejném ID (souvisejí spolu) na obou stranách
    private void EvaluateButtonPair(string word, string slovo)
    {
        UpdateWordButtonColors();
        UpdateSlovoButtonColors();

        // Samotná kontrola a změna barvy
        if (randomQuestions[wordNumbers[int.Parse(word)]].ID == randomQuestions[slovoNumbers[int.Parse(slovo)]].ID)
        {
            // DOBŘE STAT
            SetButtonColor(word, slovo, Colors.Green);

            usedWord.Add(word);
            usedSlovo.Add(slovo);
        }
        else
        {
            // CHYBA STAT
            SetButtonColor(word, slovo, Colors.Red);
        }

        // Kontrola, zda nejsou již spojeny všechna slova
        if (usedWord.Count == 5)
            CompletionButtonVisibility = Visibility.Visible;

        // Vynulování
        selectedWordButton = "";
        selectedSlovoButton = "";
    }

    // Funkce měnící barvy tlačítek na jedné straně zpět na výchozí
    private void UpdateWordButtonColors()
    {
        // Resetovat barvy všech tlačítek na výchozí (transparentní)
        if(!usedWord.Contains("0"))
            Word1Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];
        if (!usedWord.Contains("1"))
            Word2Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];
        if (!usedWord.Contains("2"))
            Word3Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];
        if (!usedWord.Contains("3"))
            Word4Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];
        if (!usedWord.Contains("4"))
            Word5Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];

        // Nastavit barvu pro aktuálně vybraná tlačítka
        if (selectedWordButton == "0") Word1Background = new SolidColorBrush(Colors.Gray);
        else if (selectedWordButton == "1") Word2Background = new SolidColorBrush(Colors.Gray);
        else if (selectedWordButton == "2") Word3Background = new SolidColorBrush(Colors.Gray);
        else if (selectedWordButton == "3") Word4Background = new SolidColorBrush(Colors.Gray);
        else if (selectedWordButton == "4") Word5Background = new SolidColorBrush(Colors.Gray);
    }

    // Funkce měnící barvy tlačítek na druhé straně zpět na výchozí
    private void UpdateSlovoButtonColors()
    {
        // Resetovat barvy všech tlačítek na výchozí (transparentní)
        if (!usedSlovo.Contains("0"))
            Slovo1Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];
        if (!usedSlovo.Contains("1"))
            Slovo2Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];
        if (!usedSlovo.Contains("2"))
            Slovo3Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];
        if (!usedSlovo.Contains("3"))
            Slovo4Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];
        if (!usedSlovo.Contains("4"))
            Slovo5Background = (SolidColorBrush)Application.Current.Resources["PrimaryButtonBackground"];

        // Nastavit barvu pro aktuálně vybraná tlačítka
        if (selectedSlovoButton == "0") Slovo1Background = new SolidColorBrush(Colors.Gray);
        else if (selectedSlovoButton == "1") Slovo2Background = new SolidColorBrush(Colors.Gray);
        else if (selectedSlovoButton == "2") Slovo3Background = new SolidColorBrush(Colors.Gray);
        else if (selectedSlovoButton == "3") Slovo4Background = new SolidColorBrush(Colors.Gray);
        else if (selectedSlovoButton == "4") Slovo5Background = new SolidColorBrush(Colors.Gray);
    }

    // Funkce pro nastavení barvy tlačítka
    private void SetButtonColor(string word, string slovo, Color color)
    {
        // Nastavit barvu tlačítka na zelenou pro správně spárovaná tlačítka
        if (word == "0") Word1Background = new SolidColorBrush(color);
        if (word == "1") Word2Background = new SolidColorBrush(color);
        if (word == "2") Word3Background = new SolidColorBrush(color);
        if (word == "3") Word4Background = new SolidColorBrush(color);
        if (word == "4") Word5Background = new SolidColorBrush(color);

        if (slovo == "0") Slovo1Background = new SolidColorBrush(color);
        if (slovo == "1") Slovo2Background = new SolidColorBrush(color);
        if (slovo == "2") Slovo3Background = new SolidColorBrush(color);
        if (slovo == "3") Slovo4Background = new SolidColorBrush(color);
        if (slovo == "4") Slovo5Background = new SolidColorBrush(color);
    }

    // Funkce pro pokračování na další otázku
    public ICommand NextQuestion { get; }
    private void ExecuteNextQuestion(object parameter)
    {
        // Kontrola, zda již neproběhlo 10 otázek
        if (turn > 0)
        {
            if (turn == 10)
            {
                // Vrací se zpět do menu
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
