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

namespace ITU_projekt
{
    public partial class Preklad_slova : UserControl, INotifyPropertyChanged
    {
        private string _wordToTranslate;
        private Question question;
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

        public Preklad_slova()
        {
            InitializeComponent();
            DataContext = this; // Nastavení datového kontextu

            string filePath = "Data/Anglictina/Data.json";
            JsonHandler jsonHandler = new JsonHandler(filePath);

            List<Question> questions = jsonHandler.LoadQuestions();
            QuestionUtils qutils = new QuestionUtils(questions);

            question = qutils.GetRandomQuestions(1)[0];

            WordToTranslate = question.QuestionText;

            // WordToTranslate = "Lion"; // Výchozí hodnota
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
}