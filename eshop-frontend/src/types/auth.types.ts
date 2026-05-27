export interface AuthUser {
  userId: string
  email: string
  fullName: string
  roles: string[]
  token: string
  refreshToken: string
}

export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  firstName: string
  lastName: string
  email: string
  password: string
  role?: string
}