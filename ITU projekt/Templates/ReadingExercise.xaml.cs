/** Reading Exercise 
 * V
 * Vojtěch Hrabovský (xhrabo18)
 * 
 * Code behind pro čtecí cvičení
 */

using System.Windows;
using System.Windows.Controls;

using ITU_projekt.Models;
using ITU_projekt.ViewModels;


namespace ITU_projekt.Templates
{
    public partial class ReadingExercise : UserControl
    {
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

        // This only checks if answer was given for each statement
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
