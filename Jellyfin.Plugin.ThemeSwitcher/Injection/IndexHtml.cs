using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Jellyfin.Plugin.ThemeSwitcher.Injection;

/// <summary>
/// Adds and removes the stylesheet tag in jellyfin-web's index.html.
/// </summary>
public static class IndexHtml
{
    private const string FileName = "index.html";

    private static readonly Regex StyleTag = new(
        @"<(link|script) plugin=""ThemeSwitcher""[^>]*>(</script>)?\s*",
        RegexOptions.Compiled);

    /// <summary>
    /// Returns the page with exactly one, current stylesheet tag.
    /// </summary>
    /// <param name="html">The index.html contents.</param>
    /// <returns>The patched contents.</returns>
    public static string Inject(string html)
    {
        html = Remove(html);

        // Relative to /web/, so it also works when the server runs under a base URL.
        var version = Plugin.Instance?.Version.ToString() ?? "0";
        var tag = $"<link plugin=\"ThemeSwitcher\" rel=\"stylesheet\" href=\"../ThemeSwitcher/theme.css?v={version}\">";

        var headEnd = html.IndexOf("</head>", StringComparison.OrdinalIgnoreCase);
        html = headEnd < 0 ? tag + html : html.Insert(headEnd, tag);

        var script = $"<script plugin=\"ThemeSwitcher\" defer src=\"../ThemeSwitcher/client.js?v={version}\"></script>";
        var bodyEnd = html.LastIndexOf("</body>", StringComparison.OrdinalIgnoreCase);
        return bodyEnd < 0 ? html + script : html.Insert(bodyEnd, script);
    }

    /// <summary>
    /// Returns the page without the stylesheet tag.
    /// </summary>
    /// <param name="html">The index.html contents.</param>
    /// <returns>The cleaned contents.</returns>
    public static string Remove(string html) => StyleTag.Replace(html, string.Empty);

    /// <summary>
    /// Writes the stylesheet tag into index.html on disk.
    /// </summary>
    /// <param name="webPath">The jellyfin-web directory.</param>
    /// <param name="error">Why it failed, if it did.</param>
    /// <returns>Whether the file now contains the tag.</returns>
    public static bool TryInjectOnDisk(string webPath, out string? error) => TryRewrite(webPath, Inject, out error);

    /// <summary>
    /// Removes the stylesheet tag from index.html on disk.
    /// </summary>
    /// <param name="webPath">The jellyfin-web directory.</param>
    /// <param name="error">Why it failed, if it did.</param>
    /// <returns>Whether the file no longer contains the tag.</returns>
    public static bool TryRemoveFromDisk(string webPath, out string? error) => TryRewrite(webPath, Remove, out error);

    private static bool TryRewrite(string webPath, Func<string, string> rewrite, out string? error)
    {
        error = null;
        var path = Path.Combine(webPath, FileName);

        try
        {
            if (!File.Exists(path))
            {
                error = $"{path} doesn't exist";
                return false;
            }

            var html = File.ReadAllText(path);
            var patched = rewrite(html);
            if (!string.Equals(html, patched, StringComparison.Ordinal))
            {
                File.WriteAllText(path, patched);
            }

            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            error = ex.Message;
            return false;
        }
    }
}
