import { create } from 'zustand'
import type { ChatMessage, ChatScope, MessageBlock } from '@/chat/types'
import { scopeKey } from '@/chat/types'

// Messages keyed by scope, so the project stream and a ticket's comment
// thread are just two keys into the same store, not two systems. Mock now,
// real persisted messages once the backend lands in Move 4.

interface ChatStoreState {
  messages: Record<string, ChatMessage[]>
  sendMessage: (scope: ChatScope, blocks: MessageBlock[], authorId: string, authorInitials: string) => void
}

const initialMessages: Record<string, ChatMessage[]> = {
  'project:1': [
    {
      id: 'm1',
      authorId: 'sk',
      authorInitials: 'SK',
      blocks: [
        { type: 'text', text: 'Started on ' },
        { type: 'ticketRef', ticketId: '127', label: 'Design dashboard layout and stat cards' },
        { type: 'text', text: ' today, should have something to review by tomorrow.' },
      ],
      createdAt: '2026-07-03T09:12:00Z',
    },
    {
      id: 'm2',
      authorId: 'vz',
      authorInitials: 'VZ',
      blocks: [
        { type: 'text', text: 'Sounds good, ' },
        { type: 'mention', userId: 'am', label: 'Aisha Malik' },
        { type: 'text', text: ' can you take a look once it is up?' },
      ],
      createdAt: '2026-07-03T09:15:00Z',
    },
  ],
  'ticket:121': [
    {
      id: 'm3',
      authorId: 'js',
      authorInitials: 'JS',
      blocks: [{ type: 'text', text: 'Storybook config is in, addon-docs is next.' }],
      createdAt: '2026-07-03T14:02:00Z',
    },
  ],
}

export const useChatStore = create<ChatStoreState>((set) => ({
  messages: initialMessages,

  sendMessage: (scope, blocks, authorId, authorInitials) => {
    const key = scopeKey(scope)
    const message: ChatMessage = {
      id: `${Date.now()}`,
      authorId,
      authorInitials,
      blocks,
      createdAt: new Date().toISOString(),
    }
    set((state) => ({
      messages: { ...state.messages, [key]: [...(state.messages[key] ?? []), message] },
    }))
  },
}))
