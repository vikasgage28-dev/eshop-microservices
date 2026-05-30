import { useState } from 'react'
import { Search, Users, UserPlus, X, Loader2, UserMinus, AlertTriangle, MapPin } from 'lucide-react'
import { useGetCustomersQuery, useCreateCustomerMutation, useDeleteCustomerMutation } from '@/api/customerApi'
import type { Customer } from '@/types/customer.types'
import { useGetSiteUsersQuery } from '@/api/identityApi'
import { useDebounce } from '@/hooks/useDebounce'
import { Button } from '@/components/ui/button'

export default function CustomersPage() {
  const { data: customers, isLoading } = useGetCustomersQuery()
  const { data: siteUsers } = useGetSiteUsersQuery()
  const [createCustomer, { isLoading: creating }] = useCreateCustomerMutation()
  const [deleteCustomer, { isLoading: deleting }] = useDeleteCustomerMutation()
  const [search, setSearch] = useState('')
  const debouncedSearch = useDebounce(search, 300)
  const [showForm, setShowForm] = useState(false)
  const [selectedUserId, setSelectedUserId] = useState('')
  const [phone, setPhone] = useState('')
  const [formError, setFormError] = useState<string | null>(null)
  const [confirmDelete, setConfirmDelete] = useState<Customer | null>(null)

  // Site users who don't yet have a customer profile
  const customerEmails = new Set(customers?.map((c) => c.email.toLowerCase()) ?? [])
  const eligible = siteUsers?.filter((u) => !customerEmails.has(u.email.toLowerCase())) ?? []

  const filtered = customers?.filter((c) => {
    const q = debouncedSearch.toLowerCase()
    return (
      c.email.toLowerCase().includes(q) ||
      c.firstName.toLowerCase().includes(q) ||
      c.lastName.toLowerCase().includes(q)
    )
  })

  const selectedUser = eligible.find((u) => u.userId === selectedUserId)

  const openForm = () => {
    setShowForm(true)
    setSelectedUserId('')
    setPhone('')
    setFormError(null)
  }

  const handleUnregister = async () => {
    if (!confirmDelete) return
    await deleteCustomer(confirmDelete.id)
    setConfirmDelete(null)
  }

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!selectedUser) return
    setFormError(null)
    const parts = selectedUser.fullName.split(' ')
    try {
      await createCustomer({
        firstName: parts[0] ?? selectedUser.email.split('@')[0],
        lastName: parts.slice(1).join(' ') || '',
        email: selectedUser.email,
        phone: phone.trim() || undefined,
      }).unwrap()
      setShowForm(false)
    } catch {
      setFormError('Failed to register customer. Please try again.')
    }
  }

  return (
    <div className="space-y-5">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-xl font-bold text-gray-900 dark:text-gray-100">Customers</h1>
          <p className="text-sm text-gray-500 mt-0.5">{customers?.length ?? 0} registered customers</p>
        </div>
        <Button
          className="bg-blue-600 hover:bg-blue-700 h-9 text-sm gap-2"
          onClick={openForm}
        >
          <UserPlus size={15} /> Register Customer
        </Button>
      </div>

      {/* Register Customer Dialog */}
      {showForm && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40">
          <div className="bg-white dark:bg-[#1e1e1e] rounded-2xl shadow-2xl w-full max-w-md p-7 relative">
            <button onClick={() => setShowForm(false)}
              className="absolute top-4 right-4 text-gray-400 hover:text-gray-700 dark:hover:text-gray-200">
              <X size={18} />
            </button>
            <h2 className="text-base font-bold text-gray-900 dark:text-gray-100 mb-1 flex items-center gap-2">
              <UserPlus size={17} className="text-blue-600" /> Register Customer
            </h2>
            <p className="text-xs text-gray-500 mb-5">Select a site user to create their customer profile.</p>

            {eligible.length === 0 && (
              <p className="text-sm text-gray-500 bg-gray-50 dark:bg-[#2a2a2a] rounded-lg p-4 text-center mb-4">
                All registered site users already have a customer profile.
              </p>
            )}

            {formError && (
              <p className="mb-4 text-xs text-red-600 bg-red-50 border border-red-200 rounded-lg px-3 py-2">{formError}</p>
            )}

            <form onSubmit={handleCreate} className="space-y-4">
              <div>
                <label className="block text-xs font-semibold text-gray-600 dark:text-gray-400 mb-1">Select User</label>
                <select
                  required
                  value={selectedUserId}
                  onChange={(e) => setSelectedUserId(e.target.value)}
                  className="w-full px-3 py-2 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-[#2a2a2a] dark:border-[#3a3a3a] dark:text-gray-100"
                >
                  <option value="">— Choose a registered user —</option>
                  {eligible.map((u) => (
                    <option key={u.userId} value={u.userId}>
                      {u.fullName} ({u.email})
                    </option>
                  ))}
                </select>
              </div>

              {selectedUser && (
                <div className="bg-blue-50 dark:bg-blue-900/20 rounded-lg px-4 py-3 text-xs text-blue-800 dark:text-blue-300 space-y-0.5">
                  <p className="font-semibold">{selectedUser.fullName}</p>
                  <p className="text-blue-500">{selectedUser.email}</p>
                </div>
              )}

              <div>
                <label className="block text-xs font-semibold text-gray-600 dark:text-gray-400 mb-1">
                  Phone <span className="text-gray-400 font-normal">(optional)</span>
                </label>
                <input
                  value={phone}
                  onChange={(e) => setPhone(e.target.value)}
                  placeholder="+91-9000000001"
                  className="w-full px-3 py-2 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-[#2a2a2a] dark:border-[#3a3a3a] dark:text-gray-100"
                />
              </div>

              <div className="flex gap-3 pt-1">
                <Button type="button" variant="outline" className="flex-1 h-9 text-sm" onClick={() => setShowForm(false)}>
                  Cancel
                </Button>
                <Button type="submit" className="flex-1 h-9 text-sm bg-blue-600 hover:bg-blue-700"
                  disabled={creating || !selectedUserId || eligible.length === 0}>
                  {creating ? <><Loader2 size={14} className="animate-spin mr-1.5" /> Registering…</> : 'Register'}
                </Button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Confirm Unregister Dialog */}
      {confirmDelete && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40">
          <div className="bg-white dark:bg-[#1e1e1e] rounded-2xl shadow-2xl w-full max-w-sm p-7 relative">
            <div className="flex flex-col items-center text-center gap-3 mb-6">
              <div className="w-12 h-12 bg-red-50 rounded-full flex items-center justify-center">
                <AlertTriangle size={22} className="text-red-500" />
              </div>
              <h2 className="text-base font-bold text-gray-900 dark:text-gray-100">Unregister Customer?</h2>
              <p className="text-sm text-gray-500">
                This will remove the customer profile for{' '}
                <span className="font-semibold text-gray-700 dark:text-gray-300">
                  {confirmDelete.firstName} {confirmDelete.lastName}
                </span>
                . Their login account will not be affected.
              </p>
            </div>
            <div className="flex gap-3">
              <Button variant="outline" className="flex-1 h-9 text-sm" onClick={() => setConfirmDelete(null)}>
                Cancel
              </Button>
              <Button
                className="flex-1 h-9 text-sm bg-red-600 hover:bg-red-700 text-white"
                onClick={handleUnregister}
                disabled={deleting}
              >
                {deleting ? <><Loader2 size={14} className="animate-spin mr-1.5" /> Removing…</> : 'Yes, Unregister'}
              </Button>
            </div>
          </div>
        </div>
      )}

      {/* Search */}
      <div className="relative max-w-sm">
        <Search size={16} className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
        <input
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          placeholder="Search by name or email…"
          className="w-full pl-9 pr-4 py-2 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </div>

      {/* Table */}
      <div className="bg-white rounded-xl border border-gray-100 shadow-sm overflow-hidden">
        <table className="w-full text-sm">
          <thead>
            <tr className="bg-gray-50">
              <th className="text-left px-5 py-3 text-xs font-semibold text-gray-500 uppercase tracking-wide">Name</th>
              <th className="text-left px-5 py-3 text-xs font-semibold text-gray-500 uppercase tracking-wide">Email</th>
              <th className="text-left px-5 py-3 text-xs font-semibold text-gray-500 uppercase tracking-wide">Phone</th>
              <th className="text-left px-5 py-3 text-xs font-semibold text-gray-500 uppercase tracking-wide">Addresses</th>
              <th className="text-left px-5 py-3 text-xs font-semibold text-gray-500 uppercase tracking-wide">Joined</th>
              <th className="px-5 py-3" />
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-50">
            {isLoading &&
              Array.from({ length: 5 }).map((_, i) => (
                <tr key={i}>
                  {Array.from({ length: 6 }).map((_, j) => (
                    <td key={j} className="px-5 py-3">
                      <div className="h-4 bg-gray-100 rounded animate-pulse" />
                    </td>
                  ))}
                </tr>
              ))}
            {filtered?.map((customer) => (
              <tr key={customer.id} className="hover:bg-gray-50 transition-colors">
                <td className="px-5 py-3">
                  <div className="flex items-center gap-3">
                    <div className="w-8 h-8 rounded-full bg-blue-100 flex items-center justify-center flex-shrink-0">
                      <Users size={14} className="text-blue-600" />
                    </div>
                    <span className="font-medium text-gray-800">
                      {customer.firstName} {customer.lastName}
                    </span>
                  </div>
                </td>
                <td className="px-5 py-3 text-gray-600">{customer.email}</td>
                <td className="px-5 py-3 text-gray-500">{customer.phone || '—'}</td>
                <td className="px-5 py-3">
                  {(customer.addresses?.length ?? 0) > 0 ? (
                    <span className="inline-flex items-center gap-1 text-xs font-medium px-2 py-0.5 rounded-full bg-blue-50 text-blue-600">
                      <MapPin size={11} /> {customer.addresses!.length}
                    </span>
                  ) : (
                    <span className="text-gray-400 text-xs">—</span>
                  )}
                </td>
                <td className="px-5 py-3 text-gray-500">
                  {customer.createdAt ? new Date(customer.createdAt).toLocaleDateString() : '—'}
                </td>
                <td className="px-5 py-3 text-right">
                  <button
                    onClick={() => setConfirmDelete(customer)}
                    title="Unregister customer"
                    className="inline-flex items-center gap-1.5 text-xs text-gray-400 hover:text-red-600 hover:bg-red-50 dark:hover:bg-red-900/20 px-2 py-1 rounded-lg transition-colors"
                  >
                    <UserMinus size={14} /> Unregister
                  </button>
                </td>
              </tr>
            ))}
            {!isLoading && filtered?.length === 0 && (
              <tr>
                <td colSpan={6} className="px-5 py-12 text-center text-gray-400">
                  {search ? 'No customers match your search' : 'No customers yet'}
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  )
}
