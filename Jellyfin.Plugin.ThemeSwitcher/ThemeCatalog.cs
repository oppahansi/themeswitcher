using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Jellyfin.Plugin.ThemeSwitcher;

/// <summary>
/// A theme loaded from an embedded <c>Web/themes/&lt;id&gt;.css</c> file.
/// </summary>
/// <param name="Id">The file name without extension.</param>
/// <param name="Name">Display name from the <c>@name</c> header comment.</param>
/// <param name="Dark">Whether the header says <c>@scheme dark</c>.</param>
/// <param name="Css">The file contents.</param>
/// <param name="Background">The <c>--ts-bg</c> value, for the settings preview.</param>
/// <param name="Surface">The <c>--ts-surface</c> value, for the settings preview.</param>
/// <param name="Text">The <c>--ts-text</c> value, for the settings preview.</param>
/// <param name="Accent">The <c>--ts-accent</c> value, for the settings preview.</param>
public sealed record Theme(string Id, string Name, bool Dark, string Css, string Background, string Surface, string Text, string Accent);

/// <summary>
/// Finds the themes embedded in the plugin and builds the stylesheet served to the web client.
/// To add a theme, drop a CSS file into Web/themes. See CONTRIBUTING.md for the format.
/// </summary>
public static partial class ThemeCatalog
{
    /// <summary>
    /// The id of the "no theme" entry.
    /// </summary>
    public const string DefaultId = "default";

    private const string Folder = ".Web.themes.";

    private static readonly Lazy<IReadOnlyList<Theme>> Loaded = new(Load);

    /// <summary>
    /// Gets the embedded themes, sorted by name.
    /// </summary>
    public static IReadOnlyList<Theme> All => Loaded.Value;

    /// <summary>
    /// Checks whether a value is a #rrggbb color.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>Whether it is a valid color.</returns>
    public static bool IsHexColor(string? value) => value is not null && HexColor().IsMatch(value);

    /// <summary>
    /// Builds the stylesheet for a configuration: shared rules, theme, accent override, custom CSS.
    /// </summary>
    /// <param name="themeId">The theme id.</param>
    /// <param name="accentOverride">An optional #rrggbb accent.</param>
    /// <param name="customCss">CSS appended at the end.</param>
    /// <param name="baseCss">The shared stylesheet that uses the variables.</param>
    /// <returns>The CSS.</returns>
    public static string Build(string themeId, string? accentOverride, string? customCss, string baseCss)
    {
        var css = new StringBuilder();
        var theme = All.FirstOrDefault(t => string.Equals(t.Id, themeId, StringComparison.OrdinalIgnoreCase));

        if (theme is not null)
        {
            css.Append(baseCss).Append('\n').Append(theme.Css).Append('\n');
            if (IsHexColor(accentOverride))
            {
                css.Append(":root{--ts-accent:").Append(accentOverride).Append(";--ts-accent-text:").Append(ReadableOn(accentOverride!)).Append("}\n");
            }
        }

        if (!string.IsNullOrWhiteSpace(customCss))
        {
            css.Append(customCss).Append('\n');
        }

        return css.ToString();
    }

    private static IReadOnlyList<Theme> Load()
    {
        var assembly = typeof(Plugin).Assembly;
        var prefix = typeof(Plugin).Namespace + Folder;
        var themes = new List<Theme>();

        foreach (var resource in assembly.GetManifestResourceNames().Where(n => n.StartsWith(prefix, StringComparison.Ordinal) && n.EndsWith(".css", StringComparison.Ordinal)))
        {
            using var stream = assembly.GetManifestResourceStream(resource);
            if (stream is null)
            {
                continue;
            }

            using var reader = new StreamReader(stream, Encoding.UTF8);
            var css = reader.ReadToEnd();
            var id = resource[prefix.Length..^".css".Length];
            themes.Add(new Theme(
                id,
                Header("name", css) ?? id,
                !string.Equals(Header("scheme", css), "light", StringComparison.OrdinalIgnoreCase),
                css,
                Variable("bg", css),
                Variable("surface", css),
                Variable("text", css),
                Variable("accent", css)));
        }

        return themes.OrderBy(t => t.Name, StringComparer.OrdinalIgnoreCase).ToList();
    }

    private static string? Header(string key, string css)
    {
        var match = Regex.Match(css, @"/\*\s*@" + key + @"\s+(.+?)\s*\*/");
        return match.Success ? match.Groups[1].Value : null;
    }

    private static string Variable(string name, string css)
    {
        var match = Regex.Match(css, @"--ts-" + name + @"\s*:\s*([^;}]+)");
        return match.Success ? match.Groups[1].Value.Trim() : "#808080";
    }

    // Black or white, whichever is easier to read on top of the given #rrggbb color (WCAG relative luminance).
    private static string ReadableOn(string hex)
    {
        double Channel(int offset)
        {
            var c = int.Parse(hex.AsSpan(offset, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture) / 255.0;
            return c <= 0.03928 ? c / 12.92 : Math.Pow((c + 0.055) / 1.055, 2.4);
        }

        var luminance = (0.2126 * Channel(1)) + (0.7152 * Channel(3)) + (0.0722 * Channel(5));
        return luminance > 0.4 ? "#000000" : "#ffffff";
    }

    [GeneratedRegex("^#[0-9a-fA-F]{6}$")]
    private static partial Regex HexColor();
}
