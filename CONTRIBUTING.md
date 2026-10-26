# Contributing

Thanks for helping improve ThemeSwitcher. Bug reports, feature ideas and pull requests are all welcome.

## Reporting bugs

Open an [issue](https://github.com/oppahansi/themeswitcher/issues/new/choose) and include:

- your Jellyfin version and how it runs (Docker, Linux package, Windows, …)
- the plugin version, shown under **Dashboard → Plugins**
- what you expected and what happened instead
- related lines from the Jellyfin log, if any (**Dashboard → Logs**)

Also say whether the File Transformation plugin is installed, and include errors from the browser console (F12) if the theme doesn't show up. Please remove file paths or library names you don't want to share.

## Development setup

1. Install the [.NET 10 SDK](https://dotnet.microsoft.com/download).
2. Clone the repository and build:

   ```sh
   dotnet build Jellyfin.Plugin.ThemeSwitcher -c Release
   ```

3. To try your build, copy `Jellyfin.Plugin.ThemeSwitcher/bin/Release/<framework>/Jellyfin.Plugin.ThemeSwitcher.dll` into a folder under your test server's `plugins/` directory and restart Jellyfin, then reload the web client. A throwaway Docker container works well for this:

   ```sh
   docker run --rm -p 8096:8096 \
     -v "$PWD/test-config:/config" \
     -v /path/to/some/media:/media:ro \
     jellyfin/jellyfin:12.3.0
   ```

   Build with `-p:JellyfinVersion=<version>` that matches the server image. See [Building](README.md#building).

## Project layout

| Path | Contents |
|---|---|
| `Jellyfin.Plugin.ThemeSwitcher/Plugin.cs` | Plugin entry point, removes the `index.html` tag on uninstall |
| `Jellyfin.Plugin.ThemeSwitcher/Api/` | The `/ThemeSwitcher` controller that serves the stylesheet |
| `Jellyfin.Plugin.ThemeSwitcher/Injection/` | Startup task that adds the stylesheet tag, via File Transformation or `index.html` on disk |
| `Jellyfin.Plugin.ThemeSwitcher/ThemeCatalog.cs` | Finds the embedded themes and builds the stylesheet |
| `Jellyfin.Plugin.ThemeSwitcher/Web/base.css` | Shared rules that apply the `--ts-*` variables to jellyfin-web |
| `Jellyfin.Plugin.ThemeSwitcher/Web/themes/` | One CSS file per theme |
| `targets.json` | Jellyfin versions that get a release build |
| `scripts/package.sh` | Builds and zips one Jellyfin version |
| `scripts/update-manifest.py` | Updates `manifest.json`, the Jellyfin plugin repository file |

## Adding a theme

Create `Jellyfin.Plugin.ThemeSwitcher/Web/themes/<id>.css` (lowercase, dashes) and rebuild. It is embedded and shows up in the settings automatically:

```css
/* @name Nord */
/* @scheme dark */
:root {
    --ts-scheme: dark;
    --ts-bg: #2e3440;          /* page background */
    --ts-header: #3b4252;      /* top bar, side drawer, player bar */
    --ts-surface: #434c5e;     /* cards, dialogs, inputs, menus */
    --ts-text: #eceff4;
    --ts-text-muted: #c0c8d8;
    --ts-border: #4c566a;
    --ts-accent: #88c0d0;      /* buttons, progress, active tab */
    --ts-accent-text: #2e3440; /* text on top of the accent */
    --ts-radius: 0.4em;        /* optional corner rounding */
    --ts-font: system-ui, sans-serif; /* optional, system fonts only */
}
```

Optional variables for effects (all off unless set):

```css
    --ts-font: "Quicksand", "Noto Sans", sans-serif; /* "Quicksand" is bundled, see below */
    --ts-backdrop-blur: 30px;  /* blur of the full-page item backdrop */
    --ts-backdrop-scale: 1.1;  /* zoom that hides the blurred edges */
    --ts-backdrop-dim: 0.9;    /* dimming layer above the backdrop, higher is darker (default 0.86) */
    --ts-glass: rgba(0, 0, 0, 0.35); /* dialogs, menus, toasts, bottom bar, drawer */
    --ts-glass-blur: 15px;
    --ts-header-blur: 8px;     /* with --ts-header: transparent */
```

Bundled fonts live in `Web/fonts/` and are served from `/ThemeSwitcher/fonts/`. Only Quicksand (SIL OFL 1.1, license next to it) so far. To add one, drop the `.woff2` files there, add `@font-face` rules to `Web/base.css` and keep the license file.

`@name` and `@scheme` (`dark` or `light`) are required. A theme may add its own rules below the variables (they load after `base.css`), but prefer variables so the accent override keeps working. `Web/base.css` maps the variables onto jellyfin-web's own `--jf-palette-*` theme variables (jellyfin-web 12.x) and patches the few places that hard-code colors. It was checked against jellyfin-web v12.3; if a new version looks off, compare with `src/themes/_base/_theme.scss` there.

## Pull requests

- Keep changes focused. Open an issue first for larger features so we can agree on the approach.
- Make sure every supported Jellyfin version still builds. CI checks this on every pull request, and locally you can run:

  ```sh
  for v in $(jq -r '.[]' targets.json); do scripts/package.sh "$v"; done
  ```

- Warnings are treated as errors. Match the existing code style; `.editorconfig` covers the basics.
- Add an entry under `## [Unreleased]` in `CHANGELOG.md` for user-facing changes.
- Don't edit `manifest.json` by hand. The release workflow maintains it.

## License

By contributing, you agree that your contributions are licensed under the [MIT License](LICENSE).
