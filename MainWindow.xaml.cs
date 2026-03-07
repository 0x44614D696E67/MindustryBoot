using Microsoft.UI.Windowing;

namespace MindustryBoot.Views;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Standard;

        /*App.Current.NavService
            .Initialize(NavView, NavFrame, NavigationPageMappings.PageDictionary)
                .ConfigureDefaultPage(typeof(HomeLandingPage))
                .ConfigureSettingsPage(typeof(SettingsPage))
                .ConfigureJsonFile("Assets/NavViewMenu/AppData.json")
                .ConfigureTitleBar(AppTitleBar)
                .ConfigureBreadcrumbBar(BreadCrumbNav, BreadcrumbPageMappings.PageDictionary);*/
    }

    public new void Activate()
    {
        base.Activate();
        NavFrame.Navigate(typeof(HomeLandingPage));
    }

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected) NavFrame.Navigate(typeof(SettingsPage));
        var selectedItem = (NavigationViewItem)args.SelectedItem;
        if ((string)selectedItem.Tag == "Settings") NavFrame.Navigate(typeof(SettingsPage));
        if ((string)selectedItem.Tag == "Home") NavFrame.Navigate(typeof(HomeLandingPage));
        if ((string)selectedItem.Tag == "GameManager") NavFrame.Navigate(typeof(GameManagerPage));
        if ((string)selectedItem.Tag == "Download") NavFrame.Navigate(typeof(DownloadPage));
    }

    private void ThemeButton_Click(object sender, RoutedEventArgs e)
    {
        App.Current.ThemeService.SetElementThemeWithoutSaveAsync();
    }

    private void AppTitleBar_BackRequested(TitleBar sender, object args)
    {
        if (NavFrame.CanGoBack) { NavFrame.GoBack(); }
    }
}
