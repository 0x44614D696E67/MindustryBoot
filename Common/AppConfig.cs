using Nucs.JsonSettings.Examples;
using Nucs.JsonSettings.Modulation;
using Windows.Storage.Pickers.Provider;

namespace MindustryBoot.Common;

[GenerateAutoSaveOnChange]
public partial class AppConfig : NotifiyingJsonSettings, IVersionable
{
    [EnforcedVersion("1.0.0.0")]
    public Version Version { get; set; } = new Version(1, 0, 0, 0);

    // 将 FileName 属性从 private 改为 public，并使用 override 关键字覆盖基类成员
    public override string FileName { get; set; } = Constants.AppConfigPath;


    // Docs: https://github.com/Nucs/JsonSettings
}
