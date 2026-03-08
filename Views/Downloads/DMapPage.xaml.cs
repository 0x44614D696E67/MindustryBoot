using System.Collections.ObjectModel;
using System.Diagnostics;
using MindustryBoot.BaoMa.Getter;
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
    private MapGetter MapGetter = new();
    public DMapPage()
    {
        InitializeComponent();
        LoadMaps();
    }

    private async void LoadMaps()
    {
        try
        {
            // 2. 调用 GetMapsAsync 获取第一页地图（无筛选条件）
            MapsCollection = await MapGetter.GetMapsAsync(
                begin: 0,
                mode: null,
                version: null,
                sorting: null,
                search: null
            );

            // 3. 输出结果
            Debug.WriteLine($"成功获取 {MapsCollection.Count} 个地图：");
            foreach (var map in MapsCollection)
            {
                Debug.WriteLine($"- {map.Name} (ID: {map.Id}, 版本: {map.Version ?? "未知"})");
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
            Console.WriteLine($"发生错误: {ex.Message}");
        }
    }
}
