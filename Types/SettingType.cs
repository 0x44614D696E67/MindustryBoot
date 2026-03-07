using System;
using System.Collections.Generic;
using System.Text;
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
}
