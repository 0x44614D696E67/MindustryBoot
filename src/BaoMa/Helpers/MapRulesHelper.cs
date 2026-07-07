using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BaoMa.Types;

namespace BaoMa.Helpers;

internal class MapRulesHelper : System.Text.Json.Serialization.JsonConverter<MapRules>
{
    // 预定义正则选项：编译、忽略大小写、单行模式
    private static readonly RegexOptions RegexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline;

    // 默认值常量（与原始代码一致）
    private const int DefaultEnemyCoreBuildRadius = 400;
    private const int DefaultWaveSpacing = 7200;
    private const int DefaultUnitCap = 0;
    private const double DefaultUnitDamageMultiplier = 1.0;
    private const double DefaultBuildCostMultiplier = 1.0;
    private const double DefaultBuildSpeedMultiplier = 1.0;
    private const double DefaultUnitBuildSpeedMultiplier = 1.0;
    private const double DefaultDeconstructRefundMultiplier = 0.0;
    private const double DefaultBlockHealthMultiplier = 1.0;
    private const double DefaultBlockDamageMultiplier = 1.0;

    /// <summary>
    /// 读取 JSON 并转换为 <see cref="MapRules"/> 对象。
    /// </summary>
    public override MapRules Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                string hjsonString = reader.GetString();
                try
                {
                    return ParseMapRulesFromHjson(hjsonString);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"正则提取失败: {ex.Message}");
                    return null;
                }

            case JsonTokenType.StartObject:
                return JsonSerializer.Deserialize<MapRules>(ref reader, options);

            case JsonTokenType.Null:
                return null;

            default:
                reader.Skip(); // 跳过无法处理的令牌
                return null;
        }
    }

    /// <summary>
    /// 从 HJSON 格式的字符串中解析出 <see cref="MapRules"/> 对象。
    /// </summary>
    private MapRules ParseMapRulesFromHjson(string hjson)
    {
        var rules = new MapRules();

        // 填充各个属性（使用辅助方法提取）
        rules.isWavesOn = GetBool(hjson, "waves");
        rules.isAttackOn = GetBool(hjson, "attackMode");
        rules.DamageExplosions = GetBool(hjson, "damageExplosions");
        rules.Fire = GetBool(hjson, "fire");
        rules.UnitAmmo = GetBool(hjson, "unitAmmo");
        rules.UnitCapVariable = GetBool(hjson, "unitCapVariable");

        rules.EnemyCoreBuildRadius = GetInt(hjson, "enemyCoreBuildRadius", DefaultEnemyCoreBuildRadius);
        rules.WaveSpacing = GetInt(hjson, "waveSpacing", DefaultWaveSpacing);
        rules.UnitCap = GetInt(hjson, "unitCap", DefaultUnitCap);

        rules.UnitDamageMultiplier = GetDouble(hjson, "unitDamageMultiplier", DefaultUnitDamageMultiplier);
        rules.BuildCostMultiplier = GetDouble(hjson, "buildCostMultiplier", DefaultBuildCostMultiplier);
        rules.BuildSpeedMultiplier = GetDouble(hjson, "buildSpeedMultiplier", DefaultBuildSpeedMultiplier);
        rules.UnitBuildSpeedMultiplier = GetDouble(hjson, "unitBuildSpeedMultiplier", DefaultUnitBuildSpeedMultiplier);
        rules.DeconstructRefundMultiplier = GetDouble(hjson, "deconstructRefundMultiplier", DefaultDeconstructRefundMultiplier);
        rules.BlockHealthMultiplier = GetDouble(hjson, "blockHealthMultiplier", DefaultBlockHealthMultiplier);
        rules.BlockDamageMultiplier = GetDouble(hjson, "blockDamageMultiplier", DefaultBlockDamageMultiplier);

        rules.Loadout = ParseLoadout(hjson);

        return rules;
    }

    #region 辅助提取方法

    /// <summary>
    /// 从 HJSON 字符串中提取指定键的布尔值。
    /// </summary>
    private static bool GetBool(string hjson, string key)
    {
        var pattern = $@"\b{key}\s*:\s*(true|false)\b";
        var match = Regex.Match(hjson, pattern, RegexOptions);
        return match.Success && match.Groups[1].Value.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 从 HJSON 字符串中提取指定键的整数值，若不存在则返回默认值。
    /// </summary>
    private static int GetInt(string hjson, string key, int defaultValue)
    {
        var pattern = $@"\b{key}\s*:\s*(-?\d+)\b";
        var match = Regex.Match(hjson, pattern, RegexOptions);
        return match.Success ? int.Parse(match.Groups[1].Value) : defaultValue;
    }

    /// <summary>
    /// 从 HJSON 字符串中提取指定键的双精度浮点数值，若不存在则返回默认值。
    /// </summary>
    private static double GetDouble(string hjson, string key, double defaultValue)
    {
        var pattern = $@"\b{key}\s*:\s*(-?\d+(?:\.\d+)?)\b";
        var match = Regex.Match(hjson, pattern, RegexOptions);
        return match.Success ? double.Parse(match.Groups[1].Value) : defaultValue;
    }

    /// <summary>
    /// 从 HJSON 字符串中解析出初始资源配给列表（loadout）。
    /// </summary>
    private static List<LoadoutItem> ParseLoadout(string hjson)
    {
        var list = new List<LoadoutItem>();

        // 匹配 loadout 数组的整体内容（非贪婪，取第一个 [...] 内的部分）
        var arrayMatch = Regex.Match(hjson, @"loadout\s*:\s*\[(.*?)\]", RegexOptions);
        if (!arrayMatch.Success)
            return list;

        string loadoutContent = arrayMatch.Groups[1].Value;

        // 逐个匹配数组中的对象：{ item: 名称, amount: 数字 }
        var itemMatches = Regex.Matches(loadoutContent, @"\{\s*item\s*:\s*([^,}\s]+)\s*,\s*amount\s*:\s*(\d+)\s*\}", RegexOptions);
        foreach (Match itemMatch in itemMatches)
        {
            list.Add(new LoadoutItem
            {
                Item = itemMatch.Groups[1].Value,
                Amount = int.Parse(itemMatch.Groups[2].Value)
            });
        }

        return list;
    }
    #endregion

    /// <summary>
    /// 将 <see cref="MapRules"/> 对象写入 JSON。
    /// </summary>
    public override void Write(Utf8JsonWriter writer, MapRules value, JsonSerializerOptions options)
    {
        // 直接使用默认序列化（保持原行为）
        JsonSerializer.Serialize(writer, value, options);
    }
}
