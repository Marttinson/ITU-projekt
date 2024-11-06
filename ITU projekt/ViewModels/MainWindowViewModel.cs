using ITU_projekt.Models;
using System.ComponentModel;
using System;
using System.Reflection.Metadata;
using System.Windows.Controls;

namespace ITU_projekt.ViewModels;
public class MainWindowViewModel : INotifyPropertyChanged
{
    // TODO COMMAND public RelayCommand<string> SelectLanguageCommand { get; private set; }
    public RelayCommand OpenMenuCommand { get; private set; }
    public RelayCommand ToggleDarkModeCommand { get; private set; } // Příklad pro přepnutí dark mode

    public MainWindowViewModel()
    {
        // Konstruktor pro příkaz výběru jazyka s parametrem typu string
        // TODO COMMAND SelectLanguageCommand = new RelayCommand<string>(SelectLanguage);

        // Konstruktor pro příkaz pro otevření menu bez parametrů
        OpenMenuCommand = new RelayCommand(OpenMenu);

        // Příklad příkazu pro přepnutí dark mode
        ToggleDarkModeCommand = new RelayCommand(ToggleDarkMode);
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

    public void OpenMenu()
    {
        // Logika pro otevření rolovacího menu
        Console.WriteLine("Menu bylo otevřeno");
    }

    public void ToggleDarkMode()
    {
        // Logika pro přepnutí dark mode
        Console.WriteLine("Přepnutí do dark mode");
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
