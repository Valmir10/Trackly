import { parseMessage } from '@/chat/parseMessage'
import { resolveUser, resolveTicket } from '@/chat/resolvers'
import type { ChatMessage } from '@/chat/types'
import type { ChatMessageDto } from '@/api/types'

export function mapChatMessageDtoToChatMessage(dto: ChatMessageDto): ChatMessage {
  return {
    id: dto.id,
    authorId: dto.authorId,
    authorInitials: dto.authorInitials,
    blocks: parseMessage(dto.content, { resolveUser, resolveTicket }),
    createdAt: dto.createdAt,
  }
}
