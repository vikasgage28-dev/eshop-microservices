import { useEffect, useRef } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth0 } from '@auth0/auth0-react'
import { useAppDispatch } from '@/app/hooks'
import { setCredentials } from '@/features/auth/authSlice'
import { authApi } from '@/api/authClient'
import { Loader2 } from 'lucide-react'

/**
 * Auth0 redirects here after the user logs in.
 *
 * What happens here (OAuth 2.0 + PKCE token exchange):
 *   1. @auth0/auth0-react automatically exchanges the ?code= in the URL
 *      for an Auth0 access token (PKCE verifier sent server-side)
 *   2. We call getAccessTokenSilently() to get that access token
 *   3. We POST it to our Identity.API /social-login endpoint
 *   4. Identity.API validates it with Auth0's /userinfo, finds/creates the user,
 *      and issues OUR OWN JWT + refresh token
 *   5. We store our JWT in Redux — ProtectedRoute now works normally
 *   6. Navigate to /products — same as a regular login ✅
 */
export default function Auth0CallbackPage() {
  const { isAuthenticated, isLoading, error, getAccessTokenSilently } = useAuth0()
  const navigate   = useNavigate()
  const dispatch   = useAppDispatch()
  const hasRun     = useRef(false)   // prevents double-call in React Strict Mode

  useEffect(() => {
    if (isLoading || !isAuthenticated || hasRun.current) return
    hasRun.current = true

    const exchangeToken = async () => {
      try {
        // Get the Auth0 access token (PKCE exchange already done by the SDK)
        const accessToken = await getAccessTokenSilently()

        // Exchange Auth0 token → our own JWT via Identity.API
        const authResponse = await authApi.socialLogin({ provider: 'auth0', accessToken })

        // Store in Redux exactly like a regular login — ProtectedRoute works ✅
        dispatch(setCredentials(authResponse))

        navigate('/products', { replace: true })
      } catch (err) {
        console.error('Social login token exchange failed:', err)
        navigate('/login', { replace: true })
      }
    }

    exchangeToken()
  }, [isLoading, isAuthenticated, getAccessTokenSilently, dispatch, navigate])

  if (error) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center space-y-2">
          <p className="text-red-600 font-semibold">Login failed</p>
          <p className="text-sm text-gray-500">{error.message}</p>
        </div>
      </div>
    )
  }

  return (
    <div className="min-h-screen flex items-center justify-center">
      <div className="text-center space-y-3">
        <Loader2 size={36} className="animate-spin text-blue-600 mx-auto" />
        <p className="text-gray-600 font-medium">Completing sign in…</p>
        <p className="text-xs text-gray-400">Verifying with Auth0…</p>
      </div>
    </div>
  )
}