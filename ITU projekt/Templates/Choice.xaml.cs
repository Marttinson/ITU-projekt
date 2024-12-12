using ITU_projekt.API;
using ITU_projekt.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using ITU_projekt.ViewModels;

namespace ITU_projekt.Templates
{
    /// <summary>
    /// Interakční logika pro Choice.xaml
    /// </summary>
    public partial class Choice : UserControl
    {
        public Choice(MainWindowViewModel _VM, string _unit)
        {
            InitializeComponent();
            DataContext = new ChoiceViewModel(_VM, _unit);
        }
    }
}
