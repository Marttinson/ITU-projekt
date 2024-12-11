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


    public UnitSelectionViewModel()
    {
        // Load Units
        Units = JsonHandler.LoadUnits();
    }
}
