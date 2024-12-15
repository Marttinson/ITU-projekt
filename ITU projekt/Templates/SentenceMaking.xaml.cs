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

namespace ITU_projekt.Templates
{
    /// <summary>
    /// Interakční logika pro SentenceMaking.xaml
    /// </summary>
    public partial class SentenceMaking : UserControl
    {
        public SentenceMaking(MainWindowViewModel _VM, string _unit, ref int _turn)
        {
            InitializeComponent();
            DataContext = new SentenceMakingViewModel(_VM, _unit, ref _turn);
        }
    }
}
