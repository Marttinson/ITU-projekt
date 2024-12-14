using ITU_projekt.Models;
using System.Windows;
using System.Windows.Controls;
using ITU_projekt.ViewModels;
using ITU_projekt.API;
using System.Collections.Generic;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;

namespace ITU_projekt.Templates;

public partial class CustomUserWordList : UserControl
{
    private CustomUserWordListViewModel _viewModel;

    public event PropertyChangedEventHandler PropertyChanged;

    private bool SBVis;

    public CustomUserWordList(UnitModel model)
    {
        _viewModel = new CustomUserWordListViewModel(model);
        DataContext = _viewModel;
        InitializeComponent();
    }


    // When text is changed, check all boxes for empty values or duplicit values
    public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var textBox = sender as TextBox;
        var question = textBox?.DataContext as Question;

        if (question != null)
        {
            question.OnPropertyChanged(nameof(question.QuestionText));  // Trigger change
        }

        _viewModel.CheckForDuplicates();
    }

    // Neslo volat primo do VM
    private void DeleteQ(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            // Call DeleteQuestion in VM
            _viewModel.DeleteQuestion(int.Parse(button.CommandParameter.ToString().Trim()));
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
