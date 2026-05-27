import axios from 'axios'
import { API_URLS } from '@/lib/constants'

// Dedicated Axios instance for auth calls (no JWT needed — these are login/register)
export const authClient = axios.create({
  baseURL: `${API_URLS.identity}/api/auth`,
  headers: { 'Content-Type': 'application/json' },
})

export interface LoginPayload {
  email: string
  password: string
}

export interface RegisterPayload {
  firstName: string
  lastName: string
  email: string
  password: string
  role?: string
}

export interface AuthResponse {
  userId: string
  email: string
  fullName: string
  roles: string[]
  token: string
  refreshToken: string
}

export const authApi = {
  login: (payload: LoginPayload) =>
    authClient.post<AuthResponse>('/login', payload).then((r) => r.data),

  register: (payload: RegisterPayload) =>
    authClient.post<AuthResponse>('/register', payload).then((r) => r.data),

  refresh: (refreshToken: string) =>
    authClient.post<AuthResponse>('/refresh', { refreshToken }).then((r) => r.data),
}
