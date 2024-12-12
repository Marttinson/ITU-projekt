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

namespace ITU_projekt.ViewModels;
public class MemoryGameViewModel : INotifyPropertyChanged
{
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

    // Pole pro obsahy tlačítek
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

    public MemoryGameViewModel()
    {
        JsonHandler jsonHandler = new JsonHandler();

        // Musí se upravit cesta (teď nejde testovat, takže až se UC někde použije)
        string filePath = "Data/Anglictina";

        List<TranslateWordQuestion> questions = jsonHandler.LoadTranslateWordQuestions(filePath + "/TranslateWord.json", "Unit 1");
        List<TranslateWordQuestion> userQuestions = jsonHandler.LoadTranslateWordUserQuestions(filePath + "/units.json", "Unit 1");
        questions.AddRange(userQuestions);

        QuestionUtils qutils = new QuestionUtils();

        randomQuestions = qutils.GetRandomTranslateWordQuestions(questions, 8);

        List<int> numbers = Enumerable.Range(0, 16).ToList();
        Random random = new Random();
        randomNumbers = numbers.OrderBy(x => random.Next()).ToList();

        for (int i = 0; i < 16; i++)
            randomWords.Add(FillWord(randomNumbers[i]));

        ButtonPressedCommand = new RelayCommand(ExecuteButtonPressed);

        Words = new ObservableCollection<string>
        {
            "", "", "", "",
            "", "", "", "",
            "", "", "", "",
            "", "", "", ""
        };

        previous[0] = previous[1] = -1;
    }

    private void ExecuteButtonPressed(object parameter)
    {
        string buttonIdentifier = parameter as string;
        int index = int.Parse(buttonIdentifier);

        if (Words[index] == "")
        {
            Words[index] = randomWords[index];

            if (previous[1] >= 0)
            {
                // kontrola 
                QuestionUtils qutils = new QuestionUtils();

                TranslateWordQuestion q1 = qutils.FindTranslateWordQuestionByAnswer(randomQuestions, Words[previous[0]]);
                if (q1 == null)
                    q1 = qutils.FindTranslateWordQuestionByText(randomQuestions, Words[previous[0]]);

                TranslateWordQuestion q2 = qutils.FindTranslateWordQuestionByAnswer(randomQuestions, Words[previous[1]]);
                if (q2 == null)
                    q2 = qutils.FindTranslateWordQuestionByText(randomQuestions, Words[previous[1]]);

                if (q1.ID != q2.ID)
                {
                    Words[previous[0]] = "";
                    Words[previous[1]] = "";
                }

                previous[0] = index;
                previous[1] = -1;
            }
            else if (previous[0] >= 0)
                previous[1] = index;
            else
                previous[0] = index;

            // Kontrola, zda mají všechny tlačítka hodnotu
            if (Words.All(content => !string.IsNullOrWhiteSpace(content)))
                DoneButtonVisibility = Visibility.Visible;
        }
    }

    private string FillWord(int number)
    {
        if(number < 8)
        {
            return randomQuestions[number].Answer;
        }
        else
        {
            return randomQuestions[number - 8].QuestionText;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
