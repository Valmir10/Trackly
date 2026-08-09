import { useEffect, useMemo, useRef, useState } from 'react'
import { useCommandRegistry } from '@/commands/registry'
import { useCommandContextGetter } from '@/commands/useCommandContext'
import { rank } from '@/commands/match'
import { runCommand } from '@/commands/useGlobalKeys'
import type { Command, CommandGroup, CommandPage } from '@/commands/types'
import '@/styles/CommandPalette.css'

const GROUP_LABELS: Record<CommandGroup, string> = {
  navigate: 'Navigate',
  ticket: 'Tickets',
  meeting: 'Meetings',
  action: 'Actions',
  help: 'Help',
}

export default function CommandPalette() {
  const isOpen = useCommandRegistry((s) => s.isOpen)
  const pageStack = useCommandRegistry((s) => s.pageStack)

  if (!isOpen) return null

  // Remounting on page-depth change gives every page a fresh query/activeIndex
  // for free (initial useState values), instead of syncing them back to zero
  // via effects that fire after an external store update.
  return <CommandPaletteView key={pageStack.length} currentPage={pageStack[pageStack.length - 1]} />
}

function CommandPaletteView({ currentPage }: { currentPage: CommandPage | undefined }) {
  const commandsMap = useCommandRegistry((s) => s.commands)
  const sourcesMap = useCommandRegistry((s) => s.sources)
  const recency = useCommandRegistry((s) => s.recency)
  const close = useCommandRegistry((s) => s.close)
  const popPage = useCommandRegistry((s) => s.popPage)
  const touchRecency = useCommandRegistry((s) => s.touchRecency)
  const pageDepth = useCommandRegistry((s) => s.pageStack.length)

  const getContext = useCommandContextGetter()

  const [query, setQuery] = useState('')
  const [activeIndex, setActiveIndex] = useState(0)
  const [dynamicCommands, setDynamicCommands] = useState<Command[]>([])
  const inputRef = useRef<HTMLInputElement>(null)

  useEffect(() => {
    requestAnimationFrame(() => inputRef.current?.focus())
  }, [])

  // Dynamic, entity-backed sources (tickets, etc.) — only relevant at the
  // top level, not inside a pushed page, since a page's commands are
  // already a fixed list. Light debounce so this is ready for an eventual
  // API-backed source without behaving differently now.
  useEffect(() => {
    if (currentPage) return
    const timeout = setTimeout(async () => {
      const results: Command[] = []
      for (const source of sourcesMap.values()) {
        if (source.prefix) {
          if (!query.toLowerCase().startsWith(source.prefix.toLowerCase())) continue
          const stripped = query.slice(source.prefix.length)
          results.push(...(await source.getCommands(stripped, getContext())))
        } else {
          results.push(...(await source.getCommands(query, getContext())))
        }
      }
      setDynamicCommands(results)
    }, 120)
    return () => clearTimeout(timeout)
  }, [query, sourcesMap, currentPage, getContext])

  const ctx = useMemo(() => getContext(), [getContext])

  const ranked = useMemo(() => {
    const pool = currentPage ? currentPage.commands : [...commandsMap.values(), ...dynamicCommands]
    const available = pool.filter((c) => !c.when || c.when(ctx))
    return rank(query, available, recency)
  }, [currentPage, commandsMap, dynamicCommands, query, recency, ctx])

  function handleRun(command: Command) {
    touchRecency(command.recencyKey ?? command.id)
    void runCommand(command, ctx, close)
  }

  function handleQueryChange(value: string) {
    setQuery(value)
    setActiveIndex(0)
  }

  function handlePopPage() {
    popPage()
  }

  function handleKeyDown(e: React.KeyboardEvent) {
    if (e.key === 'ArrowDown') {
      e.preventDefault()
      setActiveIndex((i) => Math.min(i + 1, ranked.length - 1))
    } else if (e.key === 'ArrowUp') {
      e.preventDefault()
      setActiveIndex((i) => Math.max(i - 1, 0))
    } else if (e.key === 'Home') {
      e.preventDefault()
      setActiveIndex(0)
    } else if (e.key === 'End') {
      e.preventDefault()
      setActiveIndex(ranked.length - 1)
    } else if (e.key === 'Enter') {
      e.preventDefault()
      const hit = ranked[activeIndex]
      if (hit) handleRun(hit.command)
    } else if (e.key === 'Escape') {
      e.preventDefault()
      if (pageDepth > 0) handlePopPage()
      else close()
    } else if (e.key === 'Backspace' && query === '' && pageDepth > 0) {
      e.preventDefault()
      handlePopPage()
    }
  }

  return (
    <div className="tp-modal-overlay" onClick={close}>
      <div className="tp-palette" onClick={(e) => e.stopPropagation()} role="presentation">
        <div className="tp-palette__input-row">
          {currentPage && <span className="tp-palette__crumb">{currentPage.title}</span>}
          <input
            ref={inputRef}
            className="tp-palette__input"
            role="combobox"
            aria-expanded="true"
            aria-controls="tp-palette-listbox"
            aria-activedescendant={ranked[activeIndex] ? `tp-palette-option-${ranked[activeIndex].command.id}` : undefined}
            placeholder={currentPage?.placeholder ?? 'Search commands, tickets, or jump anywhere...'}
            value={query}
            onChange={(e) => handleQueryChange(e.target.value)}
            onKeyDown={handleKeyDown}
          />
        </div>

        <div className="tp-palette__list" role="listbox" id="tp-palette-listbox">
          {ranked.length === 0 && <div className="tp-palette__empty">No matching commands</div>}
          {ranked.map((r, i) => {
            const prevGroup = i > 0 ? ranked[i - 1].command.group : null
            const showGroupHeader = r.command.group !== prevGroup
            return (
              <div key={r.command.id}>
                {showGroupHeader && <div className="tp-palette__group">{GROUP_LABELS[r.command.group]}</div>}
                <div
                  id={`tp-palette-option-${r.command.id}`}
                  role="option"
                  aria-selected={i === activeIndex}
                  className={`tp-palette__row${i === activeIndex ? ' tp-palette__row--active' : ''}`}
                  onMouseEnter={() => setActiveIndex(i)}
                  onClick={() => handleRun(r.command)}
                >
                  <span className="tp-palette__title">{r.command.title}</span>
                  {r.command.mono && <span className="tp-palette__mono">{r.command.mono}</span>}
                  {r.command.keybinding && <kbd className="tp-palette__kbd">{r.command.keybinding.label}</kbd>}
                </div>
              </div>
            )
          })}
        </div>
      </div>
    </div>
  )
}
