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
    public ObservableCollection<MapTypeSelectorOption> MapTypeFilter { get; }

    private MapGetter MapGetter = new();
    public DMapPage()
    {
        InitializeComponent();
        //MapsCollection.Add(new MapType() { Name = "Name", Describes = "DES" });

        // 初始化集合，填充原有的六个选项
        MapTypeFilter = new ObservableCollection<MapTypeSelectorOption>
            {
                new() { Glyph = "\uE774", Text = "生存" },
                new() { Glyph = "\uE835", Text = "PvP" },
                new() { Glyph = "\uE759", Text = "进攻" },
                new() { Glyph = "\uE7B8", Text = "沙盒" },
                new() { Glyph = "\uE771", Text = "编辑器" },
                new() { Glyph = "\uE9CE", Text = "未分类" }
            };

        // 如果希望默认选中某项，可设置 SelectedIndex（已在 XAML 中设为 0）
        // 也可通过绑定 SelectedItem 实现双向同步

        LoadMaps();
    }

    private async void LoadMaps()
    {
        try
        {
            // 2. 调用 GetMapsAsync 获取第一页地图（无筛选条件）
            LoadingStateToggleSwitch.IsOn = true;
            var Maps = await MapGetter.GetMapsAsync(
                begin: 0,
                mode: null,
                version: null,
                sorting: null,
                search: null
            );

            // 3. 输出结果
            Debug.WriteLine($"{Maps}");
            Debug.WriteLine($"成功获取 {Maps.Count} 个地图：");
            foreach (var map in Maps)
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
            LoadingStateToggleSwitch.IsOn = false;
        }
    }
}
