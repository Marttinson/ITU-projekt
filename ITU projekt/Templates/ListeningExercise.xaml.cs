﻿/** ListeningExercise
 * V
 *  Vojtěch Hrabovský (xhrabo18)
 * 
 * Code-behind k poslechovému cvičení
 */

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using ITU_projekt.Models;
using ITU_projekt.ViewModels;

namespace ITU_projekt.Templates
{
    public partial class ListeningExercise : UserControl
    {

        // Audio Controls
        private bool isPlaying = false;
        private DispatcherTimer timer;

        private ListeningExerciseViewModel _viewModel;

        public ListeningExercise(MainWindowViewModel VM, string unit, ref int turn)
        {
            InitializeComponent();
            _viewModel = new ListeningExerciseViewModel(VM, unit, ref turn);
            DataContext = _viewModel;
            InitializeAudioPlayer();
        }

        /**
         * 
         * AUDIO ELEMENTS
         * 
         */

        // Initialize audio player
        private void InitializeAudioPlayer()
        {
            // Set up the timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500); // Update every 500ms
            timer.Tick += Timer_Tick;

            // Load an audio file
            string audioFilePath = _viewModel.getAudio();

            AudioPlayer.Source = new Uri(audioFilePath, UriKind.Relative);

            AudioPlayer.MediaFailed += (sender, e) =>
            {
                MessageBox.Show("Failed to load the audio file. Skipping to next question.");
                _viewModel.ExecuteNextQuestion();
            };

            AudioPlayer.MediaOpened += (sender, e) =>
            {
                // Update the progress when audio is opened
                AudioProgressBar.Maximum = AudioPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            };

        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isPlaying)
            {
                AudioPlayer.Play();
                PlayPauseButton.Content = "⏸"; // Change symbol
                isPlaying = true;
                timer.Start();
            }
            else
            {
                AudioPlayer.Pause();
                PlayPauseButton.Content = "▶"; // Change symbol
                isPlaying = false;
                timer.Stop();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (AudioPlayer.NaturalDuration.HasTimeSpan)
            {
                // Update the slider value
                AudioProgressBar.Maximum = AudioPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                AudioProgressBar.Value = AudioPlayer.Position.TotalSeconds;

                // Update the time label
                AudioTimeLabel.Text = $"{AudioPlayer.Position.ToString(@"mm\:ss")} / {AudioPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss")}";
            }
        }

        private void AudioProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (AudioPlayer != null && AudioPlayer.NaturalDuration.HasTimeSpan)
            {
                // Move to the new position when the slider is moved
                AudioPlayer.Position = TimeSpan.FromSeconds(AudioProgressBar.Value);
            }
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
