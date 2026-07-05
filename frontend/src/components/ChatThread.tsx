import { useEffect, useRef, useState } from 'react'
import { Send } from 'lucide-react'
import { useChatStore } from '@/store/useChatStore'
import { useTaskStore, findTask } from '@/store/useTaskStore'
import { WORKSPACE_MEMBERS, CURRENT_MEMBER } from '@/data/users'
import { parseMessage } from '@/chat/parseMessage'
import { detectMentionTrigger } from '@/chat/mentionTrigger'
import { scopeKey } from '@/chat/types'
import type { ChatMessage, ChatScope, MessageBlock } from '@/chat/types'
import type { MentionTrigger } from '@/chat/mentionTrigger'
import type { Task } from '@/components/TaskCard'
import '@/styles/ChatThread.css'

interface ChatThreadProps {
  scope: ChatScope
  onOpenTicket: (ticketId: string) => void
}

interface PickerItem {
  id: string
  label: string
  insertText: string
  mono?: string
}

function resolveUser(handle: string) {
  const member = WORKSPACE_MEMBERS.find((m) => m.handle.toLowerCase() === handle.toLowerCase())
  return member ? { id: member.id, label: member.name } : undefined
}

function resolveTicket(id: string) {
  const found = findTask(useTaskStore.getState().columns, id)
  return found ? { id: found.task.id, label: found.task.title } : undefined
}

function matchMembers(query: string): PickerItem[] {
  const q = query.toLowerCase()
  return WORKSPACE_MEMBERS.filter((m) => m.handle.toLowerCase().startsWith(q) || m.name.toLowerCase().includes(q))
    .slice(0, 6)
    .map((m) => ({ id: m.id, label: m.name, insertText: m.handle }))
}

function matchTickets(query: string): PickerItem[] {
  const q = query.toLowerCase()
  const all: Task[] = useTaskStore.getState().columns.flatMap((c) => c.tasks)
  return all
    .filter((t) => t.id.startsWith(query) || t.title.toLowerCase().includes(q))
    .slice(0, 6)
    .map((t) => ({ id: t.id, label: t.title, insertText: t.id, mono: `#${t.id}` }))
}

function renderBlock(block: MessageBlock, key: number, onOpenTicket: (id: string) => void) {
  switch (block.type) {
    case 'text':
      return <span key={key}>{block.text}</span>
    case 'mention':
      return (
        <span key={key} className="tp-chat__mention">
          @{block.label}
        </span>
      )
    case 'ticketRef':
    case 'card':
      return (
        <button key={key} type="button" className="tp-chat__ticket-chip" onClick={() => onOpenTicket(block.ticketId)}>
          #{block.ticketId}
        </button>
      )
  }
}

const EMPTY_MESSAGES: ChatMessage[] = []

export default function ChatThread({ scope, onOpenTicket }: ChatThreadProps) {
  const messages = useChatStore((s) => s.messages[scopeKey(scope)] ?? EMPTY_MESSAGES)
  const sendMessage = useChatStore((s) => s.sendMessage)

  const [value, setValue] = useState('')
  const [trigger, setTrigger] = useState<MentionTrigger | null>(null)
  const [pickerIndex, setPickerIndex] = useState(0)
  const inputRef = useRef<HTMLInputElement>(null)
  const listRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    listRef.current?.scrollTo({ top: listRef.current.scrollHeight })
  }, [messages.length])

  const pickerItems: PickerItem[] = trigger
    ? trigger.marker === '@'
      ? matchMembers(trigger.query)
      : matchTickets(trigger.query)
    : []

  function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
    const nextValue = e.target.value
    setValue(nextValue)
    const caret = e.target.selectionStart ?? nextValue.length
    setTrigger(detectMentionTrigger(nextValue, caret))
    setPickerIndex(0)
  }

  function applyPickerItem(item: PickerItem) {
    if (!trigger) return
    const before = value.slice(0, trigger.start)
    const after = value.slice(trigger.start + trigger.marker.length + trigger.query.length)
    const inserted = `${trigger.marker}${item.insertText} `
    const nextValue = before + inserted + after
    const caret = (before + inserted).length
    setValue(nextValue)
    setTrigger(null)
    requestAnimationFrame(() => {
      inputRef.current?.focus()
      inputRef.current?.setSelectionRange(caret, caret)
    })
  }

  function handleSend() {
    const trimmed = value.trim()
    if (!trimmed) return
    const blocks = parseMessage(trimmed, { resolveUser, resolveTicket })
    sendMessage(scope, blocks, CURRENT_MEMBER.id, CURRENT_MEMBER.initials)
    setValue('')
    setTrigger(null)
  }

  function handleKeyDown(e: React.KeyboardEvent<HTMLInputElement>) {
    if (trigger && pickerItems.length > 0) {
      if (e.key === 'ArrowDown') {
        e.preventDefault()
        setPickerIndex((i) => Math.min(i + 1, pickerItems.length - 1))
        return
      }
      if (e.key === 'ArrowUp') {
        e.preventDefault()
        setPickerIndex((i) => Math.max(i - 1, 0))
        return
      }
      const alreadyResolved = trigger.marker === '@' ? resolveUser(trigger.query) : resolveTicket(trigger.query)
      if ((e.key === 'Enter' || e.key === 'Tab') && !alreadyResolved) {
        e.preventDefault()
        applyPickerItem(pickerItems[pickerIndex])
        return
      }
      if (e.key === 'Escape') {
        e.preventDefault()
        setTrigger(null)
        return
      }
    }
    if (e.key === 'Enter') {
      e.preventDefault()
      handleSend()
    }
  }

  return (
    <div className="tp-chat">
      <div className="tp-chat__list" ref={listRef}>
        {messages.length === 0 && <p className="tp-chat__empty">No messages yet.</p>}
        {messages.map((message) => (
          <div key={message.id} className="tp-chat__message">
            <span className="tp-avatar tp-avatar--sm">{message.authorInitials}</span>
            <div className="tp-chat__body">
              <span className="tp-chat__blocks">
                {message.blocks.map((block, i) => renderBlock(block, i, onOpenTicket))}
              </span>
            </div>
          </div>
        ))}
      </div>

      <div className="tp-chat__composer">
        {trigger && pickerItems.length > 0 && (
          <div className="tp-chat__picker">
            {pickerItems.map((item, i) => (
              <button
                key={item.id}
                type="button"
                className={`tp-chat__picker-item${i === pickerIndex ? ' tp-chat__picker-item--active' : ''}`}
                onMouseEnter={() => setPickerIndex(i)}
                onClick={() => applyPickerItem(item)}
              >
                <span className="tp-chat__picker-label">{item.label}</span>
                {item.mono && <span className="tp-chat__picker-mono">{item.mono}</span>}
              </button>
            ))}
          </div>
        )}

        <input
          ref={inputRef}
          type="text"
          className="tp-input tp-chat__input"
          placeholder="Message... @ to mention, # for a ticket"
          value={value}
          onChange={handleChange}
          onKeyDown={handleKeyDown}
        />
        <button type="button" className="tp-chat__send" onClick={handleSend} aria-label="Send message">
          <Send size={14} />
        </button>
      </div>
    </div>
  )
}
