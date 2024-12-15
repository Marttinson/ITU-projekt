/** MainWindow
 * V
 * Vojtěch Hrabovksý (xhrabo18)
 * 
 * Codde behind hlavního okna
 */

using ITU_projekt.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ITU_projekt.Views
{
    public partial class MainWindowView : Window
    {
        // Stav bocniho panelu
        private bool RightSideMenu_expanded = false;

        public MainWindowView()
        {
            // defaultni rezim
            SetTheme("LightTheme.xaml");
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }

        // Handler kliknuti na tlacitko nastaveni
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {

            if (RightSideMenu_expanded)
            {
                // Kompresni animace pro šířku
                var menuAnimation = new DoubleAnimation
                {
                    From = RightSideMenu.ActualWidth,
                    To = 0, // Minimalni sirka
                    Duration = new Duration(TimeSpan.FromSeconds(0.3))
                };

                // Aplikování animace na šířku
                RightSideMenu.BeginAnimation(Grid.WidthProperty, menuAnimation);

                // Zapamatování stavu
                RightSideMenu_expanded = false;
            }
            else
            {
                // Expanzní animace pro šířku
                var menuAnimation = new DoubleAnimation
                {
                    From = RightSideMenu.ActualWidth,
                    To = 150,  // Maximalni sirka
                    Duration = new Duration(TimeSpan.FromSeconds(0.3))
                };

                // Aplikování animace na šířku
                RightSideMenu.BeginAnimation(Grid.WidthProperty, menuAnimation);

                // Zapamatování stavu
                RightSideMenu_expanded = true;
            }
        }


        // Zmena modu pri prepnuti checkboxu
        private void DarkModeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SetTheme("DarkTheme.xaml");
        }

        private void DarkModeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SetTheme("LightTheme.xaml");
        }

        // Funkce pro prepnuti light/dark modu
        private void SetTheme(string themeFileName)
        {
            try
            {
                var app = Application.Current;

                // Cesta ke xaml
                var themePath = $"Resources/{themeFileName}";
                var themeDict = new ResourceDictionary { Source = new Uri(themePath, UriKind.Relative) };

                // Odstran jenom dark/light mode theme
                var existingTheme = app.Resources.MergedDictionaries.FirstOrDefault(d => d.Source.ToString().Contains("Theme"));

                if (existingTheme != null)
                {
                    app.Resources.MergedDictionaries.Remove(existingTheme);
                }

                // Pridat novy theme
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
