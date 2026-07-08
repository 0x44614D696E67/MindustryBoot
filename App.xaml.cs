using System.Diagnostics;
using MindustryBoot.Common;

namespace MindustryBoot;

public partial class App : Application
{
    public new static App Current => (App)Application.Current;
    public static Window MainWindow = Window.Current;
    public static IntPtr Hwnd => WinRT.Interop.WindowNative.GetWindowHandle(MainWindow);
    public JsonNavigationService NavService { get; set; }
    public IThemeService ThemeService { get; set; }

    public App()
    {
        this.InitializeComponent();
        UnhandledException += App_UnhandledException;
        NavService = new JsonNavigationService();
        // DebugSettings.IsTextPerformanceVisualizationEnabled = true;
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        MainWindow = new MainWindow();

        MainWindow.Title = MainWindow.AppWindow.Title = ProcessInfoHelper.ProductNameAndVersion;
        MainWindow.AppWindow.SetIcon("Assets/AppIcon.ico");

        ThemeService = new ThemeService().Initialize(MainWindow);

        MainWindow.Activate();


        InitializeApp();
    }

    private void InitializeApp()
    {

    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        WriteUnhandledExceptionLog(e.Exception);
    }

    private static void WriteUnhandledExceptionLog(Exception exception)
    {
        try
        {
            Directory.CreateDirectory(Constants.RootDirectoryPath);
            var logPath = Path.Combine(Constants.RootDirectoryPath, "UnhandledException.log");
            var logContent = $"""
                [{DateTimeOffset.Now:O}]
                {exception}

                """;

            Debug.WriteLine(logContent);
            Debug.WriteLine(exception);
            Debug.WriteLine(logPath);

            File.AppendAllText(logPath, logContent);
        }
        catch
        {
            // Avoid throwing while handling an unhandled exception.
        }
    }
}
