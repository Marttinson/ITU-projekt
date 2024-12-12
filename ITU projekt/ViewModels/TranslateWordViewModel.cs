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
public class TranslateWordViewModel : INotifyPropertyChanged
{
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

    public TranslateWordViewModel()
    {
        JsonHandler jsonHandler = new JsonHandler();

        // Musí se upravit cesta (teď nejde testovat, takže až se UC někde použije)
        string filePath = "Data/Anglictina";

        List<TranslateWordQuestion> questions = jsonHandler.LoadTranslateWordQuestions(filePath + "/TranslateWord.json", "Unit 1");
        List<TranslateWordQuestion> userQuestions = jsonHandler.LoadTranslateWordUserQuestions(filePath + "/units.json", "Unit 1");
        questions.AddRange(userQuestions);

        QuestionUtils qutils = new QuestionUtils();

        question = qutils.GetRandomTranslateWordQuestions(questions, 1)[0];
        WordToTranslate = question.QuestionText;

        EvaluateAnswerCommand = new RelayCommand(_ => EvaluateAnswer());
    }

    public ICommand EvaluateAnswerCommand { get; }
    private void EvaluateAnswer()
    {
        if (string.Equals(UserAnswer, question.Answer, StringComparison.OrdinalIgnoreCase)) // Nahraďte skutečnou logikou
        {
            ButtonOdpovedetVisibility = Visibility.Collapsed;
            ButtonDalsiVisibility = Visibility.Visible;
        }
        else
        {
            MessageBox.Show("Špatná odpověď, zkuste to znovu.");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

