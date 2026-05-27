import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import type { RootState } from '@/app/store'
import type { Order } from '@/types/ordering.types'
import { API_URLS } from '@/lib/constants'

// ── Request type — matches PlaceOrderCommand on backend ──────────────────────
export interface PlaceOrderItem {
  productId: string
  productName: string
  unitPrice: number
  quantity: number
}

export interface PlaceOrderRequest {
  customerId: string        // must be a valid GUID that exists in Customer.API
  shippingAddress?: string
  notes?: string
  items: PlaceOrderItem[]
}

export const orderingApi = createApi({
  reducerPath: 'orderingApi',
  baseQuery: fetchBaseQuery({
    baseUrl: `${API_URLS.ordering}/api`,
    prepareHeaders: (headers, { getState }) => {
      const token = (getState() as RootState).auth.token
      if (token) headers.set('Authorization', `Bearer ${token}`)
      return headers
    },
  }),
  tagTypes: ['Order'],
  endpoints: (builder) => ({
    getOrders: builder.query<Order[], void>({
      query: () => '/orders',
      providesTags: ['Order'],
    }),
    getOrderById: builder.query<Order, string>({
      query: (id) => `/orders/${id}`,
      providesTags: ['Order'],
    }),
    getOrdersByCustomer: builder.query<Order[], string>({
      query: (customerId) => `/orders/customer/${customerId}`,
      providesTags: ['Order'],
    }),
    // POST /api/orders — body matches PlaceOrderCommand
    placeOrder: builder.mutation<Order, PlaceOrderRequest>({
      query: (body) => ({ url: '/orders', method: 'POST', body }),
      invalidatesTags: ['Order'],
    }),
    // POST /api/orders/{id}/cancel — controller uses [HttpPost], not PUT!
    cancelOrder: builder.mutation<void, { id: string; reason: string }>({
      query: ({ id, reason }) => ({
        url: `/orders/${id}/cancel`,
        method: 'POST',
        body: { reason },
      }),
      invalidatesTags: ['Order'],
    }),
  }),
})

export const {
  useGetOrdersQuery,
  useGetOrderByIdQuery,
  useGetOrdersByCustomerQuery,
  usePlaceOrderMutation,
  useCancelOrderMutation,
} = orderingApi
