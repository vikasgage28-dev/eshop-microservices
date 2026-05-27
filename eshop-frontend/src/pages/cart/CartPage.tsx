import { useNavigate } from 'react-router-dom'
import { ShoppingCart, Trash2, Plus, Minus, ArrowLeft, ArrowRight } from 'lucide-react'
import { useCart } from '@/hooks/useCart'
import { Button } from '@/components/ui/button'
import { formatCurrency } from '@/lib/utils'

export default function CartPage() {
  const navigate = useNavigate()
  const { items, totalItems, totalAmount, removeFromCart, updateQuantity, clearCart } = useCart()

  if (items.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center py-24 gap-4">
        <div className="w-20 h-20 bg-blue-50 rounded-full flex items-center justify-center">
          <ShoppingCart size={36} className="text-blue-300" />
        </div>
        <h2 className="text-lg font-semibold text-gray-700">Your cart is empty</h2>
        <p className="text-sm text-gray-400">Browse products and add items to get started</p>
        <Button onClick={() => navigate('/products')} className="bg-blue-600 hover:bg-blue-700 mt-2">
          <ArrowLeft size={16} className="mr-2" /> Browse Products
        </Button>
      </div>
    )
  }

  return (
    <div className="space-y-5 max-w-3xl">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-xl font-bold text-gray-900">Cart</h1>
          <p className="text-sm text-gray-500 mt-0.5">{totalItems} item{totalItems !== 1 ? 's' : ''}</p>
        </div>
        <button
          onClick={() => clearCart()}
          className="text-xs text-red-500 hover:text-red-700 hover:underline"
        >
          Clear cart
        </button>
      </div>

      {/* Items */}
      <div className="bg-white rounded-xl border border-gray-100 shadow-sm divide-y divide-gray-50">
        {items.map((item) => (
          <div key={item.productId} className="flex items-center gap-4 px-5 py-4">
            {/* Icon */}
            <div className="w-12 h-12 bg-blue-50 rounded-lg flex items-center justify-center flex-shrink-0 text-xl">
              📦
            </div>

            {/* Name + price */}
            <div className="flex-1 min-w-0">
              <p className="text-sm font-semibold text-gray-900 truncate">{item.productName}</p>
              <p className="text-xs text-gray-400 mt-0.5">{formatCurrency(item.price)} each</p>
            </div>

            {/* Qty controls */}
            <div className="flex items-center border border-gray-200 rounded-lg">
              <button
                onClick={() => updateQuantity(item.productId, item.quantity - 1)}
                className="px-2.5 py-1.5 text-gray-500 hover:bg-gray-50 rounded-l-lg transition-colors"
              >
                <Minus size={12} />
              </button>
              <span className="px-3 py-1.5 text-sm font-semibold min-w-[32px] text-center">
                {item.quantity}
              </span>
              <button
                onClick={() => updateQuantity(item.productId, item.quantity + 1)}
                className="px-2.5 py-1.5 text-gray-500 hover:bg-gray-50 rounded-r-lg transition-colors"
              >
                <Plus size={12} />
              </button>
            </div>

            {/* Line total */}
            <div className="w-24 text-right">
              <p className="text-sm font-bold text-gray-900">
                {formatCurrency(item.price * item.quantity)}
              </p>
            </div>

            {/* Remove */}
            <button
              onClick={() => removeFromCart(item.productId)}
              className="p-1.5 text-gray-300 hover:text-red-500 transition-colors"
            >
              <Trash2 size={16} />
            </button>
          </div>
        ))}
      </div>

      {/* Summary */}
      <div className="bg-white rounded-xl border border-gray-100 shadow-sm p-5">
        <div className="space-y-2 mb-4">
          <div className="flex justify-between text-sm text-gray-600">
            <span>Subtotal ({totalItems} items)</span>
            <span>{formatCurrency(totalAmount)}</span>
          </div>
          <div className="flex justify-between text-sm text-gray-600">
            <span>Shipping</span>
            <span className="text-green-600 font-medium">Free</span>
          </div>
          <div className="border-t border-gray-100 pt-2 flex justify-between font-bold text-gray-900">
            <span>Total</span>
            <span className="text-blue-600 text-lg">{formatCurrency(totalAmount)}</span>
          </div>
        </div>

        <div className="flex gap-3">
          <Button
            variant="outline"
            onClick={() => navigate('/products')}
            className="flex-1"
          >
            <ArrowLeft size={16} className="mr-2" /> Continue Shopping
          </Button>
          <Button
            onClick={() => navigate('/checkout')}
            className="flex-1 bg-blue-600 hover:bg-blue-700"
          >
            Proceed to Checkout <ArrowRight size={16} className="ml-2" />
          </Button>
        </div>
      </div>
    </div>
  )
}
