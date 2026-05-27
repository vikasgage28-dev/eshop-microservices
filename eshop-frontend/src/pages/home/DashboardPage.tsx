import { Package, ShoppingCart, Users, TrendingUp } from 'lucide-react'
import { formatCurrency } from '@/lib/utils'
import { useGetProductsQuery } from '@/api/catalogApi'
import { useGetOrdersQuery } from '@/api/orderingApi'
import { useGetCustomersQuery } from '@/api/customerApi'
import { useAuth } from '@/hooks/useAuth'

interface StatCardProps {
  title: string
  value: string | number
  icon: React.ReactNode
  color: string
  sub?: string
}

function StatCard({ title, value, icon, color, sub }: StatCardProps) {
  return (
    <div className="bg-white dark:bg-[#2a2a2a] rounded-lg p-4 border border-[#e8e8e8] dark:border-[#3a3a3a]">
      <div className="flex items-start justify-between">
        <div>
          <p className="text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wide">{title}</p>
          <p className="text-2xl font-semibold text-gray-900 dark:text-gray-100 mt-1 leading-tight">{value}</p>
          {sub && <p className="text-xs text-gray-400 dark:text-gray-500 mt-1">{sub}</p>}
        </div>
        <div className={`w-9 h-9 rounded-lg flex items-center justify-center ${color}`}>
          {icon}
        </div>
      </div>
    </div>
  )
}

export default function DashboardPage() {
  const { fullName } = useAuth()
  const { data: products } = useGetProductsQuery({ page: 1, pageSize: 1 })
  const { data: orders } = useGetOrdersQuery()
  const { data: customers } = useGetCustomersQuery()

  const revenue = orders?.reduce((sum, o) => sum + o.totalAmount, 0) ?? 0

  return (
    <div className="space-y-5">
      {/* Greeting sub-header */}
      <p className="text-sm text-gray-500">
        Welcome back, <span className="font-medium text-gray-700">{fullName?.split(' ')[0] ?? 'there'}</span> — here's your store overview.
      </p>

      {/* Stats */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        <StatCard
          title="Total Products"
          value={products?.totalCount ?? '—'}
          icon={<Package size={20} className="text-blue-600" />}
          color="bg-blue-50"
        />
        <StatCard
          title="Total Orders"
          value={orders?.length ?? '—'}
          icon={<ShoppingCart size={20} className="text-green-600" />}
          color="bg-green-50"
        />
        <StatCard
          title="Customers"
          value={customers?.length ?? '—'}
          icon={<Users size={20} className="text-purple-600" />}
          color="bg-purple-50"
        />
        <StatCard
          title="Revenue"
          value={formatCurrency(revenue)}
          icon={<TrendingUp size={20} className="text-orange-600" />}
          color="bg-orange-50"
        />
      </div>

      {/* Recent orders */}
      <div className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a]">
        <div className="px-4 py-3 border-b border-[#e8e8e8] dark:border-[#3a3a3a]">
          <h2 className="text-sm font-semibold text-gray-900 dark:text-gray-100">Recent Orders</h2>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full text-sm">
            <thead>
              <tr className="bg-[#f8f8f8] dark:bg-[#222]">
                <th className="text-left px-4 py-2.5 text-xs font-semibold text-gray-500 dark:text-gray-400 uppercase tracking-wide">Order ID</th>
                <th className="text-left px-4 py-2.5 text-xs font-semibold text-gray-500 dark:text-gray-400 uppercase tracking-wide">Customer</th>
                <th className="text-left px-4 py-2.5 text-xs font-semibold text-gray-500 dark:text-gray-400 uppercase tracking-wide">Status</th>
                <th className="text-right px-4 py-2.5 text-xs font-semibold text-gray-500 dark:text-gray-400 uppercase tracking-wide">Amount</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-[#f0f0f0] dark:divide-[#333]">
              {orders?.slice(0, 5).map((order) => (
                <tr key={order.id} className="hover:bg-[#fafafa] dark:hover:bg-[#222] transition-colors">
                  <td className="px-4 py-2.5 font-mono text-xs text-gray-500 dark:text-gray-400">{order.id.slice(0, 8)}…</td>
                  <td className="px-4 py-2.5 text-gray-700 dark:text-gray-300">{order.customerEmail}</td>
                  <td className="px-4 py-2.5">
                    <span className={`inline-flex items-center px-2 py-0.5 rounded text-xs font-medium ${
                      order.status === 'Delivered'  ? 'bg-green-100 text-green-700'   :
                      order.status === 'Cancelled'  ? 'bg-red-100 text-red-700'       :
                      order.status === 'Shipped'    ? 'bg-indigo-100 text-indigo-700' :
                      order.status === 'Processing' ? 'bg-purple-100 text-purple-700' :
                      order.status === 'Confirmed'  ? 'bg-blue-100 text-blue-700'     :
                      'bg-yellow-100 text-yellow-700'
                    }`}>
                      {order.statusName ?? order.status}
                    </span>
                  </td>
                  <td className="px-4 py-2.5 text-right font-semibold text-gray-900 dark:text-gray-100">{formatCurrency(order.totalAmount)}</td>
                </tr>
              ))}
              {(!orders || orders.length === 0) && (
                <tr>
                  <td colSpan={4} className="px-4 py-8 text-center text-gray-400 text-sm">No orders yet</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  )
}
