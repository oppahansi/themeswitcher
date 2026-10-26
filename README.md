# Theme Switcher for Jellyfin

[![Build](https://github.com/oppahansi/themeswitcher/actions/workflows/build.yml/badge.svg)](https://github.com/oppahansi/themeswitcher/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

A Jellyfin plugin that re-skins the web client. Pick a theme in the dashboard instead of pasting CSS into **Custom CSS**.

## Features

- **Themes bundled in the plugin**, no internet access needed. List: see [`Web/themes`](Jellyfin.Plugin.ThemeSwitcher/Web/themes).
- **Accent color override** with a live swatch preview.
- **Custom CSS** appended after the theme for small tweaks.
- Applies to everyone on the server, including the login page.

Settings: **Dashboard → Plugins → Theme Switcher**. Reload the web client (Ctrl+F5) after saving.

Themes are plain CSS files that set a handful of color variables; adding one means dropping a file into `Web/themes`, see [CONTRIBUTING.md](CONTRIBUTING.md). Theme ideas and the brief used to design them are in [docs/designer-prompts.md](docs/designer-prompts.md).

## Compatibility

Jellyfin **12.3** only (.NET 10). The theme is applied through the `--jf-palette-*` variables of the 12.x web client, so older servers aren't supported. Releases are tagged `v<version>-jellyfin-12.3`.

Native apps (Swiftfin, Findroid, Android TV) don't use the web client's CSS and stay unthemed.

## How the theme gets into the web client

The plugin adds a `<link>` to its generated stylesheet (`/ThemeSwitcher/theme.css`) in jellyfin-web's `index.html`. On every server start it does one of two things:

1. **With the [File Transformation](https://github.com/IAmParadox27/jellyfin-plugin-file-transformation) plugin installed (recommended):** the tag is added in memory as `index.html` is served. No files change on disk.
2. **Without it:** the plugin writes the tag into `index.html` in Jellyfin's web directory. This needs write access; it isn't writable in some Docker images (for example linuxserver.io). The log then says so.

Uninstalling removes the tag again.

## Installation

### From the plugin repository (recommended)

1. **Dashboard → Plugins →** repository settings, add a repository:
   - Name: `Theme Switcher`
   - URL: `https://raw.githubusercontent.com/oppahansi/themeswitcher/main/manifest.json`
2. Open the **Catalog**, install **Theme Switcher** (*General*).
3. Restart Jellyfin and reload the web client.

### Manual install

Extract the zip for your Jellyfin version from [Releases](https://github.com/oppahansi/themeswitcher/releases) into a new folder in the plugin directory (e.g. `<config>/plugins/ThemeSwitcher_1.0.0/`), restart Jellyfin and reload the web client.

## API

```
GET /ThemeSwitcher/theme.css
```
The generated stylesheet. No authentication, because it loads before login.

```
GET /ThemeSwitcher/fonts/{file}.woff2
```
Bundled fonts. No authentication.

```
GET /ThemeSwitcher/Themes
```
The available themes (for the settings page). Needs a user session.

## Building

```sh
dotnet build Jellyfin.Plugin.ThemeSwitcher -c Release -p:JellyfinVersion=12.3.0
```

`scripts/package.sh <jellyfin-version> [plugin-version]` builds and zips one Jellyfin version into `dist/`. Targets are listed in `targets.json`.

## License

MIT, see [LICENSE](LICENSE).

The bundled [Quicksand](https://github.com/andrew-paglinawan/QuicksandFamily) font is licensed under the SIL Open Font License 1.1, see [`Web/fonts/Quicksand-OFL.txt`](Jellyfin.Plugin.ThemeSwitcher/Web/fonts/Quicksand-OFL.txt).
