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
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace ITU_projekt.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {

        private bool RightSideMenu_expanded = false;

        private double _sideMenu;
        public double sideMenuSize
        {
            get => _sideMenu;
            set
            {
                _sideMenu = value;
                OnPropertyChanged();
            }
        }

        public MainWindowView()
        {
            SetTheme("LightTheme.xaml");
            DataContext = new MainWindowViewModel();
            //sideMenuSize = 150;
            InitializeComponent();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("ZMACKNUTE TLACITKO!!");

            if (RightSideMenu_expanded)
            {
                // Kompresni animace pro šířku
                var menuAnimation = new DoubleAnimation
                {
                    From = RightSideMenu.ActualWidth,
                    To = 0,
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
                    To = 150,  // Předpokládaná maximální šířka panelu
                    Duration = new Duration(TimeSpan.FromSeconds(0.3))
                };

                // Aplikování animace na šířku
                RightSideMenu.BeginAnimation(Grid.WidthProperty, menuAnimation);

                // Zapamatování stavu
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

    public class HeightConverter : IValueConverter
    {
        // Konvertor pro výpočet 10% výšky okna
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double height = (double)value;
            return height * 0.1; // Vráti 10% výšky
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double width = (double)value;
            return width * 0.5; // 50% šířky
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
