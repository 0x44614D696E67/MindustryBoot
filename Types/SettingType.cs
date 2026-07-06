using Windows.UI;

namespace MindustryBoot.Types;

internal class SettingType
{
    #region Theme
    public bool IsAutoDarkMode { get; set; } = true;
    public bool IsDarkMode { get; set; } = false;
    public bool IsAutoThemeColor { get; set; } = true;
    public Color ThemeColor { get; set; } = Colors.Transparent;
    public BackdropType BackgroundType { get; set; } = BackdropType.Mica;
    #endregion

    #region DMapPage
    public bool IsSortingMethodIsComboBox { get; set; } = true; // 地图排序方式是否为组合框形式
    public bool IsMapTypeFilterIsComboBox { get; set; } = true; // 地图类型筛选是否为组合框形式
    #endregion
}
