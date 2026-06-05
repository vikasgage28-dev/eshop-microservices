import { useAppDispatch, useAppSelector } from '@/app/hooks'
import { login, register, logout, clearError, setCredentials, clear2FAPending } from '@/features/auth/authSlice'
import { authApi, type LoginPayload, type RegisterPayload, type AuthResponse } from '@/api/authClient'

export function useAuth() {
  const dispatch = useAppDispatch()
  const { token, userId, email, fullName, roles, isLoading, error, requires2FA, pending2FAUserId, pending2FAEmail } = useAppSelector((s) => s.auth)

  const isAuthenticated = !!token
  const isAdmin = roles.includes('Admin')

  return {
    token,
    userId,
    email,
    fullName,
    roles,
    isLoading,
    error,
    isAuthenticated,
    isAdmin,
    requires2FA,
    pending2FAUserId,
    pending2FAEmail,
    login: (payload: LoginPayload) => dispatch(login(payload)),
    register: (payload: RegisterPayload) => dispatch(register(payload)),
    logout: () => dispatch(logout()),
    clearError: () => dispatch(clearError()),
    completeLogin: (data: AuthResponse) => dispatch(setCredentials(data)),
    clear2FAPending: () => dispatch(clear2FAPending()),
  }
}
