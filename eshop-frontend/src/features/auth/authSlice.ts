import { createSlice, createAsyncThunk, type PayloadAction } from '@reduxjs/toolkit'
import { authApi, type LoginPayload, type RegisterPayload, type AuthResponse } from '@/api/authClient'

interface AuthState {
  token: string | null
  refreshToken: string | null
  userId: string | null
  email: string | null
  fullName: string | null
  roles: string[]
  isLoading: boolean
  error: string | null
  // 2FA — temporary state while OTP is pending
  requires2FA: boolean
  pending2FAUserId: string | null
  pending2FAEmail: string | null
}

const stored = localStorage.getItem('auth')
const initial: AuthState = stored
  ? { ...JSON.parse(stored), isLoading: false, error: null, requires2FA: false, pending2FAUserId: null, pending2FAEmail: null }
  : {
      token: null,
      refreshToken: null,
      userId: null,
      email: null,
      fullName: null,
      roles: [],
      isLoading: false,
      error: null,
      requires2FA: false,
      pending2FAUserId: null,
      pending2FAEmail: null,
    }

export const login = createAsyncThunk('auth/login', async (payload: LoginPayload, { rejectWithValue }) => {
  try {
    return await authApi.login(payload)
  } catch (err: unknown) {
    const error = err as { response?: { data?: { message?: string } } }
    return rejectWithValue(error.response?.data?.message ?? 'Login failed')
  }
})

export const register = createAsyncThunk('auth/register', async (payload: RegisterPayload, { rejectWithValue }) => {
  try {
    return await authApi.register(payload)
  } catch (err: unknown) {
    const error = err as { response?: { data?: { message?: string } } }
    return rejectWithValue(error.response?.data?.message ?? 'Registration failed')
  }
})

const persist = (state: AuthState) => {
  const { isLoading, error, ...data } = state
  localStorage.setItem('auth', JSON.stringify(data))
}

const authSlice = createSlice({
  name: 'auth',
  initialState: initial,
  reducers: {
    logout: (state) => {
      state.token = null
      state.refreshToken = null
      state.userId = null
      state.email = null
      state.fullName = null
      state.roles = []
      state.error = null
      state.requires2FA = false
      state.pending2FAUserId = null
      state.pending2FAEmail = null
      localStorage.removeItem('auth')
    },
    clearError: (state) => {
      state.error = null
    },
    setCredentials: (state, action: PayloadAction<AuthResponse>) => {
      const { token, refreshToken, userId, email, fullName, roles } = action.payload
      state.token = token
      state.refreshToken = refreshToken
      state.userId = userId
      state.email = email
      state.fullName = fullName
      state.roles = roles
      state.requires2FA = false
      state.pending2FAUserId = null
      state.pending2FAEmail = null
      persist(state)
    },
    clear2FAPending: (state) => {
      state.requires2FA = false
      state.pending2FAUserId = null
      state.pending2FAEmail = null
    },
    // Used by silent token refresh — only updates tokens, preserves userId/email/fullName/roles
    updateTokens: (state, action: PayloadAction<{ token: string; refreshToken: string }>) => {
      state.token        = action.payload.token
      state.refreshToken = action.payload.refreshToken
      persist(state)
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(login.pending, (state) => { state.isLoading = true; state.error = null })
      .addCase(login.fulfilled, (state, action) => {
        state.isLoading = false
        const { token, refreshToken, userId, email, fullName, roles, requires2FA } = action.payload
        if (requires2FA) {
          // Don't store token — wait for OTP verification
          state.requires2FA = true
          state.pending2FAUserId = userId ?? null
          state.pending2FAEmail = email ?? null
        } else {
          state.token = token; state.refreshToken = refreshToken
          state.userId = userId; state.email = email
          state.fullName = fullName; state.roles = roles
          persist(state)
        }
      })
      .addCase(login.rejected, (state, action) => {
        state.isLoading = false
        state.error = action.payload as string
      })
      .addCase(register.pending, (state) => { state.isLoading = true; state.error = null })
      .addCase(register.fulfilled, (state, action) => {
        state.isLoading = false
        const { token, refreshToken, userId, email, fullName, roles } = action.payload
        state.token = token; state.refreshToken = refreshToken
        state.userId = userId; state.email = email
        state.fullName = fullName; state.roles = roles
        persist(state)
      })
      .addCase(register.rejected, (state, action) => {
        state.isLoading = false
        state.error = action.payload as string
      })
  },
})

export const { logout, clearError, setCredentials, updateTokens, clear2FAPending } = authSlice.actions
export default authSlice.reducer
