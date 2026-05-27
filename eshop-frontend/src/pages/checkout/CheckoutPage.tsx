import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { Loader2, CheckCircle2, AlertCircle, MapPin, ShoppingBag } from 'lucide-react'
import { useCart } from '@/hooks/useCart'
import { useAuth } from '@/hooks/useAuth'
import { Button } from '@/components/ui/button'
import { formatCurrency } from '@/lib/utils'
import { useGetCustomerByEmailQuery } from '@/api/customerApi'
import { usePlaceOrderMutation } from '@/api/orderingApi'

export default function CheckoutPage() {
  const navigate = useNavigate()
  const { items, totalAmount, clearCart } = useCart()
  const { email } = useAuth()

  const [shippingAddress, setShippingAddress] = useState('')
  const [notes, setNotes] = useState('')
  const [success, setSuccess] = useState(false)

  // Step 1: Look up customer record by logged-in user email
  const {
    data: customer,
    isLoading: loadingCustomer,
    error: customerError,
  } = useGetCustomerByEmailQuery(email ?? '', { skip: !email })

  // Step 2: Place order mutation
  const [placeOrder, { isLoading: placing, error: orderError }] = usePlaceOrderMutation()

  // Guard: empty cart
  if (items.length === 0 && !success) {
    navigate('/cart')
    return null
  }

  const handlePlaceOrder = async () => {
    if (!customer) return
    if (!shippingAddress.trim()) {
      alert('Please enter a shipping address.')
      return
    }

    try {
      await placeOrder({
        customerId: customer.id,
        shippingAddress: shippingAddress.trim(),
        notes: notes.trim() || undefined,
        items: items.map((i) => ({
          productId: i.productId,
          productName: i.productName,
          unitPrice: i.price,
          quantity: i.quantity,
        })),
      }).unwrap()

      // Success! Clear cart and show confirmation
      clearCart()
      setSuccess(true)
    } catch {
      // error shown from orderError below
    }
  }

  // ── Success screen ─────────────────────────────────────────────────────────
  if (success) {
    return (
      <div className="flex flex-col items-center justify-center py-24 gap-4 max-w-md mx-auto text-center">
        <div className="w-20 h-20 bg-green-50 rounded-full flex items-center justify-center">
          <CheckCircle2 size={40} className="text-green-500" />
        </div>
        <h2 className="text-xl font-bold text-gray-900">Order Placed!</h2>
        <p className="text-sm text-gray-500">
          Your order has been received and is being processed. You'll find it in your Orders page.
        </p>
        <div className="flex gap-3 mt-2">
          <Button variant="outline" onClick={() => navigate('/products')}>Continue Shopping</Button>
          <Button className="bg-blue-600 hover:bg-blue-700" onClick={() => navigate('/orders')}>
            View My Orders
          </Button>
        </div>
      </div>
    )
  }

  // ── Error: no customer profile ─────────────────────────────────────────────
  const noProfile = !loadingCustomer && (customerError || !customer)

  return (
    <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 max-w-5xl">
      {/* Left — form */}
      <div className="lg:col-span-2 space-y-5">
        <div>
          <h1 className="text-xl font-bold text-gray-900">Checkout</h1>
          <p className="text-sm text-gray-500 mt-0.5">Review your order and enter shipping details</p>
        </div>

        {/* Customer profile warning */}
        {noProfile && (
          <div className="flex gap-3 p-4 bg-red-50 border border-red-200 rounded-xl text-sm text-red-700">
            <AlertCircle size={18} className="flex-shrink-0 mt-0.5" />
            <div>
              <p className="font-semibold">No customer profile found for {email}</p>
              <p className="text-xs mt-1 text-red-500">
                Ask an admin to create a customer profile, or contact support.
              </p>
            </div>
          </div>
        )}

        {/* Shipping address */}
        <div className="bg-white rounded-xl border border-gray-100 shadow-sm p-5 space-y-4">
          <div className="flex items-center gap-2 text-sm font-semibold text-gray-700">
            <MapPin size={16} className="text-blue-500" /> Shipping Address
          </div>
          <textarea
            rows={3}
            value={shippingAddress}
            onChange={(e) => setShippingAddress(e.target.value)}
            placeholder="e.g. 123 MG Road, Mumbai, Maharashtra, India — 400001"
            className="w-full px-3 py-2 border border-gray-200 rounded-lg text-sm resize-none focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          <div>
            <label className="block text-xs font-semibold text-gray-500 mb-1">Notes (optional)</label>
            <input
              type="text"
              value={notes}
              onChange={(e) => setNotes(e.target.value)}
              placeholder="Leave at door, call on arrival…"
              className="w-full px-3 py-2 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>
        </div>

        {/* Order error */}
        {orderError && (
          <div className="flex gap-2 p-3 bg-red-50 border border-red-200 rounded-lg text-sm text-red-700">
            <AlertCircle size={16} className="flex-shrink-0 mt-0.5" />
            <span>Failed to place order. Please try again.</span>
          </div>
        )}

        <Button
          className="w-full bg-blue-600 hover:bg-blue-700 h-11"
          disabled={placing || loadingCustomer || noProfile || !shippingAddress.trim()}
          onClick={handlePlaceOrder}
        >
          {placing
            ? <><Loader2 size={16} className="animate-spin mr-2" /> Placing Order…</>
            : <><ShoppingBag size={16} className="mr-2" /> Place Order — {formatCurrency(totalAmount)}</>}
        </Button>
      </div>

      {/* Right — order summary */}
      <div className="space-y-4">
        <div className="bg-white rounded-xl border border-gray-100 shadow-sm p-5">
          <h2 className="text-sm font-semibold text-gray-900 mb-4">Order Summary</h2>
          <div className="space-y-3">
            {items.map((item) => (
              <div key={item.productId} className="flex justify-between text-sm">
                <div className="flex-1 min-w-0">
                  <p className="text-gray-800 truncate">{item.productName}</p>
                  <p className="text-xs text-gray-400">Qty {item.quantity}</p>
                </div>
                <span className="font-medium text-gray-900 ml-2">
                  {formatCurrency(item.price * item.quantity)}
                </span>
              </div>
            ))}
          </div>
          <div className="border-t border-gray-100 mt-4 pt-3 space-y-1.5 text-sm">
            <div className="flex justify-between text-gray-500">
              <span>Shipping</span><span className="text-green-600 font-medium">Free</span>
            </div>
            <div className="flex justify-between font-bold text-gray-900">
              <span>Total</span>
              <span className="text-blue-600">{formatCurrency(totalAmount)}</span>
            </div>
          </div>
        </div>

        {/* Customer info */}
        {customer && (
          <div className="bg-blue-50 rounded-xl p-4 text-xs text-blue-700 space-y-1">
            <p className="font-semibold">Ordering as</p>
            <p>{customer.firstName} {customer.lastName}</p>
            <p className="text-blue-500">{customer.email}</p>
          </div>
        )}
        {loadingCustomer && (
          <div className="flex items-center gap-2 text-xs text-gray-400 p-3">
            <Loader2 size={12} className="animate-spin" /> Loading profile…
          </div>
        )}
      </div>
    </div>
  )
}
