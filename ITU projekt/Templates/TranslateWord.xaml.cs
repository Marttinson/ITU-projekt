using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ITU_projekt.API;

namespace ITU_projekt.Templates;

public partial class TranslateWord : UserControl, INotifyPropertyChanged
{
    private string _wordToTranslate;
    private TranslateWordQuestion question;
    public string WordToTranslate
    {
        get => _wordToTranslate;
        set
        {
            _wordToTranslate = value;
            OnPropertyChanged(nameof(WordToTranslate));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public TranslateWord()
    {
        InitializeComponent();
        DataContext = this; // Nastavení datového kontextu

        // TODO (vezmeme z appdata) Musí se upravit cesta (teď nejde testovat, takže až se UC někde použije)
        string filePath = "Data/Anglictina/TranslateWord.json";

        JsonHandler jsonHandler = new JsonHandler();

        List<TranslateWordQuestion> questions = jsonHandler.LoadTranslateWordQuestions(filePath);

        QuestionUtils qutils = new QuestionUtils();

        question = qutils.GetRandomTranslateWordQuestions(questions, 1)[0];
        WordToTranslate = question.QuestionText;
    }

    private void EvaluateAnswer(object sender, RoutedEventArgs e)
    {
        string userAnswer = AnswerTextBox.Text; // Načtení odpovědi uživatele
        userAnswer = userAnswer.ToLower();

        if (string.Equals(userAnswer, question.Answer, StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show("Správně!");
        }
        else
        {
            MessageBox.Show("Špatná odpověď. Zkuste znovu.");
        }
    }
}