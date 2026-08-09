import type { NavigateFunction } from 'react-router-dom'

// One flat registry. Surfaces contribute commands; the palette knows
// nothing about any surface. This file is the contract between them.

export type CommandGroup =
  | 'navigate'
  | 'ticket'
  | 'meeting'
  | 'action'
  | 'help'

// Everything a command is allowed to touch. Commands never import stores
// or the router directly — the context is the only capability surface,
// which keeps commands testable and means the mock-to-real data swap
// later touches zero command definitions.
export interface CommandContext {
  navigate: NavigateFunction
  slug: string
  openTicket: (ticketId: string) => void
  openMeeting: (meetingId: string) => void
  close: () => void
  push: (page: CommandPage) => void
}

// A chord is 1-2 steps; each step is a key plus modifiers. "mod" means
// Cmd on macOS, Ctrl elsewhere (see src/lib/platform.ts). Two-step chords
// are modifier-free by convention ("g d", not "mod+g mod+d").
export interface Keybinding {
  steps: Array<{ key: string; mod?: boolean; shift?: boolean; alt?: boolean }>
  label: string
}

export interface Command {
  id: string
  title: string
  group: CommandGroup
  keywords?: string[]
  /** A datum rendered in Geist Mono next to the title, e.g. "#127". */
  mono?: string
  /** Global keybinding, active even when the palette is closed. */
  keybinding?: Keybinding
  /** Unavailable commands are filtered out, never shown disabled. */
  when?: (ctx: CommandContext) => boolean
  run: (ctx: CommandContext) => void | CommandPage | Promise<void | CommandPage>
  /**
   * Recency tracking key override. Commands generated per-entity (one per
   * ticket) share a recencyKey so recency boosts the behavior, not one
   * specific ticket forever.
   */
  recencyKey?: string
}

// A pushed sub-list: how multi-step commands take arguments. "Change
// status..." runs, returns a page of status options, each option is
// itself a full Command. No special argument-parsing machinery.
export interface CommandPage {
  id: string
  title: string
  placeholder?: string
  commands: Command[]
}

// Dynamic providers for entity-backed commands (tickets, projects).
// Static commands live in the registry permanently; sources are queried
// per keystroke. Today they read mock data synchronously; once there's a
// real API they read a TanStack Query cache instead — the palette code
// does not change either way.
export interface CommandSource {
  id: string
  /** If set, only activates when the query starts with this prefix
   *  ("#" -> tickets); the prefix is stripped before the query is passed in. */
  prefix?: string
  getCommands: (query: string, ctx: CommandContext) => Command[] | Promise<Command[]>
}

export interface RankedCommand {
  command: Command
  score: number
}
