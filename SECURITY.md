# Security

The plugin adds two API endpoints:

- `POST /ThemeSwitcher/Items/{itemId}` requires a signed-in user and only changes that user's playback position, for items the user can access.
- `GET /ThemeSwitcher/theme.css` serves the generated stylesheet without authentication. It contains only the selected theme, the accent and the custom CSS, so don't put secrets in the custom CSS field.

It also adds a stylesheet tag to jellyfin-web's `index.html`, either in memory through the File Transformation plugin or on disk.

If you find a security problem, please don't open a public issue. Report it privately through [GitHub security advisories](https://github.com/oppahansi/themeswitcher/security/advisories/new). Fixes go into the latest plugin version for every supported Jellyfin line.
