import { useEffect, useRef, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { Store, Loader2, Mail, RefreshCw } from 'lucide-react'
import { useAuth } from '@/hooks/useAuth'
import { authApi } from '@/api/authClient'
import { Button } from '@/components/ui/button'

const OTP_LENGTH = 6

export default function VerifyOtpPage() {
  const navigate = useNavigate()
  const { pending2FAUserId, pending2FAEmail, isAuthenticated, isAdmin, completeLogin, clear2FAPending } = useAuth()

  const OTP_EXPIRY_SECONDS = 2 * 60   // matches backend token lifespan (2 min)
  const RESEND_COOLDOWN    = 60        // resend silently unlocks after 60s (no label shown)

  const [digits, setDigits] = useState<string[]>(Array(OTP_LENGTH).fill(''))
  const [sending, setSending] = useState(false)
  const [verifying, setVerifying] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [otpSent, setOtpSent] = useState(false)
  const [expiryCountdown, setExpiryCountdown] = useState(0)   // single countdown = OTP validity
  const [canResend, setCanResend] = useState(false)            // resend silently unlocks after 60s
  const inputRefs = useRef<(HTMLInputElement | null)[]>([])
  const hasSentOtp = useRef(false)   // guard against React Strict Mode double-invoke

  // Guard: if no pending 2FA, redirect to login
  useEffect(() => {
    if (!pending2FAUserId) navigate('/login', { replace: true })
  }, [pending2FAUserId, navigate])

  // Guard: if already authenticated, redirect
  useEffect(() => {
    if (isAuthenticated) navigate(isAdmin ? '/dashboard' : '/products', { replace: true })
  }, [isAuthenticated, isAdmin, navigate])

  // Auto-send OTP on page load — ref guard prevents React Strict Mode double-fire
  useEffect(() => {
    if (pending2FAUserId && !hasSentOtp.current) {
      hasSentOtp.current = true
      sendOtp()
    }
  }, [])

  // Single countdown — OTP validity (2 min). Resend silently unlocks at 60s mark.
  useEffect(() => {
    if (expiryCountdown <= 0) return
    const timer = setTimeout(() => {
      setExpiryCountdown((c) => {
        const next = c - 1
        if (next === OTP_EXPIRY_SECONDS - RESEND_COOLDOWN) setCanResend(true)
        return next
      })
    }, 1000)
    return () => clearTimeout(timer)
  }, [expiryCountdown])

  async function sendOtp() {
    if (!pending2FAUserId) return
    setSending(true); setError(null)
    try {
      await authApi.sendOtp(pending2FAUserId)
      setOtpSent(true)
      setExpiryCountdown(OTP_EXPIRY_SECONDS)   // start 2 min countdown
      setCanResend(false)                       // lock resend silently for 60s
      setDigits(Array(OTP_LENGTH).fill(''))
    } catch {
      setError('Failed to send code. Please try again.')
    } finally {
      setSending(false)
    }
  }

  function handleDigitChange(index: number, value: string) {
    const char = value.replace(/[^0-9]/g, '').slice(-1)
    const next = [...digits]
    next[index] = char
    setDigits(next)
    if (char && index < OTP_LENGTH - 1) inputRefs.current[index + 1]?.focus()
  }

  function handleKeyDown(index: number, e: React.KeyboardEvent) {
    if (e.key === 'Backspace' && !digits[index] && index > 0) {
      inputRefs.current[index - 1]?.focus()
    }
  }

  function handlePaste(e: React.ClipboardEvent) {
    const text = e.clipboardData.getData('text').replace(/[^0-9]/g, '').slice(0, OTP_LENGTH)
    if (text.length === OTP_LENGTH) {
      setDigits(text.split(''))
      inputRefs.current[OTP_LENGTH - 1]?.focus()
    }
    e.preventDefault()
  }

  async function handleVerify(e: React.FormEvent) {
    e.preventDefault()
    const code = digits.join('')
    if (code.length < OTP_LENGTH) { setError('Please enter the full 6-digit code.'); return }
    if (!pending2FAUserId) return
    setVerifying(true); setError(null)
    try {
      const data = await authApi.verifyOtp(pending2FAUserId, code)
      completeLogin(data)
    } catch {
      setError('Invalid or expired code. Please try again.')
      setDigits(Array(OTP_LENGTH).fill(''))
      inputRefs.current[0]?.focus()
    } finally {
      setVerifying(false)
    }
  }

  const maskedEmail = pending2FAEmail
    ? pending2FAEmail.replace(/(.{2})(.*)(@.*)/, '$1***$3')
    : ''

  const formatTime = (secs: number) => {
    const m = Math.floor(secs / 60).toString().padStart(2, '0')
    const s = (secs % 60).toString().padStart(2, '0')
    return `${m}:${s}`
  }

  const codeExpired = otpSent && expiryCountdown === 0
  const isResendDisabled = sending || (!canResend && !codeExpired)

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-blue-100 flex items-center justify-center p-4">
      <div className="w-full max-w-[480px] bg-white rounded-2xl shadow-lg p-10">
        <div className="flex flex-col items-center gap-3 mb-8">
          <div className="w-14 h-14 bg-blue-600 rounded-2xl flex items-center justify-center">
            <Store size={28} className="text-white" />
          </div>
          <h1 className="text-2xl font-bold text-gray-900 tracking-tight">Verify your identity</h1>
          {otpSent && (
            <div className="flex flex-col items-center gap-1.5">
              <div className="flex items-center gap-2 text-sm text-gray-500">
                <Mail size={15} />
                <span>Code sent to <strong>{maskedEmail}</strong></span>
              </div>
              {/* ONE countdown — OTP validity. Matches what the email says. */}
              {codeExpired
                ? <span className="text-xs font-medium text-red-600 bg-red-50 px-2.5 py-0.5 rounded-full">Code expired — request a new one</span>
                : <span className="text-xs font-medium text-amber-600 bg-amber-50 px-2.5 py-0.5 rounded-full">Expires in {formatTime(expiryCountdown)}</span>
              }
            </div>
          )}
        </div>

        {error && (
          <div className="mb-5 p-3 bg-red-50 border border-red-200 rounded-lg text-sm text-red-700">{error}</div>
        )}

        <form onSubmit={handleVerify} className="space-y-6">
          <div>
            <label className="block text-sm font-semibold text-gray-700 mb-3 text-center">
              Enter 6-digit verification code
            </label>
            <div className="flex gap-2 justify-center" onPaste={handlePaste}>
              {digits.map((d, i) => (
                <input
                  key={i}
                  ref={(el) => { inputRefs.current[i] = el }}
                  type="text"
                  inputMode="numeric"
                  maxLength={1}
                  value={d}
                  onChange={(e) => handleDigitChange(i, e.target.value)}
                  onKeyDown={(e) => handleKeyDown(i, e)}
                  className="w-11 h-14 text-center text-xl font-bold border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
                />
              ))}
            </div>
          </div>

          <Button type="submit" disabled={verifying || sending || codeExpired} className="w-full bg-blue-600 hover:bg-blue-700 h-11 text-base font-semibold disabled:opacity-50">
            {verifying ? <Loader2 size={17} className="animate-spin mr-2" /> : null}
            {verifying ? 'Verifying…' : 'Verify'}
          </Button>
        </form>

        <div className="mt-5 flex flex-col items-center gap-3">
          <button
            type="button"
            disabled={isResendDisabled}
            onClick={sendOtp}
            className="flex items-center gap-1.5 text-sm text-blue-600 hover:underline disabled:text-gray-400 disabled:no-underline disabled:cursor-not-allowed"
          >
            <RefreshCw size={14} className={sending ? 'animate-spin' : ''} />
            {sending ? 'Sending…' : "Didn't receive a code? Resend"}
          </button>
          <button
            type="button"
            onClick={() => { clear2FAPending(); navigate('/login') }}
            className="text-sm text-gray-400 hover:text-gray-600"
          >
            ← Back to login
          </button>
        </div>
      </div>
    </div>
  )
}
