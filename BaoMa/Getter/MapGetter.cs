using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using MindustryBoot.Types;
using Windows.Web.Http;

namespace MindustryBoot.BaoMa.Getter;

internal class MapGetter
{
    private readonly System.Net.Http.HttpClient _httpClient = new();

    /// <summary>
    /// 异步获取地图列表，支持分页、关键词搜索、模式/版本筛选以及排序。
    /// </summary>
    /// <param name="begin">分页起始索引（从0开始）</param>
    /// <param name="mode">游戏模式筛选，如 "pvp"、"attack"（对应查询中的 @mode）</param>
    /// <param name="version">游戏版本筛选，如 "v7.0"（对应查询中的 @version）</param>
    /// <param name="sorting">排序方式，如 "downloads"、"rating"（对应查询中的 @sort）</param>
    /// <param name="search">用户输入的地图名称搜索关键词</param>
    /// <returns>地图类型列表</returns>
    /// <exception cref="HttpRequestException">当HTTP请求失败时抛出</exception>
    /// <exception cref="InvalidOperationException">当响应内容无法反序列化为有效数据时抛出</exception>
    public async Task<ObservableCollection<MapType>> GetMapsAsync(int begin, string mode, string version, string sorting, string search)
    {
        // 1. 构建基础 URL
        var uriBuilder = new UriBuilder("https://api.mindustry.top/maps/list");

        // 2. 准备查询参数列表
        var queryParameters = new List<string>
            {
                $"begin={begin}" // 分页起始位置（必需）
            };

        // 3. 构建复合的 search 参数
        //    该参数由用户输入的关键词和多个筛选条件通过 "+" 连接而成
        var searchTokens = new List<string>();

        // 添加用户输入的关键词（如果有）
        if (!string.IsNullOrEmpty(search))
            searchTokens.Add(search);

        // 添加模式筛选（如果指定）
        if (!string.IsNullOrEmpty(mode))
            searchTokens.Add($"@mode:{mode}");

        // 添加版本筛选（如果指定）
        if (!string.IsNullOrEmpty(version))
            searchTokens.Add($"@version:{version}");

        // 添加排序方式（如果指定）
        if (!string.IsNullOrEmpty(sorting))
            searchTokens.Add($"@sort:{sorting}");

        // 如果存在任何搜索/筛选条件，将它们合并为 search 参数的值
        if (searchTokens.Count > 0)
        {
            string searchValue = string.Join("+", searchTokens);
            queryParameters.Add($"search={searchValue}");
        }

        // 4. 将参数列表连接成查询字符串并赋值给 UriBuilder
        uriBuilder.Query = string.Join("&", queryParameters);

        // 输出完整请求 URL（便于调试）
        System.Diagnostics.Debug.WriteLine($"请求 URL：{uriBuilder.Uri}");

        // 5. 创建并发送 HTTP 请求
        using var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, uriBuilder.Uri);
        using var response = await _httpClient.SendAsync(request);

        // 6. 检查 HTTP 状态码，如果失败则抛出异常
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"获取地图列表失败，状态码：{(int)response.StatusCode}，错误信息：{errorContent}");
        }

        // 7. 读取并反序列化 JSON 内容
        string json = await response.Content.ReadAsStringAsync();
        var maps = JsonSerializer.Deserialize<ObservableCollection<MapType>>(json);

        // 8. 如果反序列化后为 null（例如响应为空或格式错误），则抛出异常
        if (maps == null)
        {
            throw new InvalidOperationException("服务器返回的数据格式无效，无法解析为地图列表。");
        }

        // 9. 从地图的 Tags 列表中提取版本信息（假设版本标签以 "v" 开头，如 "v7.0"）
        foreach (var map in maps)
        {
            if (map.Tags != null)
            {
                foreach (string tag in map.Tags)
                {
                    if (tag.StartsWith("v", StringComparison.OrdinalIgnoreCase))
                    {
                        map.Version = tag; // 将第一个匹配的版本标签赋值给 Version 属性
                        break;             // 找到后即停止搜索
                    }
                }
            }
        }

        return maps;
    }
}
