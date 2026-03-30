using System.Text.Json.Serialization;

namespace MindustryBoot.Types;

/// <summary>
/// 地图上传者信息
/// </summary>
internal class WayzerUser
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("gid")]
    public string Gid { get; set; }
}
