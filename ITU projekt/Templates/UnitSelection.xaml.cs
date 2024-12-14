using ITU_projekt.Models;
using System.Windows;
using System.Windows.Controls;
using ITU_projekt.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Diagnostics;
using System.Windows.Media;

namespace ITU_projekt.Templates;

public partial class UnitSelection : UserControl
{

    private UnitSelectionViewModel viewModel;

    public UnitSelection(MainWindowViewModel VM)
    {
        InitializeComponent();
        viewModel = new UnitSelectionViewModel(VM);
        DataContext = viewModel;
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


    // Send command to VM
    private void Start_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        viewModel.ExecuteStartUnitCommand(int.Parse(button.Tag.ToString().Trim()));
    }

    // Send command to VM
    private void StartEndless_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        viewModel.ExecuteStartUnitCommand(int.Parse(button.Tag.ToString().Trim()));
    }

}
