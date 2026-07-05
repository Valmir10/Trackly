// Computed once at module load, not on every keystroke — the global command
// palette keyboard layer checks this on every keydown in the app, so it must
// not recompute or duplicate this logic inline. navigator.userAgentData is
// tried first (Chromium only, as of writing); navigator.platform is the
// fallback everywhere else, including Safari and Firefox.
function detectIsMac(): boolean {
  const uaData = (navigator as Navigator & { userAgentData?: { platform?: string } }).userAgentData
  if (uaData?.platform) return uaData.platform.toLowerCase().includes('mac')
  return navigator.platform.toLowerCase().includes('mac')
}

export const IS_MAC = detectIsMac()
