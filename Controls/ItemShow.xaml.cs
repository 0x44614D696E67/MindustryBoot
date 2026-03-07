using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Devices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindustryBoot.Controls;

public sealed partial class ItemShow : UserControl
{
    // 延迟控制专用字段
    private CancellationTokenSource _enterCts;
    private CancellationTokenSource _exitCts;
    private const int EnterDelayMs = 300;  // 悬停延迟（毫秒）
    private const int ExitDelayMs = 100;    // 离开延迟（毫秒）

    public ItemShow()
    {
        InitializeComponent();
    }
    // 依赖属性
    public static readonly DependencyProperty IconSourceProperty =
        DependencyProperty.Register(nameof(IconSource), typeof(ImageSource), typeof(ItemShow), new PropertyMetadata(null));
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(ItemShow), new PropertyMetadata(string.Empty));
    public static readonly DependencyProperty SubtitleProperty =
        DependencyProperty.Register(nameof(Subtitle), typeof(string), typeof(ItemShow), new PropertyMetadata(string.Empty));
    public static readonly DependencyProperty AuthorProperty =
        DependencyProperty.Register(nameof(Author), typeof(string), typeof(ItemShow), new PropertyMetadata(string.Empty));
    public static readonly DependencyProperty DetailProperty =
        DependencyProperty.Register(nameof(Detail), typeof(string), typeof(ItemShow), new PropertyMetadata(string.Empty));
    public static readonly DependencyProperty DownloadCommandProperty =
        DependencyProperty.Register(nameof(DownloadCommand), typeof(ICommand), typeof(ItemShow), new PropertyMetadata(null));
    public ImageSource IconSource
    {
        get => (ImageSource)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }
    public string Author
    {
        get => (string)GetValue(AuthorProperty);
        set => SetValue(AuthorProperty, value);
    }
    public string Detail
    {
        get => (string)GetValue(DetailProperty);
        set => SetValue(DetailProperty, value);
    }

    public ICommand DownloadCommand
    {
        get => (ICommand)GetValue(DownloadCommandProperty);
        set => SetValue(DownloadCommandProperty, value);
    }
    // 鼠标悬停事件
    // 鼠标进入事件：启动进入延迟，取消退出延迟
    private void UserControl_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        // 取消任何待处理的退出延迟
        _exitCts?.Cancel();
        _exitCts = null;

        // 取消之前可能存在的进入延迟
        _enterCts?.Cancel();
        _enterCts = new CancellationTokenSource();

        // 启动进入延迟任务
        Task.Delay(EnterDelayMs, _enterCts.Token).ContinueWith(t =>
        {
            if (!t.IsCanceled && !_enterCts.Token.IsCancellationRequested)
            {
                // 必须在 UI 线程上执行状态切换
                _ = DispatcherQueue.TryEnqueue(() =>
                {
                    VisualStateManager.GoToState(this, "PointerOver", true);
                });
            }
        }, TaskScheduler.Default);
    }

    // 鼠标离开事件：启动离开延迟，取消进入延迟
    private void UserControl_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        // 取消任何待处理的进入延迟
        _enterCts?.Cancel();
        _enterCts = null;

        // 取消之前可能存在的退出延迟
        _exitCts?.Cancel();
        _exitCts = new CancellationTokenSource();

        // 启动离开延迟任务
        Task.Delay(ExitDelayMs, _exitCts.Token).ContinueWith(t =>
        {
            if (!t.IsCanceled && !_exitCts.Token.IsCancellationRequested)
            {
                _ = DispatcherQueue.TryEnqueue(() =>
                {
                    VisualStateManager.GoToState(this, "Normal", true);
                });
            }
        }, TaskScheduler.Default);
    }
}
