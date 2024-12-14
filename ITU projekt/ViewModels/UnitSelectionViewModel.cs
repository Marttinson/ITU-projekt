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

        VM = _VM;
    }

    // TODO START LECTION -> send to MainWindowViewModel
    public void ExecuteStartUnitCommand(object parameter)
    {
        MessageBox.Show("Start");
        var id = parameter as int?;
        if (id.HasValue)
        {
            MessageBox.Show("Unit " + id + " limited          asdasdasd");
            // TODO LOGIC
            string unit = "Unit " + parameter;
            //MessageBox.Show(unit + " limited");

            //VM.CurrentUserControl = new MemoryGame(VM);
    
            // Vygenerování náhodného čísla v intervalu <1; 3> a podle toho zvolení počáteční otázky,
            // všechny mají stejnou pravděpodobnost
            Random random = new Random();
            int randomNumber = random.Next(1, 4);

            if(randomNumber == 1)
                VM.CurrentUserControl = new TranslateWord(VM, unit);
            else if (randomNumber == 2)
                VM.CurrentUserControl = new WordMatching(VM, unit);
            else if (randomNumber == 3)
                VM.CurrentUserControl = new Choice(VM, unit);
        }
    }
    public void ExecuteStartUnitCommand_ENDLESS(object parameter)
    {
        MessageBox.Show("Endless");
        var id = parameter as int?;
        if (id.HasValue)
        {
            // TODO LOGIC
            MessageBox.Show("Unit " + id + " endless           asdsadasd");
        }
    }
}
