using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.ThemeSwitcher.Configuration;

/// <summary>
/// Plugin configuration.
/// </summary>
public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Gets or sets the id of the active theme, see <see cref="ThemeCatalog"/>. "default" leaves Jellyfin's own look alone.
    /// </summary>
    public string Theme { get; set; } = ThemeCatalog.DefaultId;

    /// <summary>
    /// Gets or sets an optional accent color override as #rrggbb. Empty keeps the theme's own accent.
    /// </summary>
    public string AccentColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets CSS appended after the theme, for small tweaks.
    /// </summary>
    public string CustomCss { get; set; } = string.Empty;
}
