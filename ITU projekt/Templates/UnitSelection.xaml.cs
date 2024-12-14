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

    // Helper method to find all child elements of type T
    private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
            if (child is T)
                yield return (T)child;

            foreach (var childOfChild in FindVisualChildren<T>(child))
                yield return childOfChild;
        }
    }


    private void Start_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("start click");
        var button = sender as Button;
        viewModel.ExecuteStartUnitCommand(int.Parse(button.Tag.ToString().Trim()));
    }

    private void StartEndless_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("endless click");
        var button = sender as Button;
        viewModel.ExecuteStartUnitCommand(int.Parse(button.Tag.ToString().Trim()));
    }

}
