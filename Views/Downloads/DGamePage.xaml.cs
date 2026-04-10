// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindustryBoot.Views.Downloads;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class DGamePage : Page
{
    public DGamePage()
    {
        InitializeComponent();
        InitializeData();
    }

    private double AnimatedButttonHeight;
    private Microsoft.UI.Xaml.Thickness AnimatedBtnMargin;

    private void InitializeData()
    {
    }
}
