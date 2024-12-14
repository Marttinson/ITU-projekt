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
using ITU_projekt.Models;
using ITU_projekt.ViewModels;

namespace ITU_projekt.Templates;

/// <summary>
/// Interakční logika pro UserControl1.xaml
/// </summary>
public partial class Graph : UserControl
{
    public Graph(UnitModel model)
    {
        InitializeComponent();
        DataContext = new GraphViewModel(model);
    }
}
