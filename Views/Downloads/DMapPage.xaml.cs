using System.Collections.ObjectModel;
using System.Diagnostics;
using BaoMa.Getter;
using BaoMa.Types;
using MindustryBoot.Types;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindustryBoot.Views.Downloads;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class DMapPage : Page
{
    public ObservableCollection<MapType> MapsCollection = new();
    public ObservableCollection<MapFilterTypeOption> MapFilterList { get; }
    public ObservableCollection<MapFilterTypeOption> MapSortList { get; }

    private readonly MapGetter MapGetter = new();
    private int _loadRequestId;
    private bool _isInitialized;
    private bool _isLoadingNextPage;
    private bool _hasMoreMaps = true;
    private const int MapsPageSize = 15;
    private const double LoadMoreThreshold = 240;

    public DMapPage()
    {
        InitializeComponent();
        //MapsCollection.Add(new MapType() { Name = "Name", Describes = "DES" });

        MapSortList = new ObservableCollection<MapFilterTypeOption>()
        {
            new() { Glyph="\uE945", Text="更新时间" },
            new() { Glyph="\uE917", Text="发布时间" },
            new() { Glyph="\uE896", Text="下载量" },
            new() { Glyph="\uE8F2", Text="评分" },
            new() { Glyph="\uE8E1", Text="点赞数" },

        };
        MapSortComboBox.SelectedIndex = 0;
        MapSortSelectorBar.SelectedItem = MapSortSelectorBar.Items[0];

        // 初始化集合，填充原有的六个选项
        MapFilterList = new ObservableCollection<MapFilterTypeOption>
        {
            new() { Glyph = "\uE8A9", Text = "全部" },
            new() { Glyph = "\uE774", Text = "生存" },
            new() { Glyph = "\uE835", Text = "PvP" },
            new() { Glyph = "\uE759", Text = "进攻" },
            new() { Glyph = "\uE7B8", Text = "沙盒" },
            new() { Glyph = "\uE771", Text = "编辑器" },
            new() { Glyph = "\uE9CE", Text = "未分类" }
        };
        MapFilterComboBox.SelectedIndex = 0;
        MapFilterSelectorBar.SelectedItem = MapFilterSelectorBar.Items[0];

        _isInitialized = true;
        RefreshMaps();
    }

    private void MapFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ReloadMapsIfReady();
    }

    private void MapSortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ReloadMapsIfReady();
    }

    private void MapFilterSelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
    {
        ReloadMapsIfReady();
    }

    private void MapSortSelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
    {
        ReloadMapsIfReady();
    }

    private void ReloadMapsIfReady()
    {
        if (!_isInitialized) return;

        RefreshMaps();
    }

    private void MapsScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
    {
        if (e.IsIntermediate) return;

        LoadMoreMapsIfNeeded();
    }

    private void MapsScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        LoadMoreMapsIfNeeded();
    }

    private void LoadMoreMapsIfNeeded()
    {
        if (!_isInitialized || !_hasMoreMaps || _isLoadingNextPage || LoadingStateToggleSwitch.IsOn) return;

        var distanceToBottom = MapsScrollViewer.ScrollableHeight - MapsScrollViewer.VerticalOffset;
        if (distanceToBottom <= LoadMoreThreshold)
        {
            _ = LoadNextMapsPageAsync();
        }
    }

    private async void RefreshMaps()
    {
        var requestId = ++_loadRequestId;

        try
        {
            _hasMoreMaps = true;
            _isLoadingNextPage = false;
            LoadingStateToggleSwitch.IsOn = true;
            IncrementalLoadingPanel.Visibility = Visibility.Collapsed;
            MapsCollection.Clear();

            var Maps = await GetMapsPageAsync(0);

            if (requestId != _loadRequestId) return;

            AppendMaps(Maps);
            _hasMoreMaps = Maps.Count >= MapsPageSize;
            QueueLoadMoreCheck();
        }
        catch (HttpRequestException ex)
        {
            // 处理网络或 HTTP 错误
            Debug.WriteLine($"HTTP 请求失败: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            // 处理数据解析错误
            Debug.WriteLine($"数据无效: {ex.Message}");
        }
        catch (Exception ex)
        {
            // 处理其他意外异常
            Debug.WriteLine($"发生错误: {ex.Message}");
        }
        finally
        {
            if (requestId == _loadRequestId)
            {
                LoadingStateToggleSwitch.IsOn = false;
            }
        }
    }

    private async Task LoadNextMapsPageAsync()
    {
        var requestId = _loadRequestId;

        try
        {
            _isLoadingNextPage = true;
            IncrementalLoadingPanel.Visibility = Visibility.Visible;

            var Maps = await GetMapsPageAsync(MapsCollection.Count);

            if (requestId != _loadRequestId) return;

            AppendMaps(Maps);
            _hasMoreMaps = Maps.Count >= MapsPageSize;
            QueueLoadMoreCheck();
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"HTTP 请求失败: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Debug.WriteLine($"数据无效: {ex.Message}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"发生错误: {ex.Message}");
        }
        finally
        {
            if (requestId == _loadRequestId)
            {
                _isLoadingNextPage = false;
                IncrementalLoadingPanel.Visibility = Visibility.Collapsed;
            }
        }
    }

    private void QueueLoadMoreCheck()
    {
        _ = DispatcherQueue.TryEnqueue(LoadMoreMapsIfNeeded);
    }

    private Task<ObservableCollection<MapType>> GetMapsPageAsync(int begin)
    {
        return MapGetter.GetMapsAsync(
            begin: begin,
            mode: GetSelectedMapMode(),
            version: null,
            sorting: GetSelectedMapSorting(),
            search: null
        );
    }

    private void AppendMaps(ObservableCollection<MapType> maps)
    {
        Debug.WriteLine($"{maps}");
        Debug.WriteLine($"成功获取 {maps.Count} 个地图：");

        foreach (var map in maps)
        {
            Debug.WriteLine($"- {map.Name} (ID: {map.Id}, 版本: {map.Version ?? "未知"})");
            MapsCollection.Add(new MapType()
            {
                PreviewImg = map.PreviewImg,
                Name = map.Name,
                Describes = map.Describes,
                Tags = map.Tags,
                Id = map.Id,
                Version = map.Version ?? "未知"
            });
        }
    }

    private string GetSelectedMapMode()
    {
        var selectedFilterText = IsComboStyleToggleSwitch.IsOn
            ? (MapFilterComboBox.SelectedItem as MapFilterTypeOption)?.Text
            : (MapFilterSelectorBar.SelectedItem as SelectorBarItem)?.Text;

        return selectedFilterText switch
        {
            "生存" => "survival",
            "PvP" => "pvp",
            "进攻" => "attack",
            "沙盒" => "sandbox",
            "编辑器" => "editor",
            "未分类" => "unknown",
            _ => null
        };
    }

    private string GetSelectedMapSorting()
    {
        var selectedSortText = IsComboStyleToggleSwitch.IsOn
            ? (MapSortComboBox.SelectedItem as MapFilterTypeOption)?.Text
            : (MapSortSelectorBar.SelectedItem as SelectorBarItem)?.Text;

        return selectedSortText switch
        {
            "热度" => null,
            "更新时间" => "time",
            "发布时间" => "created",
            "下载量" => "downloads",
            "评分" => "rating",
            "点赞数" => "likes",
            _ => null
        };
    }
}
