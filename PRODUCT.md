# Product

## Register

product

Register is split by surface: the app (tasks, kanban, meetings, client rooms, contracts, settings, analytics) is **product** — design serves the workflow. The marketing/landing page is **brand** — design is part of the pitch. Treat each page by its own register when working on it; this field is the default for anything not clearly one or the other (e.g. auth pages, onboarding).

## Vision

Trackly is an all-in-one team workspace that replaces the fragmentation of running a project across four separate tools — Slack for comms, Azure/Jira for tasks, Teams for meetings, a CRM for clients and contracts. Everything a team needs to plan, execute, meet about, and bill for a project lives in one seamless surface. This is deliberately ambitious: the product is not "a kanban board" but the connective tissue between tasks, meetings, clients, and contracts — each pillar aware of the others.

The four pillars:

1. **Task & Workflow Management** (the existing core). Tickets, kanban boards, assignment, sprints, dependencies, rule-based automations (e.g. "when a ticket moves to Done and is linked to a contract milestone, notify the client contact"). This is the hyper-efficient, streamlined heart of the product — must never regress in speed for the sake of the other three pillars.
2. **Meeting Orchestration** (not video calls — Trackly doesn't host the call itself). Scheduling, collaborative pre-meeting agenda-building (attendees propose and upvote agenda items async, not just a calendar invite), shared live notes during the meeting, and a post-meeting feedback loop (quick per-attendee pulse, rolled up over time to surface meeting fatigue or recurring blockers). Action items captured in notes convert to tasks with one click, linked back to the meeting they came from.
3. **Client Communication.** A scoped "Client Room" per project: external stakeholders see progress, leave approvals/comments, and get status updates — without exposure to internal ticket noise, internal comments, or unrelated projects. This is the one surface with an external, non-team audience.
4. **Contract & Deal Management.** Contracts/SOWs define deliverables and milestones; tasks tagged to a milestone roll up progress automatically, so over- or under-delivery against a contract is visible before renewal, not after. A renewal/expiry radar surfaces contracts approaching end-of-term against current team capacity.

A unifying activity timeline per project correlates task activity, meeting notes, client messages, and contract events into one feed — the concrete answer to "why don't I need four tabs open anymore."

## Users

Three audiences now, not two:

- **Team members (core product surface — tasks, meetings, contracts)**: small technical teams — engineers, PMs, designers — running the actual work day-to-day. Comfortable with density, keyboard shortcuts, technical language. Efficiency and precision matter more than hand-holding.
- **External clients (Client Room surface)**: non-technical stakeholders who need clarity, trust, and a sense of progress — not workflow tools. This surface should feel calmer and more curated than the internal app, closer to "read a status update" than "manage a project."
- **Landing page (brand surface)**: non-technical evaluators and decision-makers deciding whether to consolidate their stack into Trackly. The pitch is fragmentation → one workspace; needs to build confidence that "all-in-one" doesn't mean "does everything poorly."

## Product Purpose

Trackly is a multi-tenant SaaS all-in-one workspace (tenants, users, projects, tasks, kanban boards, meetings, clients, contracts). It is also a portfolio/learning project — the codebase and its craft are themselves being evaluated, alongside the product's usability. Success looks like a tool a real technical team could run a client-facing project through end-to-end, built with visible engineering and design rigor, where the "all-in-one" claim is felt rather than just stated.

## Brand Personality

Linear-like precision with a more ambitious, futuristic edge: minimal, sharp, fast — but "cool" rather than clinical. Restrained palette, confident typography, snappy low-noise motion. The feeling to evoke is speed and control over complexity that would otherwise be scattered across four tools. Unique and sleek, never templated.

## Anti-references

- Generic AI-template SaaS: cream/sand backgrounds, gradient-text headlines, tiny uppercase eyebrows above every section, identical icon-card grids, hero-metric clichés.
- Cluttered, dated enterprise software (old Jira, Azure DevOps): dense nested panels, tiny inconsistent icons, visual noise, unclear hierarchy.
- Frankenstein integrations: four features bolted together that each look and behave like a different product (the exact failure mode Trackly exists to replace, so the UI can't visibly repeat it internally).

## Design Principles

1. **One workspace, not four bolted together.** Tasks, meetings, clients, and contracts must feel like facets of one system — shared visual language, shared navigation model, cross-links between them (a task shows its contract milestone, a meeting shows its resulting tasks). Never let a pillar look like a separately-designed module.
2. **The core stays fast as scope grows.** Task/kanban management is the pillar used most often and must never slow down or get buried under meetings/clients/contracts navigation. New pillars are additive, not competing for the same attention.
3. **Precision over decoration.** Every visual choice should read as deliberate and functional — no default AI-template scaffolding (eyebrows, gradient text, generic card grids).
4. **Density without clutter, calibrated per surface.** The internal app can be information-rich for technical users; the Client Room must be calmer and more curated for a non-technical external audience. More surface area than a simple tracker means hierarchy has to work harder, not that it can slip toward old-Jira/Azure clutter.
5. **Craft is part of the product.** This is a portfolio project — code and design quality are both being evaluated, so shortcuts that would pass in a demo but not in production are not acceptable.
6. **Vanilla CSS as a deliberate constraint, not a limitation.** Styling moves off Tailwind to hand-authored CSS (custom properties, no utility framework). The result should look and feel like a bespoke, elite visual system — not merely "Tailwind rewritten by hand."

## Accessibility & Inclusion

WCAG AA as the baseline: ≥4.5:1 contrast for body text, ≥3:1 for large text, full keyboard navigability, and `prefers-reduced-motion` support on all motion. No additional constraints specified beyond AA.
