/** View Model pro tvorbu věty
 * Martin Rybnikář (xrybni10)
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ITU_projekt.Models;
using System.Windows.Input;
using System.Windows;
using ITU_projekt.API;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using ITU_projekt.Templates;

namespace ITU_projekt.ViewModels
{
    public class SentenceMakingViewModel : INotifyPropertyChanged
    {
        private string _inputText;
        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged();
            }
        }

        private string _output = "";
        public string Output
        {
            get => _output;
            set
            {
                _output = value;
                OnPropertyChanged();
            }
        }

        private Visibility _nextButtonVisibility = Visibility.Collapsed; // Výchozí je skrytý
        public Visibility NextButtonVisibility
        {
            get => _nextButtonVisibility;
            set
            {
                _nextButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ButtonViewModel> Buttons { get; set; }

        private SentenceQuestion question;

        private int numOfButtons;
        private int buttonsPressed;
        private bool complete;

        private MainWindowViewModel VM;
        private string unit;
        private int turn;

        public SentenceMakingViewModel(MainWindowViewModel _VM, string _unit, ref int _turn)
        {
            NextQuestion = new RelayCommand(ExecuteNextQuestion);

            // Načítání otázek ze souborů
            JsonHandler jsonHandler = new JsonHandler();

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string jsonPath = Path.Combine(appDataPath, "ITU", "Sentence.json");

            unit = _unit;
            List<SentenceQuestion> questions = jsonHandler.LoadSenteceMakingQuestion(jsonPath, unit);

            QuestionUtils qutils = new QuestionUtils();

            question = qutils.GetRandomSentenceQuestions(questions, 1)[0];

            // Inicializace textu
            InputText = question.Sentence;

            // Rozdělení textu na jednotlivá slova
            string[] words = question.Translate.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Vytvoření instance Random pro náhodné generování
            Random random = new Random();

            // Náhodné zamíchání slov
            var shuffledWords = words.OrderBy(w => random.Next()).ToArray();

            Buttons = new ObservableCollection<ButtonViewModel>();

            numOfButtons = shuffledWords.Length;
            buttonsPressed = 0;
            complete = false;

            // Vytvoření tlačítek
            for (int i = 0; i < numOfButtons; i++)
            {
                string content = $"{shuffledWords[i]}";
                Buttons.Add(new ButtonViewModel(content, this));
            }

            VM = _VM;
            turn = _turn;
        }

        public void UpdateOutput(string buttonContent)
        {
            if (!complete)
            {
                // Ořezání bílých znaků z inputu
                var items = Output.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                   .Select(item => item.Trim()) // Oříznutí mezer
                                   .ToList();

                // Porovnání bez ohledu na velikost písmen
                var buttonTrimmed = buttonContent.Trim();

                // Pokud položka existuje, odstraníme ji, jinak přidáme
                if (items.Any(item => string.Equals(item, buttonTrimmed)))
                {
                    // Odstranění položky, ignorujeme velikost písmen
                    items.RemoveAll(item => string.Equals(item, buttonTrimmed));
                    buttonsPressed--;
                }
                else
                {
                    // Přidání položky, pokud ještě neexistuje
                    items.Add(buttonTrimmed);
                    buttonsPressed++;
                }

                // Spojení položek do výstupního řetězce, mezi položkami je pouze jedna mezera
                Output = string.Join(" ", items);

                if (buttonsPressed == numOfButtons)
                {
                    if (string.Equals(Output, question.Translate))
                    {
                        // SPLNĚNO STAT
                        VM.incrementRight();
                        complete = true;
                        NextButtonVisibility = Visibility.Visible;
                    }
                    else
                    {
                        // CHYBA STAT
                        VM.incrementWrong();
                        Output = "";
                        buttonsPressed = 0;
                    }
                }
            }
        }

        // Funkce sloužící pro přechod na další otázku
        public ICommand NextQuestion { get; }
        private void ExecuteNextQuestion(object parameter)
        {
            // Kontrola, zda již neproběhlo 10 otázek
            if (turn > 0)
            {
                if (turn == 10)
                {
                    // Vrací se zpět do menu
                    VM.LessonFinished();
                    VM.CurrentUserControl = new UnitSelection(VM);
                    return;
                }
                else
                    turn++;
            }

            // Vygenerování náhodného čísla v intervalu <1; 4> a podle toho zvolení následující otázky,
            // všechny mají stejnou pravděpodobnost
            Random random = new Random();
            int randomNumber = random.Next(1, 5);

            if (randomNumber == 1)
                VM.CurrentUserControl = new TranslateWord(VM, unit, ref turn);
            else if (randomNumber == 2)
                VM.CurrentUserControl = new WordMatching(VM, unit, ref turn);
            else if (randomNumber == 3)
                VM.CurrentUserControl = new Choice(VM, unit, ref turn);
            else if (randomNumber == 4)
                VM.CurrentUserControl = new SentenceMaking(VM, unit, ref turn);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ButtonViewModel
    {
        public string Content { get; set; }
        public ICommand ClickCommand { get; set; }

        public ButtonViewModel(string content, SentenceMakingViewModel parentViewModel)
        {
            Content = content;
            ClickCommand = new RelayCommand(param =>
            {
                if (param != null)
                {
                    parentViewModel.UpdateOutput(param.ToString());
                }
                else
                {
                    parentViewModel.UpdateOutput(Content); // Jako záloha použijeme Content
                }
            });
        }
    }
}
