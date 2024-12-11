using ITU_projekt.Models;
using System.ComponentModel;
using System;
using System.Reflection.Metadata;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using ITU_projekt.Templates;
using System.Windows;
using ITU_projekt.Views;
using System.Collections.ObjectModel;
using System.Printing;
using System.Windows.Input;
using System.Data.Common;

/*
 * Postup generace
 * 
 * init generator with unit number
 * 
 * loop:
 *  generator.generate task
 *  vyvolat usercontrol podle typu generovane ulohy
 *  doplnit promenny do usercontrol (binding)
 *  poslat input do generatoru k vyhodnoceni
 *  podle odpovedi informovat uzivatele
 *  
 *  updatovat progress bar s kazdou vykonanou ulohou
 * 
 * pokud pocet uloh < max pocet v lekci
 * then back to loop
 * else
 * nejaky finalni vyhodnoceni treba
 * menu
 * 
 * 
 * 
 */




namespace ITU_projekt.ViewModels;
public class MainWindowViewModel : INotifyPropertyChanged
{

    /* Unit count */
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


    public RelayCommand<UnitModel> OpenSettingsCommand { get; private set; }
    public RelayCommand OpenMenuCommand { get; private set; }
    public RelayCommand ToggleDarkModeCommand { get; private set; }

    public event PropertyChangedEventHandler PropertyChanged;


    private UserControl _CurrentUserControl;

    public RelayCommand<string> ButtonClickCommand { get; private set; }

    // ICommand for displaying statistics
    public ICommand ShowStatisticsCommand { get; }
    public ICommand AddCustomQuestionsCommand { get; }
    public ICommand StartUnitCommand { get; }
    public ICommand StartUnitCommand_endless { get; }


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


    public MainWindowViewModel()
    {
        // Na zacatku se zobrazi vyber lekci
        ShowStatisticsCommand = new RelayCommand<UnitModel>(ExecuteShowStatistics);
        // Connect Commands
        AddCustomQuestionsCommand = new RelayCommand<UnitModel>(ExecuteAddCustomQuestions);
        StartUnitCommand = new RelayCommand<UnitModel>(unit => ExecuteStartUnitCommand(unit, false));
        StartUnitCommand_endless = new RelayCommand<UnitModel>(ExecuteStartUnitCommand_ENDLESS);

        CurrentUserControl = new UnitSelection();
    }

    private void ExecuteShowStatistics(UnitModel model)
    {
            // TODO LOGIC
            MessageBox.Show("Unit " + model.ID + " statistics");
            CurrentUserControl = new Graph(model);
    }

    private void ExecuteAddCustomQuestions(UnitModel model)
    {
        // TODO LOGIC
        MessageBox.Show("Unit " + model.ID + " questions");
    }

    private void ExecuteStartUnitCommand_ENDLESS(UnitModel model)
    {
        MessageBox.Show("Unit " + model.ID + " endless");
        ExecuteStartUnitCommand(model, true);
    }

    private void ExecuteStartUnitCommand(UnitModel model, bool endeless = false)
    {
        // Start unit
        MessageBox.Show("Unit " + model.ID + " limited VM");
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
