using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using BaoMa.Extensions;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindustryBoot.Controls;

public sealed partial class AutoRichText : UserControl
{
    public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(AutoRichText),
                new PropertyMetadata(string.Empty, OnTextChanged));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public AutoRichText()
    {
        this.InitializeComponent();
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (AutoRichText)d;
        control.UpdateContent(e.NewValue as string ?? "");
    }

    private void UpdateContent(string text)
    {
        ContentRichTextBlock.Blocks.Clear();
        if (string.IsNullOrEmpty(text)) return;

        var inlines = text.ToInlineList();
        if (inlines.Count == 0) return;

        var paragraph = new Paragraph();
        foreach (var inline in inlines)
            paragraph.Inlines.Add(inline);
        ContentRichTextBlock.Blocks.Add(paragraph);
    }
}
