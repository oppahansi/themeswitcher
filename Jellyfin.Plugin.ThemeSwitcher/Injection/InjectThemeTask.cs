using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.ThemeSwitcher.Injection;

/// <summary>
/// Adds the theme stylesheet to jellyfin-web on every server start. A hidden startup task rather than a
/// hosted service, because File Transformation only accepts registrations once its own startup is done.
/// </summary>
public class InjectThemeTask : IScheduledTask, IConfigurableScheduledTask
{
    private readonly IApplicationPaths _applicationPaths;
    private readonly ILogger<InjectThemeTask> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="InjectThemeTask"/> class.
    /// </summary>
    /// <param name="applicationPaths">Instance of the <see cref="IApplicationPaths"/> interface.</param>
    /// <param name="logger">Instance of the <see cref="ILogger{T}"/> interface.</param>
    public InjectThemeTask(IApplicationPaths applicationPaths, ILogger<InjectThemeTask> logger)
    {
        _applicationPaths = applicationPaths;
        _logger = logger;
    }

    /// <inheritdoc />
    public string Name => "ThemeSwitcher: inject stylesheet";

    /// <inheritdoc />
    public string Key => "ThemeSwitcherInjectTheme";

    /// <inheritdoc />
    public string Description => "Adds the theme stylesheet to the web client.";

    /// <inheritdoc />
    public string Category => "ThemeSwitcher";

    /// <inheritdoc />
    public bool IsHidden => true;

    /// <inheritdoc />
    public bool IsEnabled => true;

    /// <inheritdoc />
    public bool IsLogged => false;

    /// <inheritdoc />
    public Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
    {
        var webPath = _applicationPaths.WebPath;

        try
        {
            if (FileTransformation.TryRegister())
            {
                // Don't leave an older on-disk tag behind, or the stylesheet would load twice.
                IndexHtml.TryRemoveFromDisk(webPath, out _);
                _logger.LogInformation("ThemeSwitcher: stylesheet registered with File Transformation");
                return Task.CompletedTask;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "ThemeSwitcher: File Transformation registration failed, falling back to index.html");
        }

        if (IndexHtml.TryInjectOnDisk(webPath, out var error))
        {
            _logger.LogInformation("ThemeSwitcher: stylesheet added to {Path}", webPath);
        }
        else
        {
            _logger.LogWarning(
                "ThemeSwitcher: couldn't add the stylesheet to index.html ({Error}). Install the File Transformation plugin or make the web directory writable",
                error);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
    {
        return
        [
            new TaskTriggerInfo
            {
#if NET9_0_OR_GREATER
                Type = TaskTriggerInfoType.StartupTrigger
#else
                Type = TaskTriggerInfo.TriggerStartup
#endif
            }
        ];
    }
}
