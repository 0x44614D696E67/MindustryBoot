using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using MindustryBoot.Helpers;

namespace MindustryBoot.Types;

/// <summary>
/// 地图类型（概要信息）类，对应 /maps/list 接口返回的 JSON 结构
/// </summary>
internal class MapType
{

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("desc")]
    public string Describes { get; set; }

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

/// <summary>
/// 初始资源配给项
/// </summary>
public class LoadoutItem
{
    [JsonPropertyName("item")]
    public string Item { get; set; }

    [JsonPropertyName("amount")]
    public int Amount { get; set; }
}

/// <summary>
/// 地图规则类，对应 DetailedMapInfoTags.Rules
/// </summary>
///
public class MapRules
{
    [JsonPropertyName("waves")]
    public bool isWavesOn { get; set; } = false;

    [JsonPropertyName("attackMode")]
    public bool isAttackOn { get; set; } = false;

    [JsonPropertyName("damageExplosions")]
    public bool DamageExplosions { get; set; } = false;

    [JsonPropertyName("fire")]
    public bool Fire { get; set; } = false;

    [JsonPropertyName("unitAmmo")]
    public bool UnitAmmo { get; set; } = false;

    [JsonPropertyName("unitDamageMultiplier")]
    public double UnitDamageMultiplier { get; set; } = 1;

    [JsonPropertyName("buildCostMultiplier")]
    public double BuildCostMultiplier { get; set; } = 1;

    [JsonPropertyName("buildSpeedMultiplier")]
    public double BuildSpeedMultiplier { get; set; } = 1;

    [JsonPropertyName("unitBuildSpeedMultiplier")]
    public double UnitBuildSpeedMultiplier { get; set; } = 1;

    [JsonPropertyName("deconstructRefundMultiplier")]
    public double DeconstructRefundMultiplier { get; set; } = 0;

    [JsonPropertyName("blockHealthMultiplier")]
    public double BlockHealthMultiplier { get; set; } = 1;

    [JsonPropertyName("blockDamageMultiplier")]
    public double BlockDamageMultiplier { get; set; } = 1;

    [JsonPropertyName("enemyCoreBuildRadius")]
    public int EnemyCoreBuildRadius { get; set; } = 400;

    [JsonPropertyName("waveSpacing")]
    public int WaveSpacing { get; set; } = 7200;

    [JsonPropertyName("unitCap")]
    public int UnitCap { get; set; } = 0;

    [JsonPropertyName("unitCapVariable")]
    public bool UnitCapVariable { get; set; } = true;

    [JsonPropertyName("loadout")]
    public List<LoadoutItem> Loadout { get; set; }
}

/// <summary>
/// 详细地图信息的 Tags 部分
/// </summary>
public class DetailedMapInfoTags
{
    [JsonPropertyName("mods")]
    public List<string> Mods { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("author")]
    public string Author { get; set; }

    [JsonPropertyName("build")]
    public int buildVer { get; set; }

    [JsonPropertyName("description")]
    public string Desc { get; set; }

    [JsonPropertyName("rules")]
    [JsonConverter(typeof(MapRulesHelper))] // 自定义转换器，需另行实现
    public MapRules Rules { get; set; }
}

/// <summary>
/// 地图详细信息类，对应 /maps/{hash}.json 接口返回的 JSON 结构
/// </summary>
internal class DetailedMapInfo
{
    [JsonPropertyName("hash")]
    public string Hash { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("tags")]
    public DetailedMapInfoTags Tags { get; set; }

    [JsonPropertyName("mode")]
    public string Mode { get; set; }

    [JsonPropertyName("user")]
    public WayzerUser Uploader { get; set; }

    [JsonPropertyName("preview")]
    public string PreviewImg { get; set; }
}
