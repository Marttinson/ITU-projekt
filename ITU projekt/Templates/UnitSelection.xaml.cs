using ITU_projekt.Models;
using System.Windows;
using System.Windows.Controls;
using ITU_projekt.ViewModels;

namespace ITU_projekt.Templates;

public partial class UnitSelection : UserControl
{
    public UnitSelection()
    {
        InitializeComponent();
        DataContext = new UnitSelectionViewModel();
    }

    private void OpenSettingsButton_Click(object sender, RoutedEventArgs e)
    {
        // Find the parent grid that contains the SettingsPanel
        var button = sender as Button;
        var grid = button?.Parent as Grid;
        var settingsPanel = grid?.FindName("SettingsPanel") as StackPanel;

        // Toggle visibility based on current state
        if (settingsPanel != null)
        {
            // If the panel is visible, collapse it, otherwise, make it visible
            settingsPanel.Visibility = settingsPanel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
