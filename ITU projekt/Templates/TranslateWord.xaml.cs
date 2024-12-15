using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ITU_projekt.API;
using ITU_projekt.ViewModels;

namespace ITU_projekt.Templates;

public partial class TranslateWord : UserControl
{
    public TranslateWord(MainWindowViewModel VM, string unit, ref int turn)
    {
        InitializeComponent();
        DataContext = new TranslateWordViewModel(VM, unit, ref turn);

        // Zajistí, že fokus bude nastaven až po vykreslení
        Dispatcher.BeginInvoke(new Action(() =>
        {
            AnswerTextBox.Focus();
        }), System.Windows.Threading.DispatcherPriority.Render);
    }

    private void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (DataContext is TranslateWordViewModel viewModel)
            {
                if (viewModel.ButtonOdpovedetVisibility == Visibility.Visible)
                {
                    if (viewModel.EvaluateAnswerCommand.CanExecute(null))
                        viewModel.EvaluateAnswerCommand.Execute(null);
                }
                else
                {
                    if (viewModel.NextQuestion.CanExecute(null))
                        viewModel.NextQuestion.Execute(null);
                }

                    e.Handled = true;
            }
        }
    }
}