import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { LogOut, ShieldCheck, User, Mail, Hash, ClipboardList, MapPin, Plus, Trash2, Loader2, Star, ShoppingBag } from 'lucide-react'
import { useAuth } from '@/hooks/useAuth'
import { useGetOrdersByCustomerQuery, useGetOrdersQuery } from '@/api/orderingApi'
import { useGetCustomerByEmailQuery, useAddAddressMutation, useDeleteAddressMutation } from '@/api/customerApi'
import { Button } from '@/components/ui/button'
import { formatCurrency } from '@/lib/utils'
import type { Address } from '@/types/customer.types'

function InfoRow({ icon, label, value }: { icon: React.ReactNode; label: string; value: string }) {
  return (
    <div className="flex items-center gap-3 py-3 border-b border-[#e8e8e8] dark:border-[#3a3a3a] last:border-0">
      <span className="text-gray-400 dark:text-gray-500 flex-shrink-0">{icon}</span>
      <span className="text-xs text-gray-500 dark:text-gray-400 w-20 flex-shrink-0">{label}</span>
      <span className="text-sm font-medium text-gray-900 dark:text-gray-100 truncate">{value}</span>
    </div>
  )
}

const EMPTY_ADDR = { street: '', city: '', state: '', country: 'India', postalCode: '', isDefault: false }

export default function ProfilePage() {
  const { fullName, email, userId, roles, isAdmin, logout } = useAuth()  // userId used for display only
  const navigate = useNavigate()

  const { data: allOrders }      = useGetOrdersQuery(undefined,              { skip: !isAdmin })
  const { data: customer }       = useGetCustomerByEmailQuery(email ?? '',   { skip: !email || isAdmin })
  // Orders are keyed by Customer profile ID (not Identity userId) — must wait for customer to load
  const { data: customerOrders } = useGetOrdersByCustomerQuery(customer?.id ?? '', { skip: isAdmin || !customer?.id })

  const [addAddress, { isLoading: addingAddr }] = useAddAddressMutation()
  const [deleteAddress]                          = useDeleteAddressMutation()

  const [showAddrForm, setShowAddrForm] = useState(false)
  const [addrForm, setAddrForm]         = useState<Omit<Address, 'id'>>(EMPTY_ADDR)

  const orders     = isAdmin ? allOrders : customerOrders
  const totalSpend = customerOrders?.reduce((s, o) => s + o.totalAmount, 0) ?? 0
  const initials   = fullName ? fullName.split(' ').map((n) => n[0]).join('').slice(0, 2).toUpperCase() : 'U'

  const handleLogout = () => { logout(); navigate('/login') }

  const handleSaveAddress = async () => {
    if (!customer) return
    if (!addrForm.street || !addrForm.city || !addrForm.state || !addrForm.postalCode) return
    await addAddress({ customerId: customer.id, address: addrForm })
    setAddrForm(EMPTY_ADDR)
    setShowAddrForm(false)
  }

  const handleDeleteAddress = (addressId: string) => {
    if (!customer) return
    deleteAddress({ customerId: customer.id, addressId })
  }

  return (
    <div className="max-w-lg space-y-5">
      {/* Avatar card */}
      <div className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a] p-6 flex items-center gap-5">
        <div className="w-16 h-16 bg-blue-600 rounded-full flex items-center justify-center flex-shrink-0">
          <span className="text-white text-2xl font-bold">{initials}</span>
        </div>
        <div className="min-w-0">
          <h2 className="text-base font-semibold text-gray-900 dark:text-gray-100 truncate">{fullName ?? 'User'}</h2>
          <p className="text-sm text-gray-500 dark:text-gray-400 truncate">{email}</p>
          <div className="flex flex-wrap gap-1.5 mt-2">
            {roles.map((r) => (
              <span key={r} className="inline-flex items-center gap-1 text-xs font-medium px-2 py-0.5 rounded bg-blue-50 dark:bg-blue-900/30 text-blue-600 dark:text-blue-400">
                <ShieldCheck size={10} /> {r}
              </span>
            ))}
          </div>
        </div>
      </div>

      {/* Account info */}
      <div className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a] px-4">
        <InfoRow icon={<User size={15} />}       label="Full name" value={fullName ?? '—'} />
        <InfoRow icon={<Mail size={15} />}        label="Email"     value={email ?? '—'} />
        <InfoRow icon={<Hash size={15} />}        label="User ID"   value={userId?.slice(0, 16) + '…' ?? '—'} />
      </div>

      {/* Stats */}
      {!isAdmin && (
        <div className="grid grid-cols-2 gap-4">
          <div className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a] p-4 text-center">
            <p className="text-2xl font-bold text-gray-900 dark:text-gray-100">{orders?.length ?? 0}</p>
            <p className="text-xs text-gray-500 dark:text-gray-400 mt-0.5 uppercase tracking-wide">Total Orders</p>
          </div>
          <div className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a] p-4 text-center">
            <p className="text-2xl font-bold text-blue-600 dark:text-blue-400">{formatCurrency(totalSpend)}</p>
            <p className="text-xs text-gray-500 dark:text-gray-400 mt-0.5 uppercase tracking-wide">Total Spent</p>
          </div>
        </div>
      )}

      {/* My Addresses — customers only */}
      {!isAdmin && (
        <div className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a] p-4 space-y-3">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-2 text-sm font-semibold text-gray-700 dark:text-gray-200">
              <MapPin size={15} className="text-blue-500" /> My Addresses
            </div>
            <button
              onClick={() => setShowAddrForm((v) => !v)}
              className="flex items-center gap-1 text-xs text-blue-600 hover:text-blue-800 font-medium"
            >
              <Plus size={13} /> Add New
            </button>
          </div>

          {/* Add address form */}
          {showAddrForm && (
            <div className="border border-blue-100 dark:border-blue-900/40 rounded-lg p-3 space-y-2 bg-blue-50/50 dark:bg-blue-900/10">
              {[
                { key: 'street',     label: 'Street *',      placeholder: '123 MG Road' },
                { key: 'city',       label: 'City *',         placeholder: 'Mumbai' },
                { key: 'state',      label: 'State *',        placeholder: 'Maharashtra' },
                { key: 'country',    label: 'Country',        placeholder: 'India' },
                { key: 'postalCode', label: 'Postal Code *',  placeholder: '400001' },
              ].map(({ key, label, placeholder }) => (
                <div key={key} className="flex gap-2 items-center">
                  <label className="text-xs text-gray-500 dark:text-gray-400 w-24 flex-shrink-0">{label}</label>
                  <input
                    value={(addrForm as Record<string, string>)[key]}
                    onChange={(e) => setAddrForm((f) => ({ ...f, [key]: e.target.value }))}
                    placeholder={placeholder}
                    className="flex-1 px-2 py-1 border border-gray-200 dark:border-[#444] rounded text-xs bg-white dark:bg-[#333] text-gray-900 dark:text-gray-100 focus:outline-none focus:ring-1 focus:ring-blue-500"
                  />
                </div>
              ))}
              <div className="flex gap-2 pt-1">
                <Button size="sm" className="h-7 text-xs bg-blue-600 hover:bg-blue-700" onClick={handleSaveAddress} disabled={addingAddr}>
                  {addingAddr ? <Loader2 size={12} className="animate-spin mr-1" /> : null} Save Address
                </Button>
                <Button size="sm" variant="outline" className="h-7 text-xs" onClick={() => setShowAddrForm(false)}>Cancel</Button>
              </div>
            </div>
          )}

          {/* Saved addresses list */}
          {customer?.addresses?.length === 0 && !showAddrForm && (
            <p className="text-xs text-gray-400 dark:text-gray-500 py-2">No saved addresses yet. Add one above.</p>
          )}
          <div className="space-y-2">
            {customer?.addresses?.map((addr) => (
              <div key={addr.id} className="flex items-start justify-between gap-2 p-2.5 rounded-lg border border-gray-100 dark:border-[#3a3a3a] bg-gray-50 dark:bg-[#333]">
                <div className="flex gap-2 items-start">
                  <MapPin size={13} className="text-blue-400 mt-0.5 flex-shrink-0" />
                  <div className="text-xs text-gray-700 dark:text-gray-300 leading-relaxed">
                    <p>{addr.street}</p>
                    <p>{addr.city}, {addr.state} — {addr.postalCode}</p>
                    <p className="text-gray-400">{addr.country}</p>
                  </div>
                </div>
                <div className="flex items-center gap-1.5 flex-shrink-0">
                  {addr.isDefault && <Star size={11} className="text-yellow-500 fill-yellow-400" />}
                  <button
                    onClick={() => handleDeleteAddress(addr.id)}
                    className="text-gray-400 hover:text-red-500 transition-colors"
                    title="Remove address"
                  >
                    <Trash2 size={13} />
                  </button>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* Actions */}
      <div className="flex gap-3 flex-wrap">
        {!isAdmin && (
          <Button
            className="flex-1 gap-2 text-sm bg-blue-600 hover:bg-blue-700 min-w-[120px]"
            onClick={() => navigate('/products')}
          >
            <ShoppingBag size={15} /> Browse Products
          </Button>
        )}
        <Button
          variant="outline"
          className="flex-1 gap-2 text-sm border-[#e8e8e8] dark:border-[#444] dark:bg-transparent dark:text-gray-300 dark:hover:bg-[#333] min-w-[110px]"
          onClick={() => navigate('/orders')}
        >
          <ClipboardList size={15} /> My Orders
        </Button>
        <Button
          variant="outline"
          className="flex-1 gap-2 text-sm text-red-600 dark:text-red-400 border-[#e8e8e8] dark:border-[#444] dark:bg-transparent hover:bg-red-50 dark:hover:bg-red-900/20 hover:border-red-200 min-w-[100px]"
          onClick={handleLogout}
        >
          <LogOut size={15} /> Sign Out
        </Button>
      </div>
    </div>
  )
}
