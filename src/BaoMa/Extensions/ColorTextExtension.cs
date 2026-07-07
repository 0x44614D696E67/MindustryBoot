using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace BaoMa.Extensions;

public static class ColorTextExtension
{// 图标资源路径前缀
    private const string BaseIconPath = "ms-appx:///Assets/MindustryIcon/Items/";

    // 预设颜色名称（不区分大小写）
    private static readonly HashSet<string> ColorNames = new(StringComparer.OrdinalIgnoreCase)
        {
            "black", "white", "red", "orange", "yellow", "green", "cyan", "blue",
            "purple", "pink", "gray", "lightgray", "darkgray", "olive", "lime"
        };

    private static readonly Dictionary<string, Brush> ColorBrushes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["black"] = new SolidColorBrush(Microsoft.UI.Colors.Black),
        ["white"] = new SolidColorBrush(Microsoft.UI.Colors.White),
        ["red"] = new SolidColorBrush(Microsoft.UI.Colors.Red),
        ["orange"] = new SolidColorBrush(Microsoft.UI.Colors.Orange),
        ["yellow"] = new SolidColorBrush(Microsoft.UI.Colors.Yellow),
        ["green"] = new SolidColorBrush(Microsoft.UI.Colors.Green),
        ["cyan"] = new SolidColorBrush(Microsoft.UI.Colors.Cyan),
        ["blue"] = new SolidColorBrush(Microsoft.UI.Colors.Blue),
        ["purple"] = new SolidColorBrush(Microsoft.UI.Colors.Purple),
        ["pink"] = new SolidColorBrush(Microsoft.UI.Colors.Pink),
        ["gray"] = new SolidColorBrush(Microsoft.UI.Colors.Gray),
        ["lightgray"] = new SolidColorBrush(Microsoft.UI.Colors.LightGray),
        ["darkgray"] = new SolidColorBrush(Microsoft.UI.Colors.DarkGray),
        ["olive"] = new SolidColorBrush(Microsoft.UI.Colors.Olive),
        ["lime"] = new SolidColorBrush(Microsoft.UI.Colors.LimeGreen),
    };

    public static List<Inline> ToInlineList(this string text)
    {
        var inlines = new List<Inline>();
        if (string.IsNullOrEmpty(text)) return inlines;

        var colorStack = new Stack<Brush>();
        Brush currentBrush = null;
        var i = 0;

        while (i < text.Length)
        {
            if (text[i] == '[')
            {
                var end = text.IndexOf(']', i);
                if (end == -1)
                {
                    inlines.Add(new Run { Text = text.Substring(i) });
                    break;
                }

                var tag = text.Substring(i + 1, end - i - 1);
                if (string.IsNullOrEmpty(tag))
                {
                    // [] 回溯颜色栈
                    if (colorStack.Count > 0)
                    {
                        colorStack.Pop();
                        currentBrush = colorStack.Count > 0 ? colorStack.Peek() : null;
                    }
                }
                else if (IsColorTag(tag))
                {
                    var brush = ParseColorTag(tag);
                    if (brush != null)
                    {
                        colorStack.Push(brush);
                        currentBrush = brush;
                    }
                }
                else
                {
                    // 图标标记
                    var icon = CreateIconInline(tag);
                    if (icon != null)
                        inlines.Add(icon);
                    else
                        inlines.Add(new Run { Text = $"[{tag}]" }); // 无效标记显示原文
                }
                i = end + 1;
            }
            else
            {
                var next = text.IndexOf('[', i);
                if (next == -1)
                {
                    AddRun(inlines, text.Substring(i), currentBrush);
                    break;
                }
                AddRun(inlines, text.Substring(i, next - i), currentBrush);
                i = next;
            }
        }
        return inlines;
    }

    private static bool IsColorTag(string tag)
    {
        if (tag.StartsWith("#"))
        {
            var hex = tag.Substring(1);
            return hex.Length == 2 || hex.Length == 6 || hex.Length == 8;
        }
        return ColorNames.Contains(tag);
    }

    private static Brush ParseColorTag(string tag)
    {
        if (tag.StartsWith("#"))
        {
            var hex = tag.Substring(1);
            string fullHex;
            if (hex.Length == 2) fullHex = hex + "0000FF";
            else if (hex.Length == 6) fullHex = hex + "FF";
            else if (hex.Length == 8) fullHex = hex;
            else return null;

            if (uint.TryParse(fullHex, System.Globalization.NumberStyles.HexNumber, null, out var argb))
            {
                var a = (byte)((argb >> 24) & 0xFF);
                var r = (byte)((argb >> 16) & 0xFF);
                var g = (byte)((argb >> 8) & 0xFF);
                var b = (byte)(argb & 0xFF);
                return new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            }
            return null;
        }
        ColorBrushes.TryGetValue(tag, out var brush);
        return brush;
    }

    private static InlineUIContainer CreateIconInline(string key)
    {
        var uriString = BaseIconPath + $"item-{key}.png";
        try
        {
            var image = new Image
            {
                Source = new BitmapImage(new Uri(uriString)),
                Width = 16,
                Height = 16,
                Margin = new Microsoft.UI.Xaml.Thickness(0, 0, 2, 0),
                VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center
            };
            return new InlineUIContainer { Child = image };
        }
        catch
        {
            return null;
        }
    }

    private static void AddRun(List<Inline> inlines, string text, Brush brush)
    {
        if (string.IsNullOrEmpty(text)) return;
        var run = new Run { Text = text };
        if (brush != null) run.Foreground = brush;
        inlines.Add(run);
    }
}