import { createApi } from '@reduxjs/toolkit/query/react'
import { API_URLS } from '@/lib/constants'
import { createBaseQueryWithReauth } from './baseQueryWithReauth'

export interface SiteUser {
  userId: string
  email: string
  fullName: string
  createdAt: string
}

export const identityApi = createApi({
  reducerPath: 'identityApi',
  baseQuery: createBaseQueryWithReauth(`${API_URLS.identity}/api/auth`),
  endpoints: (builder) => ({
    getSiteUsers: builder.query<SiteUser[], void>({
      query: () => '/users',
    }),
  }),
})

export const { useGetSiteUsersQuery } = identityApi
