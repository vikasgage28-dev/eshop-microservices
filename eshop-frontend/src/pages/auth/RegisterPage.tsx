import { useEffect, useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { Store, Loader2 } from 'lucide-react'
import { useAuth } from '@/hooks/useAuth'
import { Button } from '@/components/ui/button'

export default function RegisterPage() {
  const navigate = useNavigate()
  const { register, isLoading, error, isAuthenticated, clearError } = useAuth()

  const [form, setForm] = useState({ firstName: '', lastName: '', email: '', password: '', confirm: '' })
  const [validationError, setValidationError] = useState('')

  useEffect(() => {
    if (isAuthenticated) navigate('/dashboard', { replace: true })
  }, [isAuthenticated, navigate])

  useEffect(() => { return () => { clearError() } }, [])

  const set = (field: string) => (e: React.ChangeEvent<HTMLInputElement>) =>
    setForm((prev) => ({ ...prev, [field]: e.target.value }))

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setValidationError('')
    if (form.password !== form.confirm) {
      setValidationError('Passwords do not match')
      return
    }
    if (form.password.length < 8) {
      setValidationError('Password must be at least 8 characters')
      return
    }
    const { confirm, ...payload } = form
    await register(payload)
  }

  const displayError = validationError || error

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-blue-100 flex items-center justify-center p-4">
      <div className="w-full max-w-[700px] bg-white rounded-2xl shadow-lg p-14">
        {/* Logo */}
        <div className="flex flex-col items-center gap-3 mb-7">
          <div className="w-14 h-14 bg-blue-600 rounded-2xl flex items-center justify-center">
            <Store size={28} className="text-white" />
          </div>
          <h1 className="text-2xl font-bold text-gray-900 tracking-tight">Create account</h1>
        </div>

        {displayError && (
          <div className="mb-5 p-3 bg-red-50 border border-red-200 rounded-lg text-sm text-red-700">
            {displayError}
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="flex gap-3">
            <div className="flex-1">
              <label className="block text-sm font-semibold text-gray-700 mb-1.5">First name</label>
              <input
                value={form.firstName} onChange={set('firstName')} required
                placeholder="John"
                className="w-full px-4 py-2.5 border border-gray-200 rounded-lg text-base text-gray-900 bg-white placeholder:text-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
            </div>
            <div className="flex-1">
              <label className="block text-sm font-semibold text-gray-700 mb-1.5">Last name</label>
              <input
                value={form.lastName} onChange={set('lastName')} required
                placeholder="Smith"
                className="w-full px-4 py-2.5 border border-gray-200 rounded-lg text-base text-gray-900 bg-white placeholder:text-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
            </div>
          </div>

          <div>
            <label className="block text-sm font-semibold text-gray-700 mb-1.5">Email</label>
            <input
              type="email" value={form.email} onChange={set('email')} required
              placeholder="you@example.com"
              className="w-full px-4 py-2.5 border border-gray-200 rounded-lg text-base text-gray-900 bg-white placeholder:text-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>

          <div>
            <label className="block text-sm font-semibold text-gray-700 mb-1.5">Password</label>
            <input
              type="password" value={form.password} onChange={set('password')} required
              placeholder="Min. 8 characters"
              className="w-full px-4 py-2.5 border border-gray-200 rounded-lg text-base text-gray-900 bg-white placeholder:text-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>

          <div>
            <label className="block text-sm font-semibold text-gray-700 mb-1.5">Confirm password</label>
            <input
              type="password" value={form.confirm} onChange={set('confirm')} required
              placeholder="Re-enter your password"
              className="w-full px-4 py-2.5 border border-gray-200 rounded-lg text-base text-gray-900 bg-white placeholder:text-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>

          <Button
            type="submit"
            disabled={isLoading}
            className="w-full bg-blue-600 hover:bg-blue-700 h-11 text-base font-semibold mt-1"
          >
            {isLoading ? <Loader2 size={17} className="animate-spin mr-2" /> : null}
            {isLoading ? 'Creating account…' : 'Create Account'}
          </Button>
        </form>

        <p className="mt-6 text-center text-sm text-gray-500">
          Already have an account?{' '}
          <Link to="/login" className="text-blue-600 font-semibold hover:underline">Sign in</Link>
        </p>
      </div>
    </div>
  )
}
