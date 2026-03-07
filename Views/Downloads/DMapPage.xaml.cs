using System.Collections.ObjectModel;
using MindustryBoot.Types;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindustryBoot.Views.Downloads;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class DMapPage : Page
{
    private ObservableCollection<MapType> MapsCollection = new();
    public DMapPage()
    {
        InitializeComponent();
    }

    private void LoadMaps()
    {

    }
}
