import { fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import type { BaseQueryFn, FetchArgs, FetchBaseQueryError } from '@reduxjs/toolkit/query'
import type { RootState } from '@/app/store'
import { authApi } from './authClient'
import { logout, updateTokens } from '@/features/auth/authSlice'

// ── Token Refresh Queue ────────────────────────────────────────────────────────
// Module-level singleton — shared across ALL api instances (catalog, ordering, etc.)
// Guarantees only ONE refresh call fires even if 5 requests get 401 simultaneously.
//
// How it works:
//   Request 1 hits 401 → refreshPromise is null → creates the promise, assigns it
//   Request 2 hits 401 → refreshPromise is NOT null → awaits the SAME promise
//   Request 3 hits 401 → refreshPromise is NOT null → awaits the SAME promise
//   Refresh completes → all three proceed to retry with the new token ✅
let refreshPromise: Promise<{ token: string; refreshToken: string } | null> | null = null

// ── Factory ────────────────────────────────────────────────────────────────────
// Call this instead of fetchBaseQuery() in every createApi().
// Wraps fetchBaseQuery with silent token refresh on 401.
//
// Usage:
//   baseQuery: createBaseQueryWithReauth(`${API_URLS.catalog}/api`)
export function createBaseQueryWithReauth(
  baseUrl: string,
): BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError> {

  // Standard fetchBaseQuery — reads token from Redux on every request
  const baseQuery = fetchBaseQuery({
    baseUrl,
    prepareHeaders: (headers, { getState }) => {
      const token = (getState() as RootState).auth.token
      if (token) headers.set('Authorization', `Bearer ${token}`)
      return headers
    },
  })

  return async (args, api, extraOptions) => {
    // ── 1. Fire the original request ──────────────────────────────────────────
    let result = await baseQuery(args, api, extraOptions)

    // ── 2. If not 401 → return immediately (success or other error like 404/500)
    if (result.error?.status !== 401) return result

    // ── 3. Got 401 — check if we have a refresh token to work with ────────────
    const state = api.getState() as RootState
    const storedRefreshToken = state.auth.refreshToken

    if (!storedRefreshToken) {
      // No refresh token → user is fully logged out → clear state
      api.dispatch(logout())
      return result
    }

    // ── 4. Start refresh (or join an in-progress refresh) ─────────────────────
    if (!refreshPromise) {
      // We are the first request to hit 401 — kick off the refresh
      refreshPromise = authApi
        .refresh(storedRefreshToken)
        .then((data) => ({ token: data.token, refreshToken: data.refreshToken }))
        .catch(() => null) // null = refresh failed (expired / revoked)
    }

    // All concurrent 401s await the same promise — no duplicate refresh calls
    const refreshResult = await refreshPromise
    refreshPromise = null // Reset so the NEXT expiry can refresh again

    // ── 5a. Refresh succeeded → save new tokens → retry original request ──────
    if (refreshResult) {
      api.dispatch(updateTokens(refreshResult)) // only updates token + refreshToken
      result = await baseQuery(args, api, extraOptions) // retry with new token
    } else {
      // ── 5b. Refresh failed (token expired / revoked) → force logout ──────────
      api.dispatch(logout())
    }

    return result
  }
}
