import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { Loader2, CheckCircle2, AlertCircle, MapPin, ShoppingBag, UserPlus, ShieldAlert, Plus, CheckCircle } from 'lucide-react'
import { useCart } from '@/hooks/useCart'
import { useAuth } from '@/hooks/useAuth'
import { Button } from '@/components/ui/button'
import { formatCurrency } from '@/lib/utils'
import { useGetCustomerByEmailQuery, useCreateCustomerMutation, useAddAddressMutation } from '@/api/customerApi'
import { usePlaceOrderMutation } from '@/api/orderingApi'
import type { Address } from '@/types/customer.types'

const EMPTY_ADDR = { street: '', city: '', state: '', country: 'India', postalCode: '', isDefault: false }

function addrLabel(a: Address) {
  return `${a.street}, ${a.city}, ${a.state} — ${a.postalCode}`
}

export default function CheckoutPage() {
  const navigate = useNavigate()
  const { items, totalAmount, clearCart } = useCart()
  const { email, isAdmin, fullName } = useAuth()

  const [selectedAddressId, setSelectedAddressId] = useState<string | 'new' | null>(null)
  const [newAddr, setNewAddr]                       = useState<Omit<Address, 'id'>>(EMPTY_ADDR)
  const [saveNewAddr, setSaveNewAddr]               = useState(true)
  const [notes, setNotes]                           = useState('')
  const [success, setSuccess]                       = useState(false)
  const [addrError, setAddrError]                   = useState<string | null>(null)

  // Look up customer record by logged-in user email
  const {
    data: customer,
    isLoading: loadingCustomer,
    error: customerError,
  } = useGetCustomerByEmailQuery(email ?? '', { skip: !email })

  const [placeOrder,    { isLoading: placing,         error: orderError }] = usePlaceOrderMutation()
  const [createCustomer, { isLoading: creatingProfile }]                   = useCreateCustomerMutation()
  const [addAddress]                                                        = useAddAddressMutation()

  const savedAddresses = customer?.addresses ?? []

  // Auto-select first saved address when customer loads
  const effectiveSelected = selectedAddressId ??
    (savedAddresses.length > 0 ? savedAddresses[0].id : 'new')

  const handleCreateProfile = async () => {
    if (!email) return
    const parts = (fullName ?? email.split('@')[0]).split(' ')
    await createCustomer({
      firstName: parts[0] ?? email.split('@')[0],
      lastName: parts.slice(1).join(' ') || '',
      email,
    })
  }

  // Guard: empty cart
  if (items.length === 0 && !success) {
    navigate('/cart')
    return null
  }

  const getShippingAddressString = (): string | null => {
    if (effectiveSelected === 'new') {
      if (!newAddr.street || !newAddr.city || !newAddr.state || !newAddr.postalCode) return null
      return `${newAddr.street}, ${newAddr.city}, ${newAddr.state}, ${newAddr.country} — ${newAddr.postalCode}`
    }
    const addr = savedAddresses.find((a) => a.id === effectiveSelected)
    return addr ? addrLabel(addr) : null
  }

  const handlePlaceOrder = async () => {
    if (!customer) return
    const shippingAddress = getShippingAddressString()
    if (!shippingAddress) {
      setAddrError('Please fill in all required address fields (Street, City, State, Postal Code).')
      return
    }
    setAddrError(null)

    // If using a new address and "save to profile" is checked, persist it first
    if (effectiveSelected === 'new' && saveNewAddr) {
      await addAddress({ customerId: customer.id, address: newAddr })
    }

    try {
      await placeOrder({
        customerId: customer.id,
        shippingAddress,
        notes: notes.trim() || undefined,
        items: items.map((i) => ({
          productId: i.productId,
          productName: i.productName,
          unitPrice: i.price,
          quantity: i.quantity,
        })),
      }).unwrap()

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
          <Button variant="outline" onClick={() => navigate('/products')}>Browse Products</Button>
          <Button className="bg-blue-600 hover:bg-blue-700" onClick={() => navigate('/orders')}>
            View My Orders
          </Button>
        </div>
      </div>
    )
  }

  // ── Error: no customer profile ─────────────────────────────────────────────
  const noProfile = !loadingCustomer && !isAdmin && !!(customerError || !customer)

  return (
    <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 max-w-5xl">
      {/* Left — form */}
      <div className="lg:col-span-2 space-y-5">
        <div>
          <h1 className="text-xl font-bold text-gray-900">Checkout</h1>
          <p className="text-sm text-gray-500 mt-0.5">Review your order and enter shipping details</p>
        </div>

        {/* Admin cannot shop */}
        {isAdmin && (
          <div className="flex gap-3 p-4 bg-amber-50 border border-amber-200 rounded-xl text-sm text-amber-800">
            <ShieldAlert size={18} className="flex-shrink-0 mt-0.5 text-amber-500" />
            <div>
              <p className="font-semibold">Admin accounts cannot place orders</p>
              <p className="text-xs mt-1 text-amber-600">
                Log in as a customer account (e.g. <span className="font-mono">alice@eshop.com</span>) to shop.
              </p>
            </div>
          </div>
        )}

        {/* Customer profile missing — offer to auto-create */}
        {noProfile && (
          <div className="flex gap-3 p-4 bg-blue-50 border border-blue-200 rounded-xl text-sm text-blue-800">
            <AlertCircle size={18} className="flex-shrink-0 mt-0.5 text-blue-500" />
            <div className="flex-1">
              <p className="font-semibold">No customer profile found for {email}</p>
              <p className="text-xs mt-1 text-blue-600 mb-3">
                Your account doesn't have a shopping profile yet. Create one to continue.
              </p>
              <Button
                size="sm"
                className="bg-blue-600 hover:bg-blue-700 h-8 text-xs"
                onClick={handleCreateProfile}
                disabled={creatingProfile}
              >
                {creatingProfile
                  ? <><Loader2 size={13} className="animate-spin mr-1.5" /> Creating…</>
                  : <><UserPlus size={13} className="mr-1.5" /> Create My Profile</>}
              </Button>
            </div>
          </div>
        )}

        {/* Shipping address */}
        <div className="bg-white rounded-xl border border-gray-100 shadow-sm p-5 space-y-4">
          <div className="flex items-center gap-2 text-sm font-semibold text-gray-700">
            <MapPin size={16} className="text-blue-500" /> Shipping Address
          </div>

          {/* Saved address cards */}
          {savedAddresses.length > 0 && (
            <div className="space-y-2">
              <p className="text-xs text-gray-500 font-medium">Saved addresses</p>
              {savedAddresses.map((addr) => (
                <button
                  key={addr.id}
                  onClick={() => setSelectedAddressId(addr.id)}
                  className={`w-full text-left flex items-start gap-3 p-3 rounded-lg border transition-all ${
                    effectiveSelected === addr.id
                      ? 'border-blue-500 bg-blue-50 ring-1 ring-blue-500'
                      : 'border-gray-200 hover:border-blue-200 hover:bg-blue-50/30'
                  }`}
                >
                  <CheckCircle size={16} className={`mt-0.5 flex-shrink-0 ${effectiveSelected === addr.id ? 'text-blue-500' : 'text-gray-300'}`} />
                  <div className="text-xs text-gray-700 leading-relaxed">
                    <p className="font-medium">{addr.street}</p>
                    <p className="text-gray-500">{addr.city}, {addr.state} — {addr.postalCode}</p>
                    <p className="text-gray-400">{addr.country}</p>
                  </div>
                </button>
              ))}
              {/* "Use different address" toggle */}
              <button
                onClick={() => setSelectedAddressId('new')}
                className={`w-full text-left flex items-center gap-3 p-3 rounded-lg border transition-all ${
                  effectiveSelected === 'new'
                    ? 'border-blue-500 bg-blue-50 ring-1 ring-blue-500'
                    : 'border-dashed border-gray-300 hover:border-blue-300'
                }`}
              >
                <Plus size={15} className={effectiveSelected === 'new' ? 'text-blue-500' : 'text-gray-400'} />
                <span className="text-xs text-gray-600 font-medium">Use a different address</span>
              </button>
            </div>
          )}

          {/* New address form — shown when "new" selected OR no saved addresses */}
          {(effectiveSelected === 'new' || savedAddresses.length === 0) && (
            <div className="space-y-2">
              {savedAddresses.length === 0 && (
                <p className="text-xs text-gray-500 font-medium">Enter shipping address</p>
              )}
              {[
                { key: 'street',     label: 'Street *',     placeholder: '123 MG Road' },
                { key: 'city',       label: 'City *',       placeholder: 'Mumbai' },
                { key: 'state',      label: 'State *',      placeholder: 'Maharashtra' },
                { key: 'country',    label: 'Country',      placeholder: 'India' },
                { key: 'postalCode', label: 'Postal Code *', placeholder: '400001' },
              ].map(({ key, label, placeholder }) => (
                <div key={key} className="flex gap-2 items-center">
                  <label className="text-xs text-gray-500 w-24 flex-shrink-0">{label}</label>
                  <input
                    value={(newAddr as unknown as Record<string, string>)[key]}
                    onChange={(e) => setNewAddr((f) => ({ ...f, [key]: e.target.value }))}
                    placeholder={placeholder}
                    className="flex-1 px-2 py-1.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                  />
                </div>
              ))}
              <label className="flex items-center gap-2 text-xs text-gray-500 cursor-pointer select-none pt-1">
                <input
                  type="checkbox"
                  checked={saveNewAddr}
                  onChange={(e) => setSaveNewAddr(e.target.checked)}
                  className="accent-blue-600"
                />
                Save this address to my profile for future orders
              </label>
            </div>
          )}

          {/* Notes */}
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

        {/* Address validation error */}
        {addrError && (
          <div className="flex gap-2 p-3 bg-amber-50 border border-amber-200 rounded-lg text-sm text-amber-700">
            <AlertCircle size={16} className="flex-shrink-0 mt-0.5" />
            <span>{addrError}</span>
          </div>
        )}

        {/* Order error */}
        {orderError && (
          <div className="flex gap-2 p-3 bg-red-50 border border-red-200 rounded-lg text-sm text-red-700">
            <AlertCircle size={16} className="flex-shrink-0 mt-0.5" />
            <span>Failed to place order. Please try again.</span>
          </div>
        )}

        <Button
          className="w-full bg-blue-600 hover:bg-blue-700 h-11"
          disabled={placing || loadingCustomer || noProfile || isAdmin || !getShippingAddressString()}
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
