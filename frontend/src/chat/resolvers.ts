import { WORKSPACE_MEMBERS } from '@/data/users'
import { useTaskStore, findTask } from '@/store/useTaskStore'

// @-mentions still resolve against the mock member directory — there's no
// real "list workspace members" endpoint yet (out of scope for Move 5).
// #-ticket-refs are real: useTaskStore is populated from the live API.
export function resolveUser(handle: string) {
  const member = WORKSPACE_MEMBERS.find((m) => m.handle.toLowerCase() === handle.toLowerCase())
  return member ? { id: member.id, label: member.name } : undefined
}

export function resolveTicket(id: string) {
  const found = findTask(useTaskStore.getState().columns, id)
  return found ? { id: found.task.id, label: found.task.title } : undefined
}
