import { useState } from 'react'
import { Search, Users } from 'lucide-react'
import { useGetCustomersQuery } from '@/api/customerApi'
import { useDebounce } from '@/hooks/useDebounce'

export default function CustomersPage() {
  const { data: customers, isLoading } = useGetCustomersQuery()
  const [search, setSearch] = useState('')
  const debouncedSearch = useDebounce(search, 300)

  const filtered = customers?.filter((c) => {
    const q = debouncedSearch.toLowerCase()
    return (
      c.email.toLowerCase().includes(q) ||
      c.firstName.toLowerCase().includes(q) ||
      c.lastName.toLowerCase().includes(q)
    )
  })

  return (
    <div className="space-y-5">
      <div>
        <h1 className="text-xl font-bold text-gray-900">Customers</h1>
        <p className="text-sm text-gray-500 mt-0.5">{customers?.length ?? 0} registered customers</p>
      </div>

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
              <th className="text-left px-5 py-3 text-xs font-semibold text-gray-500 uppercase tracking-wide">Joined</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-50">
            {isLoading &&
              Array.from({ length: 5 }).map((_, i) => (
                <tr key={i}>
                  {Array.from({ length: 4 }).map((_, j) => (
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
                <td className="px-5 py-3 text-gray-500">
                  {customer.createdAt ? new Date(customer.createdAt).toLocaleDateString() : '—'}
                </td>
              </tr>
            ))}
            {!isLoading && filtered?.length === 0 && (
              <tr>
                <td colSpan={4} className="px-5 py-12 text-center text-gray-400">
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
