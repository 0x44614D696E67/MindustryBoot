using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MindustryBoot.Types;

internal class MapType
{

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("desc")]
    public string Desc { get; set; }

    [JsonPropertyName("preview")]
    public string PreviewImg { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("mode")]
    public string Mode { get; set; }

    [JsonPropertyName("latest")]
    public string latest { get; set; }

    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; }

    public string Version { get; set; }
}
