using ITU_projekt.Models;
using System.ComponentModel;
using System;
using System.Reflection.Metadata;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using ITU_projekt.Templates;
using System.Windows;
using ITU_projekt.Views;


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
    // TODO COMMAND public RelayCommand<string> SelectLanguageCommand { get; private set; }
    public RelayCommand OpenMenuCommand { get; private set; }
    public RelayCommand ToggleDarkModeCommand { get; private set; } // Příklad pro přepnutí dark mode

    public event PropertyChangedEventHandler PropertyChanged;


    private UserControl _CurrentUserControl;

    public RelayCommand<string> ButtonClickCommand { get; private set; }

    public UserControl CurrentUserControl
    {
        get => _CurrentUserControl;
        set
        {
            _CurrentUserControl = value;
            OnPropertyChanged();
        }
    }


    public MainWindowViewModel()
    {
        // Konstruktor pro příkaz výběru jazyka s parametrem typu string
        // TODO COMMAND SelectLanguageCommand = new RelayCommand<string>(SelectLanguage);

        
        // Na zacatku se zobrazi vyber lekci
        CurrentUserControl = new UnitSelection();

        // Binding tlacitek pro vyber lekce
        ButtonClickCommand = new RelayCommand<string>(OnButtonClick);

    }

    private void OnButtonClick(string buttonName)
    {
        switch (buttonName)
        {

            /*
             pridat tridu, ktera bude obstaravat lekci
             */
            case "Unit 1":
                MessageBox.Show("Unit 1 in UserControl1 was clicked via Command!");
                // TODO call function for generation
                

                // Add logic specific to Button1
                break;

            case "Unit 2":
                MessageBox.Show("Unit 2 in UserControl1 was clicked via Command!");
                // Add logic specific to Button2
                break;

            case "Unit 3":
                MessageBox.Show("Unit 3 in UserControl1 was clicked via Command!");
                // Add logic specific to Button2
                break;

            case "Unit 4":
                MessageBox.Show("Unit 4 in UserControl1 was clicked via Command!");
                // Add logic specific to Button2
                break;

            case "Unit 5":
                MessageBox.Show("Unit 5 in UserControl1 was clicked via Command!");
                // Add logic specific to Button2
                break;
            case "BACK":
                CurrentUserControl = new UnitSelection();
                break;
            default:
                MessageBox.Show(buttonName);
                break;

                // Add more cases for additional buttons as needed
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public void SelectLanguage(object parameter)
    {
        if (parameter is ComboBoxItem selectedItem)
        {
            string language = selectedItem.Content.ToString();
            // Můžete zde přepnout jazyk aplikace nebo aktualizovat lokalizaci
            Console.WriteLine($"Jazyk byl změněn na: {language}");
        }
    }
}
