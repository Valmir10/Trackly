import { useEffect } from 'react'
import type { RefObject } from 'react'

// Closes a dropdown/menu when the user clicks or taps outside the given ref.
// Used by AppSidebar's workspace switcher and AppTopBar's avatar/notifications menus.
export function useClickOutside(ref: RefObject<HTMLElement | null>, onOutside: () => void, active: boolean) {
  useEffect(() => {
    if (!active) return

    function handlePointerDown(event: PointerEvent) {
      if (ref.current && !ref.current.contains(event.target as Node)) {
        onOutside()
      }
    }

    document.addEventListener('pointerdown', handlePointerDown)
    return () => document.removeEventListener('pointerdown', handlePointerDown)
  }, [ref, onOutside, active])
}
