/* MainWindowViewModel
 * VM
 * Vojtěch Hrabovský (xhrabo18)
 * 
 * VM - Handles MainWindow control, some user control changes
 */

using System.ComponentModel;
using System;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

using ITU_projekt.API;
using ITU_projekt.Models;
using ITU_projekt.Templates;


namespace ITU_projekt.ViewModels;
public class MainWindowViewModel : INotifyPropertyChanged
{

    // Answer count for statistics
    private int right_answers;
    private int wrong_answers;

    /* Streak */
    private bool _isLessonCompletedToday;
    public bool IsLessonCompletedToday
    {
        get => _isLessonCompletedToday;
        set
        {
            if (_isLessonCompletedToday != value)
            {
                _isLessonCompletedToday = value;
                Streak s = JsonHandler.ReadStreak();
                JsonHandler.SaveStreak(s.length + 1, DateTime.Now);
                OnPropertyChanged();
            }
        }
    }
    private string _streakSymbol;
    public string StreakSymbol
    {
        get => _streakSymbol;
        set
        {
            _streakSymbol = value;
            OnPropertyChanged();
        }
    }

    /* Current unit pro statistiky */
    private UnitModel currentUnit;

    /* UC, ktery je promitan do okna */
    private UserControl _CurrentUserControl;
    public UserControl CurrentUserControl
    {
        get => _CurrentUserControl;
        set
        {
            if (_CurrentUserControl != value)
            {
                _CurrentUserControl = value;
                OnPropertyChanged(nameof(CurrentUserControl));
            }
        }
    }

    /* Visibilita tlactika pro navrat do menu */
    private Visibility _BackToMenuVisibility;
    public Visibility BackToMenuVisibility
    {
        get => _BackToMenuVisibility;
        set
        {
            _BackToMenuVisibility = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand<string> ButtonClickCommand { get; private set; }
    public RelayCommand<UnitModel> OpenSettingsCommand { get; private set; }
    public RelayCommand OpenMenuCommand { get; private set; }
    public RelayCommand ToggleDarkModeCommand { get; private set; }

    // ICommands 
    public ICommand ShowStatisticsCommand { get; }
    public ICommand AddCustomQuestionsCommand { get; }
    public ICommand BackToMenuCommand { get; }
    public ICommand ChangeStreakIcon { get; }
    public ICommand StartPexesoCommand { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Initializes instance
    /// </summary>
    public MainWindowViewModel()
    {
        // Propojeni commandu
        ShowStatisticsCommand = new RelayCommand<UnitModel>(ExecuteShowStatistics);
        AddCustomQuestionsCommand = new RelayCommand<UnitModel>(ExecuteAddCustomQuestions);
        BackToMenuCommand = new RelayCommand(BackToMenuCommandExecute);
        ChangeStreakIcon = new RelayCommand(ChangeStreakIconExecute);
        StartPexesoCommand = new RelayCommand((obj) => StartPexeso());

        right_answers = 0;
        wrong_answers = 0;

        currentUnit = null;

        // Zvoleny streak symbol -> pokud nenalezen, zvoli se default
        string symb = JsonHandler.LoadStreakSymbol();
        if (string.IsNullOrEmpty(symb))
        {
            StreakSymbol = "✔";
        }
        else
        {
            StreakSymbol = symb;
        }


        // Kontrola streaku, pripadne zresetovani
        Streak s = JsonHandler.ReadStreak();
        if (s != null)
        {
            // Nenastavuji pres public abych nezmenil streak v zaznamu
            if (s.last_date.Date == DateTime.Today)
            {
                _isLessonCompletedToday = true;
            }
            else
            {
                _isLessonCompletedToday = false;
            }

            // Pokud vic nez 24 h, reset
            if ((DateTime.Now - s.last_date).TotalHours > 24)
            {
                MessageBox.Show("Streak was reseted. Your last exercise was at: " + s.last_date.ToString("yyyy-MM-ddTHH:mm:ss"));
                s.length = 0;
                _isLessonCompletedToday = false;
                JsonHandler.SaveStreak(s.length, s.last_date);
                // Notify UI 
                OnPropertyChanged(nameof(IsLessonCompletedToday));

            }
        }

        // Defaultni UC
        CurrentUserControl = new UnitSelection(this);
        // Neni treba navratu do menu
        BackToMenuVisibility = Visibility.Hidden;
    }

    // Zmena ikony vyvolana tlacitkem
    public void ChangeStreakIconExecute(object parameter)
    {
        if (StreakSymbol == "✔")
        {
            StreakSymbol = "🏆";
        }
        else if (StreakSymbol == "🏆")
        {
            StreakSymbol = "🔥";
        }
        else if (StreakSymbol == "🔥")
        {
            StreakSymbol = "✔";
        }

        JsonHandler.SaveStreakSymbol(StreakSymbol);
    }


    // Vraceni zpet do hlavniho menu
    public void BackToMenuCommandExecute(object parameter)
    {
        wrong_answers = 0;
        right_answers = 0;
        currentUnit = null;
        BackToMenuVisibility = Visibility.Hidden;
        CurrentUserControl = new UnitSelection(this);
    }

    // Zobrazeni UC se statistikou
    private void ExecuteShowStatistics(UnitModel model)
    {
        CurrentUserControl = new Graph(model);
        BackToMenuVisibility = Visibility.Visible;
    }

    // Zobrazeni UC s uzivatelskymi otazkami
    private void ExecuteAddCustomQuestions(UnitModel model)
    {
        CurrentUserControl = new CustomUserWordList(model);
        BackToMenuVisibility = Visibility.Visible;
    }

    // Saves new error rate
    private void SaveStatistic()
    {
        JsonHandler.SaveStatistic(currentUnit.ID, (float)wrong_answers / (float)(right_answers));
    }

    // Call this when lesson is finished
    // Handles streak and back to menu button visibility
    public void LessonFinished()
    {
        Streak s = JsonHandler.ReadStreak();

        if (!IsLessonCompletedToday)
        {
            IsLessonCompletedToday = true;

            // Pokud dnes prvni lekce, inkrementovat streak
            JsonHandler.SaveStreak(s.length + 1, DateTime.Now);
        }
        else
        {
            // Pokud uz dnes byla splnena lekce, zapise se nejnovejsi datum
            JsonHandler.SaveStreak(s.length, DateTime.Now);
        }

        SaveStatistic();
        BackToMenuVisibility = Visibility.Hidden;
    }

    // Call this when start of lesson
    public void SetBackToMenuVisible()
    {
        BackToMenuVisibility = Visibility.Visible;
    }

    // Call this when start of lesson
    // Slouzi k zapsani statistik
    public void setUnitModel(UnitModel model)
    {
        currentUnit = model;
    }

    // Slouzi k ovladani statistik
    public void incrementRight()
    {
        right_answers++;
    }
    public void incrementWrong()
    {
        wrong_answers++;
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // TODO PEXESO
    private void StartPexeso()
    {
        MessageBox.Show("Start Pexeso.");
    }
}
