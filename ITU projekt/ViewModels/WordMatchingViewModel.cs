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

namespace ITU_projekt.ViewModels;

public class WordMatchingViewModel : INotifyPropertyChanged
{
    private Brush _word1Background = new SolidColorBrush(Colors.Transparent);
    private Brush _word2Background = new SolidColorBrush(Colors.Transparent);
    private Brush _word3Background = new SolidColorBrush(Colors.Transparent);
    private Brush _word4Background = new SolidColorBrush(Colors.Transparent);
    private Brush _word5Background = new SolidColorBrush(Colors.Transparent);

    private Brush _slovo1Background = new SolidColorBrush(Colors.Transparent);
    private Brush _slovo2Background = new SolidColorBrush(Colors.Transparent);
    private Brush _slovo3Background = new SolidColorBrush(Colors.Transparent);
    private Brush _slovo4Background = new SolidColorBrush(Colors.Transparent);
    private Brush _slovo5Background = new SolidColorBrush(Colors.Transparent);

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

    public List<string> usedWord = new List<string>();
    public List<string> usedSlovo = new List<string>();

    private string unit;
    private MainWindowViewModel VM;
    public WordMatchingViewModel(MainWindowViewModel _VM, string _unit)
    {
        SelectWordButtonCommand = new RelayCommand<string>(OnWordButtonSelected);
        SelectSlovoButtonCommand = new RelayCommand<string>(OnSlovoButtonSelected);

        JsonHandler jsonHandler = new JsonHandler();

        // Musí se upravit cesta (teď nejde testovat, takže až se UC někde použije)
        string filePath = "Data/Anglictina";

        unit = _unit;
        List<TranslateWordQuestion> questions = jsonHandler.LoadTranslateWordQuestions(filePath + "/TranslateWord.json", unit);
        List<TranslateWordQuestion> userQuestions = jsonHandler.LoadTranslateWordUserQuestions(filePath + "/units.json", unit);
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
        NextQuestion = new RelayCommand<object>(ExecuteNextQuestion);
    }

    private void OnWordButtonSelected(string wordButtonValue)
    {
        if (!usedWord.Contains(wordButtonValue))
        {
            selectedWordButton = wordButtonValue;
            UpdateWordButtonColors();
        }

        if (!string.IsNullOrEmpty(selectedSlovoButton) && !string.IsNullOrEmpty(selectedWordButton))
        {
            EvaluateButtonPair(selectedWordButton, selectedSlovoButton);
        }
    }

    private void OnSlovoButtonSelected(string slovoButtonValue)
    {
        if (!usedSlovo.Contains(slovoButtonValue))
        {
            selectedSlovoButton = slovoButtonValue;
            UpdateSlovoButtonColors();
        }

        if (!string.IsNullOrEmpty(selectedWordButton) && !string.IsNullOrEmpty(selectedSlovoButton))
        {
            EvaluateButtonPair(selectedWordButton, selectedSlovoButton);
        }
    }

    private void EvaluateButtonPair(string word, string slovo)
    {
        if (randomQuestions[wordNumbers[int.Parse(word)]].ID == randomQuestions[slovoNumbers[int.Parse(slovo)]].ID)
        {
            SetButtonColor(word, slovo, Colors.Green);

            usedWord.Add(word);
            usedSlovo.Add(slovo);
        }

        if (usedWord.Count == 5)
            CompletionButtonVisibility = Visibility.Visible;

        // Vynulování
        selectedWordButton = "";
        selectedSlovoButton = "";
        UpdateWordButtonColors();
        UpdateSlovoButtonColors();
    }

    private void UpdateWordButtonColors()
    {
        // Resetovat barvy všech tlačítek na výchozí (transparentní)
        if(!usedWord.Contains("0"))
            Word1Background = new SolidColorBrush(Colors.Transparent);
        if (!usedWord.Contains("1"))
            Word2Background = new SolidColorBrush(Colors.Transparent);
        if (!usedWord.Contains("2"))
            Word3Background = new SolidColorBrush(Colors.Transparent);
        if (!usedWord.Contains("3"))
            Word4Background = new SolidColorBrush(Colors.Transparent);
        if (!usedWord.Contains("4"))
            Word5Background = new SolidColorBrush(Colors.Transparent);

        // Nastavit barvu pro aktuálně vybraná tlačítka
        if (selectedWordButton == "0") Word1Background = new SolidColorBrush(Colors.Gray);
        else if (selectedWordButton == "1") Word2Background = new SolidColorBrush(Colors.Gray);
        else if (selectedWordButton == "2") Word3Background = new SolidColorBrush(Colors.Gray);
        else if (selectedWordButton == "3") Word4Background = new SolidColorBrush(Colors.Gray);
        else if (selectedWordButton == "4") Word5Background = new SolidColorBrush(Colors.Gray);
    }

    private void UpdateSlovoButtonColors()
    {
        // Resetovat barvy všech tlačítek na výchozí (transparentní)
        if (!usedSlovo.Contains("0"))
            Slovo1Background = new SolidColorBrush(Colors.Transparent);
        if (!usedSlovo.Contains("1"))
            Slovo2Background = new SolidColorBrush(Colors.Transparent);
        if (!usedSlovo.Contains("2"))
            Slovo3Background = new SolidColorBrush(Colors.Transparent);
        if (!usedSlovo.Contains("3"))
            Slovo4Background = new SolidColorBrush(Colors.Transparent);
        if (!usedSlovo.Contains("4"))
            Slovo5Background = new SolidColorBrush(Colors.Transparent);

        // Nastavit barvu pro aktuálně vybraná tlačítka
        if (selectedSlovoButton == "0") Slovo1Background = new SolidColorBrush(Colors.Gray);
        else if (selectedSlovoButton == "1") Slovo2Background = new SolidColorBrush(Colors.Gray);
        else if (selectedSlovoButton == "2") Slovo3Background = new SolidColorBrush(Colors.Gray);
        else if (selectedSlovoButton == "3") Slovo4Background = new SolidColorBrush(Colors.Gray);
        else if (selectedSlovoButton == "4") Slovo5Background = new SolidColorBrush(Colors.Gray);
    }

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

    public ICommand NextQuestion { get; }
    private void ExecuteNextQuestion(object parameter)
    {
        Random random = new Random();
        int randomNumber = random.Next(1, 4);

        if (randomNumber == 1)
            VM.CurrentUserControl = new TranslateWord(VM, unit);
        else if (randomNumber == 2)
            VM.CurrentUserControl = new WordMatching(VM, unit);
        else if (randomNumber == 3)
            VM.CurrentUserControl = new Choice(VM, unit);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
