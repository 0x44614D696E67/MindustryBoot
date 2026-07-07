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
    public bool IsComboBoxStyle { get; set; } = true; // 筛选器与排序方式是否为组合框样式
    #endregion
}
