import { createApi } from '@reduxjs/toolkit/query/react'
import type { Address, Customer } from '@/types/customer.types'
import { API_URLS } from '@/lib/constants'
import { createBaseQueryWithReauth } from './baseQueryWithReauth'

export const customerApi = createApi({
  reducerPath: 'customerApi',
  baseQuery: createBaseQueryWithReauth(`${API_URLS.customer}/api`),
  tagTypes: ['Customer'],
  endpoints: (builder) => ({
    getCustomers: builder.query<Customer[], void>({
      query: () => '/customers',
      providesTags: ['Customer'],
    }),
    getCustomerById: builder.query<Customer, string>({
      query: (id) => `/customers/${id}`,
      providesTags: ['Customer'],
    }),
    // GET /api/customers/email/{email} — used by checkout to resolve customerId
    getCustomerByEmail: builder.query<Customer, string>({
      query: (email) => `/customers/email/${encodeURIComponent(email)}`,
      providesTags: ['Customer'],
    }),
    createCustomer: builder.mutation<Customer, Partial<Customer>>({
      query: (body) => ({ url: '/customers', method: 'POST', body }),
      invalidatesTags: ['Customer'],
    }),
    updateCustomer: builder.mutation<Customer, Partial<Customer> & { id: string }>({
      query: ({ id, ...body }) => ({ url: `/customers/${id}`, method: 'PUT', body }),
      invalidatesTags: ['Customer'],
    }),
    deleteCustomer: builder.mutation<void, string>({
      query: (id) => ({ url: `/customers/${id}`, method: 'DELETE' }),
      invalidatesTags: ['Customer'],
    }),
    addAddress: builder.mutation<Address, { customerId: string; address: Omit<Address, 'id'> }>({
      query: ({ customerId, address }) => ({
        url: `/customers/${customerId}/addresses`,
        method: 'POST',
        body: address,
      }),
      invalidatesTags: ['Customer'],
    }),
    deleteAddress: builder.mutation<void, { customerId: string; addressId: string }>({
      query: ({ customerId, addressId }) => ({
        url: `/customers/${customerId}/addresses/${addressId}`,
        method: 'DELETE',
      }),
      invalidatesTags: ['Customer'],
    }),
  }),
})

export const {
  useGetCustomersQuery,
  useGetCustomerByIdQuery,
  useGetCustomerByEmailQuery,
  useCreateCustomerMutation,
  useUpdateCustomerMutation,
  useDeleteCustomerMutation,
  useAddAddressMutation,
  useDeleteAddressMutation,
} = customerApi
