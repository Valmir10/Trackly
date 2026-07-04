# Product

## Register

product

Register is split by surface: the app (tasks, kanban, meetings, client rooms, contracts, settings, analytics) is **product** — design serves the workflow. The marketing/landing page is **brand** — design is part of the pitch. Treat each page by its own register when working on it; this field is the default for anything not clearly one or the other (e.g. auth pages, onboarding).

## Vision

Trackly is built on one structural idea: **one object model, four surfaces.** A ticket is not a thing that merely links to a conversation, a meeting, and a client update — it *is* a conversation with an operational status. The board renders its workflow position. The conversation surface renders its discussion. A meeting renders the decisions that created or resolved it. A client room renders its curated, external-safe summary. Four surfaces, one live object underneath — nothing is "synced" between pillars, because there are no pillars to sync. There are only lenses.

This supersedes an earlier four-*pillars* framing that included a standalone Contract & Deal Management pillar. Contracts are cut as a fourth surface; milestone rollup survives as a feature inside the Client Room lens, not as its own surface. Three product functions plus the object model itself is a sharper story than four bolted-together features, and CRM/contract territory diluted the core positioning.

### The four surfaces

1. **Board** (workflow lens). Tickets, kanban boards, sprints, dependencies, rule-based automation. The only surface substantially built today — must never regress in speed for the sake of the other three.
2. **Conversation** (discussion lens). A project-scoped chat stream and a per-ticket comment thread, rendered by the *same component*, scoped differently — not two systems that look similar, one system rendered twice. Not yet built.
3. **Meetings** (decision lens, not video calls — Trackly doesn't host the call itself). Async pre-meeting agenda-building, shared live notes, decisions and action items converting to tasks in one click. Not yet built.
4. **Client Room** (external-read lens). External stakeholders see curated progress and leave approvals — zero internal ticket noise — and this is where contract/milestone rollup lives now. The one surface with a non-team audience. Not yet built.

### Positioning

Trackly targets the daily execution layer where technical teams actually spend their hours — currently split between Azure DevOps (tickets, sprints, boards), Slack (chat), and Teams (meeting coordination). It deliberately does not compete on infrastructure: no CI/CD, no repos, no video hosting. The wedge is that none of the three incumbents can adopt the unified object model without rebuilding themselves from the data layer up — Slack cannot make a message be a ticket, Azure DevOps cannot make a ticket be a conversation. Trackly starts there. The model's proof does not require all four surfaces to exist; it's demonstrable the moment the first two (Board + Conversation) render the same live object — see Architecture Roadmap.

## Users

- **Team members (Board + Conversation, day to day)**: small technical teams — engineers, PMs, designers. Comfortable with density, keyboard shortcuts, technical language. Efficiency and precision matter more than hand-holding.
- **External clients (Client Room)**: non-technical stakeholders who need clarity, trust, and a sense of progress, not workflow tools. Calmer and more curated than the internal app, closer to "read a status update" than "manage a project."
- **Landing page evaluators**: non-technical decision-makers deciding whether to consolidate their stack into Trackly. The pitch is fragmentation → one workspace; needs confidence that "all-in-one" doesn't mean "does everything poorly."
- **Senior technical evaluators (the whole system)**: the audience for the system as a portfolio artifact. What they need to see is evidence that the unification is structural — one component, one object, one mutation path — not cosmetic consistency dressed up to look unified.

## Product Purpose

Trackly is a multi-tenant SaaS all-in-one workspace (tenants, users, projects, tasks, kanban boards, meetings, clients, contracts). It is also a portfolio/learning project — the codebase and its craft are themselves being evaluated, alongside the product's usability. Success looks like a tool a real technical team could run a client-facing project through end-to-end, built with visible engineering and design rigor, where the "all-in-one" claim is felt rather than just stated.

## Brand Personality

Linear-like precision with a more ambitious, futuristic edge: minimal, sharp, fast — but "cool" rather than clinical. Restrained palette, confident typography, snappy low-noise motion. The feeling to evoke is speed and control over complexity that would otherwise be scattered across four tools. Unique and sleek, never templated.

## Anti-references

- Generic AI-template SaaS: cream/sand backgrounds, gradient-text headlines, tiny uppercase eyebrows above every section, identical icon-card grids, hero-metric clichés.
- Cluttered, dated enterprise software (old Jira, Azure DevOps): dense nested panels, tiny inconsistent icons, visual noise, unclear hierarchy.
- Frankenstein integrations: four features bolted together that each look and behave like a different product (the exact failure mode Trackly exists to replace, so the UI can't visibly repeat it internally).

## Design Principles

1. **One object, four lenses — not four bolted-together pillars.** Board, Conversation, Meetings, and Client Room must feel like facets of one system, because structurally they are: shared visual language, shared components (`<ChatThread>` rendered twice, not built twice), cross-links between surfaces. Never let a surface look like a separately-designed module.
2. **The core stays fast as scope grows.** Board is the surface used most often and must never slow down or get buried under Conversation/Meetings/Client Room navigation. New surfaces are additive, not competing for the same attention.
3. **Precision over decoration.** Every visual choice should read as deliberate and functional — no default AI-template scaffolding (eyebrows, gradient text, generic card grids).
4. **Density without clutter, calibrated per surface.** The internal app can be information-rich for technical users; the Client Room must be calmer and more curated for a non-technical external audience. More surface area than a simple tracker means hierarchy has to work harder, not that it can slip toward old-Jira/Azure clutter.
5. **Craft is part of the product.** This is a portfolio project — code and design quality are both being evaluated, so shortcuts that would pass in a demo but not in production are not acceptable.
6. **Vanilla CSS as a deliberate constraint, not a limitation.** Styling moves off Tailwind to hand-authored CSS (custom properties, no utility framework). The result should look and feel like a bespoke, elite visual system — not merely "Tailwind rewritten by hand."

## Architecture Roadmap: The Unified Workspace Engine

Agreed 2026-07-04, revised same day after a cross-model architecture review (a second Claude model, "Fable," was given the then-current blueprint and asked to stress-test it; corrections below were adopted where they held up, and two of its own overreaches were caught and corrected back). This is the concrete mechanism behind "one object, four lenses" — not just a visual language, shared components and state doing real structural work.

### Monetization — decided

Real backend feature-gating, not decoration. A `WorkspaceTier` concept (Starter / Growth / Scale) lives in the Domain layer, resolved from team size by a `TierResolver`, enforced through real `FeatureGate` checks the frontend reads — not a hardcoded frontend team-size check pretending to be backend logic. There is still no real payment processing anywhere; this is feature-flagging, the plumbing every SaaS actually has, not billing. Scoped into Move 4 below since it's the same Domain/Infrastructure layers already being touched there, not a separate initiative. (Superseded an earlier draft that quietly downgraded this to "marketing surface only" — that was reversed on review: the mechanic gets built for real, it just isn't tied to payment.)

### The engine itself

**Scope is one discriminated union**, consumed by every surface that needs a "lens" — dashboard, Analytics, `ChatThread`, the command palette:
```ts
type Scope =
  | { type: 'all' }
  | { type: 'project'; projectId: string }
  // deliberate extension point, not built:
  // | { type: 'team'; teamId: string }
```
Lives in a `useWorkspaceScope` Zustand store (not `useDashboardContext` — it's not a dashboard-only concern). A context switcher (same dropdown primitive as the workspace switcher, but in the page header, not the sidebar) defaults to "All Projects."

**Pulse feed replaces the prose "Weekly summary."** A static paragraph pretending to be a live report is unactionable and close to the AI-fake-narrative anti-pattern already rejected elsewhere. Pulse is a list of discrete signals — `(severity, one-line statement, link target)`, e.g. "2 tasks in API v2 are overdue" → the filtered board. Computed from real task data (overdue count, velocity delta, completion rate are queries, not narrative) — which is why it's sequenced *last* below, after there's real data to compute from.

**`<ChatThread scope={{project} | {ticket}}>` is one component, used twice** — the project-wide stream and a single ticket's comment thread. The literal structural answer to "one object, four lenses," not marketing language.

**Message content is a block array, not rich text**: `{kind:'text'} | {kind:'mention', userId} | {kind:'ticketRef', ticketId, render:'chip'|'card'}`. This is the load-bearing decision that makes live inline cards (below) additive later instead of a rewrite.

**`@`/`#` referencing**: `@` opens a filtered member picker, `#` a filtered ticket picker; selection inserts a block. A sent `#127` renders as a small mono accent-wash pill (same visual language as the landing hero's meeting/contract badges) and is a real link into the Ticket Modal. Cursor-aware trigger detection lives in a shared `useMentionAutocomplete` hook; needs a `contentEditable` surface or a small headless mentions library, not a plain `<input>` — decide deliberately when building the composer.

**Live inline ticket cards** (`render: 'card'`): the chip is phase one; phase two promotes it to a live card rendered in the stream — status control, assignee, due date — so flipping status inline is the same mutation as dragging the board card. **Definition of done is same-session**: the board updates in the same viewport when the inline card's status changes — that alone proves one entity, one mutation path. Cross-window, multi-client sync is a bonus, never the load-bearing proof (a recorded demo's climax shouldn't depend on its flakiest dependency).

**Ticket Modal**: full detail, a status control (segmented/dropdown — literal drag-and-drop inside a modal is an explicit non-goal), a time-log input, and its own `<ChatThread scope={{ticket}}>`. Three renderings (modal, inline card, board card) of one object, one mutation path.

**URL-addressable tickets**: any task click sets `?ticket=127` on the project route; `ProjectPage` reads it and opens the modal. Deep-linkable, back-button-safe, shareable, for free — and what makes `#127` chat chips honest links rather than JS-only triggers.

**⌘K command palette, promoted to committed.** The topbar's ⌘K hint currently opens nothing, for an audience explicitly defined as keyboard-first — that gap gets closed, not left as a hint. Flat command registry (surfaces register commands via `useRegisterCommands`; the palette itself knows no surface — adding a future Meetings surface means Meetings registers commands, zero palette changes). Commands only touch a `CommandContext` (navigate, scope, openTicket, etc.) — no direct store/router imports — for testability and so the mock-to-real swap later touches zero command definitions. Ranking is tiered (exact datum `#127` > exact title > prefix > word-boundary > subsequence > keyword) with a bounded recency bonus that reorders near-ties but can never let a stale habit beat a fresh literal match. Two strictly separated keyboard layers: global chords (mod+K works everywhere, including editables — the one universal exception) vs. palette-internal navigation — global chords must never fire while focus is in an input/textarea/contentEditable, since the future chat composer depends on that boundary holding.

**Honest constraint:** no backend exists yet (`Program.cs` is still the scaffold template). TanStack Query is installed but unwired — standing up the query/mutation layer is real, budgeted work, not assumed free. Everything through Move 3 below ships frontend-only, mock-data-driven. SignalR (installed, unused) is the obvious real-time transport once Move 4 lands. Component/store boundaries throughout are chosen so the mock-to-real swap is additive, not a rewrite.

### Sequencing

1. **Move 0 — Finish theme depth** (obsidian retint, glow borders, shell-wide micro-grid) — in progress. Must land before Move 1; nothing gets migrated onto a design system that's still moving.
2. **Move 1 — Kanban migration + Ticket Modal + URL addressability.** Bring the project page onto the vanilla-CSS system, give cards click behavior, build the modal, wire `?ticket=`. Substrate for everything after. Frontend, mock data.
3. **Move 2 — ⌘K command palette.** Sequenced right after Move 1 because its two best commands (open ticket by ID, jump to project) depend on addressability just built. Frontend, mock data.
4. **Move 3 — `ChatThread` + block-model messages + `@`/`#` chips.** `render: 'card'` typed now, not rendered yet. Frontend, mock data.
5. **Move 4 — Backend vertical slice + tier gating.** One thin, deliberately narrow end-to-end path: Postgres via the existing Clean Architecture layers, minimal auth, tasks CRUD, chat messages, `WorkspaceTier`/`TierResolver`/`FeatureGate`, SignalR broadcasting task-status and message events. Exit criterion: two browser sessions, one workspace, a status change in one appears in the other without a refresh. Backend + integration. *Flexible split if solo-build pace makes this feel long:* 4a — tickets only, end-to-end, right after Move 1, for an early real proof point; 4b — chat persistence + SignalR + tier gating, after Moves 2–3. Legitimate either way; decide when Move 4 is actually next.
6. **Move 5 — Live inline ticket cards.** Rides Move 4's SignalR events. Same-session is the definition of done (see above).
7. **Move 6 — Pulse feed + workspace scope rollout.** Computed from Move 4's real API — sequenced last on purpose, since Pulse computed before real data exists is decorated mock data, not the feature it claims to be. Analytics adopts `useWorkspaceScope` in the same pass.

**Below the line, not yet scheduled:** Meetings surface, Client Room (external lens + contract milestone rollup), My Tasks/Settings visual migration, board drag-and-drop if it doesn't land in Move 5, the `team` scope variant once a real multi-pod use case exists.

**Explicitly not on the roadmap:** payment processing, video hosting, repo/CI integration, native apps, AI-branded features.

### The 90-second demo script

Seed data is written as a screenplay, not filler — every entity exists to make one beat land. Seed workspace: **Northlight Studio**, 9 members, 3 projects:

| Project | State | Purpose |
|---|---|---|
| Atlas API v2 | Slipping — 2 overdue, velocity down | Hosts the red Pulse signal and the live-card scene |
| Meridian Redesign | Healthy, mid-sprint | The credible baseline |
| Marketing Site | Shipped 12/12 this week | The green signal, the happy ending |

Rule: **exactly one thing is red** — three fires reads as a broken product, not an honest one. Atlas's chat history is seeded already mid-discussion about the slipping work, so the `#`-reference beat happens in context, not as a stunt.

1. **0:00** Dashboard, scope "All Projects." Pulse shows three computed signals, each a query with a destination, not narrative.
2. **0:10** Flip scope to Atlas API v2 — every widget refilters instantly. The dashboard is a lens, not a report.
3. **0:20** ⌘K → "atlas" → Enter. ⌘K → "127" → opens the ticket directly; `?ticket=127` is now a shareable deep link.
4. **0:35** Ticket Modal for #127 — its thread is visibly the same component as the docked project chat panel.
5. **0:50** Reply in the stream, type `#127`, send — renders as a mono accent-wash chip; click it, modal opens.
6. **1:05 — the scene that sells it.** Promote the chip to a live card, flip its status. The board, visible in the same viewport, moves the card at the same moment — same-session, one window. *(Optional 1:20 extension only if clean that week: repeat across two browser windows to show the transport layer too — bonus, not the spine.)*
7. **1:25** Back to Dashboard — the red Pulse signal has recalculated, one overdue item resolved. Closes on the screen it opened, now measurably different.

## Accessibility & Inclusion

WCAG AA as the baseline: ≥4.5:1 contrast for body text, ≥3:1 for large text, full keyboard navigability, and `prefers-reduced-motion` support on all motion. No additional constraints specified beyond AA.
