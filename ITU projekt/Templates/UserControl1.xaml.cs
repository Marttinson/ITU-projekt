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

namespace ITU_projekt.Templates;

/// <summary>
/// Interakční logika pro UserControl1.xaml
/// </summary>
public partial class UserControl1 : UserControl
{
    public UserControl1()
    {
        try
        {
            InitializeComponent();
        }
        catch (Exception ex){
            MessageBox.Show($"An error occurred: {ex.Message}");
        }
    }

    // Button click handler for selecting a unit
    private void OnUnitSelected(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        string unit = button.Content.ToString();

        // TODO start task generation and switching templates
    }
}
