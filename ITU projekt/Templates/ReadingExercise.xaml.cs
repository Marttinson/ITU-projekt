using ITU_projekt.ViewModels;
using System;
using System.Collections.Generic;
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
using System;
using System.Windows;
using System.Windows.Threading;

using ITU_projekt.ViewModels;
using ITU_projekt.Models;
using System.Windows.Threading;
using System.Diagnostics;

namespace ITU_projekt.Templates
{
    public partial class ReadingExercise : UserControl
    {

        // Audio Controls
        private bool isPlaying = false;
        private DispatcherTimer timer;

        private ReadingExerciseViewModel _viewModel;

        public ReadingExercise(MainWindowViewModel VM, string unit, ref int turn)
        {
            InitializeComponent();
            _viewModel = new ReadingExerciseViewModel(VM, unit, ref turn);
            DataContext = _viewModel;
        }

        


        /**
         * USER INPUT ELEMENTS
         * 
         * 
         */

        private void TrueRadioButton_Click(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            var statement = radioButton?.DataContext as ExerciseStatement;

            if (statement != null)
            {
                // Set UserAnswer based on the selected button
                statement.UserAnswer = true;
                _viewModel.resetAnswer();
            }
        }

        private void FalseRadioButton_Click(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            var statement = radioButton?.DataContext as ExerciseStatement;

            if (statement != null)
            {
                // Set UserAnswer based on the selected button
                statement.UserAnswer = false;
                _viewModel.resetAnswer();
            }
        }

        // This only checks if answer was given
        private void CheckAnswersButton_Click(object sender, RoutedEventArgs e)
        {
            bool allAnswered = true;
            
            foreach(ExerciseStatement stat in _viewModel.Statements)
            {
                if(stat.UserAnswer == null)
                {
                    allAnswered = false;
                }
            }

            if (!allAnswered)
            {
                MessageBox.Show("Please answer all the questions before checking the answers.");
                return;
            }

            // If all answers are selected, call the CheckAnswer method
            _viewModel.CheckAnswer();
        }
    }
}
