# Changelog

All notable changes to this project are documented here.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and the project uses [semantic versioning](https://semver.org).

## [Unreleased]

## [1.0.0] - 2026-10-26

### Added

- Theme selection, an accent color override and custom CSS, set from the dashboard.
- Twelve themes: Aurora Slate, Cinema Noir, Deep Ocean, Ember, Forest Cabin, Glacier, Kaleidochromic, Midnight, Neon Arcade, Paper, Sakura Night, Terminal Phosphor and Terracotta Paper.
- Stylesheet injection through the File Transformation plugin when it's installed, otherwise into jellyfin-web's `index.html`.
- Bundled Quicksand font (SIL OFL), served from `/ThemeSwitcher/fonts/`.
- Optional theme variables for font, blurred item backdrop, glass surfaces and a blurred top bar.
- Build for Jellyfin 12.3.

[Unreleased]: https://github.com/oppahansi/themeswitcher/compare/v1.0.0-jellyfin-12.3...HEAD
[1.0.0]: https://github.com/oppahansi/themeswitcher/releases/tag/v1.0.0-jellyfin-12.3
