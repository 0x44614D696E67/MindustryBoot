using System.Windows.Input;

namespace MindustryBoot.Views;

public sealed partial class SettingsPage : Page
{
    public ICommand NavigateToGeneralSettingsCommand { get; }
    public ICommand NavigateToThemeSettingsCommand { get; }
    public ICommand NavigateToAboutUsSettingsCommand { get; }

    public SettingsPage()
    {
        NavigateToGeneralSettingsCommand = new NavigatePageCommand(this, typeof(GeneralSettingPage));
        NavigateToThemeSettingsCommand = new NavigatePageCommand(this, typeof(ThemeSettingPage));
        NavigateToAboutUsSettingsCommand = new NavigatePageCommand(this, typeof(AboutUsSettingPage));

        this.InitializeComponent();
    }

    private sealed partial class NavigatePageCommand : ICommand
    {
        private readonly SettingsPage _owner;
        private readonly Type _pageType;

        public NavigatePageCommand(SettingsPage owner, Type pageType)
        {
            _owner = owner;
            _pageType = pageType;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _owner.Frame?.Navigate(_pageType);
        }
    }
}
