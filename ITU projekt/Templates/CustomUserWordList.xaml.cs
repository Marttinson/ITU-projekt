/** CustomUserWordList
 * V
 *  Vojtěch Hrabovský (xhrabo18)
 * 
 * Code-behind pro prezentaci uživatelem vytvořených otázek
 */


using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using ITU_projekt.Models;

namespace ITU_projekt.Templates;

public partial class CustomUserWordList : UserControl
{
    private CustomUserWordListViewModel _viewModel;

    public event PropertyChangedEventHandler PropertyChanged;

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
    // Kdyz je zmacknuto tlacitko pro smazani otazky
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
