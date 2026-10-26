# Designer prompts

How to use: paste the **shared brief** once at the start of a Claude Designer session, then paste one **theme prompt** per theme. Each answer should end with a CSS block in the plugin's theme format. Save it as `Jellyfin.Plugin.ThemeSwitcher/Web/themes/<id>.css` (or hand it to Claude Code to validate and add).

## Shared brief

```
I'm designing color themes for the Jellyfin media server web UI (a Netflix/Plex-style app for movies, TV and music). I need a theme as a small set of design tokens plus mockups that show they work.

Screens the theme must cover:
1. Home: top bar with logo, Home/Favorites tabs, search and user icon (no sidebar on desktop), horizontal rows of poster cards (2:3 art, title and year below), a "Continue Watching" row of 16:9 cards with a thin progress bar at the bottom.
2. Item detail page: large backdrop image, title, metadata chips, primary Play button, secondary icon buttons, synopsis, cast row, episode list.
3. Login page and a settings form: text inputs, dropdown, checkbox, toggle switch, primary and secondary buttons, a modal dialog and a context menu.
4. Bottom now-playing bar with a seek slider.

The poster and backdrop art is colorful and out of my control, so chrome and surfaces must stay calm and let it stand out.

Deliver:
A. Mockups of screens 1, 2 and 3 in this theme.
B. A token table. Every token is a single hex color (#rrggbb), except radius and font:
   - bg: page background
   - header: top bar, drawer (admin dashboard and small screens only), now-playing bar (usually a step away from bg)
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

### 11. Kaleidochromic (dark, glass, recreation of CTalvio's skin)
Reference: https://github.com/CTalvio/Kaleidochromic/tree/main/screenshots (especially 1.png and 6.png). Quicksand is bundled with the plugin, so use it.
```
Theme 11: "Kaleidochromic" (a recreation of CTalvio's Jellyfin skin of the same name; attach the reference screenshots from the link above, especially 1.png and 6.png). Dark. Recreate its look as closely as you can in our token format and tell me anything the tokens can't express.

What defines the look, in order of importance:
1. The page is nearly black and the artwork of the current item is shown full-screen behind everything, heavily blurred and dimmed, so the page colors shift with the content. Chrome never has an opaque panel: the top bar is fully transparent over the blurred backdrop, white icons, centered "Home / Favorites" tabs (active white, inactive dimmed).
2. Glass surfaces: dialogs, menus, toasts, the bottom player bar and the drawer (which only exists on small screens and in the admin dashboard; there is no permanent sidebar on user pages) are very dark translucent black (about 35% opacity) with a strong background blur (about 15px). Cards in rows have NO background panel: rounded artwork, centered title and a smaller dimmed subtitle below.
3. One desaturated periwinkle accent, roughly #6279cd. Used for the play button (a soft rounded square tinted with the accent at low opacity, play icon in the accent), favorite heart, check mark, count badges on posters, the focused card outline and links. Never a big solid fill.
4. Rounded everywhere, about 12px: posters, buttons, dropdowns, dialogs, library tiles. "My Media" tiles are dark translucent rounded rectangles with icon and label.
5. Typography: Quicksand (weights 300 to 700) everywhere. Section titles light weight and a bit larger than body text, with a small chevron after them. Body copy compact (about 90%).
6. Item page: unframed rounded poster on the left, title/metadata on the right, a label/value table (dimmed labels, brighter values), compact translucent pill dropdowns, a row of small SQUARE rounded cast photos with the role dimmed underneath.
7. Episode progress is a thin bar on the thumbnail's bottom edge, accent on a translucent track.

Deliver everything from the shared brief (mockups of screens 1, 2 and 3, token table, contrast ratios, CSS block) with these tokens: bg about #080808; header: transparent (--ts-header: transparent); surface: the dark glass tone as a solid color; text white; text-muted about 70% white as a solid hex; border about 12% white as a solid hex; accent #6279cd; accent-text chosen for contrast; radius 0.75em; font "Quicksand", "Noto Sans", sans-serif.

In the CSS block also set these OPTIONAL variables, with the values you recommend (I implement the effects, you only choose the numbers):
    --ts-backdrop-blur: <px>;      /* blur of the full-page backdrop */
    --ts-backdrop-scale: <number>; /* zoom that hides blurred edges, usually 1.05 to 1.15 */
    --ts-backdrop-dim: <0-1>;      /* dimming layer above the backdrop, higher is darker, stock is 0.86 */
    --ts-glass: rgba(...);         /* background of dialogs, drawer, toasts, bottom bar */
    --ts-glass-blur: <px>;
    --ts-header-blur: <px>;        /* blur behind the transparent top bar, 0px for none */
Then list anything about the look that these variables cannot reproduce.
```

