---
name: Trackly
description: An all-in-one team workspace for tasks, meetings, clients, and contracts — precise, fast, and unmistakably its own.
colors:
  bg-light: "oklch(1 0 0)"
  surface-light: "oklch(0.98 0 0)"
  surface-2-light: "oklch(0.955 0 0)"
  surface-3-light: "oklch(1 0 0)"
  border-light: "oklch(0 0 0 / 8%)"
  border-strong-light: "oklch(0 0 0 / 14%)"
  text-light: "oklch(0.17 0 0)"
  text-secondary-light: "oklch(0.38 0 0)"
  text-muted-light: "oklch(0.52 0 0)"
  accent-light: "oklch(0.42 0.1 110)"
  accent-strong-light: "oklch(0.36 0.11 110)"
  on-accent-light: "oklch(0.99 0 0)"
  bg-dark: "oklch(0.08 0.006 110)"
  surface-dark: "oklch(0.135 0.008 110)"
  surface-2-dark: "oklch(0.185 0.01 110)"
  surface-3-dark: "oklch(0.235 0.012 110)"
  border-dark: "oklch(1 0 0 / 10%)"
  border-strong-dark: "oklch(1 0 0 / 18%)"
  text-dark: "oklch(0.96 0 0)"
  text-secondary-dark: "oklch(0.72 0 0)"
  text-muted-dark: "oklch(0.61 0 0)"
  accent-dark: "oklch(0.8 0.13 110)"
  accent-strong-dark: "oklch(0.87 0.14 110)"
  on-accent-dark: "oklch(0.14 0 0)"
  danger-light: "oklch(0.52 0.16 25)"
  danger-dark: "oklch(0.68 0.17 25)"
  success-light: "oklch(0.5 0.13 145)"
  success-dark: "oklch(0.68 0.15 145)"
  warning-light: "oklch(0.5 0.15 55)"
  warning-dark: "oklch(0.72 0.14 55)"
typography:
  display:
    fontFamily: "Geist Sans, -apple-system, BlinkMacSystemFont, Segoe UI, sans-serif"
    fontSize: "clamp(2.75rem, 1.6rem + 4.3vw, 4.75rem)"
    fontWeight: 600
    lineHeight: 1.05
    letterSpacing: "-0.03em"
  headline:
    fontFamily: "Geist Sans, -apple-system, BlinkMacSystemFont, Segoe UI, sans-serif"
    fontSize: "clamp(2rem, 1.4rem + 2.2vw, 3.1rem)"
    fontWeight: 600
    lineHeight: 1.15
    letterSpacing: "-0.02em"
  title:
    fontFamily: "Geist Sans, -apple-system, BlinkMacSystemFont, Segoe UI, sans-serif"
    fontSize: "1.375rem"
    fontWeight: 600
    lineHeight: 1.3
    letterSpacing: "-0.01em"
  body:
    fontFamily: "Geist Sans, -apple-system, BlinkMacSystemFont, Segoe UI, sans-serif"
    fontSize: "0.9375rem"
    fontWeight: 400
    lineHeight: 1.55
  label:
    fontFamily: "Geist Sans, -apple-system, BlinkMacSystemFont, Segoe UI, sans-serif"
    fontSize: "0.8125rem"
    fontWeight: 500
  data:
    fontFamily: "Geist Mono, ui-monospace, SFMono-Regular, Menlo, monospace"
    fontSize: "0.6875rem"
    fontWeight: 500
rounded:
  sm: "6px"
  md: "10px"
  lg: "16px"
  full: "999px"
spacing:
  3xs: "0.25rem"
  2xs: "0.5rem"
  xs: "0.75rem"
  sm: "1rem"
  md: "1.5rem"
  lg: "2rem"
  xl: "3rem"
  2xl: "4rem"
  3xl: "6rem"
components:
  button-primary:
    backgroundColor: "{colors.accent-dark}"
    textColor: "{colors.on-accent-dark}"
    rounded: "{rounded.md}"
    padding: "0.75rem 1.375rem"
  button-primary-hover:
    backgroundColor: "{colors.accent-strong-dark}"
  button-secondary:
    backgroundColor: "transparent"
    textColor: "{colors.text-dark}"
    rounded: "{rounded.md}"
    padding: "0.75rem 1.375rem"
  card:
    backgroundColor: "{colors.surface-dark}"
    textColor: "{colors.text-dark}"
    rounded: "{rounded.lg}"
    padding: "2rem"
---

# Design System: Trackly

## 1. Overview

**Creative North Star: "The Precision Console"**

Trackly reads as a single, calibrated instrument for running a project — not four bolted-together apps. The reference points are deliberate: the near-black, razor-sharp restraint of Vercel and Raycast, fused with the playful-but-precise micro-interaction polish of Arc Browser and Superhuman. Every surface should feel premium, fast, and quietly alive with detail — precision that rewards a second look, not decoration for its own sake. The product spans four pillars (tasks, meetings, clients, contracts) with very different information densities; the console metaphor is what lets all four feel like one machine instead of four modules.

This system explicitly rejects: cream/sand SaaS-template backgrounds, gradient-text headlines, tiny uppercase eyebrows, identical icon-card grids, and the cluttered, dense-panel chaos of old Jira/Azure DevOps. It also rejects the reflexive blue-and-purple SaaS accent — Trackly's identity color is Cured Olive, a desaturated yellow-green (hue 110°), used rarely and deliberately.

**Key Characteristics:**
- Near-black, dark-first surfaces (continuing the app's existing dark-mode-by-default convention), with a true off-white light mode as the alternate, toggled via the same `.dark` class convention the app already uses. Dark surfaces are retinted (revised — see below), not pure achromatic: pure black read as flat and eye-straining for an all-day tool, so `bg`/`surface`/`surface-2`/`surface-3` all carry a whisper of the accent hue.
- A single, rare accent color (Cured Olive) rather than a multi-color palette — restraint is the brand. It flips lightness by theme: bright olive on dark surfaces, deep olive on light surfaces, always the same hue.
- Dimensionality comes from two deliberate devices, not shadow: a **shell-wide micro dot-grid** (near-invisible, visible only in empty gaps between cards) and **glow borders** on genuinely interactive moments (active nav, hovered card, primary-button hover) — a soft accent-tinted blur alongside the border/background change, not just a flat color swap.
- Geist Sans for UI and headings, Geist Mono for ticket IDs, timestamps, unlock figures, and other data — reinforcing technical precision on the densest surfaces.
- Motion is Responsive in the product; the landing page (brand register) earns a one-time staggered entrance choreography (700ms, ease-out-expo) that product screens don't get.
- Vanilla CSS custom properties scoped under a `.tp-shell` root class, coexisting with legacy Tailwind/shadcn on pages not yet migrated.

## 2. Colors

The palette is a **Restrained** strategy on dark (and light) surfaces: near-achromatic surfaces do the structural work, and one rare accent (≤10% of any screen) carries all of the brand's personality. No secondary or tertiary color roles.

### Primary
- **Cured Olive** — dark surfaces: `oklch(0.8 0.13 110)` (#dbdd82-ish bright yellow-green); light surfaces: `oklch(0.42 0.1 110)` (deep moss). Used for primary buttons, active/selected states, focus rings, and small indicators — never a large fill.

### Neutral
- **Dark surfaces** (revised — obsidian retint, Move 0): bg `oklch(0.08 0.006 110)`, surface `oklch(0.135 0.008 110)`, surface-2 `oklch(0.185 0.01 110)`, surface-3 `oklch(0.235 0.012 110)` — each carries a small chroma toward the brand hue (110°), not pure achromatic. Superseded the original pure-black values (`oklch(0.09/0.14/0.19 0 0)`), which read as flat and eye-straining for a tool used all day. Depth is still built from lightness steps, just no longer chroma-0.
- **Light surfaces**: bg `oklch(1 0 0)` (pure white), surface `oklch(0.98 0 0)`, surface-2 `oklch(0.955 0 0)`, surface-3 `oklch(1 0 0)` (pure white — light-mode depth comes from shadow, not a lightness step, so surface-3 doesn't need to out-lighten an already-near-white surface-2).
- **Surface-3** (new, Move 0): reserved for floating content only — dropdown panels today, future modals/command palette. Never used for in-flow cards; that distinction (floats above other content vs. is the content) is what surface-3 vs. surface encodes.
- **Borders**: low-opacity white-on-dark (`oklch(1 0 0 / 10%)`) and black-on-light (`oklch(0 0 0 / 8%)`) hairlines rather than a separate gray token family.
- **Text**: dark-mode body text is intentionally lighter-weight (400, not bumped) since Geist Sans reads heavier on dark already; muted text sits at `oklch(0.61 0 0)` dark / `oklch(0.52 0 0)` light — both tuned to clear 4.5:1 against their surface, not just the page bg.

### Named Rules
**The One Accent Rule.** Cured Olive appears on ≤10% of any given screen — buttons, active states, focus rings, and small indicators only. It never fills a card, a section background, or a large surface.

**The No-Default-Blue Rule.** No SaaS blue/purple/indigo accent anywhere in the system, including hover states, links, or charts. (Revised, Move 0: this used to also ban glow outright; it no longer does — see The Glow-Is-Earned Rule under Elevation. The point was never "no glow," it was "no reflexive blue/purple.")

**The Theme-Flip Rule.** The accent's hue and chroma never change between themes — only lightness flips (bright on dark, deep on light) to hold contrast without changing identity.

## 3. Typography

**Display Font:** Geist Sans (self-hosted via `@fontsource/geist-sans`), falling back to `-apple-system, BlinkMacSystemFont, Segoe UI, sans-serif`.
**Body Font:** Geist Sans — one sans family across the whole product; weight (400/500/600/700) does the differentiating work.
**Label/Mono Font:** Geist Mono (via `@fontsource/geist-mono`), for ticket IDs, timestamps, unlock figures, and other data-shaped text.

**Character:** Technical and unadorned on the sans side; Geist Mono exists to make data (IDs, dates, figures) legible as *data*, reinforcing the Vercel/dev-tool reference directly.

### Hierarchy
- **Display** (600, `clamp(2.75rem, 1.6rem + 4.3vw, 4.75rem)`, 1.05 line-height, -0.03em): landing page hero only.
- **Headline** (600, `clamp(2rem, 1.4rem + 2.2vw, 3.1rem)`, 1.15, -0.02em): section headings (Pillars, Plans, CTA).
- **Title** (600, 1.375rem, -0.01em): card/pillar titles.
- **Body** (400, 0.9375rem, 1.55): default UI and prose, capped at 65–75ch.
- **Label** (500, 0.8125rem): nav items, small UI text, buttons.
- **Data** (Geist Mono, 500, 0.6875–0.75rem): ticket tags, timestamps, unlock figures, URLs.

### Named Rules
**The Data-Is-Mono Rule.** Any text that is fundamentally a datum (an ID, a timestamp, a figure, a version, a URL) renders in Geist Mono. Applied even to the plan cards' unlock figures (10+, 30+) — a deliberate, slightly unusual choice that reinforces the "precision instrument" identity over a purely decorative display treatment.

## 4. Elevation

Flat-by-default, dev-tool posture: depth comes from a 1px hairline border and lightness steps between surface/surface-2/surface-3, not drop shadows. Two exceptions carry a real shadow because they're genuinely floating: the hero product-preview mock and dropdown panels.

### Shadow Vocabulary
- **Hairline** (`border: 1px solid var(--tp-border)`): the default way surfaces separate. Primary elevation tool.
- **Floating, small** (`box-shadow: var(--tp-shadow-floating)` = `0 16px 40px -20px oklch(0 0 0 / 45%)`): dropdown panels, small popovers.
- **Floating, large** (`box-shadow: var(--tp-shadow-floating-lg)` = `0 24px 64px -32px oklch(0 0 0 / 45%)`): the hero preview, the auth card — bigger, further-off-the-page floating moments.
- **Glow** (`box-shadow: var(--tp-glow-sm)` = `0 0 12px -2px var(--tp-accent-wash)`, or `--tp-glow-md` for a larger radius): a soft accent-tinted blur, layered *alongside* a border/background change, not replacing it. Reserved for genuinely interactive moments — the active sidebar nav item, a hovered pillar/card, primary-button hover. Not applied blanket to every hover state; list-row hovers (task rows, activity rows) stay a plain background change, since glow on every row in a dense list would be noise, not signal.

### Micro dot-grid
A near-invisible dot pattern (`background-image: radial-gradient(circle, var(--tp-border[-strong]) 1px, transparent 1px)`), two variants:
- **Vignette** (`.tp-dot-grid`, landing/auth): masked into a ring that fades out both near the focal card and at the far viewport edges. Used where there's one clear focal point on an otherwise-empty page.
- **Shell** (`.tp-dot-grid.tp-dot-grid--shell`, app shell): uniform, no mask, fainter (`--tp-border` not `--tp-border-strong`), wider spacing (32px not 28px). Used where there's no single focal point — a persistent working surface, not a single card floating on emptiness. Sits at `z-index: -1` inside `.tp-shell`'s `isolation: isolate` context; only shows through the gaps between opaque cards/sidebar/topbar.

### Named Rules
**The Flat-By-Default Rule.** Cards and panels sit flush with the background, separated by hairline borders, not shadow.

**The Glow-Is-Earned Rule.** Glow marks "you are looking at or touching this right now" — active nav, hover, focus-adjacent. It is never decorative and never permanent on a resting element. If an element has glow with no interaction happening, that's a bug, not a style choice.

**The Isolation Gotcha.** Any element using a negative `z-index` (glow layers, dot-grids) needs `isolation: isolate` on an ancestor, or it silently renders behind the page's own background instead of just behind its intended siblings. Caught once already on the Auth Card; `.tp-shell` now sets `isolation: isolate` globally so this can't recur.

## 5. Components

### Buttons
- **Shape:** 10px radius (`--tp-radius-md`).
- **Primary:** background Cured Olive, text `on-accent` (near-black on dark theme's bright olive; near-white on light theme's deep olive) — chosen per-theme so text always reads as the higher-contrast option, not a fixed white/black default.
- **Secondary:** transparent background, 1px `border-strong`, hover fills with `surface`.
- **Ghost:** transparent, muted text, hover brightens to full text color + `surface` background.
- **Hover/Focus:** 150ms ease-out-quart background transition; `:active` scales to 0.97; `:focus-visible` gets a 2px Cured Olive outline with 2px offset.

### Cards (Pillars, Plans)
- **Corner style:** 16px radius (`--tp-radius-lg`).
- **Background:** `surface`, hairline border.
- **Hover:** border brightens to `border-strong`, lifts 2px on translateY.
- **Highlighted variant** (Plans "Next milestone"): border switches to full Cured Olive + background tints to an 8-14% accent wash, never a side-stripe.

### Inputs / Fields
- **Style:** `surface` background, 1px `border-strong` stroke, 10px radius (`--tp-radius-md`), 0.625rem/0.875rem padding.
- **Focus:** border switches to Cured Olive + a 3px accent-wash glow ring (`box-shadow`), not a browser default outline.
- **Placeholder:** `text-muted`.
- **Error:** border switches to the desaturated red-orange danger color; a matching danger-tinted focus ring on top of it.
- **Disabled:** 0.6 opacity, `cursor: not-allowed`.
- **Labels:** `label` type scale (0.8125rem, 500 weight), `text-secondary`, sit directly above their input with `space-3xs` gap.
- **Dividers** (`.tp-divider`): a plain 1px `border` hairline, used to separate a form's primary action from a secondary link, not a visible `<hr>` rule style.

### Segmented Control (`tp-segmented`)
A `surface-2` track with 2px of padding; the active segment sits on `surface` with `--tp-shadow-floating`, reading as a physically raised tab rather than a color swap. Used for the Board/List toggle on the project header. Not every segmented control needs to be functional the moment it's styled — Board/List ships with only Board wired; List stays visually present but inert until it's actually built, which is a scoped decision, not an oversight.

### Nav (brand register)
- Fixed, `color-mix` translucent background + `backdrop-filter: blur(12px)`, hairline bottom border. Collapses to a hamburger + slide-down panel under 768px; full inline links + sign-in above it.

### App Shell: Sidebar (product register)
- **Structure:** 16rem fixed-width column, `surface` background, hairline right border. Header (workspace switcher), nav, scrollable projects list, footer (Settings + user).
- **Workspace switcher:** a `tp-dropdown` trigger showing a mark, workspace name, and current unlock tier (Starter/Growth/Scale, never a payment-plan label) with a `ChevronsUpDown` affordance. Panel shows the workspace list (checkmark on active) and a link to workspace settings.
- **Nav items:** hairline-free; active state is an `accent-wash` background + `accent`-colored text and icon, never a side-stripe. Scoped to what's actually built — no disabled/"coming soon" entries for unbuilt pillars.
- **Projects list:** each project gets a small solid-color dot from the categorical palette (`--tp-cat-1..5`, muted hues distinct from the brand accent) for at-a-glance differentiation, not brand meaning.
- **Mobile (<1024px):** the desktop sidebar hides entirely; a hamburger in the topbar opens a fixed slide-in drawer (`tp-sidebar--mobile`) with a scrim backdrop and its own close button, animated via `translateX`.

### App Shell: TopBar (product register)
- **Structure:** 3.5rem fixed height, `bg` background, hairline bottom border. Search trigger (⌘K hint) on the left, notifications/help/avatar on the right.
- **Avatar menu:** same `tp-dropdown`/`tp-menu-item` primitives as the workspace switcher, right-aligned panel.
- **Notification dot:** a small solid `danger`-colored dot, absolutely positioned on the bell icon; presence-only, no count badge yet.

### Dropdowns / Menus (`tp-dropdown`, `tp-menu-item`)
- **Positioning:** `position: absolute` on a `position: relative` trigger wrapper. Only safe because neither the sidebar header nor the topbar have a clipping `overflow` ancestor between the trigger and the panel — if a future menu trigger sits inside a scrollable region, switch that panel to `position: fixed` with computed coordinates instead (per the live-mode popover rule).
- **Panel:** `surface` background, hairline border, floating shadow, `tp-dropdown-in` fade + scale entrance (150ms).
- **Items:** full-width, hairline-free, `surface-2` hover/focus-visible, muted icon that doesn't recolor on hover. An `--active` variant colors text `accent` for the current selection (e.g. current workspace).
- **Dismissal:** a shared `useClickOutside` hook (`src/hooks/useClickOutside.ts`) closes the panel on outside pointerdown; every dropdown in the app shell uses it rather than a one-off listener.

### Dashboard Cards (Stats, Weekly Summary, Tasks, Activity)
- **Shape:** `surface` background, hairline border, 16px radius, no drop shadow (Flat-By-Default Rule).
- **Stat card:** label (muted, 0.75rem) + value (Geist Mono, `--tp-text-xl`, since a raw number is a datum) + a uniform muted icon chip (`surface-2` background, `text-secondary` icon — never a per-stat rainbow of icon colors). Change line uses `success` for positive deltas, `text-muted` otherwise; never a bare color with no label.
- **Weekly summary card:** deliberately not AI-branded (no Sparkles icon, no "AI" in the title) — per PRODUCT.md, automation is a quiet feature, not the pitch, consistent with the landing page dropping its dedicated AI section. Tags at the bottom use `surface-2` for neutral counts and a `danger` tint only for at-risk counts.
- **List cards (Tasks, Activity):** header row with hairline bottom border, hairline-separated rows (no dividers *and* borders), `surface-2` row hover. Rows use the shared `tp-avatar` (initials, `accent-wash` background/`accent` text, Geist Mono) and `tp-priority` (colored dot + label, never color-only) primitives so task/activity surfaces stay visually identical wherever they appear next (Kanban cards, My Tasks page, Analytics).
- **Priority color mapping:** high → `danger`, medium → `warning` (muted orange, theme-split independently of the `--tp-cat-*` decorative dot palette since priority labels are text and need the stricter 4.5:1 text-contrast bar, not the lighter bar decorative dots get), low → `text-muted`.
- **Empty state:** icon + one-line title + one-line supporting text, centered, generous vertical padding — same shape for "no tasks" as the landing/auth empty states, not a bespoke treatment per card.

### Kanban Board (Move 1)
- **Column:** `surface` background, hairline border, 16px radius — a column is a card, not a bare list. Header: colored dot (decorative `--tp-cat-*`, matches the project's sidebar dot) + title + a mono count pill on `surface-2`. Cards scroll independently per column (`max-height: calc(100vh - 14rem)`), a dashed "Add task" affordance sits below the last card.
- **Task card:** a real `<button>`, not a `<div>` with an onClick — keyboard-focusable and semantically a control, since clicking one now does something (opens the Ticket Modal). Hover gets the glow treatment (`--tp-glow-sm`) per the Glow-Is-Earned Rule — this is a genuinely actionable element. Priority renders as a small solid dot (`currentColor`, colored via the `tp-priority--*` text-contrast-safe tokens, not the decorative `--tp-cat-*` palette) rather than the dot+label combo used elsewhere, since the card is small and the tag pill already carries a label.

### Ticket Modal (Move 1)
- **Trigger:** any task card click, or a deep link via `?ticket=<id>` on the project route — both paths converge on the same state, so the modal is never reachable by one path and not the other.
- **Shape:** `tp-modal` on `surface-3` (floating content), `--tp-shadow-floating-lg`, dismissed by the close button, the Escape key, or a click on the backdrop (not the modal itself — a click inside is stopped from propagating to the backdrop).
- **Status control:** a dropdown (`tp-dropdown`/`tp-menu-item`, the same primitive as the workspace switcher and avatar menu), not drag-and-drop — literal drag-and-drop inside a modal is an explicit non-goal per PRODUCT.md. Selecting a status actually moves the card between columns on the board underneath; this is real local-state mutation, not a static field.
- **Time log:** an honest, unpersisted local form — hours + optional note, appended to a list shown above the form. Explicitly not wired to any backend yet; entries reset when the modal closes. Logged hours render in Geist Mono (a datum).
- **Comments:** `<ChatThread scope={{ type: 'ticket', ticketId }}>` — see Chat Thread (Move 3) below. Rendered inside a fixed-height (`16rem`) scroll container so a short thread doesn't force the whole modal to grow, and a long one doesn't push the composer out of `.tp-modal__body`'s own scroll region permanently — it's still reachable by scrolling, just not always in the first screenful on a tall modal.

### Command Palette (Move 2)
- **Trigger:** ⌘K / Ctrl+K from anywhere, including while focused inside an input or textarea — the one universal exception to the editable-target guard. Also reachable via the TopBar search affordance (`tp-topbar__search`), a plain button, not a fake input.
- **Shape:** reuses `tp-modal-overlay` (no separate overlay primitive) with a `tp-palette` panel on `--tp-surface-3` at `--tp-z-modal`, not the lower dropdown z-index tier — a palette is a modal-weight surface, not a popover.
- **Active row:** background-color change only (`surface-2`), the same `--modifier` BEM convention used elsewhere (`tp-palette__row--active`) — not a side-stripe. A stripe reads as a selection indicator borrowed from list UIs; this is a command target, and Glow-Is-Earned already covers the one legitimate "this is interactive" signal elsewhere, so the active row stays deliberately quiet.
- **Grouping:** results are grouped by `CommandGroup` (Navigate, Tickets, Actions, Help) with a small label header wherever the group changes, not visually separated tiles — a flat list reads faster for keyboard-driven scanning than boxed sections.
- **Data type marker:** a ticket's `#127` renders in Geist Mono next to its title (Data-Is-Mono Rule), identical treatment to Kanban card tags and the Ticket Modal's logged hours — the palette borrows existing primitives rather than inventing its own datum style.
- **Keybinding chips:** shown right-aligned per row (`tp-palette__kbd`) only for commands with a global keybinding — most rows have none, and an empty column would be worse than an inconsistent one.
- **Multi-step flows ("pages"):** selecting "Change ticket status…" pushes a new page (pick ticket → pick status) rather than parsing arguments out of the query string. A breadcrumb-style crumb (`tp-palette__crumb`) sits to the left of the input showing the current page's title; Backspace on an empty query, or Escape, pops back one level.
- **Ticket search has no required prefix:** both `#127` and bare `127` resolve the same ticket via the same exact-match scoring path — matching how people actually type when they already know the number, not just when copying a tag verbatim.

### Chat Thread (Move 3)
- **One component, two scopes:** `<ChatThread scope={{ type: 'project', projectId }} />` docked as a toggleable side panel on the project page, and `<ChatThread scope={{ type: 'ticket', ticketId }} />` inside the Ticket Modal's Comments section — same component, same renderer, scoped by a `ChatScope` prop, not two chat systems that happen to look similar.
- **Messages are block-model, not plain strings:** a message's content is `MessageBlock[]` (`text` / `mention` / `ticketRef`, plus `card` typed now and deliberately unrendered — Move 5's live inline ticket card lands in that slot later). This is what lets a meeting's notes reuse the same block renderer at document scale in Move 7, rather than being a second, incompatible rendering system.
- **Composer stays plain text, chips render on send:** typing is a normal controlled `<input>`, not a rich contentEditable — `@handle` and `#id` are literal characters while composing. `parseMessage` converts the raw string to blocks only on send; an unresolved handle or id (typo, or text that merely starts with `@`/`#`) is left as literal text rather than becoming a broken chip.
- **The `@`/`#` picker is a separate, simpler system from the ⌘K registry:** intentionally not sharing `commands/match.ts` — that ranking exists for keybinding-aware command scoring, which this doesn't need. Filtering here is a plain startsWith/includes match, capped at 6 results, rendered in a `tp-chat__picker` popup styled like a compact dropdown, not the palette.
- **Reference chips borrow existing conventions, not new ones:** a ticket reference renders as `#121` in Geist Mono, accent-colored (Data-Is-Mono Rule, same treatment as the palette's `tp-palette__mono` and the Ticket Modal's `#121` id). A mention renders as `@Sarah Kim` on an accent-wash pill — prose, not data, so no mono.
- **Enter has two jobs, resolved by whether the typed token is already complete:** while the picker is open and the query is still ambiguous, Enter/Tab confirms the highlighted suggestion. The moment what's typed already resolves exactly (the full handle or a real ticket id), Enter sends the message instead of re-inserting the same token — otherwise finishing a message with "...#121" and hitting Enter once would silently reopen the picker instead of sending.
- **Mobile has no room for a side-by-side split:** below 768px, opening the chat panel replaces the board instead of squeezing beside it (`:has()` toggles board visibility off the moment the panel is present) — the same header toggle switches back.

### Signature: Hero Product Preview
A "browser chrome" strip (a live-pulse indicator + truncated mono URL, no fake traffic-light dots) above a 3-column kanban mock. One card carries two small accent-wash pill badges (calendar + file icons) linking it to a meeting and a contract, the concrete, in-product proof of "one workspace, not four," rather than a claim in copy alone.

### Signature: Connect Diagram
A small CSS-only diagram (four muted nodes connected by hairline lines converging on a glowing Cured Olive center dot) used once, on the Pillars section's "Everything connects" card, as the visual signature for cross-pillar linking. Not reused as generic decoration elsewhere.

### Signature: Auth Card
A centered card (max-width 24rem) with a soft accent-wash glow behind it, the same brand mark as the nav above it, floating shadow (the one approved use of drop-shadow outside modals/popovers) since it's the sole focal object on an otherwise empty page. A faint dot-grid backdrop (1px dots on a 28px grid, `border-strong` opacity, radial-masked into a ring that fades out both near the card and at the far edges) fills the surrounding empty space without competing with the form. Requires `isolation: isolate` on the page root; the dot grid and glow both use negative z-index and will render behind the page's own background entirely without it. Shared by Login, Register, Forgot Password, Email Verification, and Invitation, all five now vanilla CSS.

## 6. Do's and Don'ts

### Do:
- **Do** write confident, benefit-first copy: what Trackly does for the reader's team, not what it replaces or who it beats.
- **Do** use vanilla CSS custom properties scoped under `.tp-shell` for every token — no Tailwind utility classes on migrated surfaces.
- **Do** keep Cured Olive rare — buttons, active states, focus rings, small indicators only (The One Accent Rule).
- **Do** flip the accent's lightness (not hue) between light and dark themes (The Theme-Flip Rule).
- **Do** render IDs, timestamps, figures, and URLs in Geist Mono (The Data-Is-Mono Rule).
- **Do** reserve expressive entrance choreography for brand-register surfaces (landing page); keep product screens to fast, interaction-tied feedback only.
- **Do** make every pillar (tasks, meetings, clients, contracts) visibly cross-link to the others, per the Connect Diagram precedent.

### Don't:
- **Don't** use em dashes anywhere in copy. Rewrite with a period, comma, or "and" instead — em dashes are a well-known AI-writing tell and this site needs to read as hand-written.
- **Don't** name or count competing tools on the site (no "the other three tools," no "four logins," no naming Slack/Jira/Azure by brand). That comparison is fair game off-site (LinkedIn, launch posts) but the landing page sells what Trackly does, not what it replaces.
- **Don't** use a SaaS blue/purple/indigo accent anywhere, including gradients, glows, or chart colors.
- **Don't** use cream/sand backgrounds, gradient-text headlines, tiny uppercase eyebrows above sections, or identical icon-card grids.
- **Don't** let density creep toward old Jira/Azure clutter as the meetings/clients/contracts pillars add surface area.
- **Don't** reach for drop shadows as the default card treatment — hairline borders are the default (The Flat-By-Default Rule).
- **Don't** design any pillar as a visually separate module bolted onto the task tracker.
- **Don't** use fake browser traffic-light dots (red/yellow/green) in product-preview mocks — the live-pulse + mono URL treatment is the house style instead.
