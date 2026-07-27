import { createApi } from '@reduxjs/toolkit/query/react'
import type { Product, Category, Review, PagedResult } from '@/types/catalog.types'
import { API_URLS } from '@/lib/constants'
import { createBaseQueryWithReauth } from './baseQueryWithReauth'

export const catalogApi = createApi({
  reducerPath: 'catalogApi',
  baseQuery: createBaseQueryWithReauth(`${API_URLS.catalog}/api`),
  tagTypes: ['Product', 'Category', 'Review'],
  endpoints: (builder) => ({
    getProducts: builder.query<PagedResult<Product>, { page?: number; pageSize?: number; search?: string; categoryId?: string }>({
      query: (params) => ({ url: '/products', params }),
      providesTags: ['Product'],
    }),
    getProductById: builder.query<Product, string>({
      query: (id) => `/products/${id}`,
      providesTags: ['Product'],
    }),
    createProduct: builder.mutation<Product, Partial<Product>>({
      query: (body) => ({ url: '/products', method: 'POST', body }),
      invalidatesTags: ['Product'],
    }),
    updateProduct: builder.mutation<Product, Partial<Product> & { id: string }>({
      query: ({ id, ...body }) => ({ url: `/products/${id}`, method: 'PUT', body }),
      invalidatesTags: ['Product'],
    }),
    deleteProduct: builder.mutation<void, string>({
      query: (id) => ({ url: `/products/${id}`, method: 'DELETE' }),
      invalidatesTags: ['Product'],
    }),
    getCategories: builder.query<Category[], void>({
      query: () => '/categories',
      providesTags: ['Category'],
    }),
    getReviewsByProduct: builder.query<Review[], string>({
      query: (productId) => `/reviews?productId=${productId}`,
      providesTags: ['Review'],
    }),
    createReview: builder.mutation<Review, { productId: string; userId: string; userEmail: string; rating: number; comment: string; verifiedPurchase: boolean }>({
      query: (body) => ({ url: '/reviews', method: 'POST', body }),
      invalidatesTags: ['Review'],
    }),
  }),
})

export const {
  useGetProductsQuery,
  useGetProductByIdQuery,
  useCreateProductMutation,
  useUpdateProductMutation,
  useDeleteProductMutation,
  useGetCategoriesQuery,
  useGetReviewsByProductQuery,
  useCreateReviewMutation,
} = catalogApi