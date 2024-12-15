/** User Control pro spojování slov
 * 
 * Martin Rybnikář (xrybni10)
 */
using ITU_projekt.API;
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
    public partial class WordMatching : UserControl
    {   
        public WordMatching(MainWindowViewModel _VM, string _unit, ref int turn)
        {
            InitializeComponent();
            DataContext = new WordMatchingViewModel(_VM, _unit, ref turn);
        }
    }
}
