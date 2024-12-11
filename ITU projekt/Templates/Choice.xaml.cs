using ITU_projekt.API;
using ITU_projekt.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace ITU_projekt.Templates
{
    /// <summary>
    /// Interakční logika pro Choice.xaml
    /// </summary>
    public partial class Choice : UserControl, INotifyPropertyChanged
    {
        private string _textToTranslate;
        private PickFromThreeQuestion question;
        public string TextToTranslate
        {
            get => _textToTranslate;
            set
            {
                _textToTranslate = value;
                OnPropertyChanged(nameof(TextToTranslate));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand EvaluateAnswer { get; }

        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public Choice()
        {
            InitializeComponent();
            DataContext = this;

            EvaluateAnswer = new RelayCommand(ExecuteEvaluateAnswer);

            // Musí se upravit cesta (teď nejde testovat, takže až se UC někde použije)
            string filePath = "Data/Anglictina/Choice.json";

            JsonHandler jsonHandler = new JsonHandler();

            List<PickFromThreeQuestion> questions = jsonHandler.LoadOptionsQuestions(filePath);

            QuestionUtils qutils = new QuestionUtils();

            question = qutils.GetRandomOptionsQuestions(questions, 1)[0];
            TextToTranslate = question.QuestionText;

            Option1 = question.Options[0];
            Option2 = question.Options[1];
            Option3= question.Options[2];
        }

        private void ExecuteEvaluateAnswer(object parameter)
        {
            string selectedOption = parameter as string;

            switch (selectedOption)
            {
                case "Možnost 1":
                    // Logika pro Možnost 1
                    if (string.Equals(Option1, question.Answer, StringComparison.OrdinalIgnoreCase))
                        MessageBox.Show("Správně!");
                    else
                        MessageBox.Show("Špatná odpověď. Zkuste znovu.");
                    break;
                case "Možnost 2":
                    // Logika pro Možnost 2
                    if (string.Equals(Option2, question.Answer, StringComparison.OrdinalIgnoreCase))
                        MessageBox.Show("Správně!");
                    else
                        MessageBox.Show("Špatná odpověď. Zkuste znovu.");
                    break;
                case "Možnost 3":
                    // Logika pro Možnost 3
                    if (string.Equals(Option3, question.Answer, StringComparison.OrdinalIgnoreCase))
                        MessageBox.Show("Správně!");
                    else
                        MessageBox.Show("Špatná odpověď. Zkuste znovu.");
                    break;
            }
        }
    }
}
