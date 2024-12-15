/** Graph
 * V
 *  Vojtěch Hrabovský (xhrabo18)
 *  
 *  Code-behind pro prezentaci uživatelovy statistiky neúspěšnosti
 */


using System.Windows.Controls;

using ITU_projekt.Models;

namespace ITU_projekt.Templates;

public partial class Graph : UserControl
{
    public Graph(UnitModel model)
    {
        InitializeComponent();
        DataContext = new GraphViewModel(model);
    }
}
