import { useEffect, useRef } from 'react'
import { useCommandRegistry } from './registry'
import { IS_MAC } from '@/lib/platform'
import type { Command, CommandContext, Keybinding } from './types'

// Global keyboard layer. Mounted ONCE, in AppShell.
// - mod+K toggles the palette from anywhere, including editable targets —
//   the one universal exception.
// - Registered command keybindings fire only while the palette is closed
//   and focus is not in an editable surface (input/textarea/contentEditable
//   — the future chat composer depends on this boundary holding).
// - Single-step chords fire immediately; two-step, modifier-free sequences
//   ("g d") arm an 800ms window for the second key.
// The palette's *internal* navigation keys live in the palette component,
// not here — this hook is only the always-on layer.

const CHORD_TIMEOUT_MS = 800

function isEditableTarget(t: EventTarget | null): boolean {
  if (!(t instanceof HTMLElement)) return false
  if (t.isContentEditable) return true
  const tag = t.tagName
  return tag === 'INPUT' || tag === 'TEXTAREA' || tag === 'SELECT'
}

function stepMatches(e: KeyboardEvent, step: Keybinding['steps'][number]): boolean {
  const mod = IS_MAC ? e.metaKey : e.ctrlKey
  return (
    e.key.toLowerCase() === step.key.toLowerCase() &&
    mod === !!step.mod &&
    e.shiftKey === !!step.shift &&
    e.altKey === !!step.alt
  )
}

export function useGlobalCommandKeys(getContext: () => CommandContext) {
  const pendingFirstStep = useRef<{ commands: Command[]; at: number } | null>(null)

  useEffect(() => {
    function onKeyDown(e: KeyboardEvent) {
      const { isOpen, toggle, commands, touchRecency, close } = useCommandRegistry.getState()

      const mod = IS_MAC ? e.metaKey : e.ctrlKey
      if (mod && e.key.toLowerCase() === 'k') {
        e.preventDefault()
        toggle()
        pendingFirstStep.current = null
        return
      }

      // Everything below only applies while the palette is CLOSED and
      // focus is not in an editable surface. Clear any pending chord here
      // too — a "g" typed just before focusing a text field shouldn't be
      // able to complete a chord once you've clicked back out of it.
      if (isOpen || isEditableTarget(e.target)) {
        pendingFirstStep.current = null
        return
      }
      if (e.key === 'Meta' || e.key === 'Control' || e.key === 'Shift' || e.key === 'Alt') return

      const ctx = getContext()
      const all = [...commands.values()].filter((c) => c.keybinding && (!c.when || c.when(ctx)))

      const pending = pendingFirstStep.current
      if (pending && Date.now() - pending.at < CHORD_TIMEOUT_MS) {
        const hit = pending.commands.find((c) => stepMatches(e, c.keybinding!.steps[1]))
        pendingFirstStep.current = null
        if (hit) {
          e.preventDefault()
          touchRecency(hit.recencyKey ?? hit.id)
          void runCommand(hit, ctx, close)
          return
        }
      } else {
        pendingFirstStep.current = null
      }

      const singles = all.filter((c) => c.keybinding!.steps.length === 1)
      const doubles = all.filter((c) => c.keybinding!.steps.length === 2)

      const single = singles.find((c) => stepMatches(e, c.keybinding!.steps[0]))
      if (single) {
        e.preventDefault()
        touchRecency(single.recencyKey ?? single.id)
        void runCommand(single, ctx, close)
        return
      }

      const armed = doubles.filter((c) => stepMatches(e, c.keybinding!.steps[0]))
      if (armed.length > 0) {
        e.preventDefault()
        pendingFirstStep.current = { commands: armed, at: Date.now() }
      }
    }

    window.addEventListener('keydown', onKeyDown)
    return () => window.removeEventListener('keydown', onKeyDown)
  }, [getContext])
}

/** Shared runner: catches errors, supports page-returning commands. Used by both the global layer and the palette itself. */
export async function runCommand(command: Command, ctx: CommandContext, closePalette: () => void) {
  try {
    const result = await command.run(ctx)
    if (result && 'commands' in result) {
      const { open, pushPage } = useCommandRegistry.getState()
      open()
      pushPage(result)
    } else {
      closePalette()
    }
  } catch (err) {
    console.error(`[palette] command "${command.id}" failed:`, err)
  }
}
