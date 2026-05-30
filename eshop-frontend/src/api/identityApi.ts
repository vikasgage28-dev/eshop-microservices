import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import type { RootState } from '@/app/store'
import { API_URLS } from '@/lib/constants'

export interface SiteUser {
  userId: string
  email: string
  fullName: string
  createdAt: string
}

export const identityApi = createApi({
  reducerPath: 'identityApi',
  baseQuery: fetchBaseQuery({
    baseUrl: `${API_URLS.identity}/api/auth`,
    prepareHeaders: (headers, { getState }) => {
      const token = (getState() as RootState).auth.token
      if (token) headers.set('Authorization', `Bearer ${token}`)
      return headers
    },
  }),
  endpoints: (builder) => ({
    getSiteUsers: builder.query<SiteUser[], void>({
      query: () => '/users',
    }),
  }),
})

export const { useGetSiteUsersQuery } = identityApi
