#!/usr/bin/env python3
"""Adds the packaged zips in dist/ to manifest.json, the Jellyfin plugin repository file.

    scripts/update-manifest.py <plugin-version>

Each zip must already be uploaded to the GitHub release tagged
v<plugin-version>-jellyfin-<major.minor>, the manifest points at that download URL.
"""
import hashlib
import json
import re
import sys
import zipfile
from pathlib import Path

REPO = "oppahansi/themeswitcher"
ROOT = Path(__file__).resolve().parent.parent
MANIFEST = ROOT / "manifest.json"

PLUGIN = {
    "guid": "4fdfbe7d-28a8-429c-acc1-1a471dde3a1f",
    "name": "Theme Switcher",
    "description": "Changes the look of the Jellyfin web UI with selectable themes, an accent color override and custom CSS.",
    "overview": "Re-skin the Jellyfin web UI from the dashboard",
    "owner": "oppahansi",
    "category": "General",
}


def version_key(entry):
    return tuple(int(part) for part in entry["version"].split("."))


def main():
    if len(sys.argv) != 2:
        sys.exit("usage: update-manifest.py <plugin-version>")
    plugin_version = sys.argv[1]

    zips = sorted((ROOT / "dist").glob(f"themeswitcher_{plugin_version}_jellyfin-*.zip"))
    if not zips:
        sys.exit(f"no zips for {plugin_version} in dist/")

    manifest = json.loads(MANIFEST.read_text()) if MANIFEST.exists() else [dict(PLUGIN, versions=[])]
    plugin = manifest[0]
    plugin.update(PLUGIN)
    versions = {v["version"]: v for v in plugin["versions"]}

    for path in zips:
        line = re.search(r"jellyfin-(\d+\.\d+)\.zip$", path.name).group(1)
        with zipfile.ZipFile(path) as archive:
            meta = json.loads(archive.read("meta.json"))
        tag = f"v{plugin_version}-jellyfin-{line}"
        versions[meta["version"]] = {
            "version": meta["version"],
            "changelog": f"https://github.com/{REPO}/releases/tag/{tag}",
            "targetAbi": meta["targetAbi"],
            "sourceUrl": f"https://github.com/{REPO}/releases/download/{tag}/{path.name}",
            "checksum": hashlib.md5(path.read_bytes()).hexdigest(),
            "timestamp": meta["timestamp"],
        }

    plugin["versions"] = sorted(versions.values(), key=version_key, reverse=True)
    MANIFEST.write_text(json.dumps(manifest, indent=2) + "\n")
    print(f"manifest.json: {len(plugin['versions'])} versions")


if __name__ == "__main__":
    main()
