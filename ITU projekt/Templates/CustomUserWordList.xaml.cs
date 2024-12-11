using ITU_projekt.Models;
using System.Windows;
using System.Windows.Controls;
using ITU_projekt.ViewModels;
using ITU_projekt.API;
using System.Collections.Generic;

namespace ITU_projekt.Templates;

public partial class CustomUserWordList : UserControl
{
    public CustomUserWordList(UnitModel model)
    {
        DataContext = new CustomUserWordListViewModel(model);
        InitializeComponent();
    }
}
