using ITU_projekt.API;
using ITU_projekt.Models;
using ITU_projekt.Templates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace ITU_projekt.ViewModels;
public class MemoryGameViewModel : INotifyPropertyChanged
{
    // Vlastnost pro tlačítko sloužící na návrat do menu, určuje jeho viditelnost
    private Visibility _doneButtonVisibility = Visibility.Collapsed;
    public Visibility DoneButtonVisibility
    {
        get => _doneButtonVisibility;
        set
        {
            if (_doneButtonVisibility != value)
            {
                _doneButtonVisibility = value;
                OnPropertyChanged(nameof(DoneButtonVisibility));
            }
        }
    }

    // Pole s daty tlačítek, jehož hodnoty se vkládají do Contentu a vypisují se přímo v tlačítku
    private ObservableCollection<string> _words;
    public ObservableCollection<string> Words
    {
        get => _words;
        set
        {
            _words = value;
            OnPropertyChanged();
        }
    }

    private List<TranslateWordQuestion> randomQuestions;
    public List<int> randomNumbers;
    private List<string> randomWords = new List<string>();

    private int[] previous = new int[2];

    public ICommand ButtonPressedCommand { get; }

    private MainWindowViewModel VM;

    public MemoryGameViewModel(MainWindowViewModel _VM)
    {
        // Načítání otázek pomocí funkcí dostupných z námi vytvořeného API
        JsonHandler jsonHandler = new JsonHandler();

        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string jsonPath = Path.Combine(appDataPath, "ITU", "TranslateWord.json");

        List<TranslateWordQuestion> questions = jsonHandler.LoadAllQuestions(jsonPath);

        QuestionUtils qutils = new QuestionUtils();

        randomQuestions = qutils.GetRandomTranslateWordQuestions(questions, 8);

        // Generování náhodného pořadí ze získaných 8 otázek, včetně toho, zda se použije znění otázky, nebo její překlad,
        // toto je nutnost pro úplnou náhodnost, jinak by byly otázky a odpovědi vedle sebe (je to za pomocí funkce FillWord)
        List<int> numbers = Enumerable.Range(0, 16).ToList();
        Random random = new Random();
        randomNumbers = numbers.OrderBy(x => random.Next()).ToList();

        for (int i = 0; i < 16; i++)
            randomWords.Add(FillWord(randomNumbers[i]));

        // Nastavení hodnoty prázdného řetězce do všech tlačítek
        Words = new ObservableCollection<string>
        {
            "", "", "", "",
            "", "", "", "",
            "", "", "", "",
            "", "", "", ""
        };

        // Definice základních hodnot, pro pozdější využití
        previous[0] = previous[1] = -1;
        VM = _VM;

        ButtonPressedCommand = new RelayCommand(ExecuteButtonPressed);
        Complete = new RelayCommand<object>(ExecuteComplete);
    }

    // Pomocná funkce pro naplnění pole náhodných otázek
    private string FillWord(int number)
    {
        // Funkce získá jako parametr hodnotu, pokud je menší než 8 použije se odpověd, v opačném případě se použije
        // znění otázky
        if (number < 8)
        {
            return randomQuestions[number].Answer;
        }
        else
        {
            return randomQuestions[number - 8].QuestionText;
        }
    }

    // Funkce volající se po stisku tlačítka
    private void ExecuteButtonPressed(object parameter)
    {
        string buttonIdentifier = parameter as string;
        int index = int.Parse(buttonIdentifier);

        // Kontrola, zda je obsah tlačítka prázdný řetězec, pokud ne uživatel kliknul na již odkryté políčko a nic se neděje
        if (Words[index] == "")
        {
            // Odkrytí políčka
            Words[index] = randomWords[index];

            if (previous[1] >= 0)   // Případ kdy jsou odkryté již obě políčka
            {
                QuestionUtils qutils = new QuestionUtils();

                // Najde se otázka, buď podle jejího znění, nebo podle odpovědi, kterou obsahuje stisknuté tlačítko
                TranslateWordQuestion q1 = qutils.FindTranslateWordQuestionByAnswer(randomQuestions, Words[previous[0]]);
                if (q1 == null)
                    q1 = qutils.FindTranslateWordQuestionByText(randomQuestions, Words[previous[0]]);

                // Najde se druhá otázka podle hodnoty druhého tlačítka
                TranslateWordQuestion q2 = qutils.FindTranslateWordQuestionByAnswer(randomQuestions, Words[previous[1]]);
                if (q2 == null)
                    q2 = qutils.FindTranslateWordQuestionByText(randomQuestions, Words[previous[1]]);

                // Porovnají se hodnoty ID dvou výše získaných otázek, a pokud jsou rozdílné hodnoty tlačítek se vynulují,
                // pokud se nerovnají tak se hodnoty tlačítek nemění a políčka zůstanou odkryta
                if (q1.ID != q2.ID)
                {
                    Words[previous[0]] = "";
                    Words[previous[1]] = "";
                }

                previous[0] = index;
                previous[1] = -1;
            }
            else if (previous[0] >= 0)  // Případ pro odkrytí prvního políčka z dvojice
                previous[1] = index;
            else    // Případ, který nastane pouze při odkrytí úplně prvního políčka, poté tuto část nahradí sekce v if
                previous[0] = index;

            // Kontrola, zda mají všechny tlačítka hodnotu
            if (Words.All(content => !string.IsNullOrWhiteSpace(content)))
                DoneButtonVisibility = Visibility.Visible;
        }
    }

    // Funkce sloužící pro návrat do menu
    public ICommand Complete { get; }
    private void ExecuteComplete(object parameter)
    {
        VM.CurrentUserControl = new UnitSelection(VM);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
