---
name: Trackly
description: An all-in-one team workspace for tasks, meetings, clients, and contracts — precise, fast, and unmistakably its own.
colors:
  bg-light: "oklch(1 0 0)"
  surface-light: "oklch(0.98 0 0)"
  surface-2-light: "oklch(0.955 0 0)"
  border-light: "oklch(0 0 0 / 8%)"
  border-strong-light: "oklch(0 0 0 / 14%)"
  text-light: "oklch(0.17 0 0)"
  text-secondary-light: "oklch(0.38 0 0)"
  text-muted-light: "oklch(0.52 0 0)"
  accent-light: "oklch(0.42 0.1 110)"
  accent-strong-light: "oklch(0.36 0.11 110)"
  on-accent-light: "oklch(0.99 0 0)"
  bg-dark: "oklch(0.09 0 0)"
  surface-dark: "oklch(0.14 0 0)"
  surface-2-dark: "oklch(0.19 0 0)"
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
- Near-black, dark-first surfaces (continuing the app's existing dark-mode-by-default convention), with a true off-white light mode as the alternate, toggled via the same `.dark` class convention the app already uses.
- A single, rare accent color (Cured Olive) rather than a multi-color palette — restraint is the brand. It flips lightness by theme: bright olive on dark surfaces, deep olive on light surfaces, always the same hue.
- Geist Sans for UI and headings, Geist Mono for ticket IDs, timestamps, unlock figures, and other data — reinforcing technical precision on the densest surfaces.
- Motion is Responsive in the product; the landing page (brand register) earns a one-time staggered entrance choreography (700ms, ease-out-expo) that product screens don't get.
- Vanilla CSS custom properties scoped under a `.tp-shell` root class, coexisting with legacy Tailwind/shadcn on pages not yet migrated.

## 2. Colors

The palette is a **Restrained** strategy on dark (and light) surfaces: near-achromatic surfaces do the structural work, and one rare accent (≤10% of any screen) carries all of the brand's personality. No secondary or tertiary color roles.

### Primary
- **Cured Olive** — dark surfaces: `oklch(0.8 0.13 110)` (#dbdd82-ish bright yellow-green); light surfaces: `oklch(0.42 0.1 110)` (deep moss). Used for primary buttons, active/selected states, focus rings, and small indicators — never a large fill.

### Neutral
- **Dark surfaces**: bg `oklch(0.09 0 0)`, surface `oklch(0.14 0 0)`, surface-2 `oklch(0.19 0 0)` — pure achromatic (chroma 0), depth built from lightness steps, not hue.
- **Light surfaces**: bg `oklch(1 0 0)` (pure white), surface `oklch(0.98 0 0)`, surface-2 `oklch(0.955 0 0)`.
- **Borders**: low-opacity white-on-dark (`oklch(1 0 0 / 10%)`) and black-on-light (`oklch(0 0 0 / 8%)`) hairlines rather than a separate gray token family.
- **Text**: dark-mode body text is intentionally lighter-weight (400, not bumped) since Geist Sans reads heavier on dark already; muted text sits at `oklch(0.61 0 0)` dark / `oklch(0.52 0 0)` light — both tuned to clear 4.5:1 against their surface, not just the page bg.

### Named Rules
**The One Accent Rule.** Cured Olive appears on ≤10% of any given screen — buttons, active states, focus rings, and small indicators only. It never fills a card, a section background, or a large surface.

**The No-Default-Blue Rule.** No SaaS blue/purple/indigo accent, gradient, or glow anywhere in the system, including hover states, links, or charts.

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

Flat-by-default, dev-tool posture: depth comes from a 1px hairline border and lightness steps between surface/surface-2, not drop shadows. The one exception is the hero product-preview mock, which uses a single soft shadow (`0 24px 64px -32px oklch(0 0 0 / 45%)`) to read as a floating screenshot.

### Shadow Vocabulary
- **Hairline** (`border: 1px solid var(--tp-border)`): the default way surfaces separate. Primary elevation tool.
- **Floating** (`box-shadow: 0 24px 64px -32px oklch(0 0 0 / 45%)`): reserved for the hero preview and any future modal/popover.

### Named Rules
**The Flat-By-Default Rule.** Cards and panels sit flush with the background, separated by hairline borders, not shadow.

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

### Nav
- Fixed, `color-mix` translucent background + `backdrop-filter: blur(12px)`, hairline bottom border. Collapses to a hamburger + slide-down panel under 768px; full inline links + sign-in above it.

### Signature: Hero Product Preview
A "browser chrome" strip (a live-pulse indicator + truncated mono URL, no fake traffic-light dots) above a 3-column kanban mock. One card carries two small accent-wash pill badges (calendar + file icons) linking it to a meeting and a contract — the concrete, in-product proof of "one workspace, not four," rather than a claim in copy alone.

### Signature: Connect Diagram
A small CSS-only diagram (four muted nodes connected by hairline lines converging on a glowing Cured Olive center dot) used once, on the "One workspace, not four" pillar card, as the visual signature for cross-pillar linking. Not reused as generic decoration elsewhere.

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
