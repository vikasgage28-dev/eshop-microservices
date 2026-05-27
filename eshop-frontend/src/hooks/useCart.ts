import { useAppDispatch, useAppSelector } from '@/app/hooks'
import { addToCart, removeFromCart, updateQuantity, clearCart, type CartItem } from '@/features/cart/cartSlice'

export function useCart() {
  const dispatch = useAppDispatch()
  const items = useAppSelector((s) => s.cart.items)

  const totalItems = items.reduce((sum, i) => sum + i.quantity, 0)
  const totalAmount = items.reduce((sum, i) => sum + i.price * i.quantity, 0)

  return {
    items,
    totalItems,
    totalAmount,
    addToCart: (item: CartItem) => dispatch(addToCart(item)),
    removeFromCart: (productId: string) => dispatch(removeFromCart(productId)),
    updateQuantity: (productId: string, quantity: number) =>
      dispatch(updateQuantity({ productId, quantity })),
    clearCart: () => dispatch(clearCart()),
  }
}
