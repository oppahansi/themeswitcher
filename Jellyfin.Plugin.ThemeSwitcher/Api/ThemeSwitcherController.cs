using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Jellyfin.Plugin.ThemeSwitcher.Api;

/// <summary>
/// Serves the generated theme stylesheet and the list of themes.
/// </summary>
[ApiController]
[Route("ThemeSwitcher")]
public class ThemeSwitcherController : ControllerBase
{
    /// <summary>
    /// Gets the stylesheet for the configured theme. Anonymous, because the login page is themed too.
    /// </summary>
    /// <returns>The CSS file.</returns>
    [HttpGet("theme.css")]
    [AllowAnonymous]
    [Produces("text/css")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status304NotModified)]
    public ActionResult GetThemeCss()
    {
        var config = Plugin.Instance?.Configuration;
        if (config is null)
        {
            return NotFound();
        }

        var bytes = Encoding.UTF8.GetBytes(ThemeCatalog.Build(config.Theme, config.AccentColor, config.CustomCss, ReadBaseCss()));

        // The <link> in index.html is fixed, so revalidate every time and let the ETag answer unchanged settings with a 304.
        Response.Headers.CacheControl = "no-cache";
        var etag = new EntityTagHeaderValue("\"" + Convert.ToHexString(SHA256.HashData(bytes))[..16] + "\"");
        return File(bytes, "text/css; charset=utf-8", lastModified: null, entityTag: etag);
    }

    /// <summary>
    /// Gets a bundled font file. Anonymous, because the login page uses it too.
    /// </summary>
    /// <param name="file">The file name, for example quicksand-latin.woff2.</param>
    /// <returns>The font.</returns>
    [HttpGet("fonts/{file}")]
    [AllowAnonymous]
    [Produces("font/woff2")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult GetFont([FromRoute] string file)
    {
        // Only .woff2 files embedded under Web/fonts can be reached; the name is looked up, never used as a path.
        if (!file.EndsWith(".woff2", StringComparison.Ordinal))
        {
            return NotFound();
        }

        var stream = GetType().Assembly.GetManifestResourceStream(typeof(Plugin).Namespace + ".Web.fonts." + file);
        if (stream is null)
        {
            return NotFound();
        }

        Response.Headers.CacheControl = "public, max-age=604800";
        return File(stream, "font/woff2");
    }

    /// <summary>
    /// Gets the available themes for the settings page.
    /// </summary>
    /// <returns>The themes.</returns>
    [HttpGet("Themes")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetThemes()
    {
        var themes = ThemeCatalog.All
            .Select(t => new { t.Id, t.Name, t.Dark, t.Background, t.Surface, t.Text, t.Accent })
            .Prepend(new { Id = ThemeCatalog.DefaultId, Name = "Jellyfin default", Dark = true, Background = "#101010", Surface = "#202020", Text = "#ffffff", Accent = "#00a4dc" });
        return Ok(themes);
    }

    private string ReadBaseCss()
    {
        using var stream = GetType().Assembly.GetManifestResourceStream(typeof(Plugin).Namespace + ".Web.base.css");
        if (stream is null)
        {
            return string.Empty;
        }

        using var reader = new StreamReader(stream, Encoding.UTF8);
        return reader.ReadToEnd();
    }
}
