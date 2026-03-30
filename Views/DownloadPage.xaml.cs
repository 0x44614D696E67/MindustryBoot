using Microsoft.UI.Xaml.Media.Animation;
using MindustryBoot.Views.Downloads;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindustryBoot.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class DownloadPage : Page
{
    private int previousSelectedIndex;

    public DownloadPage()
    {
        InitializeComponent();
    }

    private void DownloadTypeSelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
    {
        SelectorBarItem selectedItem = sender.SelectedItem;
        int currentSelectedIndex = sender.Items.IndexOf(selectedItem);
        System.Type pageType;

        switch (currentSelectedIndex)
        {
            case 0:
                pageType = typeof(DGamePage);
                break;
            case 1:
                pageType = typeof(DJavaPage);
                break;
            case 2:
                pageType = typeof(DMapPage);
                break;
            case 3:
                pageType = typeof(DModPage);
                break;
            default:
                pageType = typeof(DGamePage);
                break;
        }

        var slideNavigationTransitionEffect = currentSelectedIndex - previousSelectedIndex > 0 ? SlideNavigationTransitionEffect.FromRight : SlideNavigationTransitionEffect.FromLeft;

        ContentFrame.Navigate(pageType, null, new SlideNavigationTransitionInfo() { Effect = slideNavigationTransitionEffect });

        previousSelectedIndex = currentSelectedIndex;

    }
}
