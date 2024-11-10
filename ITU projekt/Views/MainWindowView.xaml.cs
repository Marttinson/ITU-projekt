using ITU_projekt.ViewModels;
using ITU_projekt.Templates;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ITU_projekt.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {

        private bool RightSideMenu_expanded = false;

        public MainWindowView()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string language = selectedItem.Content.ToString();

                // Volání příkazu pro výběr jazyka z ViewModelu
                var viewModel = DataContext as MainWindowViewModel;
                viewModel?.SelectLanguage(language);
            }
        }
        private void ShowView1(object sender, RoutedEventArgs e)
        {
            MenuPanel.Visibility = Visibility.Hidden; // Skryje MenuPanel v MainWindow
            MainContent.Content = new view1(); // Načte UserControl 
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {

            Console.WriteLine("ZMACKNUTE TLACITKO!!");
            var viewModel = (MainWindowViewModel)DataContext;

            if (RightSideMenu_expanded)
            {
                // Kompresni animace
                var menuAnimation = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.3))
                };

                // Aplikovani animace
                RightSideMenu.BeginAnimation(UIElement.OpacityProperty, menuAnimation);

                // Zapamatovani stavu
                RightSideMenu_expanded = false;
            }
            else
            {
                // Expanzni animace
                var menuAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromSeconds(0.3))
                };
                    
                // Aplikovani animace
                RightSideMenu.BeginAnimation(UIElement.OpacityProperty, menuAnimation);

                // Zapamatovani stavu
                RightSideMenu_expanded = true;
            }
        
        }

        private void DarkModeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SetTheme("DarkTheme.xaml");
        }

        private void DarkModeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SetTheme("LightTheme.xaml");
        }

        private void SetTheme(string themeFileName)
        {
            try
            {
                var app = Application.Current;

                // Cesta ke xaml
                var themePath = $"Resources/{themeFileName}";
                var themeDict = new ResourceDictionary { Source = new Uri(themePath, UriKind.Relative) };

                // Odstran jenom dark/light mode theme
                var existingTheme = app.Resources.MergedDictionaries
                                            .FirstOrDefault(d => d.Source.ToString().Contains("Theme"));
                if (existingTheme != null)
                {
                    app.Resources.MergedDictionaries.Remove(existingTheme);
                }

                // Aplikace 
                app.Resources.MergedDictionaries.Add(themeDict);
            }
            
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load theme: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
