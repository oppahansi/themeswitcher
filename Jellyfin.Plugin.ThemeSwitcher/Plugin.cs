using System;
using System.Collections.Generic;
using Jellyfin.Plugin.ThemeSwitcher.Configuration;
using Jellyfin.Plugin.ThemeSwitcher.Injection;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.ThemeSwitcher;

/// <summary>
/// The Theme Switcher plugin.
/// </summary>
public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
{
    private readonly IApplicationPaths _applicationPaths;

    /// <summary>
    /// Initializes a new instance of the <see cref="Plugin"/> class.
    /// </summary>
    /// <param name="applicationPaths">Instance of the <see cref="IApplicationPaths"/> interface.</param>
    /// <param name="xmlSerializer">Instance of the <see cref="IXmlSerializer"/> interface.</param>
    public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
        : base(applicationPaths, xmlSerializer)
    {
        _applicationPaths = applicationPaths;
        Instance = this;
    }

    /// <inheritdoc />
    public override string Name => "Theme Switcher";

    /// <inheritdoc />
    public override Guid Id => Guid.Parse("4fdfbe7d-28a8-429c-acc1-1a471dde3a1f");

    /// <inheritdoc />
    public override string Description => "Changes the look of the Jellyfin web UI with selectable themes, an accent color override and custom CSS.";

    /// <summary>
    /// Gets the current plugin instance.
    /// </summary>
    public static Plugin? Instance { get; private set; }

    /// <inheritdoc />
    public IEnumerable<PluginPageInfo> GetPages()
    {
        yield return new PluginPageInfo
        {
            Name = Name,
            EmbeddedResourcePath = GetType().Namespace + ".Configuration.configPage.html"
        };
    }

    /// <inheritdoc />
    public override void OnUninstalling()
    {
        // Undo the index.html fallback so jellyfin-web doesn't keep requesting a stylesheet that no longer exists.
        IndexHtml.TryRemoveFromDisk(_applicationPaths.WebPath, out _);
        base.OnUninstalling();
    }
}
