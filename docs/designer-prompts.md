# Designer prompts

How to use: paste the **shared brief** once at the start of a Claude Designer session, then paste one **theme prompt** per theme. Each answer should end with a CSS block in the plugin's theme format. Save it as `Jellyfin.Plugin.ThemeSwitcher/Web/themes/<id>.css` (or hand it to Claude Code to validate and add).

## Shared brief

```
I'm designing color themes for the Jellyfin media server web UI (a Netflix/Plex-style app for movies, TV and music). I need a theme as a small set of design tokens plus mockups that show they work.

Screens the theme must cover:
1. Home: top bar with logo/search/user icon, side drawer, horizontal rows of poster cards (2:3 art, title and year below), a "Continue Watching" row of 16:9 cards with a thin progress bar at the bottom.
2. Item detail page: large backdrop image, title, metadata chips, primary Play button, secondary icon buttons, synopsis, cast row, episode list.
3. Login page and a settings form: text inputs, dropdown, checkbox, toggle switch, primary and secondary buttons, a modal dialog and a context menu.
4. Bottom now-playing bar with a seek slider.

The poster and backdrop art is colorful and out of my control, so chrome and surfaces must stay calm and let it stand out.

Deliver:
A. Mockups of screens 1, 2 and 3 in this theme.
B. A token table. Every token is a single hex color (#rrggbb), except radius and font:
   - bg: page background
   - header: top bar, side drawer, now-playing bar (usually a step away from bg)
   - surface: cards, dialogs, inputs, menus (a step away from bg and header)
   - text: primary text
   - text-muted: secondary text (dates, descriptions, labels)
   - border: dividers and input outlines
   - accent: primary button, progress bars, active tab, links, focus ring
   - accent-text: text and icons on top of the accent
   - radius: corner rounding as a CSS length (0 to 1em)
   - font: a CSS font stack of SYSTEM fonts only (no web fonts, no external requests)
C. A contrast check: text on bg, header and surface needs at least 4.5:1, text-muted on bg and surface at least 3:1 (4.5:1 preferred), accent-text on accent at least 4.5:1, accent against bg at least 3:1. List the ratios.
D. The final result as one CSS block in exactly this format (replace the values, keep the two header comments and the variable names):

/* @name <Display name> */
/* @scheme dark */           <- or light
:root {
    --ts-scheme: dark;        /* or light */
    --ts-bg: #......;
    --ts-header: #......;
    --ts-surface: #......;
    --ts-text: #......;
    --ts-text-muted: #......;
    --ts-border: #......;
    --ts-accent: #......;
    --ts-accent-text: #......;
    --ts-radius: 0.4em;
    --ts-font: system-ui, sans-serif;
}

Keep the look cohesive and restrained: one accent color, no gradients on large surfaces, no decorative imagery. Also give me a lowercase-dashed file id for the theme (for example "aurora-slate").

I'll send the themes one at a time. Confirm you understand, then wait for the first one.
```

## Theme prompts

Each is a single message after the shared brief.

### 1. Aurora Slate (dark, cool, calm)
```
Theme 1: "Aurora Slate". Dark. Cool blue-grey slate surfaces with a soft frost-white text and a single icy cyan-teal accent, like a northern-lights night sky seen through haze. Calm, low-fatigue, good for evening viewing. Medium rounding (about 0.5em). Font: system sans-serif.
```

### 2. Neon Arcade (dark, synthwave)
```
Theme 2: "Neon Arcade". Dark. Deep indigo-violet backgrounds (not pure black), surfaces slightly lighter violet, with a hot magenta accent and a cyan used only for the focus ring if you need a second color. Retro synthwave attitude but still legible and not garish: keep large surfaces muted, let the accent carry the energy. Rounding about 0.3em. Font: system sans-serif.
```

### 3. Cinema Noir (dark, warm, premium)
```
Theme 3: "Cinema Noir". Dark. Warm near-black backgrounds (slightly brown, not blue), warm off-white text, and a muted brass/gold accent. The feel of an old art-house cinema lobby. Sharp to slightly rounded corners (about 0.15em). Font: an elegant serif system stack (Georgia / "Iowan Old Style" / serif) for the whole UI if it stays readable, otherwise serif-free.
```

### 4. Forest Cabin (dark, green, cozy)
```
Theme 4: "Forest Cabin". Dark. Deep desaturated pine-green backgrounds, surfaces a step lighter with a hint of moss, warm cream text and a warm amber accent like firelight. Cozy and organic. Generous rounding (about 0.75em). Font: system sans-serif.
```

### 5. Terracotta Paper (light, warm)
```
Theme 5: "Terracotta Paper". LIGHT. Warm cream/parchment background, white-ish surfaces, near-black warm text, and a terracotta/burnt-orange accent. Like a printed magazine. Poster art must still pop on the cream background. Rounding about 0.4em. Font: system sans-serif. Pay extra attention to contrast, since light themes hide weak borders: make the border token clearly visible.
```

### 6. Deep Ocean (dark, teal)
```
Theme 6: "Deep Ocean". Dark. Navy to deep-teal backgrounds with a subtle layering from bg to header to surface that feels like water depth, aqua/seafoam accent, soft blue-white text. Rounding about 0.6em. Font: system sans-serif.
```

### 7. Sakura Night (dark, plum + pink)
```
Theme 7: "Sakura Night". Dark. Dark plum / aubergine backgrounds, dusty rose-tinted text, and a soft cherry-blossom pink accent. Gentle and a little romantic, not childish; saturation stays low on large surfaces. Rounding about 0.8em. Font: system sans-serif.
```

### 8. Terminal Phosphor (dark, monochrome green)
```
Theme 8: "Terminal Phosphor". Dark. Near-black with a faint green tint, text in a pale phosphor green, a single bright green accent, thin visible borders. A tasteful nod to old CRT terminals, not a parody: keep text-muted readable. No rounding (radius 0). Font: a monospace system stack ("SF Mono", Menlo, Consolas, monospace) for the whole UI.
```

### 9. Glacier (light, cool, minimal)
```
Theme 9: "Glacier". LIGHT. Cool white and very pale blue-grey backgrounds, crisp dark slate text, hairline borders, and a clear medium blue accent. Clean and minimal like a modern productivity app. Rounding about 0.5em. Font: system-ui sans-serif. Make sure the header, surface and bg are distinguishable from each other without relying on shadows.
```

### 10. Ember (dark, charcoal + orange)
```
Theme 10: "Ember". Dark. Charcoal backgrounds with a slight warm cast, surfaces a step lighter, off-white text, and a glowing orange-red accent like embers. Bold, high-contrast, sharp corners (radius 0 to 0.1em). Font: system sans-serif.
```
