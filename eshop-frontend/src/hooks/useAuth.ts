import { useAppDispatch, useAppSelector } from '@/app/hooks'
import { login, register, logout, clearError } from '@/features/auth/authSlice'
import type { LoginPayload, RegisterPayload } from '@/api/authClient'

export function useAuth() {
  const dispatch = useAppDispatch()
  const { token, userId, email, fullName, roles, isLoading, error } = useAppSelector((s) => s.auth)

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
    login: (payload: LoginPayload) => dispatch(login(payload)),
    register: (payload: RegisterPayload) => dispatch(register(payload)),
    logout: () => dispatch(logout()),
    clearError: () => dispatch(clearError()),
  }
}
