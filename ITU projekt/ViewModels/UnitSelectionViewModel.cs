using ITU_projekt.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITU_projekt.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ITU_projekt.API;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

namespace ITU_projekt.ViewModels;


class UnitSelectionViewModel : INotifyPropertyChanged
{


    private ObservableCollection<UnitModel> _units;
    public ObservableCollection<UnitModel> Units
    {
        get => _units;
        set
        {
            _units = value;
            OnPropertyChanged(nameof(Units));
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


    public ICommand AddCustomQuestions { get; set; }
    public ICommand StartUnitCommand { get; set; }
    public ICommand StartUnitCommand_endless { get; set; }
    public ICommand ViewStatistics { get; set; }

    private MainWindowViewModel VM;
    public UnitSelectionViewModel(MainWindowViewModel _VM)
    {
        // Load Units
        Units = JsonHandler.LoadUnits();

        // Connect Commands
        AddCustomQuestions = new RelayCommand(ExecuteAddCustomQuestions);
        StartUnitCommand = new RelayCommand(ExecuteStartUnitCommand);
        StartUnitCommand_endless = new RelayCommand(ExecuteStartUnitCommand_ENDLESS);
        ViewStatistics = new RelayCommand(ExecuteViewStatistics);

        VM = _VM;
    }

    // TODO CUSTOM Qs
    private void ExecuteAddCustomQuestions(object parameter)
    {
        var id = parameter as int?;
        if (id.HasValue)
        {
            // TODO LOGIC
            MessageBox.Show("Unit " + parameter + " custom");
        }
        else { MessageBox.Show("ID error"); }
    }

    // TODO START LECTION -> send to MainWindowViewModel
    private void ExecuteStartUnitCommand(object parameter)
    {
        var id = parameter as int?;
        if (id.HasValue)
        {
            string unit = "Unit " + parameter;
    
            // Vygenerování náhodného čísla v intervalu <1; 3> a podle toho zvolení počáteční otázky,
            // všechny mají stejnou pravděpodobnost
            Random random = new Random();
            int randomNumber = random.Next(1, 4);

            int turn = 1;
            if(randomNumber == 1)
                VM.CurrentUserControl = new TranslateWord(VM, unit, ref turn);
            else if (randomNumber == 2)
                VM.CurrentUserControl = new WordMatching(VM, unit, ref turn);
            else if (randomNumber == 3)
                VM.CurrentUserControl = new Choice(VM, unit, ref turn);
        }
    }
    private void ExecuteStartUnitCommand_ENDLESS(object parameter)
    {
        var id = parameter as int?;
        if (id.HasValue)
        {
            string unit = "Unit " + parameter;

            // Vygenerování náhodného čísla v intervalu <1; 3> a podle toho zvolení počáteční otázky,
            // všechny mají stejnou pravděpodobnost
            Random random = new Random();
            int randomNumber = random.Next(1, 4);

            int turn = -1;
            if (randomNumber == 1)
                VM.CurrentUserControl = new TranslateWord(VM, unit, ref turn);
            else if (randomNumber == 2)
                VM.CurrentUserControl = new WordMatching(VM, unit, ref turn);
            else if (randomNumber == 3)
                VM.CurrentUserControl = new Choice(VM, unit, ref turn);
        }
    }

    // TODO VIEW STATISTICS ->send to MainWindowViewModel
    private void ExecuteViewStatistics(object parameter)
    {
        var id = parameter as int?;
        if (id.HasValue)
        {
            // TODO LOGIC
            MessageBox.Show("Unit " + parameter + " statistics");
        }
    }
}
