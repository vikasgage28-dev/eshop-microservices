import { configureStore } from '@reduxjs/toolkit'
import { catalogApi } from '@/api/catalogApi'
import { orderingApi } from '@/api/orderingApi'
import { customerApi } from '@/api/customerApi'
import { identityApi } from '@/api/identityApi'
import authReducer from '@/features/auth/authSlice'
import cartReducer from '@/features/cart/cartSlice'

export const store = configureStore({
  reducer: {
    auth: authReducer,
    cart: cartReducer,
    [catalogApi.reducerPath]: catalogApi.reducer,
    [orderingApi.reducerPath]: orderingApi.reducer,
    [customerApi.reducerPath]: customerApi.reducer,
    [identityApi.reducerPath]: identityApi.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware()
      .concat(catalogApi.middleware)
      .concat(orderingApi.middleware)
      .concat(customerApi.middleware)
      .concat(identityApi.middleware),
})

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch