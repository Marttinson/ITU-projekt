using ITU_projekt.API;
using ITU_projekt.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ITU_projekt.ViewModels;
public class ChoiceViewModel : INotifyPropertyChanged
{
    private string _button1Background = "Transparent";
    public string Button1Background
    {
        get => _button1Background;
        set
        {
            _button1Background = value;
            OnPropertyChanged(nameof(Button1Background));
        }
    }

    private string _button2Background = "Transparent";
    public string Button2Background
    {
        get => _button2Background;
        set
        {
            _button2Background = value;
            OnPropertyChanged(nameof(Button2Background));
        }
    }

    private string _button3Background = "Transparent";
    public string Button3Background
    {
        get => _button3Background;
        set
        {
            _button3Background = value;
            OnPropertyChanged(nameof(Button3Background));
        }
    }

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
    public string Option1 { get; set; }
    public string Option2 { get; set; }
    public string Option3 { get; set; }

    public ChoiceViewModel()
    {
        EvaluateAnswerCommand = new RelayCommand(EvaluateAnswer);

        // Musí se upravit cesta (teď nejde testovat, takže až se UC někde použije)
        string filePath = "Data/Anglictina/Choice.json";

        JsonHandler jsonHandler = new JsonHandler();

        List<PickFromThreeQuestion> questions = jsonHandler.LoadOptionsQuestions(filePath);

        QuestionUtils qutils = new QuestionUtils();

        question = qutils.GetRandomOptionsQuestions(questions, 1)[0];
        TextToTranslate = question.QuestionText;

        List<int> numbers = Enumerable.Range(0, 3).ToList();
        Random random = new Random();
        numbers = numbers.OrderBy(x => random.Next()).ToList();

        Option1 = question.Options[numbers[0]];
        Option2 = question.Options[numbers[1]];
        Option3 = question.Options[numbers[2]];
    }
    private void EvaluateAnswer(object parameter)
    {
        string selectedOption = parameter as string;

        switch (selectedOption)
        {
            case "Možnost 1":
                // Logika pro Možnost 1
                if (string.Equals(Option1, question.Answer, StringComparison.OrdinalIgnoreCase))
                {
                    ButtonGroupVisibility = Visibility.Collapsed;
                    ContinueButtonVisibility = Visibility.Visible;
                }
                else
                    Button1Background = "Red";
                break;
            case "Možnost 2":
                // Logika pro Možnost 2
                if (string.Equals(Option2, question.Answer, StringComparison.OrdinalIgnoreCase))
                {
                    ButtonGroupVisibility = Visibility.Collapsed;
                    ContinueButtonVisibility = Visibility.Visible;
                }
                else
                    Button2Background = "Red";
                break;
            case "Možnost 3":
                // Logika pro Možnost 3
                if (string.Equals(Option3, question.Answer, StringComparison.OrdinalIgnoreCase))
                {
                    ButtonGroupVisibility = Visibility.Collapsed;
                    ContinueButtonVisibility = Visibility.Visible;
                }
                else
                    Button3Background = "Red";
                break;
        }
    }
}
