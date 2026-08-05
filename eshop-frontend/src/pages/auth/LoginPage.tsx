import { useEffect, useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { Store, Eye, EyeOff, Loader2 } from 'lucide-react'
import { useAuth } from '@/hooks/useAuth'
import { Button } from '@/components/ui/button'
import { useAuth0 } from '@auth0/auth0-react'

// Auth0 is only available when all three env vars are present (local dev with .env.local).
// In production (Azure SWA) these are not set — social login is not yet configured.
const auth0Enabled = !!(
  import.meta.env.VITE_AUTH0_DOMAIN &&
  import.meta.env.VITE_AUTH0_CLIENT_ID &&
  import.meta.env.VITE_AUTH0_CALLBACK_URL
)

// Isolated component so useAuth0() is only called when Auth0Provider is in the tree.
function Auth0LoginButtons() {
  const { loginWithRedirect } = useAuth0()
  return (
    <div className="flex flex-col gap-3">
      <Button
        variant="outline"
        className="w-full flex items-center justify-center gap-3 px-4 py-2.5 border border-gray-200 rounded-lg text-sm font-semibold text-gray-700 hover:bg-gray-50 transition"
        onClick={() => loginWithRedirect({ authorizationParams: { connection: 'google-oauth2', prompt: 'login' } })}
      >
        <img src="https://www.google.com/favicon.ico" className="w-5 h-5" />
        Continue with Google
      </Button>

      <Button
        variant="outline"
        className="w-full flex items-center justify-center gap-3 px-4 py-2.5 border border-gray-200 rounded-lg text-sm font-semibold text-gray-700 hover:bg-gray-50 transition"
        onClick={() => loginWithRedirect({ authorizationParams: { connection: 'github', prompt: 'login' } })}
      >
        <img src="https://github.com/favicon.ico" className="w-5 h-5" />
        Continue with GitHub
      </Button>
    </div>
  )
}

export default function LoginPage() {
  const navigate = useNavigate()
  const { login, isLoading, error, isAuthenticated, isAdmin, requires2FA, clearError } = useAuth()

  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [showPw, setShowPw] = useState(false)

  useEffect(() => {
    if (isAuthenticated) navigate(isAdmin ? '/dashboard' : '/products', { replace: true })
  }, [isAuthenticated, isAdmin, navigate])

  useEffect(() => {
    // Redirect to OTP page when 2FA is required
    if (requires2FA) navigate('/verify-otp', { replace: true })
  }, [requires2FA, navigate])

  useEffect(() => {
    return () => { clearError() }
  }, [])

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    await login({ email, password })
  }

  return (
    /* reset to 16px base for auth pages — override the 13px app shell */
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-blue-100 flex items-center justify-center p-4">
      <div className="w-full max-w-[700px] bg-white rounded-2xl shadow-lg p-14">
        {/* Logo */}
        <div className="flex flex-col items-center gap-3 mb-8">
          <div className="w-14 h-14 bg-blue-600 rounded-2xl flex items-center justify-center">
            <Store size={28} className="text-white" />
          </div>
          <h1 className="text-2xl font-bold text-gray-900 tracking-tight">Sign in to eShop</h1>
          <p className="text-sm text-gray-500">Enter your credentials to continue</p>
        </div>

        {/* Error */}
        {error && (
          <div className="mb-5 p-3 bg-red-50 border border-red-200 rounded-lg text-sm text-red-700">
            {error}
          </div>
        )}

        {/* Form */}
        <form onSubmit={handleSubmit} className="space-y-5">
          <div>
            <label className="block text-sm font-semibold text-gray-700 mb-1.5">Email</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="you@example.com"
              required
              className="w-full px-4 py-2.5 border border-gray-200 rounded-lg text-base text-gray-900 bg-white placeholder:text-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
            />
          </div>

          <div>
            <label className="block text-sm font-semibold text-gray-700 mb-1.5">Password</label>
            <div className="relative">
              <input
                type={showPw ? 'text' : 'password'}
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="••••••••"
                required
                className="w-full px-4 py-2.5 pr-11 border border-gray-200 rounded-lg text-base text-gray-900 bg-white placeholder:text-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
              />
              <button
                type="button"
                onClick={() => setShowPw((v) => !v)}
                className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-600"
              >
                {showPw ? <EyeOff size={18} /> : <Eye size={18} />}
              </button>
            </div>
          </div>

          <Button
            type="submit"
            disabled={isLoading}
            className="w-full bg-blue-600 hover:bg-blue-700 h-11 text-base font-semibold mt-1"
          >
            {isLoading ? <Loader2 size={17} className="animate-spin mr-2" /> : null}
            {isLoading ? 'Signing in…' : 'Sign In'}
          </Button>
        </form>
        {/* Social Login — only rendered when Auth0Provider is available (local dev) */}
        {auth0Enabled && (
          <>
            <div className="flex items-center gap-3 my-6">
              <div className="flex-1 h-px bg-gray-200" />
              <span className="text-sm text-gray-400">or</span>
              <div className="flex-1 h-px bg-gray-200" />
            </div>
            <Auth0LoginButtons />
          </>
        )}

        <p className="mt-6 text-center text-sm text-gray-500">
          No account?{' '}
          <Link to="/register" className="text-blue-600 font-semibold hover:underline">Register</Link>
        </p>
      </div>
    </div>
  )
}
