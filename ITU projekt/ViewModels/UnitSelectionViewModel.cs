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
using System.Diagnostics;

namespace ITU_projekt.ViewModels;


class UnitSelectionViewModel : INotifyPropertyChanged
{

    /* Units to display */
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

    private MainWindowViewModel VM;
    public UnitSelectionViewModel(MainWindowViewModel _VM)
    {
        // Load Units that will be displayed
        Units = JsonHandler.LoadUnits();

        VM = _VM;
    }


    // Start lesson
    public void ExecuteStartUnitCommand(object parameter)
    {
        VM.SetBackToMenuVisible();

        var id = parameter as int?;
        if (id.HasValue)
        {
            string unit = "Unit " + parameter;

            // Vygenerování náhodného čísla v intervalu <1; 3> a podle toho zvolení počáteční otázky,
            // všechny mají stejnou pravděpodobnost
            Random random = new Random();
            int randomNumber = random.Next(1, 4);

            // Set UnitModel for saving statistics
            if (parameter is int uid)
            {
                VM.setUnitModel(Units[uid-1]);
            }
            else
            {
                MessageBox.Show("Unit ID parsing failed");
                return;
            }

            int turn = 1;
            VM.CurrentUserControl = new ListeningExercise(VM, unit, ref turn);
            return;
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
}
