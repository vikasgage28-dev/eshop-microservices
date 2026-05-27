import { createSlice, type PayloadAction } from '@reduxjs/toolkit'

export interface CartItem {
  productId: string
  productName: string
  price: number
  quantity: number
  imageUrl?: string
}

interface CartState {
  items: CartItem[]
}

const stored = localStorage.getItem('cart')
const initialState: CartState = stored ? JSON.parse(stored) : { items: [] }

const persist = (items: CartItem[]) => localStorage.setItem('cart', JSON.stringify({ items }))

const cartSlice = createSlice({
  name: 'cart',
  initialState,
  reducers: {
    addToCart: (state, action: PayloadAction<CartItem>) => {
      const existing = state.items.find((i) => i.productId === action.payload.productId)
      if (existing) {
        existing.quantity += action.payload.quantity
      } else {
        state.items.push(action.payload)
      }
      persist(state.items)
    },
    removeFromCart: (state, action: PayloadAction<string>) => {
      state.items = state.items.filter((i) => i.productId !== action.payload)
      persist(state.items)
    },
    updateQuantity: (state, action: PayloadAction<{ productId: string; quantity: number }>) => {
      const item = state.items.find((i) => i.productId === action.payload.productId)
      if (item) {
        item.quantity = action.payload.quantity
        if (item.quantity <= 0) {
          state.items = state.items.filter((i) => i.productId !== action.payload.productId)
        }
      }
      persist(state.items)
    },
    clearCart: (state) => {
      state.items = []
      localStorage.removeItem('cart')
    },
  },
})

export const { addToCart, removeFromCart, updateQuantity, clearCart } = cartSlice.actions
export default cartSlice.reducer
