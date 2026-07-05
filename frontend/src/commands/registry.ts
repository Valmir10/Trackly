import { create } from 'zustand'
import { useEffect } from 'react'
import type { Command, CommandSource, CommandPage } from './types'

// One module-level store. Surfaces register commands/sources on mount, get
// an unregister function back, call it on unmount. The palette subscribes
// to the same store. Recency is a small LRU of recencyKeys, persisted to
// localStorage so the palette remembers usage across reloads.

const RECENCY_MAX = 24
const RECENCY_STORAGE_KEY = 'trackly.palette.recency.v1'

function loadRecency(): string[] {
  try {
    const raw = localStorage.getItem(RECENCY_STORAGE_KEY)
    return raw ? (JSON.parse(raw) as string[]) : []
  } catch {
    return []
  }
}

interface RegistryState {
  /** Keyed by command id. Map preserves insertion order, the stable tiebreak when scores are equal. */
  commands: Map<string, Command>
  sources: Map<string, CommandSource>

  isOpen: boolean
  pageStack: CommandPage[]

  /** Most-recent-first list of recencyKeys (or command ids). */
  recency: string[]

  register: (commands: Command[]) => () => void
  registerSource: (source: CommandSource) => () => void
  open: () => void
  close: () => void
  toggle: () => void
  pushPage: (page: CommandPage) => void
  popPage: () => void
  touchRecency: (key: string) => void
}

export const useCommandRegistry = create<RegistryState>((set) => ({
  commands: new Map(),
  sources: new Map(),
  isOpen: false,
  pageStack: [],
  recency: loadRecency(),

  register: (commands) => {
    set((s) => {
      const next = new Map(s.commands)
      for (const c of commands) {
        if (import.meta.env.DEV && next.has(c.id)) {
          console.warn(`[palette] duplicate command id "${c.id}" — overwriting`)
        }
        next.set(c.id, c)
      }
      return { commands: next }
    })
    return () => {
      set((s) => {
        const next = new Map(s.commands)
        for (const c of commands) next.delete(c.id)
        return { commands: next }
      })
    }
  },

  registerSource: (source) => {
    set((s) => ({ sources: new Map(s.sources).set(source.id, source) }))
    return () => {
      set((s) => {
        const next = new Map(s.sources)
        next.delete(source.id)
        return { sources: next }
      })
    }
  },

  // Doesn't touch pageStack — close() always clears it, so an already-closed
  // palette is guaranteed to have an empty stack by the time this runs. This
  // matters because runCommand calls open() immediately before pushPage() to
  // handle page-returning commands fired from a closed state (e.g. a global
  // keybinding); resetting the stack here would wipe that page right back out.
  open: () => set({ isOpen: true }),
  close: () => set({ isOpen: false, pageStack: [] }),
  toggle: () => set((s) => ({ isOpen: !s.isOpen, pageStack: [] })),

  pushPage: (page) => set((s) => ({ pageStack: [...s.pageStack, page] })),
  popPage: () => set((s) => ({ pageStack: s.pageStack.slice(0, -1) })),

  touchRecency: (key) => {
    set((s) => {
      const next = [key, ...s.recency.filter((k) => k !== key)].slice(0, RECENCY_MAX)
      try {
        localStorage.setItem(RECENCY_STORAGE_KEY, JSON.stringify(next))
      } catch {
        // recency is a nicety, not a requirement
      }
      return { recency: next }
    })
  },
}))

// React-side registration hooks. Commands re-register when deps change
// (e.g. the projects list updates -> new jump commands).
export function useRegisterCommands(factory: () => Command[], deps: React.DependencyList) {
  const register = useCommandRegistry((s) => s.register)
  useEffect(() => {
    return register(factory())
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, deps)
}

export function useRegisterCommandSource(factory: () => CommandSource, deps: React.DependencyList) {
  const registerSource = useCommandRegistry((s) => s.registerSource)
  useEffect(() => {
    return registerSource(factory())
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, deps)
}
