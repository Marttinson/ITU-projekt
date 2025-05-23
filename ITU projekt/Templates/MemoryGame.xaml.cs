﻿/** User Control pro pexeso
 * 
 * Martin Rybnikář (xrybni10)
 */
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
    public partial class MemoryGame : UserControl
    {
        public MemoryGame(MainWindowViewModel _VM)
        {
            InitializeComponent();
            DataContext = new MemoryGameViewModel(_VM);
        }
    }
}
